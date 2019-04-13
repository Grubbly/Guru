using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem.Sample;

public class Brain : MonoBehaviour
{
    int DNALength = 14;
    public int maxDNAVal = 360;
    public DNA dna;
    public Transform enemySwordTransform;
    public GameObject AISword;
    public float MAXHP = 100;
    public int damageTaken = 0;

    public bool playerIsEnemy = false;
    bool alive = true;
    bool swordPositionMoved = true;
    private Vector3 previousSwordPosition;

    public enum Direction {North, East, South, West};
    public Direction verticalSwordDirection;
    public Direction horizontalSwordDirection;
    public Direction primaryDirection;
    public LockToPoint lockPoint;
    public List<Transform> snapPoints;
    public Vector3 enemyRelativeToPlayer;

    private Vector3 startingPosition;

    private void Start() {
        if(playerIsEnemy) {
            enemySwordTransform = GameObject.Find("PlayerSwordRoot").GetComponent<Transform>();
        }
        previousSwordPosition = enemySwordTransform.position;
        lockPoint = AISword.GetComponent<LockToPoint>();

        foreach (Transform transform in snapPoints)
        {
            int index = snapPoints.IndexOf(transform);
            transform.Rotate(dna.GetGene(3*index), dna.GetGene(3*index+1), dna.GetGene(3*index+2), Space.Self);
        }
        lockPoint.snapTime = ((float)dna.GetGene(12)/dna.GetGene(13));

        startingPosition = gameObject.transform.position;
    }

    public void Init() {
        dna = new DNA(DNALength, maxDNAVal);
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "dead") {
            damageTaken += 1;

            if(damageTaken == MAXHP) 
                alive = false;
        }
    }

    private void Update() {
        if(!alive) return;

        swordPositionMoved = enemySwordTransform.position != previousSwordPosition;

        if(swordPositionMoved) {
            enemyRelativeToPlayer = (this.transform.position - enemySwordTransform.position).normalized;
            previousSwordPosition = enemySwordTransform.position;

            if (enemyRelativeToPlayer.y < 0)
                verticalSwordDirection = Direction.North;
            else
                verticalSwordDirection = Direction.South;

            if (enemyRelativeToPlayer.x < 0)
                horizontalSwordDirection = Direction.West;
            else
                horizontalSwordDirection = Direction.East;
            

            if(Mathf.Abs(enemyRelativeToPlayer.x) > Mathf.Abs(enemyRelativeToPlayer.y))
                primaryDirection = horizontalSwordDirection;
            else
                primaryDirection = verticalSwordDirection;
        }
    }

    private void FixedUpdate() {
        if(!alive) return;

        Vector3 pos = enemySwordTransform.position;
        enemyRelativeToPlayer = (this.transform.position - enemySwordTransform.position).normalized;
        Debug.DrawLine(pos, pos + enemyRelativeToPlayer * 10, Color.red, 2f);

        Vector3 AISwordRotation = AISword.transform.localEulerAngles;
        if (swordPositionMoved)
        {
            if(primaryDirection == Direction.North) {
                lockPoint.snapTo = snapPoints[0];
            } else if(primaryDirection == Direction.South) {
                lockPoint.snapTo = snapPoints[1];
            } else if(primaryDirection == Direction.East) {
                lockPoint.snapTo = snapPoints[2];
            } else if(primaryDirection == Direction.West) {
                lockPoint.snapTo = snapPoints[3];
            }
        }
    }
}
