using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//管理Grid中所有网格节点(管理整张节点)
namespace MFarm.AStar
{
    public class GridNodes
    {
        //地图整体的宽和高
        private int width;
        private int height;
        private Node[,] gridNode;//二维数组来保存地图中的节点
        /// <summary>
        /// 构造函数初始化节点范围数组
        /// </summary>
        /// <param name="width">地图宽度</param>
        /// <param name="height">地图高度</param>
        public GridNodes(int width, int height)//构造函数,初始化
        {
            this.width = width;
            this.height = height;
            //手动将节点装入数组
            gridNode = new Node[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    gridNode[x, y] = new Node(new Vector2Int(x, y));//new Node时会自动调用Node中的构造函数将不同的x,y值传递给每一个节点
                }
            }
        }
        /// <summary>
        /// 根据x,y返回一个节点
        /// </summary>
        /// <param name="xPos">横坐标</param>
        /// <param name="yPos">纵坐标</param>
        /// <returns></returns>
        public Node GetGridNode(int xPos,int yPos)
        {
            if (xPos < width && yPos < height)//确保该坐标在地图范围之内
            {
                return gridNode[xPos, yPos];
            }
            else
            {
                Debug.Log("超出网格范围!");
                return null;
            }
        }
    }
}

