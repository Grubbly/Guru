using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulationGizmos : MonoBehaviour
{
    void OnDrawGizmosSelected()
    {
        PopulationManager population = GetComponent<PopulationManager>();
        int botsPerRow = population.botsPerRow;
        float botSquareSpacing = population.botSquareSpacing;
        int populationSize = population.populationSize;

        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Vector3 origin = transform.position;
        for(int i = 0; i < populationSize; i++) {
            Gizmos.DrawCube(new Vector3(origin.x+(i%botsPerRow)*botSquareSpacing,origin.y,origin.z+(i/botsPerRow)*botSquareSpacing), new Vector3(1, 1, 1));
        } 
    }
}
