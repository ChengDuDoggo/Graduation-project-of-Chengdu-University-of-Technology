using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//时间表数据库其中存储的NPC在特定时间的特定行为数据
//例如NPC在游戏一开始出生在某个坐标位置
//NPC在经过一段时间后应该移动到什么位置,播放什么动画
[CreateAssetMenu(fileName = "SchedulDataList_SO",menuName = "NPC Schedule/SchedulDataList")]
public class SchedulDataList_SO : ScriptableObject
{
    public List<SchedulDetails> schedulList;
}
