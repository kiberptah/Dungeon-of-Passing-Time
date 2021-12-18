using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathfindingExample : MonoBehaviour
{

	public Transform seeker, target;
	GridExample grid;

	void Awake()
	{
		grid = GetComponent<GridExample>();
	}

	void Update()
	{
		FindPath(seeker.position, target.position);
	}

	void FindPath(Vector3 startPos, Vector3 targetPos)
	{
		PathfindingNode startNode = grid.NodeFromWorldPoint(startPos);
		PathfindingNode targetNode = grid.NodeFromWorldPoint(targetPos);

		List<PathfindingNode> openSet = new List<PathfindingNode>();
		HashSet<PathfindingNode> closedSet = new HashSet<PathfindingNode>();
		openSet.Add(startNode);

		while (openSet.Count > 0)
		{
			PathfindingNode node = openSet[0];
			for (int i = 1; i < openSet.Count; i++)
			{
				if (openSet[i].fCost < node.fCost || openSet[i].fCost == node.fCost)
				{
					if (openSet[i].hCost < node.hCost)
						node = openSet[i];
				}
			}

			openSet.Remove(node);
			closedSet.Add(node);

			if (node == targetNode)
			{
				RetracePath(startNode, targetNode);
				return;
			}

			foreach (PathfindingNode neighbour in grid.GetNeighbours(node))
			{
				if (!neighbour.walkable || closedSet.Contains(neighbour))
				{
					continue;
				}

				int newCostToNeighbour = node.gCost + GetDistance(node, neighbour);
				if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
				{
					neighbour.gCost = newCostToNeighbour;
					neighbour.hCost = GetDistance(neighbour, targetNode);
					neighbour.parent = node;

					if (!openSet.Contains(neighbour))
						openSet.Add(neighbour);
				}
			}
		}
	}

	void RetracePath(PathfindingNode startNode, PathfindingNode endNode)
	{
		List<PathfindingNode> path = new List<PathfindingNode>();
		PathfindingNode currentNode = endNode;

		while (currentNode != startNode)
		{
			path.Add(currentNode);
			currentNode = currentNode.parent;
		}
		path.Reverse();

		grid.path = path;

	}

	int GetDistance(PathfindingNode nodeA, PathfindingNode nodeB)
	{
		int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
		int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

		if (dstX > dstY)
			return 14 * dstY + 10 * (dstX - dstY);
		return 14 * dstX + 10 * (dstY - dstX);
	}
}