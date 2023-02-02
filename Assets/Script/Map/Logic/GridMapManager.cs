using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using MFarm.CropPlant;
using MFarm.Save;
namespace MFarm.Map
{
    public class GridMapManager : Singleton<GridMapManager>,ISaveable
    {
        [Header("种地瓦片切换信息")]
        public RuleTile digTile;//实例化两个规则瓦片
        public RuleTile waterTile;
        private Tilemap digTilemap;//实例化两个涂画规则瓦片的瓦片地图
        private Tilemap waterTilemap;
        [Header("地图信息")]
        public List<MapData_SO> mapDataList;
        //定义一个字典来保存格子场景名字+格子坐标对应的瓦片信息
        private Dictionary<string, TileDetails> tileDetailesDict = new Dictionary<string, TileDetails>();
        //记录场景是否第一次加载
        private Dictionary<string, bool> firstLoadDic = new Dictionary<string, bool>();
        private Grid currentGrid;
        private Season currentSeason;
        //杂草列表
        private List<ReapItem> itemsInRadius;

        public string GUID => GetComponent<DataGUID>().guid;

        private void OnEnable()
        {
            EventHandler.ExecuteActionAfterAnimation += OnExecuteActionAfterAnimation;
            EventHandler.AfterSceneLoadedEvent += OnAfterSceneLoadedEvent;
            EventHandler.GameDayEvent += OnGameDayEvent;
            EventHandler.RefreshCurrentMap += RefreshMap;
        }
        private void OnDisable()
        {
            EventHandler.ExecuteActionAfterAnimation -= OnExecuteActionAfterAnimation;
            EventHandler.AfterSceneLoadedEvent -= OnAfterSceneLoadedEvent;
            EventHandler.GameDayEvent -= OnGameDayEvent;
            EventHandler.RefreshCurrentMap -= RefreshMap;
        }
        /// <summary>
        /// 每天执行一次
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
                    tile.Value.daysSinceWatered = -1;//如果该格子浇过水,第二天又会变干
                }
                if (tile.Value.daysSinceDug > -1)
                {
                    tile.Value.daysSinceDug++;//如果该格子被开垦,则每天开垦天数++
                }
                if (tile.Value.daysSinceDug > 5 && tile.Value.seedItemID == -1)
                {
                    tile.Value.daysSinceDug = -1;//如果一个格子被开垦五天且没有种植种子,则变回泥土地
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
            if (firstLoadDic[SceneManager.GetActiveScene().name])
            {
                //预先生成农作物
                EventHandler.CallGenerateCropEvent();
                firstLoadDic[SceneManager.GetActiveScene().name] = false;//将当前场景的是否是第一次激活状态改为false
            }

            RefreshMap();
        }

