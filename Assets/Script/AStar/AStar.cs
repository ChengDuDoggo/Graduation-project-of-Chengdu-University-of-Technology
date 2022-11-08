using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MFarm.Map;
//ʵ��AStar�㷨���߼�,������NPC����������NPC
//NPC���ƶ�ʱִ��AStar�㷨������Լ��ƶ������·��
namespace MFarm.AStar
{
    public class AStar : MonoBehaviour
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
        public void BuildPath(string sceneName,Vector2Int startPos, Vector2Int endPos)
        {
            pathFound = false;
            if (GenerateGridNodes(sceneName, startPos, endPos))
            {
                //�������·��
                if (FindShortestPath())
                {
                    //����NPC���ƶ�·��

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
        /// �������·������
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
            }
            return pathFound;
        }
    }
}

