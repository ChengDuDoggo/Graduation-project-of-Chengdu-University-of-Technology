using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class EventHandler //����һ���ű���������Ϸ�����е��¼�����̬�ģ�ȫ�ֵ�
{

    public static event Action<InventoryLocation, List<InventoryItem>> UpdateInventoryUI; //����һ��ί���¼�����Ҫ֪�����¸��ӵ�λ�ú͸��µĵ�������
    public static void CallUpdateInventoryUI(InventoryLocation loaction, List<InventoryItem> list)//�����ű�������ʱ���и��¼�
    {
        UpdateInventoryUI?.Invoke(loaction, list);//?.:���ж��¼�ί���Ƿ�Ϊ���ټ���
    }

    public static event Action<int, Vector3> InstantiateItemInScene;//ί���¼�:�ڳ����д���һ������Ԥ����
    public static void CallInstantiateItemInScene(int ID,Vector3 pos)
    {
        InstantiateItemInScene?.Invoke(ID,pos);
    }
    public static event Action<int, Vector3,ItemType> DropItemEvent;//���ﶪ�����ߺ󴥷��¼�ί��
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
    public static event Action<string, Vector3> TransitionEvent;//�����л�ί���¼�
    public static void CallTransitionEvent(string sceneName,Vector3 pos)
    {
        TransitionEvent?.Invoke(sceneName, pos);
    }
    public static event Action BeforeSceneUnloadEvent;//����ж��֮ǰ��Ҫ����һЩ�¼������ⱨ��
    public static void CallBeforeSceneUnloadEvent()
    {
        BeforeSceneUnloadEvent?.Invoke();
    }
    public static event Action AfterSceneLoadedEvent;//���س���֮����Ҫ����һЩ�¼����л�����
    public static void CallAfterSceneLoadedEvent()
    {
        AfterSceneLoadedEvent?.Invoke();
    }
    public static event Action<Vector3> MoveToPosition;//�л����������ƶ���ָ��λ��ί���¼�
    public static void CallMoveToPosition(Vector3 targetPosition)
    {
        MoveToPosition?.Invoke(targetPosition);
    }
    public static event Action<Vector3, ItemDetails> MouseClickedEvent;//������¼�
    public static void CallMouseClickedEvent(Vector3 pos,ItemDetails itemDetails)
    {
        MouseClickedEvent?.Invoke(pos,itemDetails);
    }
    public static event Action<Vector3, ItemDetails> ExecuteActionAfterAnimation;//ʵ���¼�,����Playerĳ����������������ɺ���õ�ʵ���¼�����
    public static void CallExecuteActionAfterAnimation(Vector3 pos,ItemDetails itemDetails)
    {
        ExecuteActionAfterAnimation?.Invoke(pos, itemDetails);
    }
    public static event Action<int, Season> GameDayEvent;//ÿ��ί���¼�,ÿ�µ�һ�죬����һ�δ�ί��
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
