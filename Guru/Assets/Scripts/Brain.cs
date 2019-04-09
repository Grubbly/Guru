using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain : MonoBehaviour
{
    int DNALength = 4;
    public DNA dna;
    public Transform playerSwordTransform;
    public GameObject AISword;
    public float MAXHP = 100;
    public int damageTaken = 0;
    bool alive = true;
    bool swordPositionMoved = true;
    private Vector3 previousSwordPosition;

    public enum Direction {North, East, South, West};
    public Direction verticalSwordDirection;
    public Direction horizontalSwordDirection;

    private void Awake() {
        playerSwordTransform = GameObject.Find("PlayerSword").GetComponent<Transform>();
        previousSwordPosition = playerSwordTransform.position;
    }

    public void Init() {
        dna = new DNA(DNALength, 360);
    }

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag == "dead") {
            damageTaken += 1;

            if(damageTaken == MAXHP) 
                alive = false;
        }
    }

    private void Update() {
        if(!alive) return;

        swordPositionMoved = playerSwordTransform.position != previousSwordPosition;

        if(swordPositionMoved)
        {
            previousSwordPosition = playerSwordTransform.position;
        }


        if (playerSwordTransform.position.y > 0.6)
        {
            verticalSwordDirection = Direction.North;
        }
        else
        {
            verticalSwordDirection = Direction.South;
        }

        if (playerSwordTransform.position.x > 0)
        {
            horizontalSwordDirection = Direction.West;
        }
        else
        {
            horizontalSwordDirection = Direction.East;
        }
    }

    private void FixedUpdate() {
        if(!alive) return;

        float verticalRotateRate = 0;
        float horizontalRotateRate = 0;
        // float travelSpeed = dna.GetGene(0);

        if (swordPositionMoved)
        {
            if (verticalSwordDirection == Direction.North)
                verticalRotateRate = dna.GetGene(0);
            else
                verticalRotateRate = dna.GetGene(1);

            if (horizontalSwordDirection == Direction.East)
                horizontalRotateRate = dna.GetGene(2);
            else
                horizontalRotateRate = dna.GetGene(3);
        }
            
        
        // this.transform.Translate(0,0,travelSpeed * 0.001f);
        AISword.transform.Rotate(horizontalRotateRate,verticalRotateRate,0);
    }
}
