using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="LightPattenList_SO",menuName ="Light/Light Patten")]
public class LightPattenList_SO : ScriptableObject
{
    
}
public class LightDetails//灯光类
{
    public Season season;//每个季节不同,散发的灯光也不同
    public Color lightColor;
    public float lightAmount;//灯光强度
}
