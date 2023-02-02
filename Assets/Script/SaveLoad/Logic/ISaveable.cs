//创建一个接口ISaveable(接口不需要继承Monobehavier)
//接口是interface类型而不是class
namespace MFarm.Save
{
    public interface ISaveable
    {
        //调用ISaveable的类都必须实现以下函数方法

        string GUID { get; }//让每一个继承ISaveable接口的类必须要有一个GUID
        void RegisterSaveable()//当有一个类调用了该接口,那么调用的当前接口就会立即存储到SaveLoadManager中的列表中去
        {
            SaveLoadManager.Instance.RegisterSaveable(this);
        }
        //Save功能函数,返回一个GameSaveData类将其数据序列化保存到本地
        GameSaveData GenerateSaveData();//抽象函数方法,返回值为GameSaveData(该函数是为了存储数据,将所有数据存储进去并返回一个返回值为GameSaveData类)

        //Load功能函数,将保存的序列化数据赋值给本地
        void RestoreData(GameSaveData saveDate);//没有返回值的函数方法,该函数方法是通过读取来将所有数据又赋值在游戏所有的变量上
    }
}


