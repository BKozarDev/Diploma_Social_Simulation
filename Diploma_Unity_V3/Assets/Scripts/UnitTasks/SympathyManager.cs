using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SympathyManager : MonoBehaviour
{
    [HideInInspector]
    public GameObject agent;
    [HideInInspector]
    public int pSympathy { get; set; }
    [HideInInspector]
    public enum EStates
    {
        NEUTRAL,
        FRIEND,
        LOVER,
        FAMILY,
        PARENT
    }

    public enum ERelationStates
    {
        GOOD,
        BAD
    }

    GenericAlgorythm ga;

    private EStates currentState;

    public SympathyManager(GameObject agent_, int pSympathy_, bool parent = false)
    {
        agent = agent_;
        pSympathy = pSympathy_;
        if (!parent)
            currentState = EStates.NEUTRAL;
        else
            currentState = EStates.PARENT;

        countBabiesMAX = Random.Range(0, 2);
    }

    int max_Sym = 10;

    private void Update()
    {
        Debug.Log(pSympathy);

        if (isTimerStart)
        {
            time -= Time.deltaTime;
            if (time <= 0)
            {
                pSympathy -= 1;
                time = time__;
            }
        }
    }

    [HideInInspector]
    public bool isHaveABaby;

    public void PlusSym(int plus, bool love, AgentManager mainAgent = null)
    {
        pSympathy += plus;
        if (pSympathy > max_Sym)
        {
            UpgradeStates(love);
            pSympathy = 0;
        }

        if(currentState == EStates.LOVER)
        {
            mainAgent.lover = agent;
        }

        if (currentState == EStates.FAMILY && pSympathy == max_Sym)
        {
            isHaveABaby = true;
            pSympathy = 0;
        }
    }

    int countBabies;
    int countBabiesMAX;
    public void CreateBaby(AgentManager mainAgent)
    {
        if (countBabies < countBabiesMAX)
        {
            ga = new GenericAlgorythm(mainAgent, agent.GetComponent<AgentManager>());
            countBabies++;
        }
    }

    bool isTimerStart;
    float time;
    int time__;
    public void RunTimer()
    {
        if (!isTimerStart)
        {
            time = 5;
            time__ = (int)time;
            isTimerStart = true;
        }
    }

    public void StopTimer()
    {
        isTimerStart = false;
        time = 5;
    }

    void UpgradeStates(bool love)
    {
        if (currentState != EStates.FAMILY)
        {
            switch (currentState)
            {
                case EStates.NEUTRAL:
                    currentState = EStates.FRIEND;
                    max_Sym += 5;
                    break;
                case EStates.FRIEND:
                    if (!love)
                    {
                        currentState = EStates.LOVER;
                        max_Sym += 10;
                    }
                    break;
                case EStates.LOVER:
                    currentState = EStates.FAMILY;
                    max_Sym += 25;
                    break;
            }
        }
    }

    void DowngradeStates()
    {
        if (currentState != EStates.NEUTRAL)
        {
            switch (currentState)
            {
                case EStates.FAMILY:
                    currentState = EStates.LOVER;
                    max_Sym -= 25;
                    break;
                case EStates.LOVER:
                    currentState = EStates.FRIEND;
                    max_Sym -= 10;
                    break;
                case EStates.FRIEND:
                    currentState = EStates.NEUTRAL;
                    max_Sym -= 5;
                    break;
            }
        }
    }

    public override string ToString()
    {
        string answer = "";
        switch (currentState)
        {
            case EStates.NEUTRAL:
                answer = "Hello, Stranger";
                break;
            case EStates.FRIEND:
                answer = "Hello, Friend!";
                break;
            case EStates.LOVER:
                answer = "Hello, My Lover";
                break;
            case EStates.FAMILY:
                answer = "Hello, My Darling";
                break;
            case EStates.PARENT:
                answer = "Hello, My Parent!";
                break;
        }

        answer += "\n level sympathy = " + pSympathy;

        return answer;
    }

    public int GetID()
    {
        switch (currentState)
        {
            case EStates.NEUTRAL:
                return 0;
            case EStates.FRIEND:
                return 1;
            case EStates.LOVER:
                return 2;
            case EStates.FAMILY:
                return 3;
            case EStates.PARENT:
                return 4;
            default:
                return -1;
        }
    }
}
