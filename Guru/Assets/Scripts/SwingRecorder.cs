using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Proyecto26;
using UnityEngine.Networking;

public class SwingRecorder : MonoBehaviour
{
    public bool repeatSwingsForever = false;
    public GameObject sword;
    public const int MAX_TRAIL_POSITIONS = 1000;
    public TrailRenderer trailRenderer;
    public Vector3[] swordPath = new Vector3[MAX_TRAIL_POSITIONS];
    public List<SwordMotionReproducer> swordMotionReproducers;
    public Material swordInsideColor;

    private Material idleColor;

    private string api = "https://guru-base.firebaseio.com";

    private void Start() {
        sword = GameObject.Find("PlayerSword");
        trailRenderer = sword.GetComponentInChildren<TrailRenderer>();
        trailRenderer.emitting = false;
        idleColor = GetComponent<Renderer>().material;

        // Special debug item
        swordMotionReproducers.Add(GameObject.Find("Replay").GetComponentInChildren<SwordMotionReproducer>());
    }

    public void distributeSlashDataToAgents() {
        foreach(SwordMotionReproducer swordMotionReproducer in swordMotionReproducers) {
            swordMotionReproducer.originalMovementPoints = swordPath;
            
            try{
                swordMotionReproducer.Init();
            } catch {
                Debug.Log("Error in swordMotionReproducer.Init()");
            }

            if(repeatSwingsForever)
                swordMotionReproducer.repeatSwingForever = true;
            else
                swordMotionReproducer.repeatSwingForever = false;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "dead") {
            trailRenderer.Clear();
            swordPath = new Vector3[MAX_TRAIL_POSITIONS];
            GetComponent<Renderer>().material = swordInsideColor;
            trailRenderer.emitting = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.tag == "dead") {
            trailRenderer.GetPositions(swordPath);
            GetComponent<Renderer>().material = idleColor;
            trailRenderer.emitting = false;
            distributeSlashDataToAgents();

            RestClient.Put(api+"/slashVectors.json", new SlashVectorData(swordPath));
        }
    }
}
