using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MFarm.Map
{
    public class GridMapManager : MonoBehaviour
    {
        [Header("��ͼ��Ϣ")]
        public List<MapData_SO> mapDataList;
        //����һ���ֵ���������ӳ�������+���������Ӧ����Ƭ��Ϣ
        private Dictionary<string, TileDetails> tileDetailesDict = new Dictionary<string, TileDetails>();
        /// <summary>
        /// ���ݵ�ͼ��Ϣ�����ֵ�
        /// </summary>
        /// <param name="mapData">��ͼ��Ϣ</param>
        private void InitTileDetailsDict(MapData_SO mapData)
        {
            foreach (TileProperty tileProperty in mapData.tileProperties)
            {
                TileDetails tileDetails = new TileDetails
                {
                    gridX = tileProperty.tileCoordinate.x,
                    gridY = tileProperty.tileCoordinate.y
                };//���������ݿ��е�������Ϣ���ݸ�����Ȼ�����List
                //���������ֵ��key
                string key = tileDetails.gridX + "x" + tileDetails.gridY + "y" + mapData.sceneName;
                if (GetTileDetailes(key) != null)
                {
                    tileDetails = GetTileDetailes(key);//�����������Ѿ�������key�ˣ���ֱ�Ӹ�ֵ
                }
                switch (tileProperty.gridType)//����������Ϣ
                {
                    case GridType.Digable:
                        tileDetails.canDig = tileProperty.boolTypeValue;
                        break;
                    case GridType.DropItem:
                        tileDetails.canDropItm = tileProperty.boolTypeValue;
                        break;
                    case GridType.PlaceFurniture:
                        tileDetails.canPlaceFurniturn = tileProperty.boolTypeValue;
                        break;
                    case GridType.NPCObstacle:
                        tileDetails.isNPCObstacle = tileProperty.boolTypeValue;
                        break;
                }
                if (GetTileDetailes(key) != null)
                    tileDetailesDict[key] = tileDetails;
                else
                    tileDetailesDict.Add(key, tileDetails);//���ֵ丳ֵ
            }
        }
        /// <summary>
        /// ����key������Ƭ��Ϣ
        /// </summary>
        /// <param name="key">x+y+��ͼ����</param>
        /// <returns></returns>
        private TileDetails GetTileDetailes(string key)
        {
            if (tileDetailesDict.ContainsKey(key))
            {
                return tileDetailesDict[key];
            }
            else
            {
                return null;
            }
        }
    }
}

