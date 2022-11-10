using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "SchedulDataList_SO",menuName = "NPC Schedule/SchedulDataList")]
public class SchedulDataList_SO : ScriptableObject
{
    public List<SchedulDetails> schedulList;
}
