using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//����ű���������
public class Crop : MonoBehaviour
{
    public CropDetails cropDetails;
    private int harvestActionCount;
    public void ProcessToolAction(ItemDetails tool)//ִ�й��ߵ���Ϊ
    {
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
            }
        }
    }
}
