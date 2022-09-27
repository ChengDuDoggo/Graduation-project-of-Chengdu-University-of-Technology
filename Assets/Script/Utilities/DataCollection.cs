//此脚本存放的是不同数据类型中的详细的不同数据
using UnityEngine;
[System.Serializable] //加了序列化之后，实例化该类后里面的公有变量可以直接在UnityInspector面板显示出来，可以十分方便的可视化设置变量
public class ItemDetails //物品详细信息类
{
    public int itemID;//物品的ID
    public string itemName;//物品的名字
    public ItemType itemType;//物品的类型
    public Sprite itemIcon;//物品的图片
    public Sprite itemOnWorldIcon;//物品在世界范围内内时的图片
    public string itemDescription;//物品的详情
    public int itemUseRadius;//物品的使用范围
    public bool canPickedUp;//物品能否被拾取
    public bool canDropped;//物品能否被丢掉
    public bool canCarried;//物品能否举起
    public int itemPrice;//物品的价格
    [Range(0, 1)]
    public float sellPercentage;//物品销售的百分比(售卖物品会相对于原价打折扣)
}
[System.Serializable]
public struct InventoryItem //相比于class,struct结构体更适合来存放背包数据,因为它会默认为0而不是null,避免背包过多报null
{
    public int itemID;
    public int itemAmount;
}
