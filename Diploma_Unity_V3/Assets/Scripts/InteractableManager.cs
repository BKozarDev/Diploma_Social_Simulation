using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableManager : MonoBehaviour
{
    // Food and Plesure Dispenser positions List
    public List<GameObject> foodDisp = new List<GameObject>();
    public List<GameObject> pleasureDisp = new List<GameObject>();

    public static InteractableManager instance;

    AgentManager am;

    FoodBehaviour[] foods;
    PlayBehaviour[] plays;
    private void Start()
    {
        am = GetComponent<AgentManager>();

        foods = FindObjectsOfType<FoodBehaviour>();
        for (int i = 0; i < foods.Length; i++)
        {
            foodDisp.Add(foods[i].gameObject);
        }

        plays = FindObjectsOfType<PlayBehaviour>();
        for (int i = 0; i < plays.Length; i++)
        {
            pleasureDisp.Add(plays[i].gameObject);
        }
    }

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

    public Vector3 GetPlayPos()
    {
        var currentPlay = pleasureDisp[0];
        // var health = currentPlay.GetComponent<PlayBehaviour>().health;
        var pleasure = currentPlay.GetComponent<PlayBehaviour>().pleasure;

        if (am.Ph > am.Pp)
        {
            foreach (var zone in pleasureDisp)
            {
                var play = zone.GetComponent<PlayBehaviour>();
                var health = play.health;
                if (play.pleasure < pleasure && (am.health + health - 10) > 0)
                {
                    pleasure = play.pleasure;
                    currentPlay = zone;
                }
            }
        }
        else
        {
            foreach (var zone in pleasureDisp)
            {
                var play = zone.GetComponent<PlayBehaviour>();
                var health = play.health;
                if (play.pleasure > pleasure && (am.health + health - 10) > 0)
                {
                    pleasure = play.pleasure;
                    currentPlay = zone;
                }
            }
        }

        return currentPlay.transform.position;
    }
}
