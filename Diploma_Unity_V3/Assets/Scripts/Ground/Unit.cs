using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public Transform target;
    float speed = 10;
    Vector3[] path;
    int tIndex; // Target index
    float threadhold = 0.0f;

    public bool drawGizmos;

    private void Start()
    {
        // if (Input.GetButtonDown("Jump"))
        // PathManager.RequestPath(transform.position, target.position, OnPathFound);
    }

    public void StartWalking(Vector3 newT) // Start walking to new target || Add in queue
    {
        PathManager.RequestPath(transform.position, newT, OnPathFound);
    }

    public void OnPathFound(Vector3[] newP, bool success)
    {
        if (success)
        {
            path = newP;
            tIndex = 0;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath()
    {
        Vector3 currentWP = path[0]; // Current Way point

        while (true)
        {
            if (transform.position.Equals(currentWP))
            {
                tIndex++;
                if (tIndex >= path.Length)
                {
                    yield break;
                }

                currentWP = path[tIndex];
            }

            transform.position = Vector3.MoveTowards(transform.position, currentWP, speed * Time.deltaTime);
            yield return null;
        }
    }

    public void OnDrawGizmos()
    {
        if (drawGizmos)
        {
            if (path != null)
            {
                for (int i = tIndex; i < path.Length; i++)
                {
                    Gizmos.color = Color.black;
                    Gizmos.DrawCube(path[i], Vector3.one);

                    if (i.Equals(tIndex))
                    {
                        Gizmos.color = Color.red;
                        Gizmos.DrawLine(transform.position, path[i]);
                    }
                    else
                    {
                        Gizmos.color = Color.black;
                        Gizmos.DrawLine(path[i - 1], path[i]);
                    }
                }
            }
        }
    }
}
