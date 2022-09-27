using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFarm.Inventory //手动添加一个命名空间，别的类不使用该命名空间就不可以调用到该命名空间中的变量或者函数，达到保护作用
{
    public class InventoryManager : Singleton<InventoryManager>//数据管理类
    {
        [Header("物品数据")]
        public ItemDataList_SO itemDataList_SO;//拿到数据库
        [Header("背包数据")]
        public InventoryBag_SO PlayerBag;
        /// <summary>
        /// 通过ID返回物品信息
        /// </summary>
        /// <param name="ID">物品ID</param>
        /// <returns></returns>
        public ItemDetails GetItemDetails(int ID)//通过物品的ID来找到具体的ItemDetails数据
        {
            return itemDataList_SO.itemDetailsList.Find(i => i.itemID == ID);//找到ID与itemID相匹配的itemDetails返回,使用拉姆达表达式可简写
        }
        /// <summary>
        /// 添加物品至Player背包
        /// </summary>
        /// <param name="item"></param>
        /// <param name="toDestroy">拾取后是否销毁物品</param>
        public void AddItem(Item item,bool toDestroy)
        {
            //背包是否有空位

            //背包是否已经有这个物体

            InventoryItem newItem = new InventoryItem();
            newItem.itemID = item.itemID;
            newItem.itemAmount = 1;
            PlayerBag.itemList[0] = newItem;

            if (toDestroy)
            {
                Destroy(item.gameObject);
            }
        }
    }
}

