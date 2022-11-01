using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
namespace MFarm.Map
{
    public class GridMapManager : Singleton<GridMapManager>
    {
        [Header("�ֵ���Ƭ�л���Ϣ")]
        public RuleTile digTile;//ʵ��������������Ƭ
        public RuleTile waterTile;
        private Tilemap digTilemap;//ʵ��������Ϳ��������Ƭ����Ƭ��ͼ
        private Tilemap waterTilemap;
        [Header("��ͼ��Ϣ")]
        public List<MapData_SO> mapDataList;
        //����һ���ֵ���������ӳ�������+���������Ӧ����Ƭ��Ϣ
        private Dictionary<string, TileDetails> tileDetailesDict = new Dictionary<string, TileDetails>();
        private Grid currentGrid;
        private Season currentSeason;
        private void OnEnable()
        {
            EventHandler.ExecuteActionAfterAnimation += OnExecuteActionAfterAnimation;
            EventHandler.AfterSceneLoadedEvent += OnAfterSceneLoadedEvent;
            EventHandler.GameDayEvent += OnGameDayEvent;
        }
        private void OnDisable()
        {
            EventHandler.ExecuteActionAfterAnimation -= OnExecuteActionAfterAnimation;
            EventHandler.AfterSceneLoadedEvent -= OnAfterSceneLoadedEvent;
            EventHandler.GameDayEvent -= OnGameDayEvent;
        }
        /// <summary>
        /// ÿ��ִ��һ��
        /// </summary>
        /// <param name="day"></param>
        /// <param name="season"></param>
        private void OnGameDayEvent(int day, Season season)
        {
            currentSeason = season;
            foreach (var tile in tileDetailesDict)
            {
                if (tile.Value.daysSinceWatered > -1)
                {
                    tile.Value.daysSinceWatered = -1;//����ø��ӽ���ˮ,�ڶ����ֻ���
                }
                if (tile.Value.daysSinceDug > -1)
                {
                    tile.Value.daysSinceDug++;//����ø��ӱ�����,��ÿ�쿪������++
                }
                if (tile.Value.daysSinceDug > 5 && tile.Value.seedItemID == -1)
                {
                    tile.Value.daysSinceDug = -1;//���һ�����ӱ�����������û����ֲ����,����������
                    tile.Value.canDig = true;
                    tile.Value.growthDays = -1;
                }
                if (tile.Value.seedItemID != -1)
                {
                    tile.Value.growthDays++;
                }
            }
            RefreshMap();
        }

        private void OnAfterSceneLoadedEvent()
        {
            currentGrid = FindObjectOfType<Grid>();
            digTilemap = GameObject.FindWithTag("Dig").GetComponent<Tilemap>();
            waterTilemap = GameObject.FindWithTag("Water").GetComponent<Tilemap>();
            /*DisplayMap(SceneManager.GetActiveScene().name);*/
            RefreshMap();
        }

