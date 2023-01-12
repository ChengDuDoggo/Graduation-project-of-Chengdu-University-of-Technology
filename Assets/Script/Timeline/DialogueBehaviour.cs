using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;//�Զ���Timeline�������ռ�
using MFarm.Dialogue;
[System.Serializable]
public class DialogueBehaviour : PlayableBehaviour//һ������ӿ�,�����ô�����ʹ�øýӿ���ķ������������Զ���Timeline�е��¼�
{
    private PlayableDirector director;//�õ�Timeline
    public DialoguePiece dialoguePiece;//�Ի�Ƭ��
    public override void OnPlayableCreate(Playable playable)//�ع������е��麯��(�ڴ���Playableʱ����ʲô)
    {
        director = (playable.GetGraph().GetResolver() as PlayableDirector);//ǿ��ת���õ�Timeline
    }
    public override void OnBehaviourPlay(Playable playable, FrameData info)//�����ŵ�ǰƬ��ʱ
    {
        //��������UI
        EventHandler.CallShowDialogueEvent(dialoguePiece);
        if (Application.isPlaying)
        {
            if (dialoguePiece.hasToPause)
            {
                //��ͣTimeline
                TimelineManager.Instance.PauseTimeline(director);
            }
            else
            {
                EventHandler.CallShowDialogueEvent(null);//�رնԻ���
            }
        }
    }
}
