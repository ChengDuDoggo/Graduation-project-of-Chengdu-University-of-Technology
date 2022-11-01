using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MFarm.CropPlant
{
    public class CropManager : Singleton<CropManager>
    {
        public CropDataList_SO cropData;
        private Transform cropParent;
        private Grid currentGrid;
        private Season currentSeason;
        private void OnEnable()
        {
            EventHandler.PlantSeedEvent += OnPlantSeedEvent;//�����Ӳ���ʱִ��һ�δ�ί���¼�
            EventHandler.AfterSceneLoadedEvent += OnAfterSceneLoadedEvent;//�ڳ����л�ʱִ��һ�δ�ί���¼�
            EventHandler.GameDayEvent += OnGameDayEvent;
        }
        private void OnDisable()
        {
            EventHandler.PlantSeedEvent -= OnPlantSeedEvent;
            EventHandler.AfterSceneLoadedEvent -= OnAfterSceneLoadedEvent;
            EventHandler.GameDayEvent -= OnGameDayEvent;
        }

        private void OnGameDayEvent(int day, Season season)
        {
            currentSeason = season;
        }

        private void OnAfterSceneLoadedEvent()
        {
            currentGrid = FindObjectOfType<Grid>();
            cropParent = GameObject.FindWithTag("CropParent").transform;
        }

        private void OnPlantSeedEvent(int ID, TileDetails tileDetails)
        {
            CropDetails currentCrop = GetCropDetails(ID);
            if (currentCrop != null && SeasonAvailable(currentCrop) && tileDetails.seedItemID == -1)//���ڵ�һ����ֲ
            {
                tileDetails.seedItemID = ID;
                tileDetails.growthDays = 0;
                //��ʾũ����
                DisplayCropPlant(tileDetails, currentCrop);
            }
            else if (tileDetails.seedItemID != -1)//����ˢ�µ�ͼ
            {
                //��ʾũ����
                DisplayCropPlant(tileDetails, currentCrop);
            }
        }
        /// <summary>
        /// ��ʾũ����
        /// </summary>
        /// <param name="tileDetails">��Ƭ��ͼ��Ϣ</param>
        /// <param name="cropDetails">������Ϣ</param>
        private void DisplayCropPlant(TileDetails tileDetails,CropDetails cropDetails)
        {
            //�ɳ��׶�
            int growthStages = cropDetails.growthDays.Length;//���һ���м����ɳ��׶�
            int currentStage = 0;
            int dayCounter = cropDetails.TotalGrowthDays;//��������ܹ��ɳ��˶���������һ����ʱ��
            //������㵱ǰ�ĳɳ��׶�
            for (int i = growthStages-1; i >= 0; i--)
            {
                if (tileDetails.growthDays >= dayCounter)
                {
                    currentStage = i;
                    break;
                }
                dayCounter -= cropDetails.growthDays[i];
            }
            //��ȡ��ǰ�׶ε�Prefabs
            GameObject cropPrefab = cropDetails.growsPrefabs[currentStage];
            Sprite cropSprite = cropDetails.growthSprites[currentStage];
            Vector3 pos = new Vector3(tileDetails.gridX + 0.5f, tileDetails.gridY + 0.5f, 0);//����һ��ͼƬ,ʹ����ͼƬ�����ڸ��ӵ����м�
            //����
            GameObject cropInstance = Instantiate(cropPrefab, pos, Quaternion.identity, cropParent);
            //���ͼƬ
            cropInstance.GetComponentInChildren<SpriteRenderer>().sprite = cropSprite;
            cropInstance.GetComponent<Crop>().cropDetails = cropDetails;
        }
        /// <summary>
        /// ͨ����ƷID����������Ϣ
        /// </summary>
        /// <param name="ID">����ID</param>
        /// <returns></returns>
        public CropDetails GetCropDetails(int ID)
        {
            return cropData.cropDetailsList.Find(c => c.seedItemID == ID);//��ķ����ʽ
        }
        /// <summary>
        /// �жϵ�ǰ�����Ƿ������ֲ
        /// </summary>
        /// <param name="crop">������Ϣ</param>
        /// <returns></returns>
        private bool SeasonAvailable(CropDetails crop)
        {
            for (int i = 0; i < crop.seasons.Length; i++)
            {
                if (crop.seasons[i] == currentSeason)
                {
                    return true;
                }
            }
            return false;
        }
    }
}

