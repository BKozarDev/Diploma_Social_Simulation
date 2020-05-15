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

    private static int MAX = 50;
    private static int hungryMAX;

    public int hunger;

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
        hunger =(int)(hungryMAX * Ph);

        isDecision = true;

        _Ph = 1;
        _Pp = 1;
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

    [HideInInspector]
    public bool isDecision;
    private float _Ph;
    private float _Pp;
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

        if (isDecision)
        {
            _Ph *= Ph;
            _Pp *= Pp;

            if (_Ph >= _Pp)
            {
                if (hungry <= hunger)
                {
                    if(bm.highnoon)
                        Eat();
                }

                Walk();
                _Pp = 1;
            }
            else
            {
                var choice = Random.Range(0f, 1f);
                if (choice > _Pp)
                {
                    Walk();
                }
                else
                {
                    Play();
                }

                _Ph = 1;
            }
        }
    }

    void Walk()
    {
        if (!ut.isHungry)
        {
            ut.isWalking = true;
            isDecision = false;
        }

    }

    void Play()
    {
        ut.isPlaying = true;
        isDecision = false;
    }

    void Eat()
    {
        if (ut.isWalking)
            ut.DoneWalk();
        if (ut.isPlaying)
            ut.DonePlay();
        
        Debug.Log("EATING");
        ut.isHungry = true;
        isDecision = false;
    }

    void Died()
    {
        if (health <= 0)
        {
            this.gameObject.SetActive(false);
        }
    }
}
