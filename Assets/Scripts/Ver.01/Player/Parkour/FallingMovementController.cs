using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingMovementController : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        animator.GetComponent<PlayerController>().HasPlayerControl = false;
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        animator.GetComponent<PlayerController>().HasPlayerControl = true;
    }
}
