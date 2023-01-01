using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="BulePrintDataList_SO",menuName ="Inventory/BulePrintDataList_SO")]
public class BluePrintDataList_SO : ScriptableObject//��Ž���ͼֽ������ݿ�
{
    public List<BulePrintDetailes> bulePrintDataList;
    public BulePrintDetailes GetBulePrintDetailes(int itemID)
    {
        return bulePrintDataList.Find(b => b.ID == itemID);//b�����ص�BulePrintDetailes���
    }
}
[System.Serializable]
public class BulePrintDetailes//����ͼֽ��
{
    //����ͼֽ������
    public int ID;
    public InventoryItem[] resourceItem = new InventoryItem[4];//���ڽ�������Ҫ���ĵ���Դ
    public GameObject buildPrefab;//������Ԥ����
}
