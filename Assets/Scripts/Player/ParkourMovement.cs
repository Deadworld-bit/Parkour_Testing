using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using static EnvironmentChecker;

[CreateAssetMenu(menuName = "Parkour Menu/ Create New Move")]
public class ParkourMovement : ScriptableObject
{

    [SerializeField] private string animationName;

    [Header("Obstacle Height")]
    [SerializeField] private float minimumHeight;
    [SerializeField] private float maximumHeight;
    [SerializeField] private string vaultObstacleTag;

    [Header("Player's Rotation")]
    [SerializeField] private bool lookAtObstacle;
    [SerializeField] private float parkourMovementDelay;
    public Quaternion RequiredRotation { get; set; }

    [Header("Target Matching")]
    [SerializeField] private bool allowTargetMatching = true;
    [SerializeField] private AvatarTarget compareBodyPart;
    [SerializeField] private float compareStartTime;
    [SerializeField] private float compareEndTime;
    [SerializeField] private Vector3 comparePositionWeight = new Vector3(0, 1, 0);

    //Variables
    public string AnimationName => animationName;
    public bool LookAtObstacle => lookAtObstacle;
    public bool AllowTargetMatching => allowTargetMatching;
    public AvatarTarget CompareBodyPart => compareBodyPart;
    public float CompareStartTime => compareStartTime;
    public float CompareEndTime => compareEndTime;
    public Vector3 ComparePositionWeight => comparePositionWeight;
    public float ParkourMovementDelay => parkourMovementDelay;

    public Vector3 ComparePosition { get; set; }

    public bool CheckAvailable(ObstacleInfo obstacleData, Transform player)
    {
        if (!string.IsNullOrEmpty(vaultObstacleTag) && obstacleData.obstacleInfo.transform.tag != vaultObstacleTag)
        {
            return false;
        }

        float checkHeight = obstacleData.heightObstacleInfo.point.y - player.position.y;

        if (checkHeight < minimumHeight || checkHeight > maximumHeight)
        {
            return false;
        }

        if (lookAtObstacle)
        {
            RequiredRotation = Quaternion.LookRotation(-obstacleData.obstacleInfo.normal);
        }

        if (allowTargetMatching)
        {
            ComparePosition = obstacleData.heightObstacleInfo.point;
        }

        return true;
    }
}
