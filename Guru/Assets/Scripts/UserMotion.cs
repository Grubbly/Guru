using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserMotion : MonoBehaviour
{
    public GameObject AILeftArm;
    private HardBlock hardBlock;
    public string enter; 
    public string exit;

    private void Awake() {
        enter = "enter";
        exit = "exit";
        hardBlock = AILeftArm.GetComponent<HardBlock>();
    }

    private void Update() {
        if(enter != "enter" && exit != "exit") {
            if((enter=="East" && exit=="West") || (enter=="West" && exit=="East"))
                hardBlock.block("horizontal");

            enter = "enter";
            exit = "exit";
            Debug.Log("UserMotion entry and exit points reset");
        }
    }
    
}
