using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
namespace MFarm.Save
{
    public class SaveLoadManager : Singleton<SaveLoadManager>
    {
        private List<ISaveable> saveableList = new List<ISaveable>();//<ISaveable>�ǽӿ�,������б��п��Դ�����е�����ISaveable�ӿڵ���
        public List<DataSlot> dataSlots = new List<DataSlot>(new DataSlot[3]);//�浵����������
        private string jsonFolder;//���Json�ļ���·��
        private int currentDataIndex;//��ǰ���ڽ��е���Ϸ�Ĵ浵Slot��Index
        protected override void Awake()
        {
            base.Awake();
            jsonFolder = Application.persistentDataPath + "/SAVE DATA/";//����һ��ϵͳ·���µ���ΪSAVE DATA���ļ���(��б�ܱ��������ļ���,����б�ܱ������ļ�)
            ReadSaveData();
        }
        private void OnEnable()
        {
            EventHandler.StartNewGameEvent += OnStartNewGameEvent;
            EventHandler.EndGameEvent += OnEndGameEvent;
        }
        private void OnDisable()
        {
            EventHandler.StartNewGameEvent -= OnStartNewGameEvent;
            EventHandler.EndGameEvent -= OnEndGameEvent;
        }

        private void OnEndGameEvent()
        {
            Save(currentDataIndex);
        }

        private void OnStartNewGameEvent(int index)
        {
            currentDataIndex = index;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                Save(currentDataIndex);
            }
            if (Input.GetKeyDown(KeyCode.O))
            {
                Load(currentDataIndex);
            }
        }

        public void RegisterSaveable(ISaveable saveable)
        {
            if (!saveableList.Contains(saveable))
            {
                saveableList.Add(saveable);
            }
        }
        private void ReadSaveData()
        {
            if (Directory.Exists(jsonFolder))
            {
                for (int i = 0; i < dataSlots.Count; i++)
                {
                    var resultPath = jsonFolder + "data" + i + ".json";
                    if (File.Exists(resultPath))
                    {
                        var stringData = File.ReadAllText(resultPath);
                        var jsonData = JsonConvert.DeserializeObject<DataSlot>(stringData);
                        dataSlots[i] = jsonData;
                    }
                }
            }
        }
        private void Save(int index)//index�����ж���ҵ��������һ���浵����
        {
            DataSlot data = new DataSlot();
            foreach (var saveable in saveableList)//saveableList���Ѿ��洢��ÿһ��������<�洢����>�ӿڵ���
            {
                data.dataDict.Add(saveable.GUID, saveable.GenerateSaveData());//GenerateSaveData()�������᷵��ÿһ�����д洢�Ĳ�ͬ�������ļ�GameSaveData,��ͨ����ͬ��GUID�������ϼ�ֵ
            }
            dataSlots[index] = data;
            //��ʱÿһ�������Ϸ�����ļ������Ѿ��õ��˲��Ҹ����Ǵ��ϵĵ�һ�޶���GUID��ֵ
            //��Ҫ����Щ�������ݱ��浽����,��Ҫת��ΪJson������ʽ
            //����һ������·�����ļ�
            var resultPath = /*/*/jsonFolder/*ʵ����������б�ܵ�,����ϵͳ�ܹ��ж��������ļ��������Ǵ����ļ�/*/ + "data" + index + ".json";
            //��DataSlot�����ļ�ת��ΪString��������(Formatting.Indented�����ÿ����߸��õĹ۲�����,ʵ�ʿ����в���Ҫ���)
            var jsonData = JsonConvert.SerializeObject(dataSlots[index], Formatting.Indented);//��DataSlot�������л�ΪString����
            if (!File.Exists(resultPath))//��������ļ�������
            {
                Directory.CreateDirectory(jsonFolder);//�����ļ���
            }
            Debug.Log("DATA" + index + "SAVED!");
            File.WriteAllText(resultPath, jsonData);//��jsonData����д��resultPath�ļ���ȥ,����д�����Զ��������ļ�
        }
        public void Load(int index)
        {
            currentDataIndex = index;
            var resultPath = jsonFolder + "data" + index + ".json";
            var stringData = File.ReadAllText(resultPath);//��ȡ����·���ļ��е�����
            var jsonData = JsonConvert.DeserializeObject<DataSlot>(stringData);//��stringData�е����ݷ����л�ΪDataSlot����
            foreach (var saveable in saveableList)
            {
                saveable.RestoreData(jsonData.dataDict[saveable.GUID]);//���������ļ�
            }
        }
    }
}

