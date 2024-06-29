using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnvironmentChecker;

[CreateAssetMenu(menuName = "Parkour Menu/ Create New Move")]
public class ParkourMovement : ScriptableObject
{
    public string animationName;
    [SerializeField] private float minimumHeight;
    [SerializeField] private float maximumHeight;

    public bool CheckAvailable(ObstacleInfo obstacleInfo, Transform player)
    {
        float checkHeight = obstacleInfo.heightObstacleInfo.point.y - player.position.y;

        if (checkHeight < minimumHeight || checkHeight > maximumHeight)
        {
            return false;
        }
        return true;
    }

    public string AnimationName => animationName;
}
