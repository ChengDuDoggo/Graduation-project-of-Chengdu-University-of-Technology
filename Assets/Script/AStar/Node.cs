using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//AStar算法,计算最短节点路径(管理每一个节点)
namespace MFarm.AStar
{
    public class Node : IComparable<Node>//Unity自带的用作比较的接口
    {
        public Vector2Int gridPosition;//节点的坐标

        public int gCost = 0;//距离起点(Start)格子的距离

        public int hCost = 0;//距离终点(Target)格子的距离

        public int FCost => gCost + hCost;//当前格子的权重

        public bool isObstacle = false;//判断当前格子是否是障碍

        public Node parentNode;//当前节点的父节点

        public Node(Vector2Int pos)//构造函数,当前节点的初始化
        {
            gridPosition = pos;
            parentNode = null;
        }

        /// <summary>
        /// Unity自带的比较接口中的函数方法
        /// </summary>
        /// <param name="other">所要比较的另外一个节点</param>
        /// <returns>大于返回1,相等返回0,小于返回-1</returns>
        public int CompareTo(Node other)
        {
            int result = FCost.CompareTo(other.FCost);//比较出相邻格子中最小的权重,返回1,0,-1
            if (result == 0)//如果权重相等
            {
                result = hCost.CompareTo(other.hCost);//则比较格子的hCost(距离终点格子的距离)
            }
            return result;
        }
    }
}

