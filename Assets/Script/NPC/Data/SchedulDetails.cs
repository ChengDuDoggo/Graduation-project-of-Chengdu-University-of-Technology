using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//ʱ���ű�,������Ϸ�е�ʱ��仯������NPC��������ÿһ����Ϊ
[Serializable]
public class SchedulDetails:IComparable<SchedulDetails>
{
    public int hour, minute, day;
    public int priority;//���ȼ�,���ȼ�ԽСԽ��ִ��
    public Season season;
    public string targetScene;
    public Vector2Int targetGridPosition;
    public AnimationClip clipAtStop;
    public bool interactable;

    public SchedulDetails(int hour, int minute, int day, int priority, Season season, string targetScene, Vector2Int targetGridPosition, AnimationClip clipAtStop, bool interactable)//���캯��
    {
        this.hour = hour;
        this.minute = minute;
        this.day = day;
        this.priority = priority;
        this.season = season;
        this.targetScene = targetScene;
        this.targetGridPosition = targetGridPosition;
        this.clipAtStop = clipAtStop;
        this.interactable = interactable;
    }
    public int Time => (hour * 100) + minute;//ʱ��ϲ�Ϊ����
    public int CompareTo(SchedulDetails other)
    {
        if (Time == other.Time)//���ʱ�������Ƚ����ȼ�
        {
            if (priority > other.priority)
                return 1;
            else
                return -1;
        }
        else if (Time > other.Time)
        {
            return 1;
        }
        else if (Time < other.Time)
        {
            return -1;
        }
        return 0;
    }
}
