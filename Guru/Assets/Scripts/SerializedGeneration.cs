using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SerializedGeneration 
{
    public int generation;

    public SerializedGeneration(int gen) {
        generation = gen;
    }
}
