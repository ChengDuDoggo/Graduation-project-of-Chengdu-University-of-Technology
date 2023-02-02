using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MFarm.Save
{
    [System.Serializable]
    //这只是一个存放数据的类
    public class GameSaveData//创建一个类来记录所有Save时需要保存的数据来保存到本地下次Load游戏时直接将该类中所有数据全部输入至游戏中去
    {
        public string dataSceneName;//需要保存场景名
        public Dictionary<string, SerializableVector3> characterPosDict;//人物坐标数据(string:NPC或人物的名字)
        public Dictionary<string, List<SceneItem>> sceneItemDict;//场景中的道具数据
        public Dictionary<string, List<SceneFurniture>> sceneFurnitureDict;//场景中的家具数据
        public Dictionary<string, TileDetails> tileDetailsDict;//场景中的瓦片信息需要记录
        public Dictionary<string, bool> firstLoadDict;//场景是否第一次加载也需要记录
        public Dictionary<string, List<InventoryItem>> inventoryDict;//背包中的物品数据也需要保存
        public Dictionary<string, int> timeDict;//时间也需要保存
        public int playerMoney;//玩家的钱需要保存
        //NPC
        public string targetScene;//NPC的目标场景也需要保存
        public bool interactable;//NPC是否可以互动也需要保存
        public int animationInstanceID;//NPC动画不能直接实例化保存为数据所有通过动画ID来保存
    }
}

