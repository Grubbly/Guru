using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotStats : MonoBehaviour
{
    private Brain agentBrain;
    public TextMesh canvas;
    void Start()
    {
        agentBrain = GetComponent<Brain>();
    }

    // Update is called once per frame
    void Update()
    {
        canvas.text = gameObject.name + " Stats:\nDMG: " + agentBrain.damageTaken + "\nVertical Direction: " 
            + agentBrain.verticalSwordDirection + "\nHorizontal Direction: " + agentBrain.horizontalSwordDirection
            + "\nPrimary Direction: " + agentBrain.primaryDirection + "\nSword Relative to Agent: " + agentBrain.enemyRelativeToPlayer;
    }
}
