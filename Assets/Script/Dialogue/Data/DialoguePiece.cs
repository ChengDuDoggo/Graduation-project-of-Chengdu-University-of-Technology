using UnityEngine;
namespace MFarm.Dialogue
{
    [System.Serializable]//���л���Ľű�,���ݻ������inspector����п��Կ��ӻ��޸�
    public class DialoguePiece
    {
        [Header("�Ի�����")]
        public Sprite faceImage;
        public bool onLeft;
        public string name;
        [TextArea]//��������һ��Ƭ���ı���
        public string dialogueText;
        public bool hasToPause;
        [HideInInspector]public bool isDone;//�ж϶Ի��Ƿ����
    }
}

