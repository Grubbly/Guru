using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class PopulationManager : MonoBehaviour
{
    public GameObject botPrefab;
    public int populationSize = 50;
    List<GameObject> population = new List<GameObject>();
    public static float elapsed = 0f;
    public float trialTime = 10;
    public float timeWalkingWeight = 5f;

    public float botSquareSpacing = 10f;
    public int botsPerRow = 4;
    int generation = 1;

    GUIStyle gui = new GUIStyle();

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
        Vector3 origin = transform.position;
        for(int i = 0; i < populationSize; i++) {
            
            GameObject bot = Instantiate(
                botPrefab, 
                new Vector3(
                    origin.x+(i%botsPerRow)*botSquareSpacing,
                    origin.y,origin.z+(i/botsPerRow)*botSquareSpacing), 
                    Quaternion.Euler(0,180,0)
            );

            bot.GetComponent<Brain>().Init();
            population.Add(bot);
        }
    }

    GameObject Breed(GameObject parent1, GameObject parent2) {
        //TODO: Remove startingPos[0]
        GameObject offspring = Instantiate(botPrefab, transform.position, this.transform.rotation);
        Brain brain = offspring.GetComponent<Brain>();

        if(Random.Range(0,100) == 1) {
            brain.Init();
            brain.dna.Mutate();
        } else {
            brain.Init();
            brain.dna.Crossover(parent1.GetComponent<Brain>().dna, parent2.GetComponent<Brain>().dna);
        }

        return offspring;
    }

    void Selection() {
        List<GameObject> sortedList = population.OrderBy(o => (o.GetComponent<Brain>().damageTaken)).ToList();
        
        population.Clear();

        for(int i = 0; i < (int) (sortedList.Count/2f)-1; i++) {
            population.Add(Breed(sortedList[i], sortedList[i+1]));
            population.Add(Breed(sortedList[i+1], sortedList[i]));
        }

        foreach(GameObject bot in sortedList) {
            Destroy(bot);
        }

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
