using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Panda;
// using IronPython.Hosting;
using System;

public class UnitTasks : MonoBehaviour
{
    #region bool tasks
    [Task]
    public bool isWalking;
    [Task]
    public bool isHungry;
    [Task]
    public bool isPlaying;
    #endregion

    Grid grid;
    Unit unit;
    InteractableManager im;
    AgentManager am;

    Transform lookAtPos;

    private void Start()
    {
        // Starting Behaviour tree params
        isRandomPos = false;
        doneW = false;
        doneS = false;
        start = true;

        // Getting Components
        unit = GetComponent<Unit>();
        im = GetComponent<InteractableManager>();
        am = GetComponent<AgentManager>();
        grid = FindObjectOfType<Grid>();

        // Python programs
        // python();
    }

    bool start;
    private void LateUpdate()
    {
        // Do Walking Around if isWalking = true
        if (start && isWalking && !doneW)
        {
            isRandomPos = true;
            start = false;
        }

        if (startTimer)
        {
            timer += Time.deltaTime;
        }

        // if(isHungry && !start)
        // {
        //     Task.current.Succeed();
        //     DoneWalk();
        //     DonePlay();
        // }

        if (start && isHungry && !doneS)
        {
            isPos = true;
            start = false;
        }

        if (start && isPlaying && !doneP)
        {
            isPlay = true;
            start = false;
        }
    }

    #region walking values and bools
    float threshold = 1.51f;
    bool isRandomPos;
    bool doneW;
    bool startTimer;
    float timer;
    #endregion
    Vector3 targetPos;
    #region walking task
    [Task]
    public void WalkingAround()
    {
        // Debug.Log("Name of this GameObject: " + grid.gameObject.name);
        if (isRandomPos)
        {
            targetPos = grid.GetRandomNode();
            unit.target.position = targetPos;
            unit.StartWalking(targetPos);

            isRandomPos = false;
            doneW = true;

            Wait();
        }

        if (Vector3.Distance(transform.position, unit.target.position) <= threshold && doneW || timer > 8)
        {
            Task.current.Succeed();
            doneW = false;
            // am.isProc = false;

            am.health += 1;
            am.leisure += 2;

            isWalking = false;
            start = true;
            am.isDecision = true;
        }
    }

    public void DoneWalk()
    {
        isWalking = false;
        start = true;
        doneW = false;
    }

    // This is my method of waiting

    public void Wait()
    {
        startTimer = true;
        if (timer > 20)
        {
            timer = 0;
            startTimer = false;
        }
    }
    #endregion

    #region searching values and bools
    bool doneS;
    bool isPos;
    #endregion
    #region searching task
    [Task]
    public void Food()
    {
        Searching();
    }

    [Task]
    public void Searching()
    {
        if (isPos)
        {
            var foodPos = grid.NodeFromWorldPoint(im.GetFoodPos());
            targetPos = foodPos.wPos;
            unit.target.position = targetPos;
            unit.StartWalking(targetPos);

            isPos = false;
            doneS = true;
        }

        if (Vector3.Distance(transform.position, unit.target.position) < threshold)
        {
            doneS = false;
            // am.isProc = false;
            Task.current.Succeed();

            isHungry = false;
            start = true;

            am.isDecision = true;
        }
    }

    public void DoneSearch()
    {
        isPos = false;
        start = true;
        doneS = false;
        isHungry = false;
    }
    #endregion

    #region decision
    [Task]
    public void Decision()
    {
        am.isDecision = true;
        Task.current.Succeed();
    }
    #endregion

    #region playing values and bools
    bool doneP;
    bool isPlay;
    #endregion
    #region playing task
    [Task]
    public void Play()
    {
        if (isPlay)
        {
            Node playPos;
            if (am.isLeisure)
            {
                var __l = im.plesureDisp[0].GetComponent<PlayBehaviour>().pleasure;
                var currentPlayZone = im.plesureDisp[0];
                foreach (var zone in im.plesureDisp)
                {
                    var play = zone.GetComponent<PlayBehaviour>();
                    var __h = play.health;
                    if (play.pleasure > __l && (am.health + __h - 8) > 0)
                    {
                        __l = play.pleasure;
                        currentPlayZone = zone;
                    }
                }

                playPos = grid.NodeFromWorldPoint(currentPlayZone.transform.position);
                am.isLeisure = false;
            }
            else
            {
                System.Random rnd = new System.Random();
                int r = rnd.Next(im.plesureDisp.Count);

                var doublesPlesure = new List<GameObject>(im.plesureDisp);

                var plParam = doublesPlesure[r].GetComponent<PlayBehaviour>();
                var plPos = doublesPlesure[r];
                var res = am.health + plParam.health;
                Debug.Log(" Health: " + res);
                Debug.Log("Position: " + plPos.transform.position);
                while ((am.health + plParam.health + 8) < 0)
                {
                    doublesPlesure.RemoveAt(r);
                    if (doublesPlesure.Count == 0)
                    {
                        Task.current.Succeed();
                        break;
                    }
                    else
                    {
                        r = rnd.Next(doublesPlesure.Count);
                        plParam = doublesPlesure[r].GetComponent<PlayBehaviour>();
                        plPos = doublesPlesure[r];
                    }

                    Debug.Log("Position: " + plPos.transform.position);
                }

                playPos = grid.NodeFromWorldPoint(plPos.transform.position);
            }
            unit.target.position = playPos.wPos;
            targetPos = playPos.wPos;
            unit.StartWalking(targetPos);

            // transform.LookAt(playPos.wPos);

            doneP = true;

            // Debug.Log("Position: " + playPos.wPos);
            isPlay = false;

            Wait();
        }

        if (Vector3.Distance(transform.position, unit.target.position) < threshold || timer > 8)
        {
            doneP = false;
            // am.isProc = false;
            Task.current.Succeed();

            isPlaying = false;
            start = true;
        }
    }

    public void DonePlay()
    {
        doneP = false;
        isPlaying = false;
        start = true;
    }
    #endregion

    #region python test
    // public void python()
    // {
    //     var engine = Python.CreateEngine();

    //     ICollection<string> searchPaths = engine.GetSearchPaths();

    //     searchPaths.Add(Application.dataPath + @"\Scripts\3DRM\UnitTasks\Python\");
    //     searchPaths.Add(Application.dataPath + @"\Plugins\Lib\");
    //     // seatchPaths.Add(@"Assets/Plugins/Lib/site-packages");
    //     engine.SetSearchPaths(searchPaths);

    //     dynamic py = engine.CreateScope();
    //     engine.ExecuteFile(@"D:\ProjectGames\Generic\GA_new_16.11.19\GA_Graduation_work\Assets\Scripts\3DRM\UnitTasks\Python\Clustering.py", py);
    //     dynamic cluster = py.Clustering(2);

    //     // Debug.Log(cluster.doScan(-20, 15));
    // }
    #endregion

}
