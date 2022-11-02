using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//这个脚本代表种子
public class Crop : MonoBehaviour
{
    public CropDetails cropDetails;
    private TileDetails tileDetails;
    private int harvestActionCount;
    public void ProcessToolAction(ItemDetails tool,TileDetails tile)//执行工具的行为
    {
        tileDetails = tile;
        //工具使用次数
        int requireActionCount = cropDetails.GetTotalRequireCount(tool.itemID);
        if (requireActionCount == -1) return;
        //判断是否有动画 树木

        //点击计数器
        if (harvestActionCount < requireActionCount)//点的不够(例如树木需要点5下才能砍倒)
        {
            harvestActionCount++;
            //播放粒子
            //播放声音
        }
        if (harvestActionCount >= requireActionCount)
        {
            if (cropDetails.generateAtPlayerPostion)//判断从地底生成(农作物)还是头顶生成(树木)
            {
                //生成农作物
                SpawnHarvestItems();
            }
        }
    }
    /// <summary>
    /// 生成农作物函数
    /// </summary>
    public void SpawnHarvestItems()
    {
        for (int i = 0; i < cropDetails.producedItemID.Length; i++)
        {
            int amountToProduce;
            if (cropDetails.produceMinAmount[i] == cropDetails.produceMaxAmount[i])
            {
                //代表只生成指定数量的
                amountToProduce = cropDetails.produceMinAmount[i];
            }
            else
            {
                //物品随机数量
                amountToProduce = Random.Range(cropDetails.produceMinAmount[i], cropDetails.produceMaxAmount[i] + 1);
            }
            //执行生成指定数量的物品
            for (int j = 0; j < amountToProduce; j++)
            {
                if (cropDetails.generateAtPlayerPostion)
                {
                    EventHandler.CallHarvestAtPlayerPostion(cropDetails.producedItemID[i]);
                }
                else//在世界地图上生成物品
                {

                }
            }
        }
        if (tileDetails != null)
        {
            tileDetails.daysSinceLastHarvest++;//表明重复收获了一次
            //判断是否可以重复生长
            if (cropDetails.daysToRegrow > 0 && tileDetails.daysSinceLastHarvest < cropDetails.regrowTimes)
            {
                tileDetails.growthDays = cropDetails.TotalGrowthDays - cropDetails.daysToRegrow;
                //刷新种子
                EventHandler.CallRefreshCurrentMap();//这里刷新种子阶段是直接刷新整张地图,而不是只单纯刷新这个种子
                /*这里只是改变了当前种子的生长阶段的时间,所以刷新整张地图的话,只有当前种子数据发生变化了,并不会影响地图上的其他物品*/
            }
            else//不可重复生长
            {
                tileDetails.daysSinceLastHarvest = -1;
                tileDetails.seedItemID = -1;
            }
            Destroy(gameObject);
        }
    }
}
