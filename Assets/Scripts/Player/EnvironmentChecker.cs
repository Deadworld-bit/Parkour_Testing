using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentChecker : MonoBehaviour
{
    //position of Raycast
    [SerializeField] private Vector3 rayOffSet = new Vector3(0, 0.2f, 0);
    [SerializeField] private float rayLength = 1f;
    [SerializeField] private LayerMask obstacleLayer;

    public void CheckObstacle()
    {
        //Using raycast to check for obstacle
        var originRay = transform.position + rayOffSet;
        bool obstacleFound = Physics.Raycast(originRay, transform.forward, out RaycastHit hitInfo, rayLength, obstacleLayer);

        //visualize the ray
        Debug.DrawRay(originRay, transform.forward * rayLength, (obstacleFound) ? Color.red : Color.yellow);
    }
}
