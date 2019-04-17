using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem.Sample;

public class Brain : MonoBehaviour
{
    int DNALength = 150;
    public int maxDNAVal = 360;
    public DNA dna;
    public Transform enemySwordTransform;
    public GameObject AISword;
    public float MAXHP = 100;
    public int damageTaken = 0;
    bool alive = true;
    bool swordPositionMoved = true;
    public bool firstGeneration = false;
    private Vector3 previousSwordPosition;

    public enum Direction {North, East, South, West};
    public Direction verticalSwordDirection;
    public Direction horizontalSwordDirection;
    public Direction primaryDirection = Direction.North;
    public LockToPoint lockPoint;
    public Vector3 enemyRelativeToPlayer;
    public Transform snapPoint;
    public float agility;
    public float blockingTime = 0;

    public bool isBestAgent = false;
    private Vector3 startingPosition;
    private ControlPointHandler controlPointHandler;
    

    public void initializeGenes(bool modifying = false) {
        // Need to store these somehow
        // Could send in a bunch of genese instead of using sphere
        // Or store control point position in genes after first generated
        if(isBestAgent)
            return;

        int numAdditionalControlPoints = (dna.GetGene(14)/18);
        for(int controlPointCount = 0; controlPointCount < numAdditionalControlPoints; controlPointCount++) {
            int offset = controlPointCount*3;
            GameObject newControlPoint = Instantiate(
                controlPointHandler.controlPointPrefab, 
                new Vector3(
                    this.transform.position.x + dna.GetFGene(offset), 
                    this.transform.position.y + dna.GetFGene(offset + 1), 
                    this.transform.position.z + dna.GetFGene(offset + 2)), 
                Quaternion.Euler(0,0,0)
            );
            newControlPoint.transform.parent = this.transform;

            if(modifying)
                controlPointHandler.controlPoints[4+controlPointCount] = (newControlPoint);
            else
                controlPointHandler.controlPoints.Add(newControlPoint);
        }

        foreach (GameObject transformGO in controlPointHandler.controlPoints)
        {
            Transform transform = transformGO.transform;
            int index = controlPointHandler.controlPoints.IndexOf(transformGO);
            transform.Rotate(dna.GetGene(3*index), dna.GetGene(3*index+1), dna.GetGene(3*index+2), Space.Self);
        }

        agility = ((float)dna.GetGene(12)/dna.GetGene(13));
        lockPoint.snapTime = agility;
    }

    private void Start() {
        controlPointHandler = GetComponent<ControlPointHandler>();
        previousSwordPosition = enemySwordTransform.position;
        lockPoint = AISword.GetComponent<LockToPoint>();

        initializeGenes();

        startingPosition = gameObject.transform.position;
        snapPoint = controlPointHandler.closestControlPoint.transform;
    }

    public void Init(bool isFirstGen = false) {
        firstGeneration = isFirstGen;
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

            snapPoint = controlPointHandler.closestControlPoint.transform;
        }
    }

    private void FixedUpdate() {
        if(!alive) return;

        Vector3 pos = enemySwordTransform.position;
        enemyRelativeToPlayer = (this.transform.position - enemySwordTransform.position).normalized;
        Debug.DrawLine(pos, pos + enemyRelativeToPlayer * 10, Color.red, 2f);

        Vector3 AISwordRotation = AISword.transform.localEulerAngles;
        lockPoint.snapTo = snapPoint;
    }
}
