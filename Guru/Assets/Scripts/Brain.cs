using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain : MonoBehaviour
{
    int DNALength = 2;
    public DNA dna;
    public Transform playerSwordPosition;
    public GameObject AISword;
    bool seeWall = true;
    public float MAXHP = 100;
    public float damageTaken = 0;
    bool alive = true; 

    private enum Direction {North, East, South, West};

    private void Awake() {
        playerSwordPosition = GameObject.Find("PlayerSword").GetComponent<Transform>();
    }

    public void Init() {
        dna = new DNA(DNALength, 360);
    }

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag == "dead") {
            damageTaken = MAXHP;
            alive = false;
        }
    }

    private void Update() {
        if(!alive) return;

        if(playerSwordPosition.position.y > 2)
        {
            Debug.Log(playerSwordPosition.position.y);
        }
    }

    private void FixedUpdate() {
        if(!alive) return;

        float rotateRate = 0;
        // float travelSpeed = dna.GetGene(0);

        rotateRate = dna.GetGene(1);
        
        // this.transform.Translate(0,0,travelSpeed * 0.001f);
        AISword.transform.Rotate(0,rotateRate,0);
    }
}
