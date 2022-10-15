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
}
