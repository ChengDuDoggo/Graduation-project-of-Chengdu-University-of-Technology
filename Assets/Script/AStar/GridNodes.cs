using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//����Grid����������ڵ�
namespace MFarm.AStar
{
    public class GridNodes
    {
        //��ͼ����Ŀ�͸�
        private int width;
        private int height;
        private Node[,] gridNode;//��ά�����������ͼ�еĽڵ�
        /// <summary>
        /// ���캯����ʼ���ڵ㷶Χ����
        /// </summary>
        /// <param name="width">��ͼ���</param>
        /// <param name="height">��ͼ�߶�</param>
        public GridNodes(int width, int height)//���캯��,��ʼ��
        {
            this.width = width;
            this.height = height;
            //�ֶ����ڵ�װ������
            gridNode = new Node[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    gridNode[x, y] = new Node(new Vector2Int(x, y));//new Nodeʱ���Զ�����Node�еĹ��캯������ͬ��x,yֵ���ݸ�ÿһ���ڵ�
                }
            }
        }
        /// <summary>
        /// ����x,y����һ���ڵ�
        /// </summary>
        /// <param name="xPos">������</param>
        /// <param name="yPos">������</param>
        /// <returns></returns>
        public Node GetGridNode(int xPos,int yPos)
        {
            if (xPos < width && yPos < height)//ȷ���������ڵ�ͼ��Χ֮��
            {
                return gridNode[xPos, yPos];
            }
            else
            {
                Debug.Log("��������Χ!");
                return null;
            }
        }
    }
}

