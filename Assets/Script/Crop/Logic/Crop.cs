using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//这个脚本代表种子
public class Crop : MonoBehaviour
{
    public CropDetails cropDetails;
    public TileDetails tileDetails;
    private int harvestActionCount;
    public bool canHarvest => tileDetails.growthDays >= cropDetails.TotalGrowthDays;//判断是否能够收获
    private Animator anim;
    private Transform PlayerTransform => FindObjectOfType<Player>().transform;
    public void ProcessToolAction(ItemDetails tool,TileDetails tile)//执行工具的行为
    {
        tileDetails = tile;
        //工具使用次数
        int requireActionCount = cropDetails.GetTotalRequireCount(tool.itemID);
        if (requireActionCount == -1) return;
        anim = GetComponentInChildren<Animator>();
        //点击计数器
        if (harvestActionCount < requireActionCount)//点的不够(例如树木需要点5下才能砍倒)
        {
            harvestActionCount++;
            //判断是否有动画 树木
            if (anim != null&&cropDetails.hasAnimation)
            {
                if (PlayerTransform.position.x < transform.position.x)
                {
                    anim.SetTrigger("RotateRight");
                }
                else
                {
                    anim.SetTrigger("RotateLeft");
                }
            }
            //播放粒子
            if (cropDetails.hasParticalEffect)
            {
                EventHandler.CallParticaleEffectEvent(cropDetails.effectType, transform.position + cropDetails.effectPos);
            }

            //播放声音
            if (cropDetails.soundEffect != SoundName.none)
            {
                EventHandler.CallPlaySoundEvent(cropDetails.soundEffect);
            }
        }
        if (harvestActionCount >= requireActionCount)
        {
            if (cropDetails.generateAtPlayerPostion||!cropDetails.hasAnimation)//判断从地底生成(农作物)还是头顶生成(树木)
            {
                //生成农作物
                SpawnHarvestItems();
            }
            else if(cropDetails.hasAnimation)
            {
                if (PlayerTransform.position.x < transform.position.x)
                {
                    anim.SetTrigger("FallingRight");
                }
                else
                {
                    anim.SetTrigger("FallingLeft");
                }
                EventHandler.CallPlaySoundEvent(SoundName.TreeFalling);
                StartCoroutine(HarvestAfterAnimator());
            }
        }
    }
    /// <summary>
    /// 在动画播放完成之后生成果实的协程方法
    /// </summary>
    /// <returns></returns>
    private IEnumerator HarvestAfterAnimator()
    {
        while (!anim.GetCurrentAnimatorStateInfo(0).IsName("END"))//当"END"动画没有播放完毕则一直循环为null直到"END"动画播放完毕才生成果实
        {
            yield return null;
        }
        SpawnHarvestItems();
        //转化为新物体
        if (cropDetails.transferItemID > 0)//先判断当前物体有转化的物品
        {
            CreateTransferCrop();
        }

    }
    /// <summary>
    /// 生成转换的物体
    /// </summary>
    private void CreateTransferCrop()
    {
        tileDetails.seedItemID = cropDetails.transferItemID;//转化物品
        tileDetails.daysSinceLastHarvest = -1;//现在不能再收割了
        tileDetails.growthDays = 0;
        EventHandler.CallRefreshCurrentMap();//刷新一下当前物品的信息
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
                    //判断应该生成的物品方向
                    var dirX = transform.position.x > PlayerTransform.position.x ? 1 : -1;
                    //一定范围内随机
                    var spawnPos = new Vector3(transform.position.x + Random.Range(dirX, cropDetails.spawnRadius.x * dirX), transform.position.y + Random.Range(-cropDetails.spawnRadius.y, cropDetails.spawnRadius.y), 0);
                    //在世界场景中生成物品
                    EventHandler.CallInstantiateItemInScene(cropDetails.producedItemID[i], spawnPos);
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
