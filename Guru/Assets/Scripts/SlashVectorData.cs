using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SlashVectorData
{
    public Vector3[] slashVectors;

    public SlashVectorData() { }

    public SlashVectorData(Vector3[] slashes) {
        
        int SIZE = 0;
        for(int i = 0; i < 1000; i++)
        {
            if(slashes[i] == Vector3.zero)
                break;

            SIZE++;
        }
        
        slashVectors = new Vector3[SIZE];

        for(int i = 0; i < SIZE; i++) {
            if(slashes[i] == Vector3.zero) 
                break; 

            slashVectors[i] = (slashes[i]);
        }
    }
}