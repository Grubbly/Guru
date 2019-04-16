using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DNA
{
    public List<int> genes = new List<int>();
    public List<float> fGenes = new List<float>();
    public int dnaLength = 0;
    public int maxValue = 0;

    public DNA(List<int> _genes, List<float> _fGenes, int len, int val) {
        genes = _genes;
        fGenes = _fGenes;
        dnaLength = len;
        maxValue = val;   
    }

    public DNA(int length, int value) {
        dnaLength = length;
        maxValue = value;
        SetRandom();
    }

    public void SetRandom() {
        genes.Clear();
        fGenes.Clear();
        for(int i = 0; i < dnaLength; i++) {
            genes.Add(Random.Range(0, maxValue));
            fGenes.Add(Random.Range(-maxValue, maxValue)/(1f*maxValue));
        }
    }

    public void SetGene(int genePosition, int value) {
        genes[genePosition] = value;
    }

    public void SetFGene(int genePosition, float value) {
        fGenes[genePosition] = value;
    }

    public void Crossover(DNA d1, DNA d2) {
        for(int i = 0; i < dnaLength; i++) {
            if(i < dnaLength/2.0) {
                int chromosome = d1.genes[i];
                float fChromosome = d1.fGenes[i];
                genes[i] = chromosome;
                fGenes[i] = fChromosome;
            } else {
                int chromosome = d2.genes[i];
                float fChromosome = d2.fGenes[i];
                genes[i] = chromosome;
                fGenes[i] = fChromosome;
            }
        }
    }

    public void Mutate() {
        genes[Random.Range(0,dnaLength)] = Random.Range(0, maxValue);
    }

    public void FMutate() {
        fGenes[Random.Range(0,dnaLength)] = Random.Range(-maxValue, maxValue)/maxValue;
    }

    public int GetGene(int genePosition) {
        return genes[genePosition];
    }

    public float GetFGene(int genePosition) {
        return fGenes[genePosition];
    }
}
