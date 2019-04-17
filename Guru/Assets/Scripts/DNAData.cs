using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class DNAData
{
    public List<int> genes = new List<int>();
    public List<float> fGenes = new List<float>();
    public int dnaLength = 0;
    public int maxValue = 0;
    public float score = 0;

    public DNAData(List<int> genes) { }

    public DNAData(List<int> _genes, List<float> _fGenes, int len, int val, float score) {
        genes = _genes;
        fGenes = _fGenes;
        dnaLength = len;
        maxValue = val;   
    }
}
