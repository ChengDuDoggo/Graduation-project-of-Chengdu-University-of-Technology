using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MFarm.Save
{
    public class SaveLoadManager : Singleton<SaveLoadManager>
    {
        private List<ISaveable> saveableList = new List<ISaveable>();//<ISaveable>是接口,代表该列表中可以存放所有调用了ISaveable接口的类
        public void RegisterSaveable(ISaveable saveable)
        {
            if (!saveableList.Contains(saveable))
            {
                saveableList.Add(saveable);
            }
        }
    }
}

