using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodBehaviour : MonoBehaviour
{
    public int health;
    public int pleasure;

    public GameObject foodAsset;

    // GameObject agent;
    AgentManager am;

    BehaviourManager bm;
    private void Start()
    {
        // agent = GameObject.FindGameObjectWithTag("Agent");
        // am = agent.GetComponent<AgentManager>();
        bm = FindObjectOfType<BehaviourManager>();
    }

    private void Awake()
    {
        give = false;
    }

    bool eat;
    bool spawn;
    GameObject food;
    private void Update()
    {
        // if (eat)
        // {
        //     bm.highnoon = false;
        // }

        if (bm.highnoon)
        {
            if (!spawn && !eat)
            {
                food = Instantiate(foodAsset, transform.position, Quaternion.identity, this.gameObject.transform);
                spawn = true;
            }

            if (eat)
            {
                Destroy(food);
                spawn = false;
                eat = false;
                give = false;
            }
        }
        else
        {
            Destroy(food);
            spawn = false;
            eat = false;
            give = false;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Agent") && bm.highnoon)
        {
            am = other.GetComponent<AgentManager>();
            Debug.Log(other.name + ": " + am.hungry + "; " + am.health);
            //Do Something Good
            am.hungry += 25;
            am.health += health;
            am.leisure += pleasure;
            am.timer = 0;

            am.strength += 2;

            eat = true;
        }
    }

    bool isGive;
    private void OnTriggerStay(Collider other)
    {
        if (other.tag.Equals("Agent") && bm.highnoon)
        {
            if (!isGive)
            {
                am = other.GetComponent<AgentManager>();
                Debug.Log(other.name + ": " + am.hungry + "; " + am.health);
                //Do Something Good
                am.hungry += 25;
                am.health += health;
                am.leisure += pleasure;
                am.timer = 0;

                am.strength += 2;

                eat = true;

                isGive = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Agent") && bm.highnoon)
        {
            isGive = false;
        }
    }

    bool give;
    // private void OnTriggerStay(Collider other)
    // {
    //     if (other.tag.Equals(agent.tag) && bm.highnoon && spawn)
    //     {
    //         if (!give)
    //         {
    //             bm.highnoon = false;
    //             //Do Something Good
    //             give = true;
    //             am.hunger += 10;
    //             am.health += health;
    //             am.leisure += pleasure;
    //             am.timeH = 0;
    //             eat = true;
    //         }
    //     }
    // }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
