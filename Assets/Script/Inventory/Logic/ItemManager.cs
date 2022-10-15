using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//专门用来管理世界地图中所有的物品的管理类
namespace MFarm.Inventory
{
    public class ItemManager : Singleton<ItemManager>
    {
        public Item itemPrefab;
        private Transform itemParent;
        private void OnEnable()
        {
            EventHandler.InstantiateItemInScene += OnInstantiateItemInScene;//为委托事件添加函数方法
        }
        private void OnDisable()
        {
            EventHandler.InstantiateItemInScene -= OnInstantiateItemInScene;
        }
        private void Start()
        {
            itemParent = GameObject.FindWithTag("ItemParent").transform;
        }
        private void OnInstantiateItemInScene(int ID, Vector3 pos)
        {
            var item = Instantiate(itemPrefab, pos, Quaternion.identity, itemParent);//克隆物体
            item.itemID = ID;
        }
    }
}

