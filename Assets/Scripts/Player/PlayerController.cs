using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Movement")]
    [SerializeField] private float movementSpeed = 4f;
    [SerializeField] private float rotationSpeed = 500f;
    [SerializeField] private CameraController cameraController;

    [Header("Player Animator")]
    public Animator animator;

    Quaternion requiredRotation;

    private void Update()
    {
        PlayerMovement();
    }

    void PlayerMovement()
    {
        //Access pre setup input manager in unity
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        //Checking if the player has any movement input
        float movementAmount = Mathf.Clamp01(horizontal) + Mathf.Clamp01(vertical);

        var movementInput = (new Vector3(horizontal, 0, vertical)).normalized;
        var movementDirection = cameraController.flatRotation * movementInput;

        if (movementAmount > 0)
        {
            transform.position += movementDirection * movementSpeed * Time.deltaTime;
            requiredRotation = Quaternion.LookRotation(movementDirection);
        }

        transform.rotation = Quaternion.RotateTowards(transform.rotation, requiredRotation, rotationSpeed * Time.deltaTime);

        //Animation
        animator.SetFloat("movementValue", movementAmount, 0.2f, Time.deltaTime);
    }
}
