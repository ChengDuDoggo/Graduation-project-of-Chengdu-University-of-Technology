//此脚本存放的是不同数据类型中的详细的不同数据
using System.Collections.Generic;
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
[System.Serializable]
public class AnimatorType//要切换的不同类型的动画
{
    public PartType partType;
    public PartName partName;
    public AnimatorOverrideController overrideController;
}
[System.Serializable]
//该类代表在场景中物体的坐标类
public class SerializableVector3//无论是2D游戏还是3D游戏,都需要使用此方法创建此类来实现物品的坐标获取
{
    public float x, y, z;

    public SerializableVector3(Vector3 pos)//构造函数,该类被触发时立即调用(初始化)
    {
        this.x = pos.x;
        this.y = pos.y;
        this.z = pos.z;
    }
    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }
    public Vector2Int ToVector2Int()//只返回x,y且为整形
    {
        return new Vector2Int((int)x, (int)y);
    }
}
[System.Serializable]
public class SceneItem//代表在场景中的物体的类型
{
    public int ItemID;
    public SerializableVector3 position;
}
[System.Serializable]
public class SceneFurniture//代表场景中玩家自己新建造的家具类
{
    //它们有什么属性↓
    //TODO:更多属性信息
    public int ItemID;
    public SerializableVector3 position;//它们有ID和位置信息
    public int boxIndex;
}
[System.Serializable]
public class TileProperty//瓦片属性类,定义一片瓦片是否可以种植,挖掘,丢弃道具,放置家具等等
{
    public Vector2Int tileCoordinate;//瓦片坐标
    public GridType gridType;
    public bool boolTypeValue;
}
[System.Serializable]
public class TileDetails//定义一个格子的具体信息类，这个格子上面有什么属性
{
    public int gridX, gridY;//格子的坐标
    public bool canDig;//格子能被挖掘
    public bool canDropItm;//格子上能丢物品
    public bool canPlaceFurniturn;//格子能放置家具
    public bool isNPCObstacle;//格子是NPC的路径
    public int daysSinceDug = -1;//格子被挖掘后开始计算天数，来控制农作物成长的逻辑
    public int daysSinceWatered = -1;//格子被浇水后计算天数
    public int seedItemID = -1;//格子上被种植的种子的ID
    public int growthDays = -1;//植物成长的天数
    public int daysSinceLastHarvest = -1;//某些植物可以反复种植，在收获后回到一定的天数，阶段
}
[System.Serializable]
public class NPCPosition
{
    public Transform npc;
    public string startScene;
    public Vector3 position;
}
//场景路径
[System.Serializable]
public class SceneRoute
{
    public string fromSceneName;
    public string gotoSceneName;
    public List<ScenePath> scenePathList;
}
[System.Serializable]
public class ScenePath
{
    public string sceneName;
    public Vector2Int fromGridCell;
    public Vector2Int gotoGridCell;
}
