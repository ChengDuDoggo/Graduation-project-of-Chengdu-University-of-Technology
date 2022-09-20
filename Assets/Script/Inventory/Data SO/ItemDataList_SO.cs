using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="ItemDataList_SO",menuName ="Inventory/ItemDataList")] //在Unity面板中右键可以创建指定文件指定路径进行可视化操作
public class ItemDataList_SO : ScriptableObject //物品管理数据库
{
    public List<ItemDetails> itemDetailsList;
}
