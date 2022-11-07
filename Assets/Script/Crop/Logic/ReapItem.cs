using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MFarm.CropPlant
{
    public class ReapItem : MonoBehaviour
    {
        private CropDetails cropDetails;
        private Transform PlayerTransform => FindObjectOfType<Player>().transform;
        public void InitCropData(int ID)
        {
            cropDetails = CropManager.Instance.GetCropDetails(ID);//通过ID查找种子信息
        }
        /// <summary>
        /// 生成果实函数
        /// </summary>
        public void SpawnHarvestItems()
        {
            if (cropDetails != null)//这里有BUG,无论如何cropDetails永远为null,割草后无法掉干草块!为了赶进度后续再解决
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

            }

        }
    }
}

