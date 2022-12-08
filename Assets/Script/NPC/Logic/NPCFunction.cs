using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCFunction : MonoBehaviour
{
    public InventoryBag_SO shopData;//���õ��̵��������ĵ������ݿ�
    private bool isOpen;
    private void Update()
    {
        if (isOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            //�رձ���
            CloseShop();
        }
    }
    public void OpenShop()
    {
        isOpen = true;
        EventHandler.CallBaseBagOpenEvent(SlotType.Shop, shopData);//�����򿪱���ί���¼�
        EventHandler.CallUpdateGameStateEvent(GameState.Pause);//������Ϸ��ͣ�¼�
    }
    public void CloseShop()
    {
        isOpen = false;
        EventHandler.CallBaseBagCloseEvent(SlotType.Shop, shopData);//�����رձ���ί���¼�
        EventHandler.CallUpdateGameStateEvent(GameState.Gameplay);//������Ϸ�����¼�
    }
}
