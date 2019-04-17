using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceToward : MonoBehaviour
{
    public Transform player;
    public string gameObjectName;
    private void Start() {
        if(!player) {
            player = GameObject.Find(gameObjectName).GetComponent<Transform>();
        }
    }

    private void Update() {
        transform.LookAt(player);   
    }
}
