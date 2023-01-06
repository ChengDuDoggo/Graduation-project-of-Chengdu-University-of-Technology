using UnityEngine;
[System.Serializable]//必须序列化之后,在引擎面板才能可视化
public class CropDetails
{
    public int seedItemID;
    [Header("不同阶段成长需要的天数")]
    public int[] growthDays;
    public int TotalGrowthDays
    {
        get
        {
            int amount = 0;
            foreach (var days in growthDays)
            {
                amount += days;
            }
            return amount;
        }
    }//种子成长需要的总的天数
    [Header("不同生长阶段物品的Prefab")]
    public GameObject[] growsPrefabs;
    [Header("不同阶段的图片")]
    public Sprite[] growthSprites;
    [Header("可种植的季节")]
    public Season[] seasons;
    [Space]
    [Header("收割工具")]
    public int[] harvestToolItemID;
    [Header("每种工具的使用次数")]
    public int[] requireActionCount;
    [Header("转换新物品的ID")]
    public int transferItemID;
    [Space]
    [Header("收割果实信息")]
    public int[] producedItemID;
    public int[] produceMinAmount;//收割果实的最小获得数
    public int[] produceMaxAmount;//收割果实的最大获得数
    public Vector2 spawnRadius;//生成果实的最小范围
    [Header("再次生长时间")]
    public int daysToRegrow;
    public int regrowTimes;//再次生长几次
    [Header("Options")]
    public bool generateAtPlayerPostion;
    public bool hasAnimation;//是否有动画
    public bool hasParticalEffect;//是否有粒子特效
    public Vector3 effectPos;//特效产生点的坐标
    public SoundName soundEffect;
    public ParticaleEffectType effectType;
    public bool CheckToolAvailable(int toolID)//判断当前所持工具是否可用
    {
        foreach (var tool in harvestToolItemID)
        {
            if (tool == toolID)
                return true;
        }
        return false;
    }
    /// <summary>
    /// 返回工具需要使用的次数
    /// </summary>
    /// <param name="toolID">工具ID</param>
    /// <returns></returns>
    public int GetTotalRequireCount(int toolID)
    {
        for (int i = 0; i < harvestToolItemID.Length; i++)
        {
            if (harvestToolItemID[i] == toolID)
                return requireActionCount[i];
        }
        return -1;
    }
}
