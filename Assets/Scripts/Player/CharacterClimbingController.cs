using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterClimbingController : MonoBehaviour
{
    private EnvironmentChecker environmentChecker;

    private void Awake()
    {
        environmentChecker = GetComponent<EnvironmentChecker>();
    }

    private void Update()
    {
        if (Input.GetButton("Jump"))
        {
            if (environmentChecker.CheckClimbableEdge(transform.forward, out RaycastHit climbableInfo))
            {
                Debug.Log("Climb Point Found");
            }
        }
    }
}
