using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//ʱ������ݿ����д洢��NPC���ض�ʱ����ض���Ϊ����
//����NPC����Ϸһ��ʼ������ĳ������λ��
//NPC�ھ���һ��ʱ���Ӧ���ƶ���ʲôλ��,����ʲô����
[CreateAssetMenu(fileName = "SchedulDataList_SO",menuName = "NPC Schedule/SchedulDataList")]
public class SchedulDataList_SO : ScriptableObject
{
    public List<SchedulDetails> schedulList;
}
