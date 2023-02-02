using UnityEngine;
[ExecuteAlways]//代表该脚本需要一直运行
public class DataGUID : MonoBehaviour
{
    public string guid;
    private void Awake()
    {
        if (guid == string.Empty)
        {
            guid = System.Guid.NewGuid().ToString();//生产新的GUID,GUID:是一个十六位的字符串且是唯一性的,用它来保存每一个SaveableItem
        }
    }
}
