using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MFarm.AStar
{
    public class MovementStep//移动的每一步类,它有场景名,时间,坐标属性
    {
        public string sceneName;
        public int hour;
        public int minute;
        public int second;
        public Vector2Int gridCoordinate;//坐标
    }
}

