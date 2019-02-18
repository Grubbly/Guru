using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDetection : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        Debug.Log(gameObject.name + " detected " + other.name);
    }
}
