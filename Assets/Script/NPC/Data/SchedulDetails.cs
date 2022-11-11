using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//时间表脚本,根据游戏中的时间变化来操作NPC接下来的每一步行为
[Serializable]
public class SchedulDetails:IComparable<SchedulDetails>
{
    public int hour, minute, day;
    public int priority;//优先级,优先级越小越先执行
    public Season season;
    public string targetScene;
    public Vector2Int targetGridPosition;
    public AnimationClip clipAtStop;
    public bool interactable;

    public SchedulDetails(int hour, int minute, int day, int priority, Season season, string targetScene, Vector2Int targetGridPosition, AnimationClip clipAtStop, bool interactable)//构造函数
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
    public int Time => (hour * 100) + minute;//时间合并为分钟
    public int CompareTo(SchedulDetails other)
    {
        if (Time == other.Time)//如果时间相等则比较优先级
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
