using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour //������һ�����������
{
    private Rigidbody2D rb;
    public float speed;
    private float InputX;
    private float InputY;
    private Vector2 MovementInput;
    private Animator[] animators;
    private bool isMoving;
    private bool inputDisable;//��Ҵ�ʱ���ܲ���
    //����ʹ�ù���
    private float mouseX;
    private float mouseY;
    private bool useTool;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animators = GetComponentsInChildren<Animator>();
    }
    private void OnEnable()
    {
        EventHandler.BeforeSceneUnloadEvent += OnBeforeSceneUnloadEvent;
        EventHandler.AfterSceneLoadedEvent += OnAfterSceneLoadedEvent;
        EventHandler.MoveToPosition += OnMoveToPosition;
        EventHandler.MouseClickedEvent += OnMouseClickedEvent;
    }
    private void OnDisable()
    {
        EventHandler.BeforeSceneUnloadEvent -= OnBeforeSceneUnloadEvent;
        EventHandler.AfterSceneLoadedEvent -= OnAfterSceneLoadedEvent;
        EventHandler.MoveToPosition -= OnMoveToPosition;
        EventHandler.MouseClickedEvent -= OnMouseClickedEvent;
    }

    private void OnMouseClickedEvent(Vector3 mouseWorldPos, ItemDetails itemDatils)
    {
        if (useTool)
            return;
        //TODO:ִ�ж���
        if(itemDatils.itemType!= ItemType.Seed && itemDatils.itemType != ItemType.Commodity && itemDatils.itemType != ItemType.Furniture)
        {
            mouseX = mouseWorldPos.x - transform.position.x;
            mouseY = mouseWorldPos.y - transform.position.y;
            if (Mathf.Abs(mouseX) > Mathf.Abs(mouseY))
            {
                mouseY = 0;
            }
            else
            {
                mouseX = 0;
            }
            StartCoroutine(UseToolRoutine(mouseWorldPos, itemDatils));
        }
        else
        {
            EventHandler.CallExecuteActionAfterAnimation(mouseWorldPos, itemDatils);
        }
    }
    private IEnumerator UseToolRoutine(Vector3 mouseWorldPos,ItemDetails itemDetails)
    {
        useTool = true;
        inputDisable = true;
        yield return null;
        foreach (var anim in animators)
        {
            anim.SetTrigger("useTool");
            //������泯����
            anim.SetFloat("InputX", mouseX);
            anim.SetFloat("InputY", mouseY);
        }
        yield return new WaitForSeconds(0.45f);
        EventHandler.CallExecuteActionAfterAnimation(mouseWorldPos, itemDetails);
        yield return new WaitForSeconds(0.25f);
        //�ȴ���������
        useTool = false;
        inputDisable = false;
    }

    private void OnBeforeSceneUnloadEvent()
    {
        inputDisable = true;
    }

    private void OnAfterSceneLoadedEvent()
    {
        inputDisable = false;//�����곡��֮����Ҳ��ܲٿ�
    }

    private void OnMoveToPosition(Vector3 targetPosition)
    {
        transform.position = targetPosition;
    }

    private void Update()
    {
        if (!inputDisable)
        {
            PlayerInput();
        }
        else
        {
            isMoving = false;
        }

        SwitchAnimator();
    }
    private void FixedUpdate()
    {
        if(!inputDisable)
        Movement();
    }
    private void PlayerInput()
    {
        InputX = Input.GetAxisRaw("Horizontal");
        InputY = Input.GetAxisRaw("Vertical");
        if (InputX != 0 && InputY != 0)
        {
            InputX *= 0.6f;
            InputY *= 0.6f;
        }
        //��·״̬�ٶ�
        if (Input.GetKey(KeyCode.LeftShift))
        {
            InputX *= 0.5f;
            InputY *= 0.5f;
        }
        MovementInput = new Vector2(InputX, InputY);
        isMoving = MovementInput != Vector2.zero;
    }
    private void Movement()
    {
        rb.MovePosition(rb.position + MovementInput * speed * Time.deltaTime);
    }
    private void SwitchAnimator()
    {
        foreach (var anim in animators)
        {
            anim.SetBool("isMoving", isMoving);
            anim.SetFloat("mouseX", mouseX);
            anim.SetFloat("mouseY", mouseY);
            if (isMoving)
            {
                anim.SetFloat("InputX", InputX);
                anim.SetFloat("InputY", InputY);
            }
        }
    }
}
