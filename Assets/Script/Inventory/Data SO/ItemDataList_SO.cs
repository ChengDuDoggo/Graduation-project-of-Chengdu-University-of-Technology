using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="ItemDataList_SO",menuName ="Inventory/ItemDataList")] //��Unity������Ҽ����Դ���ָ���ļ�ָ��·�����п��ӻ�����
//��Ʒ�������ݿ�,�൱��һ�����ݿ�
public class ItemDataList_SO : ScriptableObject //ScriptableObject:�������ô洢����,����һ��������������������ݵ���������,���ǿ��Խ�������Ϊ�Զ����������Դ�ļ�
{
    public List<ItemDetails> itemDetailsList; 
}

//ScriptableObject,ͨ�������ݿ⣬���ǿ��Խ���Ϸ�д����Ĵ�����ͬ�����������������ͬһ�����߲�ͬ���ݿ��е�����
//����ÿһ����ͬ����Ʒ���߿�¡�������ͬ�Ľű��������Դ�����˷�
//ͬʱ������Ϸ�������ݿ���Ը������ֱ�۵Ĳ�����Ϸ������Ʒ������
