using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="ItemDataList_SO",menuName ="Inventory/ItemDataList")] //在Unity面板中右键可以创建指定文件指定路径进行可视化操作
//物品管理数据库,相当于一个数据库
public class ItemDataList_SO : ScriptableObject //ScriptableObject:数据配置存储基类,它是一个可以用来保存大量数据的数据容器,我们可以将它保存为自定义的数据资源文件
{
    public List<ItemDetails> itemDetailsList; 
}

//ScriptableObject,通过该数据库，我们可以将游戏中创建的大量相同的物体的引用来引用同一个或者不同数据库中的数据
//不必每一个相同的物品道具克隆出许多相同的脚本来造成资源性能浪费
//同时创建游戏管理数据库可以更方便更直观的操作游戏道具物品的数据
