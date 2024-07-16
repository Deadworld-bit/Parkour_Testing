using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ClimbEdge : MonoBehaviour
{
    [SerializeField] private List<CloseEdge> closeEdges;

    void Awake()
    {
        var twoWayEdge = closeEdges.Where(n => n.isEdgeTwoWay);
        foreach (var edge in twoWayEdge)
        {
            edge.climbEdge?.CreateEdgeConnection(this, -edge.edgeDirection, edge.connectionType, edge.isEdgeTwoWay);
        }
    }

    public void CreateEdgeConnection(ClimbEdge climbEdge, Vector2 edgeDirection, ConnectionType connectionType, bool isEdgeTwoWay)
    {
        var closeEdge = new CloseEdge()
        {
            climbEdge = climbEdge,
            edgeDirection = edgeDirection,
            connectionType = connectionType,
            isEdgeTwoWay = isEdgeTwoWay
        };

        closeEdges.Add(closeEdge);
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position, transform.forward, Color.blue);
        foreach (var edge in closeEdges)
        {
            if (edge.climbEdge != null)
            {
                Debug.DrawLine(transform.position, edge.climbEdge.transform.position, (edge.isEdgeTwoWay) ? Color.white : Color.black);
            }
        }
    }
}

[System.Serializable]
public class CloseEdge
{
    public ClimbEdge climbEdge;
    public Vector2 edgeDirection;
    public ConnectionType connectionType;
    public bool isEdgeTwoWay = true;
}

public enum ConnectionType { Jump, Move }
