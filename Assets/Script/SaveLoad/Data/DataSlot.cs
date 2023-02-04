using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MFarm.Save
{
    public class DataSlot
    {
        /// <summary>
        /// 进度条,String是每一个类独一无二的GUID
        /// 每一个GUID对应每一个单独需要保存的类中的所有数据(GameSaveData)
        /// </summary>
        public Dictionary<string, GameSaveData> dataDict = new Dictionary<string, GameSaveData>();
    }
}

