﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SwordMotionReproducer : MonoBehaviour
{
    public Vector3[] originalMovementPoints;
    public bool startMoving = false;
    public Rigidbody sword;

    public int movementCounter = 0;
    
    private Vector3 lastPosition;
    private Vector3 startPosition;

    private void Start() {
        startPosition = transform.position;
        lastPosition = startPosition;
        sword = GetComponent<Rigidbody>();
    }

    public void Init() {
        transform.position = new Vector3(originalMovementPoints[0].x, originalMovementPoints[0].y, transform.position.z);
        startPosition = transform.position;
        lastPosition = startPosition;
        startMoving = true;
    }

    void FixedUpdate()
    {
        if(startMoving) {
            if(movementCounter < originalMovementPoints.Length-1) {

                if(originalMovementPoints[movementCounter] == Vector3.zero) {
                    lastPosition = startPosition;
                    movementCounter = 0;
                    startMoving = false;
                }

                Vector3 nextPosition = (lastPosition + (originalMovementPoints[movementCounter+1] - originalMovementPoints[movementCounter]));
                sword.MovePosition(nextPosition + transform.forward * Time.deltaTime);
                movementCounter++;
                lastPosition = nextPosition;
            }
        } 
    }
}
