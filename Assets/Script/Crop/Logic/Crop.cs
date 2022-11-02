using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//����ű���������
public class Crop : MonoBehaviour
{
    public CropDetails cropDetails;
    private TileDetails tileDetails;
    private int harvestActionCount;
    public void ProcessToolAction(ItemDetails tool,TileDetails tile)//ִ�й��ߵ���Ϊ
    {
        tileDetails = tile;
        //����ʹ�ô���
        int requireActionCount = cropDetails.GetTotalRequireCount(tool.itemID);
        if (requireActionCount == -1) return;
        //�ж��Ƿ��ж��� ��ľ

        //���������
        if (harvestActionCount < requireActionCount)//��Ĳ���(������ľ��Ҫ��5�²��ܿ���)
        {
            harvestActionCount++;
            //��������
            //��������
        }
        if (harvestActionCount >= requireActionCount)
        {
            if (cropDetails.generateAtPlayerPostion)//�жϴӵص�����(ũ����)����ͷ������(��ľ)
            {
                //����ũ����
                SpawnHarvestItems();
            }
        }
    }
    /// <summary>
    /// ����ũ���ﺯ��
    /// </summary>
    public void SpawnHarvestItems()
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

                }
            }
        }
        if (tileDetails != null)
        {
            tileDetails.daysSinceLastHarvest++;//�����ظ��ջ���һ��
            //�ж��Ƿ�����ظ�����
            if (cropDetails.daysToRegrow > 0 && tileDetails.daysSinceLastHarvest < cropDetails.regrowTimes)
            {
                tileDetails.growthDays = cropDetails.TotalGrowthDays - cropDetails.daysToRegrow;
                //ˢ������
                EventHandler.CallRefreshCurrentMap();//����ˢ�����ӽ׶���ֱ��ˢ�����ŵ�ͼ,������ֻ����ˢ���������
                /*����ֻ�Ǹı��˵�ǰ���ӵ������׶ε�ʱ��,����ˢ�����ŵ�ͼ�Ļ�,ֻ�е�ǰ�������ݷ����仯��,������Ӱ���ͼ�ϵ�������Ʒ*/
            }
            else//�����ظ�����
            {
                tileDetails.daysSinceLastHarvest = -1;
                tileDetails.seedItemID = -1;
            }
            Destroy(gameObject);
        }
    }
}
