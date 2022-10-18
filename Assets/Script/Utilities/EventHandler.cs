using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class EventHandler //创建一个脚本来控制游戏中所有的事件，静态的，全局的
{

    public static event Action<InventoryLocation, List<InventoryItem>> UpdateInventoryUI; //定义一个委托事件，需要知道更新格子的位置和更新的道具数据
    public static void CallUpdateInventoryUI(InventoryLocation loaction, List<InventoryItem> list)//其他脚本可以随时呼叫该事件
    {
        UpdateInventoryUI?.Invoke(loaction, list);//?.:先判断事件委托是否为空再激活
    }

    public static event Action<int, Vector3> InstantiateItemInScene;//委托事件:在场景中创建一个道具预制体
    public static void CallInstantiateItemInScene(int ID,Vector3 pos)
    {
        InstantiateItemInScene?.Invoke(ID,pos);
    }
    public static event Action<ItemDetails, bool> ItemSelectedEvent;
    public static void CallItemSelectedEvent(ItemDetails itemDetails,bool isSelected)
    {
        ItemSelectedEvent?.Invoke(itemDetails, isSelected);
    }
}
