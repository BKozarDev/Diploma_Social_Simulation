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
    private void Update()
    {
        // Do Walking Around if isWalking = true
        if (start && isWalking && !doneW)
        {
            // isRandomPos = true;
            start = false;
        }

        if (startTimer)
        {
            timer += Time.deltaTime;
        }

        if (start && isHungry && !doneS)
        {
            // isPos = true;
            start = false;
        }

        if (start && isPlaying && !doneP)
        {
            // isPlay = true;
            start = false;
        }
    }

    #region walking values and bools
    float threshold = 1.51f;
    public bool isRandomPos;
    bool doneW;
    bool startTimer;
    float timer;
    #endregion
    Vector3 targetPos;
    #region walking task
    [Task]
    public void WalkingAround()
    {
        if (isRandomPos)
        {
            Debug.Log("Walk");
            targetPos = grid.GetRandomNode();
            unit.target.position = targetPos;
            unit.StartWalking(targetPos);

            isRandomPos = false;
            doneW = true;

            // Wait();
        }

        if (Vector3.Distance(transform.position, unit.target.position) <= threshold && doneW || timer > 8)
        {
            Task.current.Succeed();
            doneW = false;

            am.health += 1;
            am.leisure += 2;

            isWalking = false;
            start = true;
            am.isDecision = true;

            isRandomPos = false;
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
    public bool isPos;
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
            Task.current.Succeed();

            isHungry = false;
            start = true;

            am.isDecision = true;

            isPos = false;
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
    public bool isPlay;
    #endregion
    #region playing task
    [Task]
    public void Play()
    {
        if (isPlay)
        {
            var playPos = grid.NodeFromWorldPoint(im.GetPlayPos());

            targetPos = playPos.wPos;
            unit.target.position = targetPos;
            unit.StartWalking(targetPos);

            isPlay = false;
            doneP = true;

            // Wait();
        }

        if (Vector3.Distance(transform.position, unit.target.position) < threshold || timer > 8)
        {
            doneP = false;
            Task.current.Succeed();

            isPlaying = false;  
            start = true;

            am.isDecision = true;

            isPlay = false;
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
