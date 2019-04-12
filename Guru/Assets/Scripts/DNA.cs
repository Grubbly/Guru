using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DNA
{
    List<int> genes = new List<int>();
    int dnaLength = 0;
    int maxValue = 0;

    public DNA(int length, int value) {
        dnaLength = length;
        maxValue = value;
        SetRandom();
    }

    public void SetRandom() {
        genes.Clear();
        for(int i = 0; i < dnaLength; i++) {
            genes.Add(Random.Range(0, maxValue));
        }
    }

    public void SetInt(int genePosition, int value) {
        genes[genePosition] = value;
    }

    public void Crossover(DNA d1, DNA d2) {
        for(int i = 0; i < dnaLength; i++) {
            if(i < dnaLength/2.0) {
                int chromosome = d1.genes[i];
                genes[i] = chromosome;
            } else {
                int chromosome = d2.genes[i];
                genes[i] = chromosome;
            }
        }
    }

    public void Mutate() {
        genes[Random.Range(0,dnaLength)] = Random.Range(0, maxValue);
    }

    public int GetGene(int genePosition) {
        return genes[genePosition];
    }
}
