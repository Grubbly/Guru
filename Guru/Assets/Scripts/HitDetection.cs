using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDetection : MonoBehaviour
{
    private UserMotion userMotion;

    private void Awake() {
        userMotion = gameObject.GetComponentInParent<UserMotion>();
        Debug.Log(userMotion.enter);
    }

    private void OnTriggerEnter(Collider other) {
        if(userMotion.enter == "enter") {
            userMotion.enter = gameObject.name;
            Debug.Log("enter set to " + userMotion.enter);
        } else if(userMotion.exit == "exit") {
            userMotion.exit = gameObject.name;
            Debug.Log("exit set to " + userMotion.exit);
        }
    }
}
