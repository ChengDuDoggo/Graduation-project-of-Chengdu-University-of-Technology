using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MFarm.Map
{
    public class GridMapManager : MonoBehaviour
    {
        [Header("地图信息")]
        public List<MapData_SO> mapDataList;
        //定义一个字典来保存格子场景名字+格子坐标对应的瓦片信息
        private Dictionary<string, TileDetails> tileDetailesDict = new Dictionary<string, TileDetails>();
        /// <summary>
        /// 根据地图信息生成字典
        /// </summary>
        /// <param name="mapData">地图信息</param>
        private void InitTileDetailsDict(MapData_SO mapData)
        {
            foreach (TileProperty tileProperty in mapData.tileProperties)
            {
                TileDetails tileDetails = new TileDetails
                {
                    gridX = tileProperty.tileCoordinate.x,
                    gridY = tileProperty.tileCoordinate.y
                };//将格子数据库中的坐标信息传递给格子然后放入List
                //创建定义字典的key
                string key = tileDetails.gridX + "x" + tileDetails.gridY + "y" + mapData.sceneName;
                if (GetTileDetailes(key) != null)
                {
                    tileDetails = GetTileDetailes(key);//如果这个格子已经创建了key了，则直接赋值
                }
                switch (tileProperty.gridType)//传递类型信息
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
                    tileDetailesDict.Add(key, tileDetails);//给字典赋值
            }
        }
        /// <summary>
        /// 更具key返回瓦片信息
        /// </summary>
        /// <param name="key">x+y+地图名字</param>
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

