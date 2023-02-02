using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MFarm.Save
{
    [System.Serializable]
    //��ֻ��һ��������ݵ���
    public class GameSaveData//����һ��������¼����Saveʱ��Ҫ��������������浽�����´�Load��Ϸʱֱ�ӽ���������������ȫ����������Ϸ��ȥ
    {
        public string dataSceneName;//��Ҫ���泡����
        public Dictionary<string, SerializableVector3> characterPosDict;//������������(string:NPC�����������)
        public Dictionary<string, List<SceneItem>> sceneItemDict;//�����еĵ�������
        public Dictionary<string, List<SceneFurniture>> sceneFurnitureDict;//�����еļҾ�����
        public Dictionary<string, TileDetails> tileDetailsDict;//�����е���Ƭ��Ϣ��Ҫ��¼
        public Dictionary<string, bool> firstLoadDict;//�����Ƿ��һ�μ���Ҳ��Ҫ��¼
        public Dictionary<string, List<InventoryItem>> inventoryDict;//�����е���Ʒ����Ҳ��Ҫ����
        public Dictionary<string, int> timeDict;//ʱ��Ҳ��Ҫ����
        public int playerMoney;//��ҵ�Ǯ��Ҫ����
        //NPC
        public string targetScene;//NPC��Ŀ�곡��Ҳ��Ҫ����
        public bool interactable;//NPC�Ƿ���Ի���Ҳ��Ҫ����
        public int animationInstanceID;//NPC��������ֱ��ʵ��������Ϊ��������ͨ������ID������
    }
}

