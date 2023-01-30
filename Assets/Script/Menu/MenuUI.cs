using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUI : MonoBehaviour
{
    public GameObject[] panels;
    public void SwitchPanel(int index)
    {
        for (int i = 0; i < panels.Length; i++)
        {
            if (i == index)
            {
                //Tips:Unity在Hierarchy同层级面板中优先显示在底部的Object!
                panels[i].transform.SetAsLastSibling();//SetAsLastSibling()函数:将当前object中的Hierarchy面板的transfrom移动到列表最下面(最后)
            }
        }
    }
    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("EXIT GAME");
    }
    //一看到Sibling就应该想到和Hierarchy有关
}
