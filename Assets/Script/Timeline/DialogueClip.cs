using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class DialogueClip : PlayableAsset/*改基类可以完成Playable的实例化*/,ITimelineClipAsset//该接口可以实现Timeline剪辑的高级功能
{
    public ClipCaps clipCaps => ClipCaps.None;
    public DialogueBehaviour dialogue = new DialogueBehaviour();

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<DialogueBehaviour>.Create(graph, dialogue);
        return playable;
    }
}
