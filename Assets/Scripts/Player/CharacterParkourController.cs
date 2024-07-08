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
        yield return new WaitForSeconds(animationState.length);

        playerController.SetControl(true);
        playerInAction = false;
    }
}
