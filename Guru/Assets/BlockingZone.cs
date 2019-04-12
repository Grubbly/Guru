using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockingZone : MonoBehaviour
{
    public GameObject enemyWeapon;
    public float thrust = 5f;
    private SwordMotionReproducer swordMotionReproducer;

    private void Start() {
        swordMotionReproducer = enemyWeapon.GetComponent<SwordMotionReproducer>();
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log(gameObject + " Collision entered with " + other.gameObject);
        if(other.tag == "dead") {
            swordMotionReproducer.startMoving = false;
            enemyWeapon.GetComponent<Rigidbody>().AddForce(-enemyWeapon.transform.forward * thrust);
        }
    }

}
