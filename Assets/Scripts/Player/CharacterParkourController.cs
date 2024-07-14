using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterParkourController : MonoBehaviour
{
    [SerializeField] private EnvironmentChecker environmentChecker;
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private ParkourMovement jumpDown1;

    [Header("Parkour Movements")]
    public List<ParkourMovement> newParkourMovement;

    private void Update()
    {
        if (Input.GetButton("Jump") && !playerController.playerInAction)
        {
            var hitData = environmentChecker.CheckObstacle();

            if (hitData.obstacleFound)
            {
                Debug.Log("Obejct Founded" + hitData.obstacleInfo.transform.name);
                foreach (var movement in newParkourMovement)
                {
                    if (movement.CheckAvailable(hitData, transform))
                    {
                        //perform parkour action
                        StartCoroutine(PerformParkourMovement(movement));
                        break;
                    }
                }
            }
        }

        if (playerController.playerOnLedge && !playerController.playerInAction && Input.GetButtonDown("Jump"))
        {
            if (playerController.LedgeInfo.angle <= 50)
            {
                playerController.playerOnLedge = false;
                StartCoroutine(PerformParkourMovement(jumpDown1));
            }
        }
    }

    IEnumerator PerformParkourMovement(ParkourMovement movement)
    {
        playerController.SetControl(false);

        CompareTargetParameter compareTargetParameter = null;
        if (movement.AllowTargetMatching)
        {
            compareTargetParameter = new CompareTargetParameter()
            {
                position = movement.ComparePosition,
                bodyPart = movement.CompareBodyPart,
                positionWeight = movement.ComparePositionWeight,
                startTime = movement.CompareStartTime,
                endTime = movement.CompareEndTime,
            };
        }

        yield return playerController.PerformMovement(movement.AnimationName, compareTargetParameter, movement.RequiredRotation, movement.LookAtObstacle,
        movement.ParkourMovementDelay);

        playerController.SetControl(true);
    }

    void CompareTarget(ParkourMovement movement)
    {
        animator.MatchTarget(movement.ComparePosition, transform.rotation, movement.CompareBodyPart,
        new MatchTargetWeightMask(movement.ComparePositionWeight, 0), movement.CompareStartTime, movement.CompareEndTime);
    }
}
