using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;//自定义Timeline的命名空间
using MFarm.Dialogue;
[System.Serializable]
public class DialogueBehaviour : PlayableBehaviour//一个基类接口,可以让创作者使用该接口里的方法函数等来自定义Timeline中的事件
{
    private PlayableDirector director;//拿到Timeline
    public DialoguePiece dialoguePiece;//对话片段
    public override void OnPlayableCreate(Playable playable)//重构基类中的虚函数(在创建Playable时发生什么)
    {
        director = (playable.GetGraph().GetResolver() as PlayableDirector);//强制转换拿到Timeline
    }
    public override void OnBehaviourPlay(Playable playable, FrameData info)//当播放当前片段时
    {
        //呼叫启动UI
        EventHandler.CallShowDialogueEvent(dialoguePiece);
        if (Application.isPlaying)
        {
            if (dialoguePiece.hasToPause)
            {
                //暂停Timeline
                TimelineManager.Instance.PauseTimeline(director);
            }
            else
            {
                EventHandler.CallShowDialogueEvent(null);//关闭对话框
            }
        }
    }
    //Timeline父类中自带的函数方法,可以使Timeline在播放过程中每帧执行一次此函数
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        if (Application.isPlaying)
            TimelineManager.Instance.IsDone = dialoguePiece.isDone;
    }
    //Timeline父类自带函数,在Timeline播放完毕之后执行一次
    public override void OnBehaviourPause(Playable playable,FrameData info)
    {
        EventHandler.CallShowDialogueEvent(null);//关闭对话框
    }
    public override void OnGraphStart(Playable playable)
    {
        EventHandler.CallUpdateGameStateEvent(GameState.Pause);
    }
    public override void OnGraphStop(Playable playable)
    {
        EventHandler.CallUpdateGameStateEvent(GameState.Gameplay);//激活委托里面存放的事件
    }
}