        private void Start()
        {
            foreach (var mapData in mapDataList)
            {
                firstLoadDic.Add(mapData.sceneName, true);//所有的场景都是第一次加载,加载场景之后就把它改为False
                InitTileDetailsDict(mapData);
            }
            ISaveable saveable = this;
            saveable.RegisterSaveable();
        }
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
        /// 根据key返回瓦片信息
        /// </summary>
        /// <param name="key">x+y+地图名字</param>
        /// <returns></returns>
        public TileDetails GetTileDetailes(string key)
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
        /// 根据鼠标网格坐标返回瓦片信息
        /// </summary>
        /// <param name="mouseGridPos">鼠标网格坐标</param>
        /// <returns></returns>
        public TileDetails GetTileDetailsOnMousePosition(Vector3Int mouseGridPos)
        {
            string key = mouseGridPos.x + "x" + mouseGridPos.y + "y" + SceneManager.GetActiveScene().name;
            return GetTileDetailes(key);
        }
        /// <summary>
        /// 执行实际工具或物品功能
        /// </summary>
        /// <param name="mouseWorldPos">鼠标坐标</param>
        /// <param name="itemDetails">物品信息</param>
        private void OnExecuteActionAfterAnimation(Vector3 mouseWorldPos, ItemDetails itemDetails)
        {
            var mouseGridPos = currentGrid.WorldToCell(mouseWorldPos);//返回鼠标所指瓦片的坐标
            var currentTile = GetTileDetailsOnMousePosition(mouseGridPos);//返回鼠标所指瓦片
            if (currentTile != null)//如果当前瓦片信息不为null
            {
                Crop currentCrop = GetCropObject(mouseWorldPos);
                //WORKFLOW:物品使用实际功能
                switch (itemDetails.itemType)
                {
                    case ItemType.Seed:
                        EventHandler.CallPlantSeedEvent(itemDetails.itemID, currentTile);
                        EventHandler.CallDropItemEvent(itemDetails.itemID, mouseWorldPos,itemDetails.itemType);
                        EventHandler.CallPlaySoundEvent(SoundName.Plant);
                        break;
                    case ItemType.Commodity:
                        EventHandler.CallDropItemEvent(itemDetails.itemID, mouseWorldPos, itemDetails.itemType);//复制一个商品到鼠标位置
                        break;
                    case ItemType.HolTool:
                        SetDigGround(currentTile);
                        currentTile.daysSinceDug = 0;//从现在开始这个坑儿被挖了
                        currentTile.canDig = false;
                        currentTile.canDropItm = false;
                        //音效
                        EventHandler.CallPlaySoundEvent(SoundName.Hoe);
                        break;
                    case ItemType.WaterTool:
                        SetWaterGround(currentTile);
                        currentTile.daysSinceWatered = 0;
                        //音效
                        EventHandler.CallPlaySoundEvent(SoundName.Water);
                        break;
                    case ItemType.BreakTool:
                    case ItemType.ChopTool:
                        //执行收割方法
                        currentCrop?.ProcessToolAction(itemDetails, currentCrop.tileDetails);
                        break;
                    case ItemType.CollectTool:
                        //执行收割方法
                        currentCrop.ProcessToolAction(itemDetails,currentTile);
                        break;
                    case ItemType.ReapTool:
                        var reapCount = 0;
                        for (int i = 0; i < itemsInRadius.Count; i++)
                        {
                            EventHandler.CallParticaleEffectEvent(ParticaleEffectType.ReapableScenery, itemsInRadius[i].transform.position + Vector3.up);
                            itemsInRadius[i].SpawnHarvestItems();
                            Destroy(itemsInRadius[i].gameObject);
                            reapCount++;
                            if (reapCount >= Settings.reapAmount)
                            {
                                break;//限制一下一次性能收割的草数量避免直接收割一大片
                            }
                        }
                        EventHandler.CallPlaySoundEvent(SoundName.Reap);
                        break;
                    case ItemType.Furniture:
                        //在地图上生成物品 ItemManager
                        //移除当前物品(图纸) InventoryManager
                        //移除资源物品 InventoryManager
                        //这里只需要触发一个事件即可,然后激活事件之后,对应的脚本去干它们自己该干的活儿
                        EventHandler.CallBuildFurnitureEvent(itemDetails.itemID,mouseWorldPos);
                        break;
                }
                UpdateTileDetails(currentTile);
            }
        }
        /// <summary>
        /// 鼠标点击返回成熟的植物果实Object
        /// </summary>
        /// <param name="mouseWorldPos">鼠标点击位置</param>
        /// <returns></returns>
        public Crop GetCropObject(Vector3 mouseWorldPos)
        {
            Collider2D[] colliders = Physics2D.OverlapPointAll(mouseWorldPos);//Physics2D.OverlapPointAll:返回某个点周围的所有碰撞体放到一个数组中
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
        /// 判断在鼠标检测范围之内是否有可实现收割的物品(草)
        /// </summary>
        /// <returns></returns>
        public bool HaveReapableItemsInRadius(Vector3 mouseWorldPos,ItemDetails tool)
        {
            itemsInRadius = new List<ReapItem>();
            Collider2D[] colliders = new Collider2D[20];//检测到的碰撞体放入数组中
            //圆形物理检测(检测的中心点,检测的范围,检测到的物体放入到的数组)
            //OverlapCircleNonAlloc和OverlapCircle的区别
            //OverlapCircle会系统自动帮你将检测到的物体不断new创建放入到临时List列表中
            //OverlapCircleNonAlloc是你事先设定好一个固定大小的数组,避免系统自己new列表来产生缓存,减少CG
            Physics2D.OverlapCircleNonAlloc(mouseWorldPos, tool.itemUseRadius, colliders);
            if (colliders.Length > 0)
            {
                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i] != null)
                    {
                        if (colliders[i].GetComponent<ReapItem>())
                        {
                            var item = colliders[i].GetComponent<ReapItem>();
                            itemsInRadius.Add(item);//将ReapItem放入列表中
                        }
                    }
                }
            }
            return itemsInRadius.Count > 0;
        }
        /// <summary>
        /// 显示挖坑瓦片
        /// </summary>
        /// <param name="tile"></param>
        private void SetDigGround(TileDetails tile)
        {
            Vector3Int pos = new Vector3Int(tile.gridX, tile.gridY, 0);
            if (digTilemap != null)
            {
                digTilemap.SetTile(pos, digTile);//绘制瓦片函数(位置,瓦片)
            }
        }
        /// <summary>
        /// 显示浇水瓦片
        /// </summary>
        /// <param name="tile"></param>
        private void SetWaterGround(TileDetails tile)
        {
            Vector3Int pos = new Vector3Int(tile.gridX, tile.gridY, 0);
            if (waterTilemap != null)
            {
                waterTilemap.SetTile(pos, waterTile);//绘制瓦片函数(位置,瓦片)
            }
        }
        /// <summary>
        /// 更新瓦片信息
        /// </summary>
        /// <param name="tileDetails"></param>
        public void UpdateTileDetails(TileDetails tileDetails)
        {
            string key = tileDetails.gridX + "x" + tileDetails.gridY + "y" + SceneManager.GetActiveScene().name;
            if (tileDetailesDict.ContainsKey(key))
            {
                tileDetailesDict[key] = tileDetails;
            }
            else
            {
                tileDetailesDict.Add(key, tileDetails);
            }
        }
        /// <summary>
        /// 刷新当前地图
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
        /// 显示地图瓦片
        /// </summary>
        /// <param name="sceneName">场景名字</param>
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
                    //TODO:种子
                    if (tileDetails.seedItemID > -1)//当前格子中有种子信息
                        EventHandler.CallPlantSeedEvent(tileDetails.seedItemID, tileDetails);
                }
            }
        }
        /// <summary>
        /// 根据场景名字构建网格范围,输出范围和原点
        /// </summary>
        /// <param name="sceneName">场景名字</param>
        /// <param name="gridDimensions">网格范围</param>
        /// <param name="gridOrigin">网格原点</param>
        /// <returns>是否有当前场景信息</returns>
        public bool GetGridDimensions(string sceneName,out Vector2Int gridDimensions,out Vector2Int gridOrigin)
        {
            gridDimensions = Vector2Int.zero;
            gridOrigin = Vector2Int.zero;//初始化为0
            foreach (var mapData in mapDataList)//循环拿出每一张地图
            {
                if (mapData.sceneName == sceneName)//如果有该场景
                {
                    gridDimensions.x = mapData.gridWidth;
                    gridDimensions.y = mapData.gridHeight;
                    gridOrigin.x = mapData.originX;
                    gridOrigin.y = mapData.originY;
                    return true;
                }//则将当前场景中的数据全部输出传入节点中并返回true
            }
            return false;
        }

        public GameSaveData GenerateSaveData()
        {
            GameSaveData saveData = new GameSaveData();
            saveData.tileDetailsDict = this.tileDetailesDict;
            saveData.firstLoadDict = this.firstLoadDic;
            return saveData;
        }

        public void RestoreData(GameSaveData saveDate)
        {
            this.tileDetailesDict = saveDate.tileDetailsDict;
            this.firstLoadDic = saveDate.firstLoadDict;
        }
    }
}

