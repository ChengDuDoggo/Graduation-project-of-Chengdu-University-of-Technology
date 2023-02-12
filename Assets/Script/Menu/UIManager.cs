using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private GameObject menuCanvas;
    public GameObject menuPrefab;
    public Button settingsBtn;
    public GameObject pausePanel;
    public Slider volumeSlider;
    private void Awake()
    {
        settingsBtn.onClick.AddListener(TogglePausePanel);
    }
    private void OnEnable()
    {
        EventHandler.AfterSceneLoadedEvent += OnAfterSceneLoadedEvent;
    }
    private void OnDisable()
    {
        EventHandler.AfterSceneLoadedEvent -= OnAfterSceneLoadedEvent;
    }

    private void OnAfterSceneLoadedEvent()
    {
        if (menuCanvas.transform.childCount > 0)
        {
            Destroy(menuCanvas.transform.GetChild(0).gameObject);
        }
    }

    private void Start()
    {
        menuCanvas = GameObject.FindWithTag("MenuCanvas");
        Instantiate(menuPrefab, menuCanvas.transform);
    }
    private void TogglePausePanel()
    {
        bool isOpen = pausePanel.activeInHierarchy;//�жϵ�ǰGameObject�Ƿ���Hierarchy������Ǽ���״̬
        if (isOpen)
        {
            pausePanel.SetActive(false);
            Time.timeScale = 1;
        }
        else
        {
            System.GC.Collect();//����Ϸ��ͣʱǿ�ƽ�����������,�����Ϸ����
            pausePanel.SetActive(true);
            Time.timeScale = 0;
        }
    }
    public void ReturnMenuCanvas()
    {
        Time.timeScale = 1;
        StartCoroutine(BackToMenu());
    }
    private IEnumerator BackToMenu()
    {
        pausePanel.SetActive(false);
        yield return new WaitForSeconds(1.0f);
        Instantiate(menuPrefab, menuCanvas.transform);
    }
}
