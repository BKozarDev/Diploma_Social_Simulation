using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PathManager : MonoBehaviour
{
    Queue<PathRequest> pr_Queue = new Queue<PathRequest>(); // Queue of the PathRequest's objects
    PathRequest currentPR; // current PathRequest

    static PathManager instance;
    Finding pFinding;

    bool isProcessingPath;

    private void Awake()
    {
        instance = this;
        pFinding = GetComponent<Finding>();
    }

    // Path to start, Path to move (end) and array of positions
    public static void RequestPath(Vector3 pStart, Vector3 pEnd, Action<Vector3[], bool> callback)
    {
        PathRequest newR = new PathRequest(pStart, pEnd, callback); // Create new Request
        instance.pr_Queue.Enqueue(newR);
        instance.TryProcessNext();
    }

    void TryProcessNext()
    {
        if (!isProcessingPath && pr_Queue.Count > 0)
        {
            currentPR = pr_Queue.Dequeue(); // Take the first item in a queue
            isProcessingPath = true;
            pFinding.StartFindPath(currentPR.pStart, currentPR.pEnd);
        }
    }

    public void FinishedProcessingPath(Vector3[] path, bool success)
    {
        currentPR.callback(path, success);
        isProcessingPath = false;
        TryProcessNext();
    }

    struct PathRequest
    {
        public Vector3 pStart; // Start position
        public Vector3 pEnd; // End position of the path
        public Action<Vector3[], bool> callback; // Array of positions to go through dots of the path

        public PathRequest(Vector3 _s, Vector3 _e, Action<Vector3[], bool> _call)
        {
            pStart = _s;
            pEnd = _e;
            callback = _call;
        }
    }
}
