using UnityEngine;
namespace MFarm.Dialogue
{
    [System.Serializable]//序列化后的脚本,数据会呈现在inspector面板中可以可视化修改
    public class DialoguePiece
    {
        [Header("对话详情")]
        public Sprite faceImage;
        public bool onLeft;
        public string name;
        [TextArea]//可以输入一大片的文本框
        public string dialogueText;
        public bool hasToPause;
        [HideInInspector]public bool isDone;//判断对话是否结束
    }
}

