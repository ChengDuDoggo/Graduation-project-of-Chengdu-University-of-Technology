using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineManager : Singleton<TimelineManager>
{
    public PlayableDirector startDirector;//游戏开始时的Director
    private PlayableDirector currentDirector;
    private bool isPause;
    protected override void Awake()
    {
        base.Awake();
        currentDirector = startDirector;
    }
    private void Update()
    {
        if (isPause && Input.GetKeyDown(KeyCode.Space))
        {
            isPause = false;
            currentDirector.playableGraph.GetRootPlayable(0).SetSpeed(1d);
        }
    }
    public void PauseTimeline(PlayableDirector director)
    {
        currentDirector = director;
        currentDirector.playableGraph.GetRootPlayable(0).SetSpeed(0d);
        isPause = true;
    }
}
