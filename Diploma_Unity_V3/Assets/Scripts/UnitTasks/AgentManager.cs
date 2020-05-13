using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentManager : MonoBehaviour
{
    public float Ph;
    public float Pp;

    public int vitality;
    public int endurance;
    public int strength;
    public int gluttony; // Обжорство

    public int health;
    public int hungry;
    public int leisure;

    public bool isLeisure;
    public bool isLeisureGo;
    public bool decision;

    private static int MAX = 50;
    private static int hungryMAX;

    UnitTasks ut;
    BehaviourManager bm;

    public AgentManager(float Ph, float Pp, int vitality, int endurance, int strength, int gluttony)
    {
        this.Ph = Ph;
        this.Pp = Pp;
        this.vitality = vitality;
        this.endurance = endurance;
        this.strength = strength;
        this.gluttony = gluttony;
    }

    // Start is called before the first frame update
    void Start()
    {
        health = (int)(MAX * (1f + vitality / 100f));
        leisure = MAX;
        hungryMAX = (int)(MAX * (1f + gluttony / 100f));

        ut = GetComponent<UnitTasks>();
        bm = FindObjectOfType<BehaviourManager>();

        // Hungry
        isStartHungry = true;

        timer = 0;
        time_day = bm.time;
        hungry = hungryMAX;
    }

    private bool isHungry;
    private void LateUpdate()
    {
        if (hungry <= hungryMAX * 0.8 && hungry > hungryMAX * 0.6)
        {
            if (!isHungry)
            {
                isHungry = true;
                StopAllCoroutines();
                StartCoroutine(Hunger(1));
            }
        }
        else if (hungry <= hungryMAX * 0.6 && hungry > hungryMAX * 0.4)
        {
            if (!isHungry)
            {
                isHungry = true;
                StopAllCoroutines();
                StartCoroutine(Hunger(3));
            }
        }
        else if (hungry <= hungryMAX * 0.4)
        {
            if (!isHungry)
            {
                isHungry = true;
                StopAllCoroutines();
                StartCoroutine(Hunger(5));
            }
        }
    }

    IEnumerator Hunger(int value)
    {
        health -= value;
        yield return new WaitForSeconds(3);
        isHungry = false;
    }

    private bool isStartHungry;
    [HideInInspector]
    public float timer;
    private float time_day;
    // Update is called once per frame
    void Update()
    {
        if (isStartHungry)
        {
            timer += Time.deltaTime;

            if (timer >= time_day / 4)
            {
                timer = 0;
                hungry -= 5;
            }
        }
    }
}
