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
[System.Serializable]
public class AnimatorType//Ҫ�л��Ĳ�ͬ���͵Ķ���
{
    public PartType partType;
    public PartName partName;
    public AnimatorOverrideController overrideController;
}
[System.Serializable]
//��������ڳ����������������
public class SerializableVector3
{
    public float x, y, z;

    public SerializableVector3(Vector3 pos)//���캯��,���౻����ʱ��������(��ʼ��)
    {
        this.x = pos.x;
        this.y = pos.y;
        this.z = pos.z;
    }
    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }
    public Vector2Int ToVector2Int()//ֻ����x,y��Ϊ����
    {
        return new Vector2Int((int)x, (int)y);
    }
}
[System.Serializable]
public class SceneItem//�����ڳ����е����������
{
    public int ItemID;
    public SerializableVector3 position;
}
