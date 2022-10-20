using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace MFarm.Transition
{
    public class TransitionManager : MonoBehaviour
    {
        [SceneName]//�Լ���д��Unity�������ܱ��
        public string startSceneName = string.Empty;
        private CanvasGroup fadeCanvasGroup;
        private bool isFade;//�жϳ����л������������Ƿ����
        private void Start()
        {
            StartCoroutine(LoadSceneSetActive(startSceneName));
            fadeCanvasGroup = FindObjectOfType<CanvasGroup>();
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
            if(!isFade)
            StartCoroutine(Transition(sceneToGo, positionToGo));
        }

        /// <summary>
        /// �����л�
        /// </summary>
        /// <param name="sceneName">Ŀ�곡��</param>
        /// <param name="targetPosition">����Ŀ�곡����λ��</param>
        /// <returns></returns>
        private IEnumerator Transition(string sceneName,Vector3 targetPosition)
        {
            EventHandler.CallBeforeSceneUnloadEvent();//��ִ��һ��ж�س���֮ǰҪ�����¶�
            yield return Fade(1);
            yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());//ж�ص���ǰ����
            yield return LoadSceneSetActive(sceneName);//�����µĳ���
            //�ƶ���������
            EventHandler.CallMoveToPosition(targetPosition);//�������غ��˾Ͱ�����Ų��ȥ
            yield return Fade(0);
            EventHandler.CallAfterSceneLoadedEvent();//���س���֮������Ҫ��һЩ�¼�
        }
        /// <summary>
        /// ���س���������Ϊ����
        /// </summary>
        /// <param name="sceneName">���صĳ�����</param>
        /// <returns></returns>
        private IEnumerator LoadSceneSetActive(string sceneName)//���еĳ����л������ض���Ҫʹ��Э�̵��첽���������ϳ��������л��������Ͱ�ȫ
        {
            yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);//�첽����(���صĳ�����,���ص�ģʽ:����)��ʱֻ�Ǽ����˳�������û�м���
            //����ģʽ:һ����Ŀ��ͬʱ�������ೡ��������һ��һ�������������������ʹ��ڷǼ���״̬
            //����ģʽ:һ����Ŀ��һ����ֻ����һ���������л�����ʱ���������л���������ǰ�����л���ȥ
            Scene newScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);//��ȡ���뵽������������ĵ�ǰ����
            SceneManager.SetActiveScene(newScene);//�����
        }
        /// <summary>
        /// ���뵭������
        /// </summary>
        /// <param name="targetAlpha">1�Ǻ�,0��͸��</param>
        /// <returns></returns>
        private IEnumerator Fade(float targetAlpha)//�����л�ʱ�ļ���ת���������ʺ���Э�������
        {
            isFade = true;//��ʼ������ֵת��
            fadeCanvasGroup.blocksRaycasts = true;//��������ڵ�,��������ʱ��겻�ܵ���κ���Ʒ
            float speed = Mathf.Abs(fadeCanvasGroup.alpha - targetAlpha) / Settings.fadeDuration;//���һ����������͸�����ٶ�
            while (!Mathf.Approximately(fadeCanvasGroup.alpha, targetAlpha))//����ǰ������ֵ������Ŀ�갢����ֵ�ͻ�һֱִ�ж���ת��
            {
                fadeCanvasGroup.alpha = Mathf.MoveTowards(fadeCanvasGroup.alpha, targetAlpha, speed * Time.deltaTime);
                yield return null;
            }
            fadeCanvasGroup.blocksRaycasts = false;//����ִ�����,�ر�����ڵ�
            isFade = false;
        }
    }
}

