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
                //Tips:Unity��Hierarchyͬ�㼶�����������ʾ�ڵײ���Object!
                panels[i].transform.SetAsLastSibling();//SetAsLastSibling()����:����ǰobject�е�Hierarchy����transfrom�ƶ����б�������(���)
            }
        }
    }
    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("EXIT GAME");
    }
    //һ����Sibling��Ӧ���뵽��Hierarchy�й�
}
