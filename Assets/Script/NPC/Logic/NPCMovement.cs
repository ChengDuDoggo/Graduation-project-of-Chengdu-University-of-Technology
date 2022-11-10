using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MFarm.AStar;
using UnityEngine.SceneManagement;
using System;
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class NPCMovement : MonoBehaviour
{
    public SchedulDataList_SO schedulData;
    private SortedSet<SchedulDetails> scheduleSet;//ʼ�ձ����������ݵ������Լ�Ψһ
    private SchedulDetails currentSchedule;
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
    private Grid grid;
    private Stack<MovementStep> movementSteps;
    private bool isInitialised;//�ж�NPC�Ƿ��ǵ�һ�μ���
    private TimeSpan GameTime => TimeManager.Instance.GameTime;
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
        grid = FindObjectOfType<Grid>();
        CheckVisiable();
        if (!isInitialised)
        {
            InitNPC();//��һ�μ��صĻ��ͳ�ʼ��NPC
            isInitialised = true;
        }
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
    private void InitNPC()
    {
        targetScene = currentScene;//��ʼ��Ŀ�곡�����ǵ�ǰ����
        //�����ڵ�ǰ������������ĵ�
        currentGridPostion = grid.WorldToCell(transform.position);//��������ת��Ϊ��������
        transform.position = new Vector3(currentGridPostion.x + Settings.gridCellSize / 2f, currentGridPostion.y + Settings.gridCellSize / 2f, 0);
        targetGridPostion = currentGridPostion;
    }
    public void BuildPath(SchedulDetails schedul)
    {
        movementSteps.Clear();//����֮ǰ��ջ�е�·��
        currentSchedule = schedul;
        if (schedul.targetScene == currentScene)
        {
            AStar.Instance.BuildPath(schedul.targetScene, (Vector2Int)currentGridPostion, schedul.targetGridPosition, movementSteps);
        }
        if (movementSteps.Count > 1)//·������1��������ƶ���
        {
            //����ÿһ����ʱ���
            UpdateTimeOnPath();
        }
    }
    private void UpdateTimeOnPath()
    {
        MovementStep previousSetp = null;

        TimeSpan currentGameTime = GameTime;

        foreach (MovementStep step in movementSteps)
        {
            if (previousSetp == null)
                previousSetp = step;

            step.hour = currentGameTime.Hours;
            step.minute = currentGameTime.Minutes;
            step.second = currentGameTime.Seconds;

            TimeSpan gridMovementStepTime;

            if (MoveInDiagonal(step, previousSetp))
                gridMovementStepTime = new TimeSpan(0, 0, (int)(Settings.gridCellDiagonalSize / normalSpeed / Settings.secondThreshold));
            else
                gridMovementStepTime = new TimeSpan(0, 0, (int)(Settings.gridCellSize / normalSpeed / Settings.secondThreshold));

            //�ۼӻ����һ����ʱ���
            currentGameTime = currentGameTime.Add(gridMovementStepTime);
            //ѭ����һ��
            previousSetp = step;
        }
    }

    /// <summary>
    /// �ж��Ƿ���б����
    /// </summary>
    /// <param name="currentStep"></param>
    /// <param name="previousStep"></param>
    /// <returns></returns>
    private bool MoveInDiagonal(MovementStep currentStep, MovementStep previousStep)
    {
        return (currentStep.gridCoordinate.x != previousStep.gridCoordinate.x) && (currentStep.gridCoordinate.y != previousStep.gridCoordinate.y);
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
