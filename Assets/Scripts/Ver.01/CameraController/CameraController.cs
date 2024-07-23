using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera vcam;
    [SerializeField] private float rotationY;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        //Get the current camera's state
        var state = vcam.State;

        //Extract the rotation quaternion from the state 
        var rotation = state.FinalOrientation;
        //Convert the rotation to Euler angles
        var euler = rotation.eulerAngles;
        //Get the y-axis value from the Euler angles
        rotationY = euler.y;
        //Round the rotation y value to the nearest integer
        var roundedRotationY = Mathf.RoundToInt(rotationY);
    }

    public Quaternion flatRotation => Quaternion.Euler(0, rotationY, 0);
}
