using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MFarm.Map;
//实现AStar算法的逻辑,挂载至NPC身上来管理NPC
//NPC在移动时执行AStar算法计算出自己移动的最短路径
namespace MFarm.AStar
{
    public class AStar : Singleton<AStar>
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
        /// <summary>
        /// 构建路径更新Stack每一步
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="startPos"></param>
        /// <param name="endPos"></param>
        /// <param name="npcMovementStep"></param>
        public void BuildPath(string sceneName,Vector2Int startPos, Vector2Int endPos,Stack<MovementStep> npcMovementStep)
        {
            pathFound = false;
            if (GenerateGridNodes(sceneName, startPos, endPos))
            {
                //查找最短路劲
                if (FindShortestPath())
                {
                    //构建NPC的移动路劲
                    UpdatePathOnMovementStepStack(sceneName, npcMovementStep);
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
                    var key = tilePos.x + "x" + tilePos.y + "y" + sceneName;
                    TileDetails tile = GridMapManager.Instance.GetTileDetailes(key);
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
        /// 查找最短路径所有node添加到closedNodeList
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
                EvaluateNeighbourNodes(closeNode);
            }
            return pathFound;
        }
        /// <summary>
        /// 评估周围八个点并生成对应消耗值
        /// </summary>
        /// <param name="currentNode"></param>
        private void EvaluateNeighbourNodes(Node currentNode)//计算相邻8个节点
        {
            Vector2Int currentNodePos = currentNode.gridPosition;//当前节点的坐标
            Node validNeighbourNode;//可行的节点
            //通过双重for循环来依次取得自身节点周围的8个节点
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0)//忽略自身节点
                    {
                        continue;
                    }
                    validNeighbourNode = GetValidNeighbourNode(currentNodePos.x + x, currentNodePos.y + y);
                    if (validNeighbourNode != null)
                    {
                        if (!openNodeList.Contains(validNeighbourNode))
                        {
                            validNeighbourNode.gCost = currentNode.gCost + GetDistance(currentNode, validNeighbourNode);
                            validNeighbourNode.hCost = GetDistance(validNeighbourNode, targetNode);
                            //链接父节点
                            validNeighbourNode.parentNode = currentNode;
                            openNodeList.Add(validNeighbourNode);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 找到有效的Node,非障碍,非已选择
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private Node GetValidNeighbourNode(int x,int y)//得到可行的相邻节点
        {
            if (x >= gridWidth || y >= gridHeight || x < 0 || y < 0)//判断没有超出地图范围
            {
                return null;
            }
            Node neighbourNode = gridNodes.GetGridNode(x, y);
            if (neighbourNode.isObstacle || closedNodeList.Contains(neighbourNode))//判断是否是障碍或者已经存入所有路径点列表中
            {
                return null;
            }
            else
            {
                return neighbourNode;
            }
        }
        /// <summary>
        /// 返回两点的距离值
        /// </summary>
        /// <param name="nodeA"></param>
        /// <param name="nodeB"></param>
        /// <returns>14的倍数+10的倍数</returns>
        private int GetDistance(Node nodeA,Node nodeB)//得到两点之间距离
        {
            int xDistance = Mathf.Abs(nodeA.gridPosition.x - nodeB.gridPosition.x);//判断两点之间X距离
            int yDistance = Mathf.Abs(nodeA.gridPosition.y - nodeB.gridPosition.y);//判断两点之间Y距离
            if (xDistance > yDistance)
            {
                return 14 * yDistance + 10 * (xDistance - yDistance);
            }
            return 14 * xDistance + 10 * (yDistance - xDistance);
        }
        /// <summary>
        /// 更新路劲每一步的坐标和场景名字
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="npcMovementStep"></param>
        private void UpdatePathOnMovementStepStack(string sceneName,Stack<MovementStep> npcMovementStep)
        {
            //我们是从终点依次推到起点,反向获取路劲(因为每一个节点都对应有一个父节点,除了起点)
            Node nextNode = targetNode;
            while (nextNode != null)
            {
                MovementStep newStep = new MovementStep();
                newStep.sceneName = sceneName;
                newStep.gridCoordinate = new Vector2Int(nextNode.gridPosition.x + originX, nextNode.gridPosition.y + originY);
                //压入堆栈
                npcMovementStep.Push(newStep);
                nextNode = nextNode.parentNode;//当这个节点压入堆栈之后就将它的父节点变为当前节点,依次下去就能反向得到整条路径
            }
            /*
             1.Stack<>:堆栈
               堆栈的作用和列表List<>大体相似,类似于数组用来存放一系列数据
               但是,堆栈严格执行"先进后出"的原则,即第一个压入(放入)堆栈的数据一定要等待在它之后压入的数据取出之后才可以取出
             2.堆栈不同于List的命令有:
               Stack<>.Peek():拿到堆栈最顶部的数据但不移除它
               Stack<>.Pop():拿到堆栈最顶部的数据并移除它
               Stack<>.Push():将一个数据压入堆栈(它只能是在堆栈的最顶部)
               其他命令就和List相同
             */
        }
    }
}

