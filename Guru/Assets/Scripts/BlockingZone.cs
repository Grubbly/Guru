using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockingZone : MonoBehaviour
{
    public float thrust = 5f;
    public SwordMotionReproducer swordMotionReproducer;
    public float elapsed = 0;
    public float totalElapsed = 0;

    public Brain brain;

    private void OnTriggerEnter(Collider other) {
        // Debug.Log(gameObject + " Collision entered with " + other.gameObject);
        if(other.tag == "dead") {
            swordMotionReproducer.startMoving = false;
        }
    }

    private void countBlockingTime(){
        totalElapsed += elapsed;
        brain.blockingTime = totalElapsed;
        elapsed = 0;
    }
    private void OnDestroy() {
        countBlockingTime();
    }

    private void Update() {
        if(swordMotionReproducer.startMoving) {
            elapsed += Time.deltaTime;
        } else {
            if(elapsed > 0) {
                countBlockingTime();
            }
        }
    }

}