        private void Start()
        {
            foreach (var mapData in mapDataList)
            {
                InitTileDetailsDict(mapData);
            }
        }
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
        /// <summary>
        /// ��������������귵����Ƭ��Ϣ
        /// </summary>
        /// <param name="mouseGridPos">�����������</param>
        /// <returns></returns>
        public TileDetails GetTileDetailsOnMousePosition(Vector3Int mouseGridPos)
        {
            string key = mouseGridPos.x + "x" + mouseGridPos.y + "y" + SceneManager.GetActiveScene().name;
            return GetTileDetailes(key);
        }
        /// <summary>
        /// ִ��ʵ�ʹ��߻���Ʒ����
        /// </summary>
        /// <param name="mouseWorldPos">�������</param>
        /// <param name="itemDetails">��Ʒ��Ϣ</param>
        private void OnExecuteActionAfterAnimation(Vector3 mouseWorldPos, ItemDetails itemDetails)
        {
            var mouseGridPos = currentGrid.WorldToCell(mouseWorldPos);//���������ָ��Ƭ������
            var currentTile = GetTileDetailsOnMousePosition(mouseGridPos);//���������ָ��Ƭ
            if (currentTile != null)//�����ǰ��Ƭ��Ϣ��Ϊnull
            {
                //WORKFLOW:��Ʒʹ��ʵ�ʹ���
                switch (itemDetails.itemType)
                {
                    case ItemType.Seed:
                        EventHandler.CallPlantSeedEvent(itemDetails.itemID, currentTile);
                        EventHandler.CallDropItemEvent(itemDetails.itemID, mouseWorldPos,itemDetails.itemType);
                        break;
                    case ItemType.Commodity:
                        EventHandler.CallDropItemEvent(itemDetails.itemID, mouseWorldPos, itemDetails.itemType);//����һ����Ʒ�����λ��
                        break;
                    case ItemType.HolTool:
                        SetDigGround(currentTile);
                        currentTile.daysSinceDug = 0;//�����ڿ�ʼ����Ӷ�������
                        currentTile.canDig = false;
                        currentTile.canDropItm = false;
                        //��Ч
                        break;
                    case ItemType.WaterTool:
                        SetWaterGround(currentTile);
                        currentTile.daysSinceWatered = 0;
                        //��Ч
                        break;
                    case ItemType.CollectTool:
                        Crop currentCrop = GetCropObject(mouseWorldPos);
                        //ִ���ո��
                        break;
                }
                UpdateTileDetails(currentTile);
            }
        }
        /// <summary>
        /// ��������س����ֲ���ʵObject
        /// </summary>
        /// <param name="mouseWorldPos">�����λ��</param>
        /// <returns></returns>
        private Crop GetCropObject(Vector3 mouseWorldPos)
        {
            Collider2D[] colliders = Physics2D.OverlapPointAll(mouseWorldPos);//Physics2D.OverlapPointAll:����ĳ������Χ��������ײ��ŵ�һ��������
            Crop currentCrop = null;
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].GetComponent<Crop>())
                {
                    currentCrop = colliders[i].GetComponent<Crop>();
                }
            }
            return currentCrop;
        }
        /// <summary>
        /// ��ʾ�ڿ���Ƭ
        /// </summary>
        /// <param name="tile"></param>
        private void SetDigGround(TileDetails tile)
        {
            Vector3Int pos = new Vector3Int(tile.gridX, tile.gridY, 0);
            if (digTilemap != null)
            {
                digTilemap.SetTile(pos, digTile);//������Ƭ����(λ��,��Ƭ)
            }
        }
        /// <summary>
        /// ��ʾ��ˮ��Ƭ
        /// </summary>
        /// <param name="tile"></param>
        private void SetWaterGround(TileDetails tile)
        {
            Vector3Int pos = new Vector3Int(tile.gridX, tile.gridY, 0);
            if (waterTilemap != null)
            {
                waterTilemap.SetTile(pos, waterTile);//������Ƭ����(λ��,��Ƭ)
            }
        }
        /// <summary>
        /// ������Ƭ��Ϣ
        /// </summary>
        /// <param name="tileDetails"></param>
        private void UpdateTileDetails(TileDetails tileDetails)
        {
            string key = tileDetails.gridX + "x" + tileDetails.gridY + "y" + SceneManager.GetActiveScene().name;
            if (tileDetailesDict.ContainsKey(key))
            {
                tileDetailesDict[key] = tileDetails;
            }
        }
        /// <summary>
        /// ˢ�µ�ǰ��ͼ
        /// </summary>
        private void RefreshMap()
        {
            if (digTilemap != null)
            {
                digTilemap.ClearAllTiles();
            }
            if (waterTilemap != null)
            {
                waterTilemap.ClearAllTiles();
            }
            foreach (var crop in FindObjectsOfType<Crop>())
            {
                Destroy(crop.gameObject);
            }
            DisplayMap(SceneManager.GetActiveScene().name);
        }
        /// <summary>
        /// ��ʾ��ͼ��Ƭ
        /// </summary>
        /// <param name="sceneName">��������</param>
        private void DisplayMap(string sceneName)
        {
            foreach (var tile in tileDetailesDict)
            {
                var key = tile.Key;
                var tileDetails = tile.Value;
                if (key.Contains(sceneName))
                {
                    if (tileDetails.daysSinceDug > -1)
                    {
                        SetDigGround(tileDetails);
                    }
                    if (tileDetails.daysSinceWatered > -1)
                    {
                        SetWaterGround(tileDetails);
                    }
                    //TODO:����
                    if (tileDetails.seedItemID > -1)//��ǰ��������������Ϣ
                        EventHandler.CallPlantSeedEvent(tileDetails.seedItemID, tileDetails);
                }
            }
        }
    }
}

