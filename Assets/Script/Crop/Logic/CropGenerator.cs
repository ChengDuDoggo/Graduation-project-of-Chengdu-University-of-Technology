using System.Collections;
using System.Collections.Generic;
using MFarm.Map;
using UnityEngine;
//ũ����������
namespace MFarm.CropPlant
{
    public class CropGenerator : MonoBehaviour
    {
        private Grid currentGrid;//Grid�����ŵ�ͼ��һƬGrid������һ������
        public int seedItemID;
        public int growthDays;//Ԥ�ȷ����ڵ�ͼ�е�ũ�����Ѿ���������������
        private void Awake()
        {
            currentGrid = FindObjectOfType<Grid>();
        }
        private void OnEnable()
        {
            EventHandler.GenerateCropEvent += GenerateCrop;
        }
        private void OnDisable()
        {
            EventHandler.GenerateCropEvent -= GenerateCrop;
        }
        private void GenerateCrop()
        {
            Vector3Int cropGridPos = currentGrid.WorldToCell(transform.position);//��������ת��Ϊ�������겢�õ�(����ֻ���õ�����,û���õ�����)
            if (seedItemID != 0)
            {
                var tile = GridMapManager.Instance.GetTileDetailsOnMousePosition(cropGridPos);//�õ�������Ϣ
                if(tile == null)//�����ǰ��Ƭ��ϢΪnull,��newһ����Ƭ��Ϣ
                {
                    tile = new TileDetails();
                    tile.gridX = cropGridPos.x;
                    tile.gridY = cropGridPos.y;
                }
                tile.daysSinceWatered = -1;
                tile.seedItemID = seedItemID;
                tile.growthDays = growthDays;//���¸�����Ϣ
                GridMapManager.Instance.UpdateTileDetails(tile);//������Ƭ��Ϣ
            }
        }
    }
}

