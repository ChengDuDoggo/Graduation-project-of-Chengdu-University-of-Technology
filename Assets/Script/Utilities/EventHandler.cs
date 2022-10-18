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
    public static event Action<ItemDetails, bool> ItemSelectedEvent;
    public static void CallItemSelectedEvent(ItemDetails itemDetails,bool isSelected)
    {
        ItemSelectedEvent?.Invoke(itemDetails, isSelected);
    }
}
