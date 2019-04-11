using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem.Sample;

public class Brain : MonoBehaviour
{
    int DNALength = 12;
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
    public Direction primaryDirection;
    public LockToPoint lockPoint;
    public List<Transform> snapPoints;
    public Vector3 enemyRelativeToPlayer;

    private Vector3 startingPosition;

    private void Start() {
        playerSwordTransform = GameObject.Find("PlayerSwordRoot").GetComponent<Transform>();
        previousSwordPosition = playerSwordTransform.position;
        lockPoint = AISword.GetComponent<LockToPoint>();

        foreach (Transform transform in snapPoints)
        {
            int index = snapPoints.IndexOf(transform);
            transform.Rotate(dna.GetGene(3*index), dna.GetGene(3*index+1), dna.GetGene(3*index+2), Space.Self);
        }

        startingPosition = gameObject.transform.position;
    }

    public void Init() {
        dna = new DNA(DNALength, 360);
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

        swordPositionMoved = playerSwordTransform.position != previousSwordPosition;

        if(swordPositionMoved) {
            enemyRelativeToPlayer = (this.transform.position - playerSwordTransform.position).normalized;
            previousSwordPosition = playerSwordTransform.position;

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

        Vector3 pos = playerSwordTransform.position;
        enemyRelativeToPlayer = (this.transform.position - playerSwordTransform.position).normalized;
        Debug.DrawLine(pos, pos + enemyRelativeToPlayer * 10, Color.red, 2f);

        float verticalRotateRate = 0;
        float horizontalRotateRate = 0;
        float verticalRotationStop = 0;
        float horizontalRotationStop = 0;
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
