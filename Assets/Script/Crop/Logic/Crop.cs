using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//这个脚本代表种子
public class Crop : MonoBehaviour
{
    public CropDetails cropDetails;
    private int harvestActionCount;
    public void ProcessToolAction(ItemDetails tool)//执行工具的行为
    {
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
            }
        }
    }
}
