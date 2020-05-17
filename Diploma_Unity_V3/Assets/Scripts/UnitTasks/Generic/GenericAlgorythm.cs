using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class AgentParent
{
    public int vitality { get; set; }
    public int endurance { get; set; }
    public int strength { get; set; }
    public int gluttony { get; set; }

    public float Ph { get; set; }
    public float Pp { get; set; }

    public Vector3 position { get; set; }

    public AgentParent(int vitality, int endurance, int strength, int gluttony, float Ph, float Pp, Vector3 position)
    {
        this.vitality = vitality;
        this.endurance = endurance;
        this.strength = strength;
        this.gluttony = gluttony;

        this.Ph = Ph;
        this.Pp = Pp;

        this.position = position;
    }

    public List<int> ToList()
    {
        return new List<int>(new int[] { vitality, endurance, strength, gluttony });
    }

    public float GetP(string pName)
    {
        switch (pName)
        {
            case "Ph":
                return Ph;
            case "Pp":
                return Pp;
            default:
                return 0f;
        }
    }
}

public class GenericAlgorythm : MonoBehaviour
{
    AgentParent pStats1;
    AgentParent pStats2;

    AgentManager parent1;
    AgentManager parent2;

    public GameObject child;

    Grid grid;

    public GenericAlgorythm(AgentManager parent1, AgentManager parent2)
    {
        this.pStats1 = new AgentParent(parent1.vitality, parent1.endurance, parent1.strength, parent1.gluttony, parent1.Ph, parent1.Pp, parent1.gameObject.transform.position);
        this.pStats2 = new AgentParent(parent2.vitality, parent2.endurance, parent2.strength, parent2.gluttony, parent2.Ph, parent2.Pp, parent2.gameObject.transform.position);

        this.parent1 = parent1;
        this.parent2 = parent2;

        CreateChild();
    }

    public GenericAlgorythm(int vitality, int endurance, int strength, int gluttony, float ph, float pp, string name)
    {
        grid = FindObjectOfType<Grid>();
        CreateChild(vitality, endurance, strength, gluttony, ph, pp, name);
    }

    void CreateChild(int vitality, int endurance, int strength, int gluttony, float ph, float pp, string name)
    {
        GameObject newBorn = (GameObject)Instantiate(Resources.Load<GameObject>("Prefabs/Agent"));
        newBorn.transform.position = grid.GetRandomNode();

        newBorn.name = name;
        newBorn.AddComponent(typeof(AgentManager));
        newBorn.GetComponent<AgentManager>().NewAM(ph, pp, vitality, endurance, strength, gluttony);
    }

    void CreateChild()
    {
        int vitality = 0;
        int endurance = 0;
        int strength = 0;
        int gluttony = 0;

        float Ph = 0f;
        float Pp = 0f;

        Ph = pStats1.GetP("Ph") * pStats2.GetP("Ph");
        Pp = pStats1.GetP("Pp") * pStats2.GetP("Pp");

        int[] stats = new int[4];

        var choice = Random.Range(0f, 1f);
        var mutationRate = Random.Range(0f, 1f);
        var p1 = pStats1.ToList();
        var p2 = pStats2.ToList();

        for (int i = 0; i < p1.Count; i++)
        {
            if (choice <= 0.5f)
            {
                stats[i] = p1[i];
            }
            else
            {
                stats[i] = p2[i];
            }
            if (mutationRate > 0.86)
            {
                var mutationValue = Random.Range(-10, 10);
                stats[i] += mutationValue;
            }
            switch (i)
            {
                case 0:
                    vitality = stats[i];
                    break;
                case 1:
                    endurance = stats[i];
                    break;
                case 2:
                    strength = stats[i];
                    break;
                case 3:
                    gluttony = stats[i];
                    break;
            }
        }

        GameObject newBorn = (GameObject) Instantiate(Resources.Load<GameObject>("Prefabs/Agent"));
        newBorn.transform.position = pStats1.position + pStats2.position / 2f;
        // AgentManager am = new AgentManager(Ph, Pp, vitality, endurance, strength, gluttony, parent1, parent2);
        newBorn.AddComponent(typeof(AgentManager));
        newBorn.GetComponent<AgentManager>().NewAM(Ph, Pp, vitality, endurance, strength, gluttony, parent1, parent2);
    }
}
