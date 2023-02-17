using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MFarm.Save;
namespace MFarm.Transition
{
    public class TransitionManager : Singleton<TransitionManager>,ISaveable
    {
        [SceneName]//自己编写的Unity辅助功能标记
        public string startSceneName = string.Empty;
        private CanvasGroup fadeCanvasGroup;
        private bool isFade;//判断场景切换阿尔法加载是否完成

        public string GUID => GetComponent<DataGUID>().guid;
        protected override void Awake()
        {
            base.Awake();
            SceneManager.LoadScene("UI", LoadSceneMode.Additive);
        }
        private void Start()
        {
            ISaveable saveable = this;
            saveable.RegisterSaveable();
            fadeCanvasGroup = FindObjectOfType<CanvasGroup>();
        }
        private void OnEnable()
        {
            EventHandler.TransitionEvent += OnTransitionEvent;
            EventHandler.StartNewGameEvent += OnStartNewGameEvent;
            EventHandler.EndGameEvent += OnEndGameEvent;
        }
        private void OnDisable()
        {
            EventHandler.TransitionEvent -= OnTransitionEvent;
            EventHandler.StartNewGameEvent -= OnStartNewGameEvent;
            EventHandler.EndGameEvent -= OnEndGameEvent;
        }

        private void OnEndGameEvent()
        {
            StartCoroutine(UnloadScene());
        }

        private void OnStartNewGameEvent(int index)
        {
            StartCoroutine(LoadSaveDataScene(startSceneName));
        }

        private void OnTransitionEvent(string sceneToGo, Vector3 positionToGo)
        {
            if(!isFade)
            StartCoroutine(Transition(sceneToGo, positionToGo));
        }

        /// <summary>
        /// 场景切换
        /// </summary>
        /// <param name="sceneName">目标场景</param>
        /// <param name="targetPosition">到达目标场景的位置</param>
        /// <returns></returns>
        private IEnumerator Transition(string sceneName,Vector3 targetPosition)
        {
            EventHandler.CallBeforeSceneUnloadEvent();//先执行一下卸载场景之前要做的事儿
            yield return Fade(1);
            yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());//卸载掉当前场景
            yield return LoadSceneSetActive(sceneName);//加载新的场景
            //移动人物坐标
            EventHandler.CallMoveToPosition(targetPosition);//场景加载好了就把人物挪过去
            EventHandler.CallAfterSceneLoadedEvent();//加载场景之后又需要做一些事件
            yield return Fade(0);
        }
        /// <summary>
        /// 加载场景并设置为激活
        /// </summary>
        /// <param name="sceneName">加载的场景名</param>
        /// <returns></returns>
        private IEnumerator LoadSceneSetActive(string sceneName)//所有的场景切换，加载都需要使用协程的异步加载来保障场景加载切换的流畅和安全
        {
            yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);//异步加载(加载的场景名,加载的模式:叠加)此时只是加载了场景但是没有激活
            //叠加模式:一个项目中同时存放着许多场景叠加在一起，一个场景激活其他场景就处于非激活状态
            //单独模式:一个项目中一次性只加载一个场景，切换场景时其他场景切换进来，当前场景切换出去
            Scene newScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);//获取放入到场景管理器里的当前场景
            SceneManager.SetActiveScene(newScene);//激活场景
        }
        /// <summary>
        /// 淡入淡出场景
        /// </summary>
        /// <param name="targetAlpha">1是黑,0是透明</param>
        /// <returns></returns>
        private IEnumerator Fade(float targetAlpha)//场景切换时的加载转换动画更适合用协程来完成
        {
            isFade = true;//开始阿尔法值转换
            fadeCanvasGroup.blocksRaycasts = true;//开启鼠标遮挡,场景加载时鼠标不能点击任何物品
            float speed = Mathf.Abs(fadeCanvasGroup.alpha - targetAlpha) / Settings.fadeDuration;//获得一个阿尔法变透明的速度
            while (!Mathf.Approximately(fadeCanvasGroup.alpha, targetAlpha))//当当前阿尔法值不等于目标阿尔法值就会一直执行动画转换
            {
                fadeCanvasGroup.alpha = Mathf.MoveTowards(fadeCanvasGroup.alpha, targetAlpha, speed * Time.deltaTime);
                yield return null;
            }
            fadeCanvasGroup.blocksRaycasts = false;//动画执行完毕,关闭鼠标遮挡
            isFade = false;
        }
        private IEnumerator LoadSaveDataScene(string sceneName)
        {
            yield return Fade(1f);
            if (SceneManager.GetActiveScene().name != "PersistentScene")//在游戏过程中加载另外的游戏进度
            {
                EventHandler.CallBeforeSceneUnloadEvent();
                yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
            }
            yield return LoadSceneSetActive(sceneName);
            EventHandler.CallAfterSceneLoadedEvent();
            yield return Fade(0);
        }
        private IEnumerator UnloadScene()
        {
            EventHandler.CallBeforeSceneUnloadEvent();
            yield return Fade(1f);
            yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
            yield return Fade(0);
        }
        public GameSaveData GenerateSaveData()
        {
            GameSaveData saveData = new GameSaveData();
            saveData.dataSceneName = SceneManager.GetActiveScene().name;
            return saveData;
        }

        public void RestoreData(GameSaveData saveDate)
        {
            //加载游戏进度场景
            StartCoroutine(LoadSaveDataScene(saveDate.dataSceneName));
        }
    }
}

