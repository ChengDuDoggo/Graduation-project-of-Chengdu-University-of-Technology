using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;//Unity�Դ��Ķ����ϵͳ

public class PoolManager : MonoBehaviour
{
    public List<GameObject> poolPrefabs;//����һ���б�������Ч��Ԥ����
    //�ٴ���һ���б��Ŷ����(ÿһ�����������һ���������б�)
    //��Ϊ�������в�ͬ����Ч,��ͬ�������Ч����Ҫ�����������ͬ����Ч��
    //���,���м��������,ÿһ����������м�����Ч,���Խ������Ҳ����List��
    private List<ObjectPool<GameObject>> poolEffectList = new List<ObjectPool<GameObject>>();
    private Queue<GameObject> soundQueue = new Queue<GameObject>();//Queue:����(�Ƚ��ȳ�,�������������Ŷ�)
    private void Start()
    {
        CreatPool();//��Ϸһ��ʼ��ҪΪÿһ����ЧԤ���崴��һ�������,��Ӧ�Ķ���طŶ�Ӧ����Ч
    }
    private void OnEnable()
    {
        //ע�Ქ������Ч��ί���¼�
        EventHandler.ParticaleEffectEvent += OnParticaleEffectEvent;
        EventHandler.InitSoundEffect += InitSoundEffect;
    }
    private void OnDisable()
    {
        EventHandler.ParticaleEffectEvent -= OnParticaleEffectEvent;
        EventHandler.InitSoundEffect -= InitSoundEffect;
    }
    /// <summary>
    /// ���������,Ϊÿһ����ЧԤ���崴��һ��ר���Ķ����
    /// </summary>
    private void CreatPool()
    {
        foreach (GameObject item in poolPrefabs)
        {
            Transform parent = new GameObject(item.name).transform;//���������ʱ,�ȴ���һ���µ�GameObject��Ϊ������������Ч�ĸ�����
            parent.SetParent(transform);//���parent���������������ؽű���������

            //���������,Ϊ�˼�෽��,�������ʹ����ķ����ʽ
            //��ķ����ʽʵ�ʾ���һ���������صļ�д��ʽ,���Բ���,����ʹ�õĻ����ô�����߼�
            var newPool = new ObjectPool<GameObject>(
                    () => Instantiate(item,parent),//���ص�һ��:���������ʱ��Ҫ��ʲô(����)[Get����ʱ]
                    e => { e.SetActive(true); },//���صڶ���:�Ӷ�����еõ�����ʱ��Ҫ��ʲô(����)[Get����ʱ]
                    e => { e.SetActive(false); },//���ص�����:�Ӷ�������ͷŵ���Ʒ��ʱ����Ҫ��ʲô(����)[Relesea����ʱ]
                    e => { Destroy(e); }//���ص�����:���ٶ����ʱ��Ҫ��ʲô(����)
                    //e,������Ƕ�����д�ŵ�ÿһ��Object,������GameObject���͵�
                    //�����еĺ�������������ָ���ĺ�������ʱ�����ص�,�ص�ʱִ�����еĺ���
                );
            poolEffectList.Add(newPool);//���´����Ķ���ط��뵽������б���
        }
    }
    private void OnParticaleEffectEvent(ParticaleEffectType effectType, Vector3 pos)
    {
        //���ݷ��ص���Ч����,�õ���Ӧ�Ķ����(�������Լ��������˶����,��Ҫ�õ�����ض������һϵ�в���)
        //WORKFLLOW:������Ч��ʱ��ȫ
        ObjectPool<GameObject> objPool = effectType switch//�﷨��
        {
            ParticaleEffectType.LeavesFalling01 => poolEffectList[0],
            ParticaleEffectType.LeavesFalling02 => poolEffectList[1],
            ParticaleEffectType.Rock => poolEffectList[2],
            ParticaleEffectType.ReapableScenery => poolEffectList[3],
            _ => null,//Ĭ��Ϊnull
        };
        GameObject obj = objPool.Get();//�Ӷ�������ó��������Ч(����);Get():Unity�����ϵͳ�Դ��ĺ�������
        obj.transform.position = pos;//�趨��Ч����������
        StartCoroutine(ReleaseRoutine(objPool, obj));
    }
    /// <summary>
    /// ͨ��Я�̵ȴ�������ʱ��֮����ִ�ж����ϵͳ�Դ���Release����,�ͷŵ�������о������Ʒ
    /// </summary>
    /// <param name="pool">�õ��Ķ����</param>
    /// <param name="obj">������о������Ч</param>
    /// <returns></returns>
    private IEnumerator ReleaseRoutine(ObjectPool<GameObject> pool,GameObject obj)
    {
        yield return new WaitForSeconds(1.5f);
        pool.Release(obj);//Unity�����ϵͳ�Դ����ͷŶ�����
    }
/*    private void InitSoundEffect(SoundDetails soundDetails)
    {
        ObjectPool<GameObject> pool = poolEffectList[4];
        var obj = pool.Get();
        obj.GetComponent<Sound>().SetSound(soundDetails);
        StartCoroutine(DisableSound(pool, obj, soundDetails));
    }
    private IEnumerator DisableSound(ObjectPool<GameObject> pool,GameObject obj,SoundDetails soundDetails)
    {
        yield return new WaitForSeconds(soundDetails.soundClip.length);
        pool.Release(obj);
    }*/
    //��ͳ��������صķ�ʽ
    private void CreateSoundPool()//������Ч�����
    {
        var parent = new GameObject(poolPrefabs[4].name).transform;
        parent.SetParent(transform);
        for (int i = 0; i < 20; i++)
        {
            GameObject newObj = Instantiate(poolPrefabs[4], parent);
            newObj.SetActive(false);
            soundQueue.Enqueue(newObj);//��������е�objѹ�����
        }
    }
    private GameObject GetPoolObject()//�Ӷ�����еõ���Ч
    {
        if (soundQueue.Count < 2)
            CreateSoundPool();//�������ص�����С��2�����´��������(����)
        return soundQueue.Dequeue();//���ض������Ƚ�����Ч(������ѭ�Ƚ��ȳ�����)
    }
    private void InitSoundEffect(SoundDetails soundDetails)
    {
        var obj = GetPoolObject();
        obj.GetComponent<Sound>().SetSound(soundDetails);
        obj.SetActive(true);
        StartCoroutine(DisableSound(obj, soundDetails.soundClip.length));   
    }
    private IEnumerator DisableSound(GameObject obj,float duration)
    {
        yield return new WaitForSeconds(duration);
        obj.SetActive(false);
        soundQueue.Enqueue(obj);//��Ч������Ϻ����½���ѹ�����
    }
}
