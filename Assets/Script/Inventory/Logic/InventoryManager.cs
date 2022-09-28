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
            //背包是否已经有这个物体
            var index = GetItemIndexInBag(item.itemID);
            AddItemAtIndex(item.itemID, index, 1);
            if (toDestroy)
            {
                Destroy(item.gameObject);
            }
        }
        /// <summary>
        /// 检查背包是否有空位
        /// </summary>
        /// <returns></returns>
        private bool CheckBagCapacity()
        {
            for (int i = 0; i < PlayerBag.itemList.Count; i++)
            {
                if (PlayerBag.itemList[i].itemID == 0)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 通过物品ID找到背包中物品所在的位置(索引)
        /// </summary>
        /// <param name="ID">传入的物品ID</param>
        /// <returns>没有找到就返回-1</returns>
        private int GetItemIndexInBag(int ID)
        {
            for (int i = 0; i < PlayerBag.itemList.Count; i++)
            {
                if(PlayerBag.itemList[i].itemID == ID)
                {
                    return i;
                }
            }
            return -1;
        }
        private void AddItemAtIndex(int ID,int index,int amount)
        {
            if(index == -1 && CheckBagCapacity())//背包中没有这个物体且背包中有空位
            {
                var item = new InventoryItem { itemID = ID, itemAmount = amount };//new一个在背包中的Item
                for (int i = 0; i < PlayerBag.itemList.Count; i++)
                {
                    if(PlayerBag.itemList[i].itemID == 0)
                    {
                        PlayerBag.itemList[i] = item;//将空位的这个地方将捡到的新物品赋值给List
                        break;
                    }
                }
            }
            else//背包有这个物体
            {
                int currentAmount = PlayerBag.itemList[index].itemAmount+amount;//获取背包中这个物体的当前数量
                var item = new InventoryItem { itemID = ID, itemAmount = currentAmount };//获得新物体的ID和Amount
                PlayerBag.itemList[index] = item;
            }
        }
    }
}

