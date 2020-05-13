using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Diagnostics;

public class Finding : MonoBehaviour
{
    // public Transform seeker;
    // public Transform target;

    // private void Update() {
    //     if(Input.GetButtonDown("Jump"))
    //         FindPath(seeker.position, target.position);
    // }

    PathManager rManager;
    Grid grid;

    // Starting FindPath method (Start position, Target position)
    public void StartFindPath(Vector3 sPos, Vector3 tPos)
    {
        StartCoroutine(FindPath(sPos, tPos));
    }

    private void Awake()
    {
        rManager = GetComponent<PathManager>();
        grid = GetComponent<Grid>();
    }

    IEnumerator FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Vector3[] wPoints = new Vector3[0]; // Create way points
        bool pathS = false; // Is Path Successful?

        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        if (startNode.Walk && targetNode.Walk) // ONLY IF START AND TARGET NODES ARE WALKABLE!!!
        {
            var openSet = new Heap<Node>(grid.MaxSize);
            var closedSet = new HashSet<Node>();

            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                Node currentNode = openSet.RemoveFirst();

                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    pathS = true;
                    break;
                }

                // node -- neighbour's node
                foreach (var node in grid.GetNeighbours(currentNode))
                {
                    if (!node.Walk || closedSet.Contains(node))
                        continue;

                    // new Movement cost to Neighbour
                    int mCost = currentNode.gCost + GetDistance(currentNode, node);
                    if (mCost < node.gCost || !openSet.Contains(node))
                    {
                        node.gCost = mCost;
                        node.hCost = GetDistance(node, targetNode);
                        node.parent = currentNode;

                        if (!openSet.Contains(node))
                        {
                            openSet.Add(node);
                        }
                    }
                }
            }
        }

        yield return null;
        if (pathS)
        {
            wPoints = RetracePath(startNode, targetNode);
        }
        rManager.FinishedProcessingPath(wPoints, pathS);
    }

    Vector3[] RetracePath(Node start, Node end)
    {
        var path = new List<Node>();
        var currentNode = end;

        while (currentNode != start)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        Vector3[] wPoints = SimplifyPath(path);
        Array.Reverse(wPoints);

        return wPoints;
    }

    Vector3[] SimplifyPath(List<Node> path)
    {
        var wPoints = new List<Vector3>();
        var dOld = Vector2.zero; // The OLD direction

        for (int i = 1; i < path.Count; i++)
        {
            // The NEW direction
            Vector2 dNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
            if (!dNew.Equals(dOld))
            {
                wPoints.Add(path[i].wPos);
            }
            dOld = dNew;
        }

        return wPoints.ToArray();
    }

    int GetDistance(Node A, Node B)
    {
        int distX = Mathf.Abs(A.gridX - B.gridX);
        int distY = Mathf.Abs(A.gridY - B.gridY);

        if (distX > distY)
            return 14 * distY + 10 * (distX - distY);

        return 14 * distX + 10 * (distY - distX);
    }
}
