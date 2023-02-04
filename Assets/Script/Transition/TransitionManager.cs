using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MFarm.Save;
namespace MFarm.Transition
{
    public class TransitionManager : MonoBehaviour,ISaveable
    {
        [SceneName]//�Լ���д��Unity�������ܱ��
        public string startSceneName = string.Empty;
        private CanvasGroup fadeCanvasGroup;
        private bool isFade;//�жϳ����л������������Ƿ����

        public string GUID => GetComponent<DataGUID>().guid;
        private void Awake()
        {
            SceneManager.LoadScene("UI", LoadSceneMode.Additive);
        }
        private IEnumerator Start()
        {
            ISaveable saveable = this;
            saveable.RegisterSaveable();
            fadeCanvasGroup = FindObjectOfType<CanvasGroup>();
            yield return LoadSceneSetActive(startSceneName);
            EventHandler.CallAfterSceneLoadedEvent();
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
            EventHandler.CallAfterSceneLoadedEvent();//���س���֮������Ҫ��һЩ�¼�
            yield return Fade(0);
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
        private IEnumerator LoadSaveDataScene(string sceneName)
        {
            yield return Fade(1f);
            if (SceneManager.GetActiveScene().name != "PersistentScene")//����Ϸ�����м����������Ϸ����
            {
                EventHandler.CallBeforeSceneUnloadEvent();
                yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
            }
            yield return LoadSceneSetActive(sceneName);
            EventHandler.CallAfterSceneLoadedEvent();
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
            //������Ϸ���ȳ���
            StartCoroutine(LoadSaveDataScene(saveDate.dataSceneName));
        }
    }
}

