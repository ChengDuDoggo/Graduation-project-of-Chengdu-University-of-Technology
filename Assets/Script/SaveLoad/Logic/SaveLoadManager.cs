using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MFarm.Save
{
    public class SaveLoadManager : Singleton<SaveLoadManager>
    {
        private List<ISaveable> saveableList = new List<ISaveable>();//<ISaveable>�ǽӿ�,������б��п��Դ�����е�����ISaveable�ӿڵ���
        public void RegisterSaveable(ISaveable saveable)
        {
            if (!saveableList.Contains(saveable))
            {
                saveableList.Add(saveable);
            }
        }
    }
}

