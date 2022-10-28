using UnityEngine;
[System.Serializable]//�������л�֮��,�����������ܿ��ӻ�
public class CropDetails
{
    public int seedItemID;
    [Header("��ͬ�׶γɳ���Ҫ������")]
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
    }//���ӳɳ���Ҫ���ܵ�����
    [Header("��ͬ�����׶���Ʒ��Prefab")]
    public GameObject[] growsPrefabs;
    [Header("��ͬ�׶ε�ͼƬ")]
    public Sprite[] growthSprites;
    [Header("����ֲ�ļ���")]
    public Season[] seasons;
    [Space]
    [Header("�ո��")]
    public int[] harvestToolItemID;
    [Header("ÿ�ֹ��ߵ�ʹ�ô���")]
    public int[] requireActionCount;
    [Header("ת������Ʒ��ID")]
    public int transferItemID;
    [Space]
    [Header("�ո��ʵ��Ϣ")]
    public int[] producedItemID;
    public int[] produceMinAmount;//�ո��ʵ����С�����
    public int[] produceMaxAmount;//�ո��ʵ���������
    public Vector2 spawnRadius;//���ɹ�ʵ����С��Χ
    [Header("�ٴ�����ʱ��")]
    public int daysToRegrow;
    public int regrowTimes;//�ٴ���������
    [Header("Options")]
    public bool generateAtPlayerPostion;
    public bool hasAmiontion;//�Ƿ��ж���
    public bool hasParticalEffect;//�Ƿ���������Ч
    //TODO:��Ч,��Ч��
}