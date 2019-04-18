using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using Valve.VR.InteractionSystem.Sample;
using Proyecto26;
using UnityEngine.Networking;


public class PopulationManager : MonoBehaviour
{
    public GameObject botPrefab;
    public int populationSize = 16;
    List<GameObject> population = new List<GameObject>();
    public static float elapsed = 0f;
    public float trialTime = 10;
    public int mutationRate = 10;

    public float botSquareSpacing = 10f;
    public int botsPerRow = 4;
    public SwingRecorder swingRecorder;
    public int generation = 0;

    public GameObject bestAgent;

    public GameObject playerSwordTracker;
    private GameObject oldBestAgent;

    GUIStyle gui = new GUIStyle();

    private Vector3 origin;
    private int uid;

    private string api = "https://guru-base.firebaseio.com";
    public bool databasing = true;
    public string archivedPopulationEndpoint;

    List<GameObject> sortedPopulation;

    private void OnGUI() {
        gui.fontSize = 30;
        gui.normal.textColor = Color.red;
        GUI.BeginGroup (new Rect(10,10,250,150));
        GUI.Box(new Rect(0,0,140,140), "Stats", gui);
        GUI.Label(new Rect(10,25,200,30), "Gen: " + generation, gui);
        GUI.Label(new Rect(10,50,200,30), string.Format("Time: {0:0.00}", elapsed), gui);
        GUI.Label(new Rect(10,75,200,30), "Population: " + population.Count, gui);
        GUI.EndGroup();
    }

    public void loadArchivedPopulation(){

        Debug.Log("Loading data from " + archivedPopulationEndpoint);

        RestClient.GetArray<DNAData>(archivedPopulationEndpoint + ".json").Then(response => {
            int i = 0;
            foreach (DNAData data in response)
            {
                DNA newDNA = new DNA(data.genes, data.fGenes, data.dnaLength, data.maxValue);
                population[i].GetComponent<Brain>().dna = newDNA;
                i++;
            }
        }).Then(() => {
            foreach (GameObject bot in population)
            {
                bot.GetComponent<Brain>().initializeGenes();
            }
        });
    }

    private void Start() {
        if(databasing)
            archivePreviousSession();

        swingRecorder = GameObject.Find("TrainingDummy").GetComponentInChildren<SwingRecorder>();
        origin = transform.position;

        for(int i = 0; i < populationSize; i++) {

            GameObject bot = Instantiate(
                botPrefab, 
                new Vector3(
                    origin.x+(i%botsPerRow)*botSquareSpacing,
                    origin.y,
                    origin.z+(i/botsPerRow)*botSquareSpacing), 
                Quaternion.Euler(0,180,0)
            );

            bot.GetComponent<Brain>().Init(true);
            population.Add(bot);
        }

        if(archivedPopulationEndpoint.Length > 0 && databasing) {
            loadArchivedPopulation();
        } 
    }

    GameObject Breed(GameObject parent1, GameObject parent2, int i) {
        //TODO: Remove startingPos[0]
        GameObject offspring = Instantiate(
            botPrefab, 
            new Vector3(
                origin.x+(i%botsPerRow)*botSquareSpacing,
                origin.y,
                origin.z+(i/botsPerRow)*botSquareSpacing), 
            Quaternion.Euler(0,180,0)
        );

        Brain brain = offspring.GetComponent<Brain>();

        if(Random.Range(0,100) <= mutationRate) {
            brain.Init();
            brain.dna.Mutate();
        }
        if(Random.Range(0,100) <= mutationRate) {
            brain.Init();
            brain.dna.FMutate();
        } else {
            brain.Init();
            brain.dna.Crossover(parent1.GetComponent<Brain>().dna, parent2.GetComponent<Brain>().dna);
        }
        
        return offspring;
    }

    private void clearAll() {
        population.Clear();
        swingRecorder.swordMotionReproducers.Clear();
    }

