//����һ���ӿ�ISaveable(�ӿڲ���Ҫ�̳�Monobehavier)
//�ӿ���interface���Ͷ�����class
namespace MFarm.Save
{
    public interface ISaveable
    {
        //����ISaveable���඼����ʵ�����º�������

        string GUID { get; }//��ÿһ���̳�ISaveable�ӿڵ������Ҫ��һ��GUID
        void RegisterSaveable()//����һ��������˸ýӿ�,��ô���õĵ�ǰ�ӿھͻ������洢��SaveLoadManager�е��б���ȥ
        {
            SaveLoadManager.Instance.RegisterSaveable(this);
        }
        //Save���ܺ���,����һ��GameSaveData�ཫ���������л����浽����
        GameSaveData GenerateSaveData();//����������,����ֵΪGameSaveData(�ú�����Ϊ�˴洢����,���������ݴ洢��ȥ������һ������ֵΪGameSaveData��)

        //Load���ܺ���,����������л����ݸ�ֵ������
        void RestoreData(GameSaveData saveDate);//û�з���ֵ�ĺ�������,�ú���������ͨ����ȡ�������������ָ�ֵ����Ϸ���еı�����
    }
}


