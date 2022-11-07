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
            cropDetails = CropManager.Instance.GetCropDetails(ID);//ͨ��ID����������Ϣ
        }
        /// <summary>
        /// ���ɹ�ʵ����
        /// </summary>
        public void SpawnHarvestItems()
        {
            if (cropDetails != null)//������BUG,�������cropDetails��ԶΪnull,��ݺ��޷����ɲݿ�!Ϊ�˸Ͻ��Ⱥ����ٽ��
            {
                for (int i = 0; i < cropDetails.producedItemID.Length; i++)
                {
                    int amountToProduce;
                    if (cropDetails.produceMinAmount[i] == cropDetails.produceMaxAmount[i])
                    {
                        //����ֻ����ָ��������
                        amountToProduce = cropDetails.produceMinAmount[i];
                    }
                    else
                    {
                        //��Ʒ�������
                        amountToProduce = Random.Range(cropDetails.produceMinAmount[i], cropDetails.produceMaxAmount[i] + 1);
                    }
                    //ִ������ָ����������Ʒ
                    for (int j = 0; j < amountToProduce; j++)
                    {
                        if (cropDetails.generateAtPlayerPostion)
                        {
                            EventHandler.CallHarvestAtPlayerPostion(cropDetails.producedItemID[i]);
                        }
                        else//�������ͼ��������Ʒ
                        {
                            //�ж�Ӧ�����ɵ���Ʒ����
                            var dirX = transform.position.x > PlayerTransform.position.x ? 1 : -1;
                            //һ����Χ�����
                            var spawnPos = new Vector3(transform.position.x + Random.Range(dirX, cropDetails.spawnRadius.x * dirX), transform.position.y + Random.Range(-cropDetails.spawnRadius.y, cropDetails.spawnRadius.y), 0);
                            //�����糡����������Ʒ
                            EventHandler.CallInstantiateItemInScene(cropDetails.producedItemID[i], spawnPos);
                        }
                    }
                }

            }

        }
    }
}

