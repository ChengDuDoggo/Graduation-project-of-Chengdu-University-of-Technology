//专门存储枚举的一个类
public enum ItemType //通过枚举可以很方便的列出不同类型，而没必要每个类型创建一个类
{
    Seed,Commodity/*商品*/,Furniture,
    HolTool/*锄地的工具*/,ChopTool/*砍树的工具*/,BreakTool/*砸石头的工具*/,ReapTool/*割草的工具*/,WaterTool/*浇水的工具*/,CollectTool/*搜集菜的工具*/,
    ReapableScenery/*可以被割的杂草*/
}
public enum SlotType//UI中放东西的格子在不同地方是不同的类型(人物身上，商店里，盒子里等等)
{
    Bag,Shop,Box
}
public enum InventoryLocation//判断UI更新到哪里，是更新人物身上的格子还是箱子里的格子
{
    Player,Box
}
public enum PartType//判断拿到手上的工具类型
{
    None,Carry,Hoe,Break,Water,Collect,Chop,Reap
}
public enum PartName//物体需要身上那个部位拿取
{
    Body,Hair,Arm,Tool
}
public enum Season//季节类型
{
    春天,夏天,秋天,冬天
}
public enum GridType//网格的类型
{
    Digable,DropItem,PlaceFurniture,NPCObstacle//NPC行走的路径
}
public enum ParticaleEffectType//特效的类型
{
    None,LeavesFalling01,LeavesFalling02,Rock,ReapableScenery/*割稻草特效*/
}
public enum GameState//游戏状态枚举
{
    Gameplay/*游戏正常运行状态*/,Pause/*游戏暂停状态*/
}
public enum LightShift//游戏灯光(早上还是晚上)
{
    Morning,Night
}
public enum SoundName
{
    none,FootStepSoft,FootStepHard,
    Axe,Pickaxe,Hoe,Reap,Water,Basket,Chop,
    Pickup,Plant,TreeFalling,Rustle,
    AmbientCountryside1,AmbientCountryside2,MusicCalm1, MusicCalm2, MusicCalm4, MusicCalm5, MusicCalm6,MusicCalm3, AmbientIndoor1
}
