using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableManager : MonoBehaviour
{
    // Food and Plesure Dispenser positions List
    public List<GameObject> foodDisp = new List<GameObject>();
    public List<GameObject> plesureDisp = new List<GameObject>();

    public static InteractableManager instance;

    private void Awake()
    {
        instance = this;
    }

    public Vector3 GetFoodPos()
    {
        float dist = Vector3.Distance(transform.position, foodDisp[0].transform.position);
        var pos = foodDisp[0].transform.position;
        foreach (var food in foodDisp)
        {
            var __dist = Vector3.Distance(transform.position, food.transform.position);
            if (__dist < dist)
            {
                dist = __dist;
                pos = food.transform.position;
            }
        }

        pos = new Vector3(pos.x, 0, pos.z);

        return pos;
    }
}
