using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using static EnvironmentChecker;

public class PlayerController : MonoBehaviour
{
    [Header("Player Movement")]
    [SerializeField] private float movementSpeed = 4f;
    public float rotationSpeed = 500f;
    [SerializeField] private EnvironmentChecker environmentChecker;
    [SerializeField] private CameraController cameraController;
    Quaternion requiredRotation;
    public bool playerControl = true;

    [Header("Player Animator")]
    public Animator animator;

    [Header("Player Collision & Gravity")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] Vector3 groundCheckOffset;
    [SerializeField] LayerMask groundLayer;
    private bool onGround;

    public bool playerOnLedge { get; set; }
    public LedgeInfo LedgeInfo { get; set; }
    [SerializeField] private float gravity;
    [SerializeField] private Vector3 moveDir;
    [SerializeField] private Vector3 requiredMoveDir;
    private Vector3 velocity;

    private void Update()
    {
        PlayerMovement();
        if (!playerControl)
        {
            return;
        }
        velocity = Vector3.zero;

        if (onGround)
        {
            gravity = 0f;
            velocity = moveDir * movementSpeed;
            playerOnLedge = environmentChecker.CheckLedge(moveDir, out LedgeInfo ledgeInfo);

            if (playerOnLedge)
            {
                LedgeInfo = ledgeInfo;
                playerLedgeMovement();
                Debug.Log("Player is on ledge");
            }

            // //Animation
            // animator.SetFloat("movementValue", velocity.magnitude / movementSpeed, 0.2f, Time.deltaTime);
        }
        else
        {
            //Gravity method 
            gravity += Physics.gravity.y * Time.deltaTime;
            velocity = transform.forward * movementSpeed / 2;
        }

        velocity.y = gravity;

        GroundCheck();
        animator.SetBool("onGround", onGround);
        Debug.Log("OnGround: " + onGround);
    }

    void PlayerMovement()
    {
        //Access pre setup input manager in unity
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        //Checking if the player has any movement input
        float movementAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));

        var movementInput = (new Vector3(horizontal, 0, vertical)).normalized;
        requiredMoveDir = cameraController.flatRotation * movementInput;

        characterController.Move(velocity * Time.deltaTime);
        if (movementAmount > 0 && moveDir.magnitude > 0.2f)
        {
            requiredRotation = Quaternion.LookRotation(moveDir);
        }

        moveDir = requiredMoveDir;

        transform.rotation = Quaternion.RotateTowards(transform.rotation, requiredRotation, rotationSpeed * Time.deltaTime);

        //Animation
        animator.SetFloat("movementValue", movementAmount, 0.2f, Time.deltaTime);
    }

    void GroundCheck()
    {
        //create a sphere to check if the character is on the ground
        onGround = Physics.CheckSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius, groundLayer);
    }

    void playerLedgeMovement()
    {
        float angle = Vector3.Angle(LedgeInfo.groundHit.normal, requiredMoveDir);
        if (angle < 90)
        {
            velocity = Vector3.zero;
            moveDir = Vector3.zero;
        }
    }

    //Visualize the sphere
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius);
    }

    public void SetControl(bool hasControl)
    {
        this.playerControl = hasControl;
        characterController.enabled = hasControl;

        if (!hasControl)
        {
            animator.SetFloat("movementValue", 0f);
            requiredRotation = transform.rotation;
        }
    }

    public bool HasPlayerControl
    {
        get => playerControl;
        set => playerControl = value;
    }
}
