using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
namespace MFarm.Save
{
    public class SaveLoadManager : Singleton<SaveLoadManager>
    {
        private List<ISaveable> saveableList = new List<ISaveable>();//<ISaveable>是接口,代表该列表中可以存放所有调用了ISaveable接口的类
        public List<DataSlot> dataSlots = new List<DataSlot>(new DataSlot[3]);//存档的三个格子
        private string jsonFolder;//存放Json文件的路径
        private int currentDataIndex;//当前正在进行的游戏的存档Slot的Index
        protected override void Awake()
        {
            base.Awake();
            jsonFolder = Application.persistentDataPath + "/SAVE DATA/";//创建一个系统路径下的名为SAVE DATA的文件夹(加斜杠表明这是文件夹,不加斜杠表明是文件)
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
        private void Save(int index)//index用来判断玩家点击的是哪一个存档格子
        {
            DataSlot data = new DataSlot();
            foreach (var saveable in saveableList)//saveableList中已经存储了每一个调用了<存储加载>接口的类
            {
                data.dataDict.Add(saveable.GUID, saveable.GenerateSaveData());//GenerateSaveData()函数将会返回每一个类中存储的不同的数据文件GameSaveData,并通过不同的GUID给它打上键值
            }
            dataSlots[index] = data;
            //此时每一个类的游戏数据文件我们已经拿到了并且给它们打上的第一无二的GUID键值
            //需要将这些所有数据保存到本地,需要转化为Json数据形式
            //生成一个最终路径的文件
            var resultPath = /*/*/jsonFolder/*实际这里是有斜杠的,所以系统能够判断哪里是文件夹哪里是创建文件/*/ + "data" + index + ".json";
            //将DataSlot数据文件转化为String数据类型(Formatting.Indented可以让开发者更好的观察数据,实际开发中不需要添加)
            var jsonData = JsonConvert.SerializeObject(dataSlots[index], Formatting.Indented);//将DataSlot类型序列化为String类型
            if (!File.Exists(resultPath))//如果最终文件不存在
            {
                Directory.CreateDirectory(jsonFolder);//创建文件夹
            }
            Debug.Log("DATA" + index + "SAVED!");
            File.WriteAllText(resultPath, jsonData);//将jsonData数据写入resultPath文件中去,数据写入后就自动创建了文件
        }
        public void Load(int index)
        {
            currentDataIndex = index;
            var resultPath = jsonFolder + "data" + index + ".json";
            var stringData = File.ReadAllText(resultPath);//读取最终路径文件中的数据
            var jsonData = JsonConvert.DeserializeObject<DataSlot>(stringData);//将stringData中的数据反序列化为DataSlot类型
            foreach (var saveable in saveableList)
            {
                saveable.RestoreData(jsonData.dataDict[saveable.GUID]);//加载数据文件
            }
        }
    }
}

