using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="BulePrintDataList_SO",menuName ="Inventory/BulePrintDataList_SO")]
public class BluePrintDataList_SO : ScriptableObject//存放建造图纸类的数据库
{
    public List<BulePrintDetailes> bulePrintDataList;
    public BulePrintDetailes GetBulePrintDetailes(int itemID)
    {
        return bulePrintDataList.Find(b => b.ID == itemID);//b代表返回的BulePrintDetailes结果
    }
}
[System.Serializable]
public class BulePrintDetailes//建造图纸类
{
    //建造图纸的属性
    public int ID;
    public InventoryItem[] resourceItem = new InventoryItem[4];//用于建造所需要消耗的资源
    public GameObject buildPrefab;//建筑的预制体
}
