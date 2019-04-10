using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentStats : MonoBehaviour
{
    public TextMesh canvas;

    public Brain agentBrain;

    private void FixedUpdate() {
        if(!agentBrain)
            agentBrain = GameObject.Find("Bot(Clone)").GetComponent<Brain>();

        canvas.text = "Stats:\nDMG: " + agentBrain.damageTaken;
    }
}
