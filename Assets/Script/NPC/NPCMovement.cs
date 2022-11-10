using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MFarm.AStar;
using UnityEngine.SceneManagement;
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class NPCMovement : MonoBehaviour
{
    //��ʱ�洢��Ϣ
    [SerializeField]private string currentScene;
    private string targetScene;
    private Vector3Int currentGridPostion;//��ǰ����λ��
    private Vector3Int targetGridPostion;//Ŀ������λ��
    public string StartScene { set => currentScene = value; }//NPC��һ��ʼ���ڵĳ���
    [Header("�ƶ�����")]
    public float normalSpeed = 2f;
    private float minSpeed = 1;
    private float maxSpeed = 3;
    private Vector2 dir;//����
    public bool isMoving;
    //Component
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D coll;
    private Animator anim;
    private Stack<MovementStep> movementSteps;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
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
        CheckVisiable();
    }

    private void CheckVisiable()
    {
        if (currentScene == SceneManager.GetActiveScene().name)
        {
            SetActiveInScene();
        }
        else
        {
            SetInActiveInScene();
        }
    }
    #region ����NPC��ʾ���
    private void SetActiveInScene()
    {
        spriteRenderer.enabled = true;
        coll.enabled = true;
        //TODO:Ӱ�ӿ���
        //transform.GetChild(0).gameObject.SetActive(true);
    }
    private void SetInActiveInScene()
    {
        spriteRenderer.enabled = false;
        coll.enabled = false;
        //TODO:Ӱ�ӹر�
        //transform.GetChild(0).gameObject.SetActive(fales);
    }
    #endregion
}
