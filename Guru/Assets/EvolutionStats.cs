using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvolutionStats : MonoBehaviour
{
    PopulationManager populationManager;
    public TextMesh canvas;
    void Start()
    {
        populationManager = GameObject.Find("PopulationManager").GetComponent<PopulationManager>();
    }

    // Update is called once per frame
    void Update()
    {
        canvas.text = "Generation: " + populationManager.generation 
        + "\n" + string.Format("Time: {0:0.00}", PopulationManager.elapsed) 
        + "\nPopulation: " + populationManager.populationSize;
    }
}
