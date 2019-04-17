using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotStats : MonoBehaviour
{
    private Brain agentBrain;
    public TextMesh canvas;
    public BlockingZone blockingZone;
    void Start()
    {
        agentBrain = GetComponent<Brain>();
    }

    // Update is called once per frame
    void Update()
    {
        canvas.text = gameObject.name + " Stats:\nDMG: " + agentBrain.damageTaken
            + "\nAgility: " + agentBrain.agility 
            + "\nElapsed Blocking Time: " + (agentBrain.blockingTime+blockingZone.elapsed) 
            + "\nSword Relative to Agent: " + agentBrain.enemyRelativeToPlayer
            + "\nScore (lower is better): " + (agentBrain.damageTaken+agentBrain.blockingTime+blockingZone.elapsed);
    }
}
