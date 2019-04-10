using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem.Sample;

public class Brain : MonoBehaviour
{
    int DNALength = 8;
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
    public LockToPoint lockPoint;
    public List<Transform> snapPoints;

    private void Awake() {
        playerSwordTransform = GameObject.Find("PlayerSword").GetComponent<Transform>();
        previousSwordPosition = playerSwordTransform.position;
        lockPoint = AISword.GetComponent<LockToPoint>();

        // DEBUG //
        snapPoints.Add(GameObject.Find("North").GetComponent<Transform>());
        snapPoints.Add(GameObject.Find("East").GetComponent<Transform>());
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
        float verticalRotationStop = 0;
        float horizontalRotationStop = 0;
        Vector3 AISwordRotation = AISword.transform.localEulerAngles;
        // float travelSpeed = dna.GetGene(0);

        if (swordPositionMoved)
        {
            if(Mathf.Round(AISwordRotation.y)%360 != verticalRotationStop) {
                if (verticalSwordDirection == Direction.North) {
                    verticalRotateRate = dna.GetGene(0)/180f;
                    verticalRotationStop = dna.GetGene(5);
                }
                else {
                    verticalRotateRate = dna.GetGene(1)/180f;
                    verticalRotationStop = dna.GetGene(6);
                }
            }

            if(Mathf.Round(AISwordRotation.x)%360 != horizontalRotationStop) {
                if (horizontalSwordDirection == Direction.East) {
                    horizontalRotateRate = dna.GetGene(2)/180f;
                    horizontalRotationStop = dna.GetGene(7);
                    lockPoint.snapTo = snapPoints[1];
                }
                else {
                    horizontalRotateRate = dna.GetGene(3)/180f;
                    horizontalRotationStop = dna.GetGene(8);
                }
            }


            if(verticalSwordDirection == Direction.North) {
                lockPoint.snapTo = snapPoints[0];
            } else if(horizontalSwordDirection == Direction.East) {
                lockPoint.snapTo = snapPoints[1];
            }
        }
            
        
        // this.transform.Translate(0,0,travelSpeed * 0.001f);
        AISword.transform.Rotate(horizontalRotateRate,verticalRotateRate,0);
    }
}
