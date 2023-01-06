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
    public bool hasAnimation;//�Ƿ��ж���
    public bool hasParticalEffect;//�Ƿ���������Ч
    public Vector3 effectPos;//��Ч�����������
    public SoundName soundEffect;
    public ParticaleEffectType effectType;
    public bool CheckToolAvailable(int toolID)//�жϵ�ǰ���ֹ����Ƿ����
    {
        foreach (var tool in harvestToolItemID)
        {
            if (tool == toolID)
                return true;
        }
        return false;
    }
    /// <summary>
    /// ���ع�����Ҫʹ�õĴ���
    /// </summary>
    /// <param name="toolID">����ID</param>
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
