using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterParkourController : MonoBehaviour
{
    [SerializeField] private EnvironmentChecker environmentChecker;
    [SerializeField] private Animator animator;
    private bool playerInAction;
    [SerializeField] private PlayerController playerController;

    [Header("Parkour Movements")]
    public List<ParkourMovement> newParkourMovement;

    private void Update()
    {
        if (Input.GetButton("Jump") && !playerInAction)
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
                        StartCoroutine(PerformParkourMovemement(movement));
                        break;
                    }
                }
            }
        }
    }

    IEnumerator PerformParkourMovemement(ParkourMovement movement)
    {
        playerInAction = true;
        playerController.SetControl(false);

        animator.CrossFade(movement.AnimationName, 0.2f);
        yield return null;

        var animationState = animator.GetNextAnimatorStateInfo(0);
        if (!animationState.IsName(movement.AnimationName))
        {
            Debug.Log("Animation's name is not match");
        }

        float timeCounter = 0f;
        while (timeCounter <= animationState.length)
        {
            timeCounter += Time.deltaTime;

            //Make the player to look at the obstacle
            if (movement.LookAtObstacle)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, movement.RequiredRotation, playerController.rotationSpeed * Time.deltaTime);
            }

            if (movement.AllowTargetMatching)
            {
                CompareTarget(movement);
            }

            yield return null;
        }

        playerController.SetControl(true);
        playerInAction = false;
    }

    void CompareTarget(ParkourMovement movement)
    {
        animator.MatchTarget(movement.ComparePosition, transform.rotation, movement.CompareBodyPart,
        new MatchTargetWeightMask(new Vector3(0, 1, 0), 0), movement.CompareStartTime, movement.CompareEndTime);
    }
}
