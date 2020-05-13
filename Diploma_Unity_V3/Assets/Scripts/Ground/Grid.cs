using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    //Display only path Gizmos
    public bool displayGizmos;

    public Transform player;
    public List<GameObject> objects;
    private List<Vector3> oldObj;
    public Vector2 gridWS;
    public float nodeR;
    public LayerMask unWalkM;
    Node[,] grids;

    float nodeDiam;
    int gridS_X, gridS_Y;

    private void Awake()
    {
        objects = FindObjectsWithLayer(9);
        oldObj = new List<Vector3>();
        for (int i = 0; i < objects.Count; i++)
        {
            var pos = objects[i].transform.position;
            oldObj.Add(pos);
        }

        nodeDiam = nodeR * 2;
        gridS_X = Mathf.RoundToInt(gridWS.x / nodeDiam);
        gridS_Y = Mathf.RoundToInt(gridWS.y / nodeDiam);

        CreateGrid();
    }

    public int MaxSize
    {
        get
        {
            return gridS_X * gridS_Y;
        }
    }

    public Vector3 GetRandomNode()
    {
        Node randomNode = NodeFromWorldPoint(new Vector3(Random.Range(-gridS_X / 2, gridS_X / 2), 0, Random.Range(-gridS_Y / 2, gridS_Y / 2)));
        while (true)
        {
            foreach (var n in grids)
            {
                if (randomNode == n)
                {
                    if (!n.Walk)
                    {
                        int x = Random.Range(-gridS_X / 2, gridS_X / 2);
                        int y = Random.Range(-gridS_Y / 2, gridS_Y / 2);
                        Vector3 randPos = new Vector3(x, 0, y);
                        randomNode = NodeFromWorldPoint(randPos);
                    }
                    else
                    {
                        return randomNode.wPos;
                    }
                }
            }
        }
    }
    private void Update()
    {
        for (int i = 0; i < objects.Count; i++)
        {
            if (!objects[i].transform.position.Equals(oldObj[i]))
            {
                CreateGrid();
                oldObj[i] = objects[i].transform.position;
            }
        }
    }

    void CreateGrid()
    {
        grids = new Node[gridS_X, gridS_Y];
        Vector3 wBottomLeft = transform.position - Vector3.right * gridWS.x / 2 - Vector3.forward * gridWS.y / 2;

        for (int x = 0; x < gridS_X; x++)
        {
            for (int y = 0; y < gridS_Y; y++)
            {
                Vector3 wPoint = wBottomLeft + Vector3.right * (x * nodeDiam + nodeR) + Vector3.forward * (y * nodeDiam + nodeR);
                bool isWalkable = !(Physics.CheckSphere(wPoint, nodeR, unWalkM));
                grids[x, y] = new Node(isWalkable, wPoint, x, y);
            }
        }
    }

    public List<GameObject> FindObjectsWithLayer(int layer)
    {
        var goArray = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[];

        var trList = new List<GameObject>();

        foreach (var go in goArray)
        {
            if (go.layer == layer)
            {
                trList.Add(go);
            }
        }

        return trList;
    }

    public List<Node> GetNeighbours(Node node)
    {
        var neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridS_X && checkY >= 0 && checkY < gridS_Y)
                {
                    neighbours.Add(grids[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }

    public Node NodeFromWorldPoint(Vector3 wPos)
    {
        float percentX = (wPos.x + gridWS.x / 2) / gridWS.x;
        float percentY = (wPos.z + gridWS.y / 2) / gridWS.y;
        //Give correct position of node
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridS_X - 1) * percentX);
        int y = Mathf.RoundToInt((gridS_Y - 1) * percentY);
        return grids[x, y];
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWS.x, 1, gridWS.y));
        if (grids != null && displayGizmos)
        {
            foreach (Node grid in grids)
            {
                Gizmos.color = (grid.Walk) ? Color.white : Color.red;
                Gizmos.DrawCube(grid.wPos, Vector3.one * (nodeDiam - .1f));
            }
        }
    }
}
