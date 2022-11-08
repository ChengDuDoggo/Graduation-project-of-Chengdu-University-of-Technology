using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MFarm.Map;
//实现AStar算法的逻辑,挂载至NPC身上来管理NPC
//NPC在移动时执行AStar算法计算出自己移动的最短路径
namespace MFarm.AStar
{
    public class AStar : MonoBehaviour
    {
        private GridNodes gridNodes;//获取每一张地图的网格节点信息
        private Node startNode;//起点
        private Node targetNode;//终点
        private int gridWidth;//地图的宽
        private int gridHeight;//地图的高
        private int originX;//原点X
        private int originY;//原点Y
        private List<Node> openNodeList;//创建一个列表来放选中的当前节点的周围八个节点
        private HashSet<Node> closedNodeList;//所有被选中的点
        //HashSet:和List功能相同但是所存放的数据是无序且唯一的,并且Contain检索的速度要比List快
        private bool pathFound;//是否找到路劲
        public void BuildPath(string sceneName,Vector2Int startPos, Vector2Int endPos)
        {
            pathFound = false;
            if (GenerateGridNodes(sceneName, startPos, endPos))
            {
                //查找最短路劲
                if (FindShortestPath())
                {
                    //构建NPC的移动路劲

                }

            }
        }
        /// <summary>
        /// 构建网格节点信息,初始化两个列表
        /// </summary>
        /// <param name="sceneName">场景名字</param>
        /// <param name="startPos">起点</param>
        /// <param name="endPos">终点</param>
        /// <returns></returns>
        private bool GenerateGridNodes(string sceneName,Vector2Int startPos,Vector2Int endPos)
        {
            if(GridMapManager.Instance.GetGridDimensions(sceneName,out Vector2Int gridDimensions,out Vector2Int gridOrigin))//是否有当前场景信息
            {
                //根据瓦片地图范围来构建网格移动节点范围数组
                gridNodes = new GridNodes(gridDimensions.x, gridDimensions.y);
                //保存临时变量
                gridWidth = gridDimensions.x;
                gridHeight = gridDimensions.y;
                originX = gridOrigin.x;
                originY = gridOrigin.y;
                //有当前地图信息了,可以将列表初始化了
                openNodeList = new List<Node>();
                closedNodeList = new HashSet<Node>();
            }
            else
                return false;

            //gridNodes的范围是从0,0开始所以需要减去原点坐标来得到实际位置
            startNode = gridNodes.GetGridNode(startPos.x - originX, startPos.y - originY);//获取NPC开始的那一个节点
            targetNode = gridNodes.GetGridNode(endPos.x - originX, endPos.y - originY);//获取NPC最终目标那一个节点
            for (int x = 0; x < gridWidth; x++)
            {
                for (int y = 0; y < gridHeight; y++)
                {
                    Vector3Int tilePos = new Vector3Int(x + originX, y + originY, 0);
                    TileDetails tile = GridMapManager.Instance.GetTileDetailsOnMousePosition(tilePos);
                    if (tile != null)
                    {
                        Node node = gridNodes.GetGridNode(x, y);
                        if (tile.isNPCObstacle)
                        {
                            node.isObstacle = true;
                        }
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// 查找最短路径函数
        /// </summary>
        /// <returns></returns>
        private bool FindShortestPath()
        {
            //添加起点
            openNodeList.Add(startNode);
            //节点排序,Node节点内涵比较函数
            while (openNodeList.Count > 0)
            {
                openNodeList.Sort();
                Node closeNode = openNodeList[0];//最近的节点
                openNodeList.RemoveAt(0);//找到了就把它从8个点列表中移除
                closedNodeList.Add(closeNode);//放入到所有被选中点列表中
                if (closeNode == targetNode)//找到路径了!
                {
                    pathFound = true;
                    break;
                }
                //计算周围8个Node补充到OpenList
            }
            return pathFound;
        }
    }
}

