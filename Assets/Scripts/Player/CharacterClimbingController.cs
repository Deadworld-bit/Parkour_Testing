using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterClimbingController : MonoBehaviour
{
    [SerializeField] private EnvironmentChecker environmentChecker;
    [SerializeField] private PlayerController playerController;

    private float inOutValue;
    private float upDownValue;
    private float leftRightValue;

    private void Awake()
    {
        environmentChecker = GetComponent<EnvironmentChecker>();
    }

    private void Update()
    {
        if (!playerController.playerHanging)
        {
            if (Input.GetButton("Jump") && !playerController.playerInAction)
            {
                if (environmentChecker.CheckClimbableEdge(transform.forward, out RaycastHit climbableInfo))
                {
                    Debug.Log("Climb Point Found");
                    playerController.SetControl(false);
                    StartCoroutine(ClimbToEdge("Idle To Braced Hang", climbableInfo.transform, 0.47f, 0.54f));
                }
            }
        }
        else
        {
            //Ledge to Ledge
        }
    }

    private IEnumerator ClimbToEdge(string animationName, Transform edgePoint, float compareStartTime, float compareEndTime)
    {
        var compareParams = new CompareTargetParameter()
        {
            position = SetHandPosition(edgePoint),
            bodyPart = AvatarTarget.LeftHand,
            positionWeight = Vector3.one,
            startTime = compareStartTime,
            endTime = compareEndTime,
        };

        var requiredRotation = Quaternion.LookRotation(-edgePoint.forward);
        yield return playerController.PerformMovement(animationName, compareParams, requiredRotation, true);
        playerController.playerHanging = true;
    }

    Vector3 SetHandPosition(Transform edge)
    {
        inOutValue = 0f;
        upDownValue = 0f;
        leftRightValue = 0f;
        return edge.position + edge.forward * inOutValue + Vector3.up * upDownValue - edge.right * leftRightValue;
    }
}
