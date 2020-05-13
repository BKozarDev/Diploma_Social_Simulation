using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBehaviour : MonoBehaviour
{
    [SerializeField]
    public int health;
    [SerializeField]
    public int pleasure;

    // GameObject agent;
    AgentManager am;
    BehaviourManager bm;
    private void Start()
    {
        // agent = GameObject.FindGameObjectWithTag("Agent");
        // am = agent.GetComponent<AgentManager>();
        bm = FindObjectOfType<BehaviourManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Agent"))
        {
            am = other.GetComponent<AgentManager>();
            if (am.isLeisureGo)
            {
                am.health += health;
                am.leisure += pleasure;

                am.strength--;
                am.endurance--;

                am.isLeisure = false;
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
