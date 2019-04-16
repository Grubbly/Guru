using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using Valve.VR.InteractionSystem.Sample;

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
    int generation = 1;

    public GameObject bestAgent;

    public GameObject playerSwordTracker;
    private GameObject oldBestAgent;

    GUIStyle gui = new GUIStyle();

    private Vector3 origin;

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

    private void Start() {
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
        oldBestAgent.GetComponent<Brain>().enemySwordTransform = playerSwordTracker.transform;
        oldBestAgent.GetComponent<ControlPointHandler>().enemySword = playerSwordTracker.transform;
        oldBestAgent.transform.Find("Replay").gameObject.SetActive(false);
        
        GameObject AISword = oldBestAgent.transform.Find("AISword").gameObject;
        
        AISword.GetComponent<LockToPoint>().enabled = true;
        AISword.transform.Find("Blade Collider").GetComponent<BlockingZone>().enemyWeapon = playerSwordTracker;
        oldBestAgent.GetComponent<Brain>().enabled = true;
        oldBestAgent.GetComponent<BotStats>().enabled = true;
        oldBestAgent.GetComponent<ControlPointHandler>().enabled = true;
    }

    void Selection() {
        List<GameObject> sortedList = population.OrderBy(o => (o.GetComponent<Brain>().damageTaken)).ToList();
        
        bestAgent = sortedList[0];
        
        clearAll();

        for(int i = 0; i < (int) (sortedList.Count/2f); i++) {
            population.Add(Breed(sortedList[i], sortedList[i+1], 2*i));
            population.Add(Breed(sortedList[i+1], sortedList[i], 2*i+1));
        }

        foreach(GameObject bot in sortedList) {
            Destroy(bot);
        }

        spawnBestAgent();
        generation++;
    }

    private void Update() {
        elapsed += Time.deltaTime;
        if(elapsed >= trialTime) {
            Selection();
            elapsed = 0;
        }

        if(Input.GetKeyDown(KeyCode.R)) {
            SceneManager.LoadScene(0);
        }
    }

}
