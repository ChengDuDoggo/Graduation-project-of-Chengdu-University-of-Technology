using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCFunction : MonoBehaviour
{
    public InventoryBag_SO shopData;//先拿到商店中售卖的道具数据库
    private bool isOpen;
    private void Update()
    {
        if (isOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            //关闭背包
        }
    }
    public void OpenShop()
    {
        isOpen = true;
        EventHandler.CallBaseBagOpenEvent(SlotType.Shop, shopData);//触发打开背包委托事件
    }
}
