using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceToward : MonoBehaviour
{
    public Transform player;

    private void Start() {
        player = GameObject.Find("PlayerSword").GetComponent<Transform>();
    }

    private void Update() {
        transform.LookAt(player);   
    }
}
