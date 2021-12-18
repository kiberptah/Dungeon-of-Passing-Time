using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PathfindingNode : IHeapItem<PathfindingNode>
{
    public bool walkable;
    public Vector3 worldPosition;
    public int gridX, gridY;


    public int gCost;
    public int hCost;
    public PathfindingNode parent;
    int heapIndex;


    public PathfindingNode(bool _walkable, Vector3 _worldPos, int _gridX, int _gridY)
    {
        walkable = _walkable;
        worldPosition = _worldPos;

        gridX = _gridX;
        gridY = _gridY;
    }

    public int fCost 
    { get 
        { 
            return gCost + hCost; 
        }
    }

    public int HeapIndex
    {
        get
        {
            return heapIndex;
        }
        set
        {
            heapIndex = value;
        }
    }

    public int CompareTo(PathfindingNode _nodeToCompare)
    {
        int compare = fCost.CompareTo(_nodeToCompare.fCost);
        if (compare == 0)
        {
            compare = hCost.CompareTo(_nodeToCompare.hCost);
        }
        return -compare;

    }

}
