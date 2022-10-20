using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace MFarm.Transition
{
    public class TransitionManager : MonoBehaviour
    {
        public string startSceneName = string.Empty;
        private void Start()
        {
            StartCoroutine(LoadSceneSetActive(startSceneName));
        }
        private void OnEnable()
        {
            EventHandler.TransitionEvent += OnTransitionEvent;
        }
        private void OnDisable()
        {
            EventHandler.TransitionEvent -= OnTransitionEvent;
        }

        private void OnTransitionEvent(string sceneToGo, Vector3 positionToGo)
        {
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
            yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());//卸载掉当前场景
            yield return LoadSceneSetActive(sceneName);//加载新的场景
            //移动人物坐标
            EventHandler.CallMoveToPosition(targetPosition);//场景加载好了就把人物挪过去
            EventHandler.CallAfterSceneLoadedEvent();//加载场景之后又需要做一些事件
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
    }
}

