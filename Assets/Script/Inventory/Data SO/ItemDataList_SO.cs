using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="ItemDataList_SO",menuName ="Inventory/ItemDataList")] //��Unity������Ҽ����Դ���ָ���ļ�ָ��·�����п��ӻ�����
public class ItemDataList_SO : ScriptableObject //��Ʒ�������ݿ�
{
    public List<ItemDetails> itemDetailsList;
}
