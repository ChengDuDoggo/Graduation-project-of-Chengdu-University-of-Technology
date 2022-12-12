using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="InventoryBag_SO",menuName ="Inventory/InventoryBag_SO")]
public class InventoryBag_SO : ScriptableObject//背包数据库
{
    public List<InventoryItem> itemList;
    public InventoryItem GetInventoryItem(int ID)
    {
        return itemList.Find(i => i.itemID == ID);//拉姆达表达式
    }
}
