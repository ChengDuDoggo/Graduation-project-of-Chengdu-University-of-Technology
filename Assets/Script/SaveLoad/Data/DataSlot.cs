using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MFarm.Save
{
    public class DataSlot
    {
        /// <summary>
        /// ������,String��ÿһ�����һ�޶���GUID
        /// ÿһ��GUID��Ӧÿһ��������Ҫ��������е���������(GameSaveData)
        /// </summary>
        public Dictionary<string, GameSaveData> dataDict = new Dictionary<string, GameSaveData>();
    }
}

