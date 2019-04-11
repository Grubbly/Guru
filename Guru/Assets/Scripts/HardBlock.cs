using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardBlock : MonoBehaviour
{
    Animator animator;
    int horizontalBlockHash = Animator.StringToHash("blockHorizontal");

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void block(string id) {
        if(id == "horizontal")
            animator.SetTrigger(horizontalBlockHash);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger(horizontalBlockHash);
        }
    }
}
