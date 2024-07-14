using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnvironmentChecker : MonoBehaviour
{
    [Header("Check Height and Obstacle")]
    //position of Raycast
    [SerializeField] private Vector3 rayOffSet = new Vector3(0, 0.2f, 0);
    [SerializeField] private float rayLength = 1f;
    [SerializeField] private float heightRayLength = 7f;
    [SerializeField] private LayerMask obstacleLayer;

    [Header("Check Obstacle's Ledge")]
    [SerializeField] private float ledgeRayLength = 10f;
    [SerializeField] private float ledgeRayHeightThreshold = 0.75f;

    [Header("Check Climbable Edge")]
    [SerializeField] private float climbableRayLength = 1.5f;
    [SerializeField] private LayerMask climbableLayer;
    [SerializeField] private int numberOfRays = 12;

    public struct ObstacleInfo
    {
        public bool obstacleFound;
        public bool heightObstacleFound;
        public RaycastHit obstacleInfo;
        public RaycastHit heightObstacleInfo;
    }

    public struct LedgeInfo
    {
        public float angle;
        public float height;
        public RaycastHit groundHit;
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

    public bool CheckLedge(Vector3 movementDirection, out LedgeInfo ledgeInfo)
    {
        ledgeInfo = new LedgeInfo();

        if (movementDirection == Vector3.zero)
        {
            return false;
        }

        float ledgeOriginalOffset = 0.5f;
        var ledgeOrigin = transform.position + movementDirection * ledgeOriginalOffset + Vector3.up;

        if (Physics.Raycast(ledgeOrigin, Vector3.down, out RaycastHit hit, ledgeRayLength, obstacleLayer))
        {
            Debug.DrawRay(ledgeOrigin, Vector3.down * ledgeRayLength, Color.blue);

            var groundRayCastOrigin = transform.position + movementDirection - new Vector3(0, 0.1f, 0);
            if (Physics.Raycast(groundRayCastOrigin, -movementDirection, out RaycastHit groundHit, 2, obstacleLayer))
            {
                float ledgeHeight = transform.position.y - hit.point.y;
                if (ledgeHeight > ledgeRayHeightThreshold)
                {
                    ledgeInfo.angle = Vector3.Angle(transform.forward, groundHit.normal);
                    ledgeInfo.height = ledgeHeight;
                    ledgeInfo.groundHit = groundHit;
                    return true;
                }
            }
        }
        return false;
    }

    public bool CheckClimbableEdge(Vector3 climbDirection, out RaycastHit climbableInfo)
    {
        climbableInfo = new RaycastHit();
        if (climbDirection == Vector3.zero)
        {
            return false;
        }

        var climbOrigin = transform.position + Vector3.up * 1.5f;
        var climbOffset = new Vector3(0, 0.19f, 0);

        for (int i = 0; i < numberOfRays; i++)
        {
            Debug.DrawRay(climbOrigin + climbOffset * i, climbDirection, Color.blue);
            if (Physics.Raycast(climbOrigin + climbOffset * i, climbDirection, out RaycastHit climbHit, climbableRayLength, climbableLayer))
            {
                climbableInfo = climbHit;
                return true;
            }
        }
        return false;
    }
}
