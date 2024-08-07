using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterClimbingController : MonoBehaviour
{
    private EnvironmentChecker environmentChecker;
    [SerializeField] private PlayerController playerController;
    ClimbEdge currentEdge;

    private float inOutValue;
    private float upDownValue;
    private float leftRightValue;

    void Awake()
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
                    Debug.Log($"Climbable edge detected at: {climbableInfo.transform.position}");
                    currentEdge = climbableInfo.transform.GetComponent<ClimbEdge>();
                    Debug.Log("Climb Point Found");
                    playerController.SetControl(false);
                    StartCoroutine(ClimbToEdge("Idle To Braced Hang", climbableInfo.transform, 0.39f, 0.45f));
                }
            }
        }
        else
        {
            //edge to edge
            float horizontal = Mathf.Round(Input.GetAxisRaw("Horizontal"));
            float vertical = Mathf.Round(Input.GetAxisRaw("Vertical")); 

            Debug.Log($"Input Direction: {horizontal}, {vertical}");

            var inputDirection = new Vector2(horizontal, vertical);

            if (playerController.playerInAction || inputDirection == Vector2.zero) return;

            var closeEdge = currentEdge.GetCloseEdge(inputDirection);

            if (closeEdge != null) return;
            Debug.Log($"Close edge found in direction: {inputDirection}");
            if (closeEdge.connectionType == ConnectionType.Jump && Input.GetButtonDown("Jump"))
            {
                currentEdge = closeEdge.climbEdge;
                if (closeEdge.edgeDirection.y == 1)
                {
                    StartCoroutine(ClimbToEdge("Braced Hang Hop Up", currentEdge.transform, 0.35f, 0.70f));
                }
                else if (closeEdge.edgeDirection.y == -1)
                {
                    StartCoroutine(ClimbToEdge("Braced Hang Drop", currentEdge.transform, 0.31f, 0.64f));
                }
                else if (closeEdge.edgeDirection.x == 1)
                {
                    StartCoroutine(ClimbToEdge("Braced Hang Hop Right", currentEdge.transform, 0.18f, 0.44f));
                }
                else if (closeEdge.edgeDirection.x == -1)
                {
                    StartCoroutine(ClimbToEdge("Braced Hang Hop Left", currentEdge.transform, 0.18f, 0.50f));
                }
            }
        }
    }

    private IEnumerator ClimbToEdge(string animationName, Transform edgePoint, float compareStartTime, float compareEndTime)
    {
        Debug.Log($"Starting ClimbToEdge coroutine with animation: {animationName}");
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
        Debug.Log("Player is now hanging");
    }

    Vector3 SetHandPosition(Transform edge)
    {
        inOutValue = 0.22f;
        upDownValue = -0.4f;
        leftRightValue = -0.1f;
        return edge.position + edge.forward * inOutValue + Vector3.up * upDownValue - edge.right * leftRightValue;
    }
}
