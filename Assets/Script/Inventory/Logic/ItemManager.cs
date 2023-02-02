using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MFarm.Save;
//ר���������������ͼ�����е���Ʒ�Ĺ�����
namespace MFarm.Inventory
{
    public class ItemManager : Singleton<ItemManager>,ISaveable
    {
        public Item itemPrefab;
        public Item bounceItemPrefab;
        private Transform itemParent;
        private Transform playerTransform => FindObjectOfType<Player>().transform;

        public string GUID => GetComponent<DataGUID>().guid;

        //��¼����Item
        private Dictionary<string, List<SceneItem>> sceneItemDict = new Dictionary<string, List<SceneItem>>();//���ֵ�ͨ�������������泡���д��ڵ�����
        //��¼�����еļҾ�(string:��������)
        private Dictionary<string, List<SceneFurniture>> sceneFurnitureDict = new Dictionary<string, List<SceneFurniture>>();
        private void Start()
        {
            ISaveable saveable = this;
            saveable.RegisterSaveable();
        }
        private void OnEnable()
        {
            EventHandler.InstantiateItemInScene += OnInstantiateItemInScene;//Ϊί���¼���Ӻ�������
            EventHandler.DropItemEvent += OnDropItemEvent;//�Ӷ���ί���¼�
            EventHandler.BeforeSceneUnloadEvent += OnBeforeSceneUnloadEvent;
            EventHandler.AfterSceneLoadedEvent += OnAfterSceneLoadedEvent;
            EventHandler.BuildFurnitureEvent += OnBuildFurnitureEvent;
        }
        private void OnDisable()
        {
            EventHandler.InstantiateItemInScene -= OnInstantiateItemInScene;
            EventHandler.DropItemEvent -= OnDropItemEvent;
            EventHandler.BeforeSceneUnloadEvent -= OnBeforeSceneUnloadEvent;
            EventHandler.AfterSceneLoadedEvent -= OnAfterSceneLoadedEvent;
            EventHandler.BuildFurnitureEvent -= OnBuildFurnitureEvent;
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
            //TODO:�Ӷ�����Ч��
            var item = Instantiate(bounceItemPrefab, playerTransform.position, Quaternion.identity, itemParent);//��¡����
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
            itemParent = GameObject.FindWithTag("ItemParent").transform;//��������֮����Ѱ��ItemParent���ⱨ��
            RecreateAllItems();
            RebuildFurniturn();
        }
        private void OnInstantiateItemInScene(int ID, Vector3 pos)
        {
            var item = Instantiate(bounceItemPrefab, pos, Quaternion.identity, itemParent);//��¡����
            item.itemID = ID;
            item.GetComponent<ItemBounce>().InitBounceItem(pos, Vector3.up);
        }
        /// <summary>
        /// ��õ�ǰ�����е�����Item
        /// </summary>
        private void GetAllSceneItems()
        {
            List<SceneItem> currentSceneItems = new List<SceneItem>();//��ŵ�ǰ�����������ƿ��
            foreach (var item in FindObjectsOfType<Item>())
            {
                SceneItem sceneItem = new SceneItem()
                {
                    ItemID = item.itemID,
                    position = new SerializableVector3(item.transform.position)
                };//�ҵ���ǰ�����е�����Item�����������Դ��뵽sceneItem��
                currentSceneItems.Add(sceneItem);//�ٽ�sceneItem������ʱ�б�
            }
            if (sceneItemDict.ContainsKey(SceneManager.GetActiveScene().name))
            {
                //�ҵ����ݾ͸���Item�����б�
                sceneItemDict[SceneManager.GetActiveScene().name] = currentSceneItems;
            }
            else//������³���
            {
                sceneItemDict.Add(SceneManager.GetActiveScene().name, currentSceneItems);
            }
        }
        /// <summary>
        /// ˢ���ؽ���ǰ�����е���Ʒ
        /// </summary>
        private void RecreateAllItems()
        {
            List<SceneItem> currentSceneItems = new List<SceneItem>();
            //TryGetValue:���ݼ����Եõ�ֵ,����bool
            //out currentSceneItems:�������,���֮ǰ��boolΪtrue�������currentSceneItems��sceneItemDict��Value�����currentSceneItems��
            if (sceneItemDict.TryGetValue(SceneManager.GetActiveScene().name,out currentSceneItems))
            {
                if (currentSceneItems != null)
                {
                    //���۳�������û����Ʒ���峡
                    foreach (var item in FindObjectsOfType<Item>())
                    {
                        Destroy(item.gameObject);
                    }
                    //����ǰ�������е���Ʒ�����Ƶ�������
                    foreach (var item in currentSceneItems)
                    {
                        Item newItem = Instantiate(itemPrefab, item.position.ToVector3(), Quaternion.identity, itemParent);
                        newItem.Init(item.ItemID);
                    }
                }
            }
        }
        /// <summary>
        /// ��ȡ�����е����мҾ�
        /// </summary>
        private void GetAllSceneFurnitures()
        {
            List<SceneFurniture> currentSceneFurnitures = new List<SceneFurniture>();//��ŵ�ǰ�����������ƿ��
            foreach (var item in FindObjectsOfType<Furniture>())
            {
                SceneFurniture sceneFurniture = new SceneFurniture()
                {
                    ItemID = item.itemID,
                    position = new SerializableVector3(item.transform.position)
                };//�ҵ���ǰ�����е�����Furniture�����������Դ��뵽sceneFurniture��
                currentSceneFurnitures.Add(sceneFurniture);//�ٽ�sceneFurniture������ʱ�б�
            }
            if (sceneFurnitureDict.ContainsKey(SceneManager.GetActiveScene().name))
            {
                //�ҵ����ݾ͸���Furniture�����б�
                sceneFurnitureDict[SceneManager.GetActiveScene().name] = currentSceneFurnitures;
            }
            else//������³���
            {
                sceneFurnitureDict.Add(SceneManager.GetActiveScene().name, currentSceneFurnitures);
            }
        }
        /// <summary>
        /// �ؽ���ǰ�����Ҿ�
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
            GetAllSceneFurnitures();//��Ϊ�����е���Ʒ���ڳ�������ж�غ�ű����,δж��ǰ�ò���������Ʒ��Ҿ�,���������ֶ���һ��
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
            RebuildFurniturn();//ˢ��
        }
    }
}

