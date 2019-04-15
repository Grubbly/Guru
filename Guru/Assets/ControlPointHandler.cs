using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPointHandler : MonoBehaviour
{
    public GameObject controlPointPrefab;
    public bool generate = false;
    public Material neutralMaterial;
    public Material highlight;
    public List<GameObject> controlPoints;
    public Transform enemySword;
    public GameObject snapto;
    public GameObject closestControlPoint;

    private void Start() {
        if(!neutralMaterial) {
            neutralMaterial = controlPoints[0].GetComponent<Renderer>().material;
        }
        updateClosestControlPoint();
    }

    public void generateRandomControlPoint() {
        GameObject newControlPoint = Instantiate(controlPointPrefab, this.transform.position + Random.onUnitSphere, Quaternion.Euler(0,0,0));
        newControlPoint.transform.parent = this.transform;
        controlPoints.Add(newControlPoint);
    }

    GameObject GetClosestControlPoint()
    {
        GameObject bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = enemySword.position;
        foreach(GameObject potentialTargetGO in controlPoints)
        {
            Transform potentialTarget = potentialTargetGO.transform;
            Vector3 directionToTarget = potentialTarget.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if(dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTargetGO;
            }
        }
     
        return bestTarget;
    }

    public void updateClosestControlPoint() {
        GameObject newClosestControlPoint = GetClosestControlPoint();
        if(closestControlPoint != newClosestControlPoint) {
            closestControlPoint.GetComponent<Renderer>().material = neutralMaterial;
            closestControlPoint = newClosestControlPoint;
            closestControlPoint.GetComponent<Renderer>().material = highlight;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(generate) {
            generateRandomControlPoint();    
            generate = !generate;
        }
    }
    private void FixedUpdate() {
        updateClosestControlPoint();
    }
}
