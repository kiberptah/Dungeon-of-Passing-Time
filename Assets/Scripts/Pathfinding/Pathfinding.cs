using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;

public class Pathfinding : MonoBehaviour
{
    PathRequestManager requestManager;
    Grid grid;

    private void Awake()
    {
        requestManager = GetComponent<PathRequestManager>();
        grid = GetComponent<Grid>();
    }

    public void StartFindPath(Vector3 startPos, Vector3 targetPos)
    {
        StartCoroutine(FindPath(startPos, targetPos));
    }
    IEnumerator FindPath(Vector3 _startPos, Vector3 _targetPos)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        Vector3[] waypoints = new Vector3[0];
        bool pathSucess = false;

        Node startNode = grid.NodeFromWorldPoint(_startPos);
        Node targetNode = grid.NodeFromWorldPoint(_targetPos);

        /// ??? ??? ??? 
        if (!startNode.walkable || !targetNode.walkable)
        {
            /*
            startNode.walkable = true;
            targetNode.walkable = true;
            */
        }

        if (startNode.walkable && targetNode.walkable)
        {
            Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
            HashSet<Node> closeSet = new HashSet<Node>();
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                Node currentNode = openSet.RemoveFirst();
                closeSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    sw.Stop();
                    //print("Path found: " + sw.ElapsedMilliseconds + " ms");
                    pathSucess = true;
                    //pathSucess = false;
                    break;
                }
                foreach (Node neighbour in grid.GetNeighbours(currentNode))
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


        
        requestManager.FinishedProcessingPath(waypoints, pathSucess);

        yield return null;
    }


    Vector3[] RetracePath(Node _startNode, Node _endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = _endNode;

        while (currentNode != _startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        Vector3[] waypoints = SimplifyPath(path);    
        Array.Reverse(waypoints);

        return waypoints;
    }

    Vector3[] SimplifyPath(List<Node> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        for (int i = 1; i < path.Count; i++)
        {
            Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
            if (directionNew != directionOld)
            {
                waypoints.Add(path[i - 1].worldPosition);
            }
            directionOld = directionNew;
        }
        return waypoints.ToArray();
    }
    int GetDistance(Node nodeA, Node nodeB)
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
