using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//ר���������������ͼ�����е���Ʒ�Ĺ�����
namespace MFarm.Inventory
{
    public class ItemManager : Singleton<ItemManager>
    {
        public Item itemPrefab;
        private Transform itemParent;
        //��¼����Item
        private Dictionary<string, List<SceneItem>> sceneItemDict = new Dictionary<string, List<SceneItem>>();//���ֵ�ͨ�������������泡���д��ڵ�����
        private void OnEnable()
        {
            EventHandler.InstantiateItemInScene += OnInstantiateItemInScene;//Ϊί���¼���Ӻ�������
            EventHandler.BeforeSceneUnloadEvent += OnBeforeSceneUnloadEvent;
            EventHandler.AfterSceneLoadedEvent += OnAfterSceneLoadedEvent;
        }
        private void OnDisable()
        {
            EventHandler.InstantiateItemInScene -= OnInstantiateItemInScene;
            EventHandler.BeforeSceneUnloadEvent -= OnBeforeSceneUnloadEvent;
            EventHandler.AfterSceneLoadedEvent -= OnAfterSceneLoadedEvent;
        }

        private void OnBeforeSceneUnloadEvent()
        {
            GetAllSceneItems();
        }

        private void OnAfterSceneLoadedEvent()
        {
            itemParent = GameObject.FindWithTag("ItemParent").transform;//��������֮����Ѱ��ItemParent���ⱨ��
            RecreateAllItems();
        }
        private void OnInstantiateItemInScene(int ID, Vector3 pos)
        {
            var item = Instantiate(itemPrefab, pos, Quaternion.identity, itemParent);//��¡����
            item.itemID = ID;
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
    }
}

