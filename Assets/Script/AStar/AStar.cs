using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MFarm.Map;
//ʵ��AStar�㷨���߼�,������NPC����������NPC
//NPC���ƶ�ʱִ��AStar�㷨������Լ��ƶ������·��
namespace MFarm.AStar
{
    public class AStar : Singleton<AStar>
    {
        private GridNodes gridNodes;//��ȡÿһ�ŵ�ͼ������ڵ���Ϣ
        private Node startNode;//���
        private Node targetNode;//�յ�
        private int gridWidth;//��ͼ�Ŀ�
        private int gridHeight;//��ͼ�ĸ�
        private int originX;//ԭ��X
        private int originY;//ԭ��Y
        private List<Node> openNodeList;//����һ���б�����ѡ�еĵ�ǰ�ڵ����Χ�˸��ڵ�
        private HashSet<Node> closedNodeList;//���б�ѡ�еĵ�
        //HashSet:��List������ͬ��������ŵ�������������Ψһ��,����Contain�������ٶ�Ҫ��List��
        private bool pathFound;//�Ƿ��ҵ�·��
        /// <summary>
        /// ����·������Stackÿһ��
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
                //�������·��
                if (FindShortestPath())
                {
                    //����NPC���ƶ�·��
                    UpdatePathOnMovementStepStack(sceneName, npcMovementStep);
                }

            }
        }
        /// <summary>
        /// ��������ڵ���Ϣ,��ʼ�������б�
        /// </summary>
        /// <param name="sceneName">��������</param>
        /// <param name="startPos">���</param>
        /// <param name="endPos">�յ�</param>
        /// <returns></returns>
        private bool GenerateGridNodes(string sceneName,Vector2Int startPos,Vector2Int endPos)
        {
            if(GridMapManager.Instance.GetGridDimensions(sceneName,out Vector2Int gridDimensions,out Vector2Int gridOrigin))//�Ƿ��е�ǰ������Ϣ
            {
                //������Ƭ��ͼ��Χ�����������ƶ��ڵ㷶Χ����
                gridNodes = new GridNodes(gridDimensions.x, gridDimensions.y);
                //������ʱ����
                gridWidth = gridDimensions.x;
                gridHeight = gridDimensions.y;
                originX = gridOrigin.x;
                originY = gridOrigin.y;
                //�е�ǰ��ͼ��Ϣ��,���Խ��б��ʼ����
                openNodeList = new List<Node>();
                closedNodeList = new HashSet<Node>();
            }
            else
                return false;

            //gridNodes�ķ�Χ�Ǵ�0,0��ʼ������Ҫ��ȥԭ���������õ�ʵ��λ��
            startNode = gridNodes.GetGridNode(startPos.x - originX, startPos.y - originY);//��ȡNPC��ʼ����һ���ڵ�
            targetNode = gridNodes.GetGridNode(endPos.x - originX, endPos.y - originY);//��ȡNPC����Ŀ����һ���ڵ�
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
        /// �������·������node��ӵ�closedNodeList
        /// </summary>
        /// <returns></returns>
        private bool FindShortestPath()
        {
            //������
            openNodeList.Add(startNode);
            //�ڵ�����,Node�ڵ��ں��ȽϺ���
            while (openNodeList.Count > 0)
            {
                openNodeList.Sort();
                Node closeNode = openNodeList[0];//����Ľڵ�
                openNodeList.RemoveAt(0);//�ҵ��˾Ͱ�����8�����б����Ƴ�
                closedNodeList.Add(closeNode);//���뵽���б�ѡ�е��б���
                if (closeNode == targetNode)//�ҵ�·����!
                {
                    pathFound = true;
                    break;
                }
                //������Χ8��Node���䵽OpenList
                EvaluateNeighbourNodes(closeNode);
            }
            return pathFound;
        }
        /// <summary>
        /// ������Χ�˸��㲢���ɶ�Ӧ����ֵ
        /// </summary>
        /// <param name="currentNode"></param>
        private void EvaluateNeighbourNodes(Node currentNode)//��������8���ڵ�
        {
            Vector2Int currentNodePos = currentNode.gridPosition;//��ǰ�ڵ������
            Node validNeighbourNode;//���еĽڵ�
            //ͨ��˫��forѭ��������ȡ������ڵ���Χ��8���ڵ�
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0)//��������ڵ�
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
                            //���Ӹ��ڵ�
                            validNeighbourNode.parentNode = currentNode;
                            openNodeList.Add(validNeighbourNode);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// �ҵ���Ч��Node,���ϰ�,����ѡ��
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private Node GetValidNeighbourNode(int x,int y)//�õ����е����ڽڵ�
        {
            if (x >= gridWidth || y >= gridHeight || x < 0 || y < 0)//�ж�û�г�����ͼ��Χ
            {
                return null;
            }
            Node neighbourNode = gridNodes.GetGridNode(x, y);
            if (neighbourNode.isObstacle || closedNodeList.Contains(neighbourNode))//�ж��Ƿ����ϰ������Ѿ���������·�����б���
            {
                return null;
            }
            else
            {
                return neighbourNode;
            }
        }
        /// <summary>
        /// ��������ľ���ֵ
        /// </summary>
        /// <param name="nodeA"></param>
        /// <param name="nodeB"></param>
        /// <returns>14�ı���+10�ı���</returns>
        private int GetDistance(Node nodeA,Node nodeB)//�õ�����֮�����
        {
            int xDistance = Mathf.Abs(nodeA.gridPosition.x - nodeB.gridPosition.x);//�ж�����֮��X����
            int yDistance = Mathf.Abs(nodeA.gridPosition.y - nodeB.gridPosition.y);//�ж�����֮��Y����
            if (xDistance > yDistance)
            {
                return 14 * yDistance + 10 * (xDistance - yDistance);
            }
            return 14 * xDistance + 10 * (yDistance - xDistance);
        }
        /// <summary>
        /// ����·��ÿһ��������ͳ�������
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="npcMovementStep"></param>
        private void UpdatePathOnMovementStepStack(string sceneName,Stack<MovementStep> npcMovementStep)
        {
            //�����Ǵ��յ������Ƶ����,�����ȡ·��(��Ϊÿһ���ڵ㶼��Ӧ��һ�����ڵ�,�������)
            Node nextNode = targetNode;
            while (nextNode != null)
            {
                MovementStep newStep = new MovementStep();
                newStep.sceneName = sceneName;
                newStep.gridCoordinate = new Vector2Int(nextNode.gridPosition.x + originX, nextNode.gridPosition.y + originY);
                //ѹ���ջ
                npcMovementStep.Push(newStep);
                nextNode = nextNode.parentNode;//������ڵ�ѹ���ջ֮��ͽ����ĸ��ڵ��Ϊ��ǰ�ڵ�,������ȥ���ܷ���õ�����·��
            }
            /*
             1.Stack<>:��ջ
               ��ջ�����ú��б�List<>��������,�����������������һϵ������
               ����,��ջ�ϸ�ִ��"�Ƚ����"��ԭ��,����һ��ѹ��(����)��ջ������һ��Ҫ�ȴ�����֮��ѹ�������ȡ��֮��ſ���ȡ��
             2.��ջ��ͬ��List��������:
               Stack<>.Peek():�õ���ջ��������ݵ����Ƴ���
               Stack<>.Pop():�õ���ջ��������ݲ��Ƴ���
               Stack<>.Push():��һ������ѹ���ջ(��ֻ�����ڶ�ջ�����)
               ��������ͺ�List��ͬ
             */
        }
    }
}

