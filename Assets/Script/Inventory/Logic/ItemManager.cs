using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MFarm.Save;
//专门用来管理世界地图中所有的物品的管理类
namespace MFarm.Inventory
{
    public class ItemManager : Singleton<ItemManager>,ISaveable
    {
        public Item itemPrefab;
        public Item bounceItemPrefab;
        private Transform itemParent;
        private Transform playerTransform => FindObjectOfType<Player>().transform;

        public string GUID => GetComponent<DataGUID>().guid;

        //记录场景Item
        private Dictionary<string, List<SceneItem>> sceneItemDict = new Dictionary<string, List<SceneItem>>();//该字典通过场景名来保存场景中存在的物体
        //记录场景中的家具(string:场景名字)
        private Dictionary<string, List<SceneFurniture>> sceneFurnitureDict = new Dictionary<string, List<SceneFurniture>>();
        private void Start()
        {
            ISaveable saveable = this;
            saveable.RegisterSaveable();
        }
        private void OnEnable()
        {
            EventHandler.InstantiateItemInScene += OnInstantiateItemInScene;//为委托事件添加函数方法
            EventHandler.DropItemEvent += OnDropItemEvent;//扔东西委托事件
            EventHandler.BeforeSceneUnloadEvent += OnBeforeSceneUnloadEvent;
            EventHandler.AfterSceneLoadedEvent += OnAfterSceneLoadedEvent;
            EventHandler.BuildFurnitureEvent += OnBuildFurnitureEvent;
            EventHandler.StartNewGameEvent += OnStartNewGameEvent;
        }
        private void OnDisable()
        {
            EventHandler.InstantiateItemInScene -= OnInstantiateItemInScene;
            EventHandler.DropItemEvent -= OnDropItemEvent;
            EventHandler.BeforeSceneUnloadEvent -= OnBeforeSceneUnloadEvent;
            EventHandler.AfterSceneLoadedEvent -= OnAfterSceneLoadedEvent;
            EventHandler.BuildFurnitureEvent -= OnBuildFurnitureEvent;
            EventHandler.StartNewGameEvent -= OnStartNewGameEvent;
        }

        private void OnStartNewGameEvent(int index)
        {
            sceneItemDict.Clear();
            sceneFurnitureDict.Clear();
        }

        private void OnBuildFurnitureEvent(int ID,Vector3 mousePos)
        {
            BulePrintDetailes bulePrint = InventoryManager.Instance.bulePrintData.GetBulePrintDetailes(ID);
            var buildItem = Instantiate(bulePrint.buildPrefab, mousePos, Quaternion.identity, itemParent);
            if (buildItem.GetComponent<Box>())
            {
                buildItem.GetComponent<Box>().index = InventoryManager.Instance.BoxDataAmount;
                buildItem.GetComponent<Box>().InitBox(buildItem.GetComponent<Box>().index);
            }
        }

        private void OnDropItemEvent(int ID, Vector3 mousePos,ItemType itemType)
        {
            if (itemType == ItemType.Seed) return;
            //TODO:扔东西的效果
            var item = Instantiate(bounceItemPrefab, playerTransform.position, Quaternion.identity, itemParent);//克隆物体
            item.itemID = ID;
            var dir = (mousePos - playerTransform.position).normalized;
            item.GetComponent<ItemBounce>().InitBounceItem(mousePos, dir);
        }

        private void OnBeforeSceneUnloadEvent()
        {
            GetAllSceneItems();
            GetAllSceneFurnitures();
        }

