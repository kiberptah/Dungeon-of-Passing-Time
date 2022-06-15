using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;

public class Pathfinding : MonoBehaviour
{
    Grid grid;

    private void Awake()
    {
        grid = GetComponent<Grid>();
    }

    public void StartFindPath(Vector3 startPos, Vector3 targetPos, Action<Vector3[], bool> managerCallback)
    {
        StartCoroutine(FindPath(startPos, targetPos, managerCallback));
    }
    IEnumerator FindPath(Vector3 _startPos, Vector3 _targetPos, Action<Vector3[], bool> managerCallback)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        Vector3[] waypoints = new Vector3[0];
        bool pathSucess = false;

        PathfindingNode startNode = grid.NodeFromWorldPoint(_startPos);
        PathfindingNode targetNode = grid.NodeFromWorldPoint(_targetPos);

        if (startNode.walkable && targetNode.walkable && startNode != targetNode)
        {
            Heap<PathfindingNode> openSet = new Heap<PathfindingNode>(grid.GridSize);
            HashSet<PathfindingNode> closeSet = new HashSet<PathfindingNode>();
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                PathfindingNode currentNode = openSet.RemoveFirst();
                closeSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    sw.Stop();
                    pathSucess = true;
                    break;
                }
                foreach (PathfindingNode neighbour in grid.GetNeighbours(currentNode))
                {
                    if (!neighbour.walkable || closeSet.Contains(neighbour))
                    {
                        continue;
                    }
                    int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                    if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = newMovementCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, targetNode);
                        neighbour.parent = currentNode;

                        if (!openSet.Contains(neighbour))
                        {
                            openSet.Add(neighbour);
                        }
                        else
                        {
                            openSet.UpdateItem(neighbour);
                        }
                    }
                }
            }
            if (pathSucess)
            {
                waypoints = RetracePath(startNode, targetNode);
            }
        }
        else
        {
            pathSucess = false;
        }


        if (waypoints.Length == 0)
        {
            // I don't know why it happens, but sometimes length is zero and it breaks therefor here's the fix...
            pathSucess = false;
        }
        //requestManager.FinishedProcessingPath(waypoints, pathSucess);
        //print(waypoints.Length);
        managerCallback(waypoints, pathSucess);


        yield return null;
    }


    Vector3[] RetracePath(PathfindingNode _startNode, PathfindingNode _endNode)
    {
        List<PathfindingNode> path = new List<PathfindingNode>();
        PathfindingNode currentNode = _endNode;

        while (currentNode != _startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        Vector3[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);

        return waypoints;
    }

    Vector3[] SimplifyPath(List<PathfindingNode> path)
    {
        ///Get rid of waypoints that dont chage direction///

        List<Vector3> waypoints = new List<Vector3>();
        Vector3 directionOld = Vector3.zero;

        for (int i = 1; i < path.Count; i++)
        {
            Vector3 directionNew = new Vector3(path[i - 1].gridX - path[i].gridX, 0, path[i - 1].gridY - path[i].gridY);
            if (directionNew != directionOld)
            {
                waypoints.Add(path[i - 1].worldPosition);
            }
            directionOld = directionNew;
        }
        return waypoints.ToArray();
    }
    int GetDistance(PathfindingNode nodeA, PathfindingNode nodeB)
    {

        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);


        if (dstX < 10 || dstY < 10)
        {
            //looks more organic over short distances
            if (dstX > dstY)
            {
                return 14 * dstY + 10 * (dstX - dstY);
            }
            else
            {
                return 14 * dstX + 10 * (dstY - dstX);
            }
        }
        else
        {
            //looks more organic over long distances
            return Mathf.RoundToInt(Mathf.Sqrt(dstX * dstX + dstY * dstY));
        }

        /* 
        // looks better over long open distances
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);


        return Mathf.RoundToInt(Mathf.Sqrt(dstX * dstX + dstY * dstY));
        */
    }

}
