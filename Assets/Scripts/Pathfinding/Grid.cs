using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Grid : MonoBehaviour
{
    public static Grid instance;

    PathfindingNode[,] grid;
    public Vector2 gridWorldSize;

    public float nodeDiameter = 1;
    public float nodeScanScale = 2f;

    public LayerMask unwalkableMask;


    float nodeRadius;
    int gridSizeX, gridSizeY;
    public bool displayGridGizmos;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }


        nodeRadius = nodeDiameter * 0.5f;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }
    void CreateGrid()
    {
        grid = new PathfindingNode[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2;

        for (int x = 0; x < gridSizeX; ++x)
        {
            for (int y = 0; y < gridSizeY; ++y)
            {
                /*  Vector3 worldPoint = worldBottomLeft
                     + Vector3.right * (x * nodeDiameter + nodeRadius)
                     + Vector3.up * (y * nodeDiameter + nodeRadius); */
                Vector3 worldPoint = worldBottomLeft
                    + Vector3.right * x * nodeDiameter + Vector3.right * nodeRadius
                    + Vector3.up * y * nodeDiameter + Vector3.up * nodeRadius;

                bool walkable = !Physics2D.OverlapBox(worldPoint, new Vector2(nodeDiameter * nodeScanScale, nodeDiameter * nodeScanScale), 0, unwalkableMask.value);
                grid[x, y] = new PathfindingNode(walkable, worldPoint, x, y);
                //Debug.Log("grid[" + x + "," + y + "]: " + worldPoint);
            }
        }
    }
    public int GridSize
    {
        get
        {
            return gridSizeX * gridSizeY;
        }
    }
    public List<PathfindingNode> GetNeighbours(PathfindingNode node)
    {
        List<PathfindingNode> neighbours = new List<PathfindingNode>();

        for (int x = -1; x <= 1; ++x)
        {
            for (int y = -1; y <= 1; ++y)
            {
                if (x == 0 && y == 0)
                {
                    continue;
                }
                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }

    public PathfindingNode NodeFromWorldPoint(Vector3 _worldPosition)
    {
        /*
        float percentX = (_worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (_worldPosition.y + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        */


        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2;

        int x = Mathf.FloorToInt(Mathf.Abs((_worldPosition - worldBottomLeft).x) / nodeDiameter);
        int y = Mathf.FloorToInt(Mathf.Abs((_worldPosition - worldBottomLeft).y) / nodeDiameter);

        //Debug.Log("grid["+x+","+y+"]");
        return grid[x, y];
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y));
        if (grid != null && displayGridGizmos)
        {
            foreach (PathfindingNode n in grid)
            {
                Color currentColor = (n.walkable) ? Color.white : Color.red;


                currentColor.a = 0.2f;
                Gizmos.color = currentColor;

                if (!n.walkable)
                {
                    Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter * 0.9f));
                }

            }
        }
    }

}
