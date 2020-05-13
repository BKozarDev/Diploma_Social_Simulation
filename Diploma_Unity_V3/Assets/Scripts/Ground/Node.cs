using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node>
{
    public bool Walk;
    public Vector3 wPos;
    public int gridX;
    public int gridY;

    public int gCost;
    public int hCost;
    public Node parent;

    int heapIndex;
    public Node(bool _Walk, Vector3 _wPos, int _gridX, int _gridY)
    {
        Walk = _Walk;
        wPos = _wPos;
        gridX = _gridX;
        gridY = _gridY;
    }

    public int fCost
    {
        get
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

    //Node to Compare Method
    public int CompareTo(Node nToCompare)
    {
        int compare = fCost.CompareTo(nToCompare.fCost);
        if (compare == 0)
        {
            compare = hCost.CompareTo(nToCompare.hCost);
        }
        return -compare;
    }
}
