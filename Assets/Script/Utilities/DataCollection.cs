//�˽ű���ŵ��ǲ�ͬ���������е���ϸ�Ĳ�ͬ����
using UnityEngine;
[System.Serializable] //�������л�֮��ʵ�������������Ĺ��б�������ֱ����UnityInspector�����ʾ����������ʮ�ַ���Ŀ��ӻ����ñ���
public class ItemDetails //��Ʒ��ϸ��Ϣ��
{
    public int itemID;//��Ʒ��ID
    public string itemName;//��Ʒ������
    public ItemType itemType;//��Ʒ������
    public Sprite itemIcon;//��Ʒ��ͼƬ
    public Sprite itemOnWorldIcon;//��Ʒ�����緶Χ����ʱ��ͼƬ
    public string itemDescription;//��Ʒ������
    public int itemUseRadius;//��Ʒ��ʹ�÷�Χ
    public bool canPickedUp;//��Ʒ�ܷ�ʰȡ
    public bool canDropped;//��Ʒ�ܷ񱻶���
    public bool canCarried;//��Ʒ�ܷ����
    public int itemPrice;//��Ʒ�ļ۸�
    [Range(0, 1)]
    public float sellPercentage;//��Ʒ���۵İٷֱ�(������Ʒ�������ԭ�۴��ۿ�)
}
[System.Serializable]
public struct InventoryItem //�����class,struct�ṹ����ʺ�����ű�������,��Ϊ����Ĭ��Ϊ0������null,���ⱳ�����౨null
{
    public int itemID;
    public int itemAmount;
}