    void spawnBestAgent() {
        if(oldBestAgent)
            Destroy(oldBestAgent);

        oldBestAgent = Instantiate(bestAgent, new Vector3(2.5f,0.75f, -1f), Quaternion.Euler(0,270,0));
        
        Brain bestAgentBrain = oldBestAgent.GetComponent<Brain>();
        
        bestAgentBrain.isBestAgent = true;
        bestAgentBrain.enemySwordTransform = playerSwordTracker.transform;
        oldBestAgent.GetComponent<ControlPointHandler>().enemySword = playerSwordTracker.transform;
        oldBestAgent.transform.Find("Replay").gameObject.SetActive(false);
        
        GameObject AISword = oldBestAgent.transform.Find("AISword").gameObject;
        
        AISword.GetComponent<LockToPoint>().enabled = true;
        // AISword.transform.Find("Blade Collider").GetComponent<BlockingZone>().enemyWeapon = playerSwordTracker;
        oldBestAgent.GetComponent<Brain>().enabled = true;
        oldBestAgent.GetComponent<BotStats>().enabled = true;
        oldBestAgent.GetComponent<ControlPointHandler>().enabled = true;
    }

    void Selection() {
        sortedPopulation = population.OrderBy(o => (o.GetComponent<Brain>().damageTaken + o.GetComponent<Brain>().blockingTime + o.GetComponent<Brain>().blockingZone.elapsed)).ToList();
        
        bestAgent = sortedPopulation[0];
        
        clearAll();

        for(int i = 0; i < (int) (sortedPopulation.Count/2f); i++) {
            population.Add(Breed(sortedPopulation[i], sortedPopulation[i+1], 2*i));
            population.Add(Breed(sortedPopulation[i+1], sortedPopulation[i], 2*i+1));
        }

        foreach(GameObject bot in sortedPopulation) {
            Destroy(bot);
        }

        spawnBestAgent();
        generation++;
    }

    private void postDNA(List<GameObject> snapShotPopulation, List<float> scoreList) {
        SerializedGeneration currentGen = new SerializedGeneration(generation);
        RestClient.Put(api+"/generation.json", currentGen).Then((response) => {
            int i = 0;
            foreach (GameObject bot in snapShotPopulation)
            {
                Brain botBrain = bot.GetComponent<Brain>();
                DNA botDNA = botBrain.dna;
                float score = scoreList[i];

                DNAData botRecord = new DNAData(botDNA.genes, botDNA.fGenes, botDNA.dnaLength, botDNA.maxValue, score);

                RestClient.Put(api + "/currentSession/" + (generation-1) + "/" + i + ".json", botRecord);
                i++;
            }
        });
    }

    private void deletePreviousDatabaseSession() {
        RestClient.Delete(api + "/currentSession.json");
    }

    private void archivePreviousSession() {
        RestClient.Get<SerializedGeneration>(api+"/generation.json").Then(previousGen => {
            RestClient.GetArray<DNAData>("https://guru-base.firebaseio.com/currentSession/" + previousGen.generation + ".json").Then((response) => {
                int i = 0;
                foreach (DNAData item in response)
                {
                    RestClient.Put(api + "/archivedSessions/" + System.DateTime.Now + "/gen" + previousGen.generation + "/" + i + ".json", item);
                    i++; 
                }
            }).Then(() => {
                deletePreviousDatabaseSession();
            }); 
        });
    }

    private void Update() {
        elapsed += Time.deltaTime;
        if(elapsed >= trialTime) {
            
            List<float> scoreList = new List<float>();
            foreach (GameObject bot in population)
            {
                Brain botBrain = bot.GetComponent<Brain>();
                float score = botBrain.damageTaken + botBrain.blockingTime + botBrain.blockingZone.elapsed;
                scoreList.Add(score);
            }

            if(databasing) 
                postDNA(population, scoreList);

            Selection();
            elapsed = 0;
            
        }

        if(Input.GetKeyDown(KeyCode.R)) {
            SceneManager.LoadScene(0);
        }
    }
}
