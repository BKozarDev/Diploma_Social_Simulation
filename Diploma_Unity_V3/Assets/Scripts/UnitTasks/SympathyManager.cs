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
        WIFE,
    }

    private EStates currentState;

    public SympathyManager(GameObject agent_, int pSympathy_)
    {
        agent = agent_;
        pSympathy = pSympathy_;
        currentState = EStates.NEUTRAL;
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

    public void PlusSym(int plus)
    {
        pSympathy += plus;
        if (pSympathy > max_Sym)
        {
            UpgradeStates();
            pSympathy = 0;
        }
        Debug.Log(pSympathy);
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

    void UpgradeStates()
    {
        if (currentState != EStates.WIFE)
        {
            switch (currentState)
            {
                case EStates.NEUTRAL:
                    currentState = EStates.FRIEND;
                    max_Sym += 5;
                    break;
                case EStates.FRIEND:
                    currentState = EStates.LOVER;
                    max_Sym += 10;
                    break;
                case EStates.LOVER:
                    currentState = EStates.WIFE;
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
                case EStates.WIFE:
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
            case EStates.WIFE:
                answer = "Hello, My Darling";
                break;
        }

        return answer;
    }
}
