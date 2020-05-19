using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BehaviourManager : MonoBehaviour
{
    public List<Text> text;
    // [0] - Timer
    // [1] - Text ("New Day!")
    // [2] - Day count

    public float time; // Current Time
    int timeInt;
    [HideInInspector]
    public int dayTime; // Max day time
    float __time; // Delay for text
    float __timeHigh; // It's hiiighnooon

    public int day; // Current Day
    private void Start()
    {
        dayTime = (int)time;
        day = 0;
        text[2].text = "Day: " + day.ToString();
    }

    public bool highnoon;
    void Update()
    {
        time -= Time.deltaTime;
        text[0].text = "Time: " + time.ToString("0");
        timeInt = (int)time;

        if (time <= 0)
        {
            // New Day! 
            // DoSomething()
            text[1].text = "New Day!";

            time = dayTime;
            day++;
            text[2].text = "Day: " + day.ToString();
        }
        if (text[1].text.Equals("New Day!"))
        {
            __time += Time.deltaTime;
            if (__time > 3)
            {
                text[1].text = "";
                __time = 0;
            }
        }
        if (highnoon)
        {
            __timeHigh += Time.deltaTime;
            if (__timeHigh > 10)
            {
                __timeHigh = 0;
                highnoon = false;
            }
        }
        else
        {
            __timeHigh = 0;
        }

        if (timeInt.Equals(dayTime / 2))
        {
            highnoon = true;
        }
    }
}
