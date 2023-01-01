using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="LightPattenList_SO",menuName ="Light/Light Patten")]
public class LightPattenList_SO : ScriptableObject
{
    public List<LightDetails> lightPattenList;
    /// <summary>
    /// ���ݼ��ں����ڷ��صƹ�����
    /// </summary>
    /// <param name="season">����</param>
    /// <param name="lightShift">����</param>
    /// <returns></returns>
    public LightDetails GetLightDetails(Season season,LightShift lightShift)
    {
        return lightPattenList.Find(l => l.season == season && l.lightShift == lightShift);//��ķ����ʽ
    }
}
[System.Serializable]
public class LightDetails//�ƹ���
{
    public Season season;//ÿ�����ڲ�ͬ,ɢ���ĵƹ�Ҳ��ͬ
    public LightShift lightShift;
    public Color lightColor;
    public float lightAmount;//�ƹ�ǿ��
}
