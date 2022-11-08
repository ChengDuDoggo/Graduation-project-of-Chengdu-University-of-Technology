using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//AStar�㷨,������̽ڵ�·��
namespace MFarm.AStar
{
    public class Node : IComparable<Node>//Unity�Դ��������ȽϵĽӿ�
    {
        public Vector2Int gridPosition;//�ڵ������

        public int gCost = 0;//�������(Start)���ӵľ���

        public int hCost = 0;//�����յ�(Target)���ӵľ���

        public int FCost => gCost + hCost;//��ǰ���ӵ�Ȩ��

        public bool isObstacle = false;//�жϵ�ǰ�����Ƿ����ϰ�

        public Node parentNode;//��ǰ�ڵ�ĸ��ڵ�

        public Node(Vector2Int pos)//���캯��,��ǰ�ڵ�ĳ�ʼ��
        {
            gridPosition = pos;
            parentNode = null;
        }

        /// <summary>
        /// Unity�Դ��ıȽϽӿ��еĺ�������
        /// </summary>
        /// <param name="other">��Ҫ�Ƚϵ�����һ���ڵ�</param>
        /// <returns>���ڷ���1,��ȷ���0,С�ڷ���-1</returns>
        public int CompareTo(Node other)
        {
            int result = FCost.CompareTo(other.FCost);//�Ƚϳ����ڸ�������С��Ȩ��,����1,0,-1
            if (result == 0)//���Ȩ�����
            {
                result = hCost.CompareTo(other.hCost);//��Ƚϸ��ӵ�hCost(�����յ���ӵľ���)
            }
            return result;
        }
    }
}

