using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;//Unity自带的对象池系统

public class PoolManager : MonoBehaviour
{
    public List<GameObject> poolPrefabs;//创建一个列表存放粒子效果预制体
    //再创建一个列表存放对象池(每一个对象池又是一个单独的列表)
    //因为场景中有不同的特效,不同种类的特效又需要创建出许多相同的特效。
    //因此,会有几个对象池,每一个对象池中有几个特效,所以将对象池也放入List中
    private List<ObjectPool<GameObject>> poolEffectList = new List<ObjectPool<GameObject>>();
    private Queue<GameObject> soundQueue = new Queue<GameObject>();//Queue:队列(先进先出,类似于做核酸排队)
    private void Start()
    {
        CreatPool();//游戏一开始就要为每一个特效预制体创建一个对象池,对应的对象池放对应的特效
    }
    private void OnEnable()
    {
        //注册播放粒子效果委托事件
        EventHandler.ParticaleEffectEvent += OnParticaleEffectEvent;
        EventHandler.InitSoundEffect += InitSoundEffect;
    }
    private void OnDisable()
    {
        EventHandler.ParticaleEffectEvent -= OnParticaleEffectEvent;
        EventHandler.InitSoundEffect -= InitSoundEffect;
    }
    /// <summary>
    /// 创建对象池,为每一个特效预制体创建一个专属的对象池
    /// </summary>
    private void CreatPool()
    {
        foreach (GameObject item in poolPrefabs)
        {
            Transform parent = new GameObject(item.name).transform;//创建对象池时,先创建一个新的GameObject作为创建出来的特效的父物体
            parent.SetParent(transform);//这个parent本身又是这个对象池脚本的子物体

            //创建对象池,为了简洁方便,这里可以使用拉姆达表达式
            //拉姆达表达式实质就是一个函数重载的简写形式,可以不用,但是使用的话会让代码更高级
            var newPool = new ObjectPool<GameObject>(
                    () => Instantiate(item,parent),//重载第一项:触发对象池时你要做什么(函数)[Get调用时]
                    e => { e.SetActive(true); },//重载第二项:从对象池中得到对象时你要做什么(函数)[Get调用时]
                    e => { e.SetActive(false); },//重载第三项:从对象池中释放掉物品的时候你要做什么(函数)[Relesea调用时]
                    e => { Destroy(e); }//重载第四项:销毁对象池时你要做什么(函数)
                    //e,代表的是对象池中存放的每一个Object,这里是GameObject类型的
                    //重载中的函数将会在其他指定的函数调用时产生回调,回调时执行其中的函数
                );
            poolEffectList.Add(newPool);//将新创建的对象池放入到对象池列表当中
        }
    }
    private void OnParticaleEffectEvent(ParticaleEffectType effectType, Vector3 pos)
    {
        //根据返回的特效类型,得到对应的对象池(这里是以及创建好了对象池,需要拿到对象池对其进行一系列操作)
        //WORKFLLOW:根据特效随时补全
        ObjectPool<GameObject> objPool = effectType switch//语法糖
        {
            ParticaleEffectType.LeavesFalling01 => poolEffectList[0],
            ParticaleEffectType.LeavesFalling02 => poolEffectList[1],
            ParticaleEffectType.Rock => poolEffectList[2],
            ParticaleEffectType.ReapableScenery => poolEffectList[3],
            _ => null,//默认为null
        };
        GameObject obj = objPool.Get();//从对象池中拿出具体的特效(对象);Get():Unity对象池系统自带的函数方法
        obj.transform.position = pos;//设定特效产生的坐标
        StartCoroutine(ReleaseRoutine(objPool, obj));
    }
    /// <summary>
    /// 通过携程等待几秒钟时间之后再执行对象池系统自带的Release函数,释放掉对象池中具体的物品
    /// </summary>
    /// <param name="pool">拿到的对象池</param>
    /// <param name="obj">对象池中具体的特效</param>
    /// <returns></returns>
    private IEnumerator ReleaseRoutine(ObjectPool<GameObject> pool,GameObject obj)
    {
        yield return new WaitForSeconds(1.5f);
        pool.Release(obj);//Unity对象池系统自带的释放对象函数
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
    //传统创建对象池的方式
    private void CreateSoundPool()//创建音效对象池
    {
        var parent = new GameObject(poolPrefabs[4].name).transform;
        parent.SetParent(transform);
        for (int i = 0; i < 20; i++)
        {
            GameObject newObj = Instantiate(poolPrefabs[4], parent);
            newObj.SetActive(false);
            soundQueue.Enqueue(newObj);//将对象池中的obj压入队列
        }
    }
    private GameObject GetPoolObject()//从对象池中得到音效
    {
        if (soundQueue.Count < 2)
            CreateSoundPool();//如果对象池的容量小于2则重新创建对象池(队列)
        return soundQueue.Dequeue();//返回队列中先进的特效(队列遵循先进先出规则)
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
        soundQueue.Enqueue(obj);//音效播放完毕后重新将他压入队列
    }
}
