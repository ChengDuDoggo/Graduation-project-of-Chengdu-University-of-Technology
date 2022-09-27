using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFarm.Inventory //手动添加一个命名空间，别的类不使用该命名空间就不可以调用到该命名空间中的变量或者函数，达到保护作用
{
    public class InventoryManager : Singleton<InventoryManager>//数据管理类
    {
        public ItemDataList_SO itemDataList_SO;//拿到数据库
        /// <summary>
        /// 通过ID返回物品信息
        /// </summary>
        /// <param name="ID">物品ID</param>
        /// <returns></returns>
        public ItemDetails GetItemDetails(int ID)//通过物品的ID来找到具体的ItemDetails数据
        {
            return itemDataList_SO.itemDetailsList.Find(i => i.itemID == ID);//找到ID与itemID相匹配的itemDetails返回,使用拉姆达表达式可简写
        }
    }
}

