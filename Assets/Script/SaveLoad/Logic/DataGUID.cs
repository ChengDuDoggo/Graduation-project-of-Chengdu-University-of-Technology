using UnityEngine;
[ExecuteAlways]//����ýű���Ҫһֱ����
public class DataGUID : MonoBehaviour
{
    public string guid;
    private void Awake()
    {
        if (guid == string.Empty)
        {
            guid = System.Guid.NewGuid().ToString();//�����µ�GUID,GUID:��һ��ʮ��λ���ַ�������Ψһ�Ե�,����������ÿһ��SaveableItem
        }
    }
}
