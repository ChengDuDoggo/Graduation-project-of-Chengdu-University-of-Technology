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
    public static event Action<int, Vector3,ItemType> DropItemEvent;//人物丢弃道具后触发事件委托
    public static void CallDropItemEvent(int ID,Vector3 pos,ItemType itemType)
    {
        DropItemEvent?.Invoke(ID, pos,itemType);
    }
    public static event Action<ItemDetails, bool> ItemSelectedEvent;
    public static void CallItemSelectedEvent(ItemDetails itemDetails,bool isSelected)
    {
        ItemSelectedEvent?.Invoke(itemDetails, isSelected);
    }
    public static event Action<int, int> GameMinuteEvent;
    public static void CallGameMinuteEvent(int minute,int hour)
    {
        GameMinuteEvent?.Invoke(minute,hour);
    }
    public static event Action<int, int, int, int, Season> GameDateSeason;
    public static void CallGameDateSeason(int hour,int day,int month,int year,Season season)
    {
        GameDateSeason?.Invoke(hour,day,month,year,season);
    }
    public static event Action<string, Vector3> TransitionEvent;//场景切换委托事件
    public static void CallTransitionEvent(string sceneName,Vector3 pos)
    {
        TransitionEvent?.Invoke(sceneName, pos);
    }
    public static event Action BeforeSceneUnloadEvent;//场景卸载之前需要触发一些事件来避免报错
    public static void CallBeforeSceneUnloadEvent()
    {
        BeforeSceneUnloadEvent?.Invoke();
    }
    public static event Action AfterSceneLoadedEvent;//加载场景之后需要触发一些事件来切换数据
    public static void CallAfterSceneLoadedEvent()
    {
        AfterSceneLoadedEvent?.Invoke();
    }
    public static event Action<Vector3> MoveToPosition;//切换场景人物移动到指定位置委托事件
    public static void CallMoveToPosition(Vector3 targetPosition)
    {
        MoveToPosition?.Invoke(targetPosition);
    }
    public static event Action<Vector3, ItemDetails> MouseClickedEvent;//鼠标点击事件
    public static void CallMouseClickedEvent(Vector3 pos,ItemDetails itemDetails)
    {
        MouseClickedEvent?.Invoke(pos,itemDetails);
    }
    public static event Action<Vector3, ItemDetails> ExecuteActionAfterAnimation;//实际事件,放在Player某个动作动画播放完成后调用的实际事件功能
    public static void CallExecuteActionAfterAnimation(Vector3 pos,ItemDetails itemDetails)
    {
        ExecuteActionAfterAnimation?.Invoke(pos, itemDetails);
    }
    public static event Action<int, Season> GameDayEvent;//每日委托事件,每新的一天，调用一次此委托
    public static void CallGameDayEvent(int day,Season season)
    {
        GameDayEvent?.Invoke(day, season);
    }
    public static event Action<int, TileDetails> PlantSeedEvent;
    public static void CallPlantSeedEvent(int ID,TileDetails tile)
    {
        PlantSeedEvent?.Invoke(ID, tile);
    }
}
