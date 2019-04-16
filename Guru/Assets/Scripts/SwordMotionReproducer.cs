using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SwordMotionReproducer : MonoBehaviour
{
    public Vector3[] originalMovementPoints;
    public bool startMoving = false;
    public Rigidbody sword;

    public int movementCounter = 0;
    public bool repeatSwingForever = false;
    private Vector3 lastPosition;
    private Vector3 startPosition;

    public Brain brain;
    public Transform resetPosition;

    private Vector3 rootPosition;

    private void Start() {
        SwingRecorder swingRecorder = GameObject.Find("TrainingDummy").GetComponentInChildren<SwingRecorder>();
        swingRecorder.swordMotionReproducers.Add(GetComponent<SwordMotionReproducer>());
        originalMovementPoints = swingRecorder.swordPath;
        
        rootPosition = transform.position;
        Init();
        
        sword = GetComponent<Rigidbody>();
    }

    public void Init() {
        transform.position = new Vector3(rootPosition.x+originalMovementPoints[0].x, originalMovementPoints[0].y, rootPosition.z);
        startPosition = transform.position;
        lastPosition = startPosition;
        startMoving = true;
        repeatSwingForever = true;
    }

    void FixedUpdate()
    {
        if(movementCounter < originalMovementPoints.Length-1) {
            if(originalMovementPoints[movementCounter] == Vector3.zero) {
                    lastPosition = startPosition;
                    movementCounter = 0;
                    startMoving = true;

                    if(!repeatSwingForever)
                        startMoving = false;
            }

            if(startMoving) {
                Vector3 nextPosition = (lastPosition + (originalMovementPoints[movementCounter+1] - originalMovementPoints[movementCounter]));
                sword.MovePosition(nextPosition + transform.forward * Time.deltaTime);
                lastPosition = nextPosition;
            }
            movementCounter++;
        } 
    }
}
