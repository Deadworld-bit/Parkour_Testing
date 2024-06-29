using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentChecker : MonoBehaviour
{
    //position of Raycast
    [SerializeField] private Vector3 rayOffSet = new Vector3(0, 0.2f, 0);
    [SerializeField] private float rayLength = 1f;
    [SerializeField] private float heightRayLength = 7f;
    [SerializeField] private LayerMask obstacleLayer;

    public struct ObstacleInfo
    {
        public bool obstacleFound;
        public bool heightObstacleFound;
        public RaycastHit obstacleInfo;
        public RaycastHit heightObstacleInfo;
    }

    public ObstacleInfo CheckObstacle()
    {
        var hitData = new ObstacleInfo();

        //Using raycast to check for obstacle
        var originRay = transform.position + rayOffSet;
        hitData.obstacleFound = Physics.Raycast(originRay, transform.forward, out hitData.obstacleInfo, rayLength, obstacleLayer);

        //visualize the ray
        Debug.DrawRay(originRay, transform.forward * rayLength, (hitData.obstacleFound) ? Color.red : Color.yellow);

        //Create a vertical raycast to check height
        if (hitData.obstacleFound)
        {
            var heightOrigin = hitData.obstacleInfo.point + Vector3.up * heightRayLength;
            hitData.heightObstacleFound = Physics.Raycast(heightOrigin, Vector3.down, out hitData.heightObstacleInfo, heightRayLength, obstacleLayer);

            Debug.DrawRay(heightOrigin, Vector3.down * heightRayLength, (hitData.heightObstacleFound) ? Color.blue : Color.green);
        }

        return hitData;
    }

}