        private void OnAfterSceneLoadedEvent()
        {
            itemParent = GameObject.FindWithTag("ItemParent").transform;//场景加载之后再寻找ItemParent避免报空
            RecreateAllItems();
            RebuildFurniturn();
        }
        private void OnInstantiateItemInScene(int ID, Vector3 pos)
        {
            var item = Instantiate(bounceItemPrefab, pos, Quaternion.identity, itemParent);//克隆物体
            item.itemID = ID;
            item.GetComponent<ItemBounce>().InitBounceItem(pos, Vector3.up);
        }
        /// <summary>
        /// 获得当前场景中的所有Item
        /// </summary>
        private void GetAllSceneItems()
        {
            List<SceneItem> currentSceneItems = new List<SceneItem>();//存放当前场景中物体的瓶子
            foreach (var item in FindObjectsOfType<Item>())
            {
                SceneItem sceneItem = new SceneItem()
                {
                    ItemID = item.itemID,
                    position = new SerializableVector3(item.transform.position)
                };//找到当前场景中的所有Item并将它的属性传入到sceneItem中
                currentSceneItems.Add(sceneItem);//再将sceneItem放入临时列表
            }
            if (sceneItemDict.ContainsKey(SceneManager.GetActiveScene().name))
            {
                //找到数据就更新Item数据列表
                sceneItemDict[SceneManager.GetActiveScene().name] = currentSceneItems;
            }
            else//如果是新场景
            {
                sceneItemDict.Add(SceneManager.GetActiveScene().name, currentSceneItems);
            }
        }
        /// <summary>
        /// 刷新重建当前场景中的物品
        /// </summary>
        private void RecreateAllItems()
        {
            List<SceneItem> currentSceneItems = new List<SceneItem>();
            //TryGetValue:根据键尝试得到值,返回bool
            //out currentSceneItems:反向输出,如果之前的bool为true则反向输出currentSceneItems将sceneItemDict的Value输出到currentSceneItems中
            if (sceneItemDict.TryGetValue(SceneManager.GetActiveScene().name,out currentSceneItems))
            {
                if (currentSceneItems != null)
                {
                    //无论场景中有没有物品先清场
                    foreach (var item in FindObjectsOfType<Item>())
                    {
                        Destroy(item.gameObject);
                    }
                    //将当前场景中有的物品给复制到场景中
                    foreach (var item in currentSceneItems)
                    {
                        Item newItem = Instantiate(itemPrefab, item.position.ToVector3(), Quaternion.identity, itemParent);
                        newItem.Init(item.ItemID);
                    }
                }
            }
        }
        /// <summary>
        /// 获取场景中的所有家具
        /// </summary>
        private void GetAllSceneFurnitures()
        {
            List<SceneFurniture> currentSceneFurnitures = new List<SceneFurniture>();//存放当前场景中物体的瓶子
            foreach (var item in FindObjectsOfType<Furniture>())
            {
                SceneFurniture sceneFurniture = new SceneFurniture()
                {
                    ItemID = item.itemID,
                    position = new SerializableVector3(item.transform.position)
                };//找到当前场景中的所有Furniture并将它的属性传入到sceneFurniture中
                currentSceneFurnitures.Add(sceneFurniture);//再将sceneFurniture放入临时列表
            }
            if (sceneFurnitureDict.ContainsKey(SceneManager.GetActiveScene().name))
            {
                //找到数据就更新Furniture数据列表
                sceneFurnitureDict[SceneManager.GetActiveScene().name] = currentSceneFurnitures;
            }
            else//如果是新场景
            {
                sceneFurnitureDict.Add(SceneManager.GetActiveScene().name, currentSceneFurnitures);
            }
        }
        /// <summary>
        /// 重建当前场景家具
        /// </summary>
        private void RebuildFurniturn()
        {
            List<SceneFurniture> currentSceneFurniture = new List<SceneFurniture>();
            if(sceneFurnitureDict.TryGetValue(SceneManager.GetActiveScene().name,out currentSceneFurniture))
            {
                if (currentSceneFurniture != null)
                {
                    foreach (SceneFurniture sceneFurniture in currentSceneFurniture)
                    {
                        BulePrintDetailes bulePrint = InventoryManager.Instance.bulePrintData.GetBulePrintDetailes(sceneFurniture.ItemID);
                        var buildItem = Instantiate(bulePrint.buildPrefab, sceneFurniture.position.ToVector3(), Quaternion.identity, itemParent);
                        if (buildItem.GetComponent<Box>())
                        {
                            buildItem.GetComponent<Box>().InitBox(sceneFurniture.boxIndex);
                        }
                    }
                }
            }
        }

        public GameSaveData GenerateSaveData()
        {
            GetAllSceneItems();
            GetAllSceneFurnitures();//因为场景中的物品是在场景加载卸载后才保存的,未卸载前拿不到所有物品或家具,必须这里手动拿一下
            GameSaveData saveData = new GameSaveData();
            saveData.sceneItemDict = this.sceneItemDict;
            saveData.sceneFurnitureDict = this.sceneFurnitureDict;
            return saveData;
        }

        public void RestoreData(GameSaveData saveDate)
        {
            this.sceneItemDict = saveDate.sceneItemDict;
            this.sceneFurnitureDict = saveDate.sceneFurnitureDict;
            RecreateAllItems();
            RebuildFurniturn();//刷新
        }
    }
}

