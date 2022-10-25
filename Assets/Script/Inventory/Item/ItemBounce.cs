using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MFarm.Inventory
{
    public class ItemBounce : MonoBehaviour
    {
        private Transform spriteTrans;
        private BoxCollider2D coll;
        public float gravity = -3.5f;//����
        private bool isGround;//�ж��Ƿ��Ѿ����
        private float distance;//�׳��ľ���
        private Vector2 direction;//�׳��ķ���
        private Vector3 targetPos;//Ŀ���������Ϣ
        private void Awake()
        {
            spriteTrans = transform.GetChild(0);
            coll = GetComponent<BoxCollider2D>();
            coll.enabled = false;//��Ʒ���ӳ�ȥʱ�ر���ײ��,���ⷢ��������ײ
        }
        private void Update()
        {
            Bounce();
        }
        /// <summary>
        /// ��ʼ���������Ʒ
        /// </summary>
        /// <param name="target">Ŀ��λ��</param>
        /// <param name="dir">����</param>
        public void InitBounceItem(Vector3 target,Vector2 dir)
        {
            coll.enabled = false;
            direction = dir;
            targetPos = target;
            distance = Vector3.Distance(target, transform.position);
            spriteTrans.position += Vector3.up * 1.5f;
        }
        /// <summary>
        /// �����ȥ
        /// </summary>
        private void Bounce()
        {
            //�ж��Ƿ���½
            isGround = spriteTrans.position.y <= transform.position.y;
            if (Vector3.Distance(transform.position, targetPos) > 0.1f)//����û����Ŀ���
            {
                transform.position += (Vector3)direction * distance * -gravity * Time.deltaTime;
            }
            if (!isGround)//����û�������
            {
                spriteTrans.position += Vector3.up * gravity * Time.deltaTime;
            }
            else
            {
                spriteTrans.position = transform.position;
                coll.enabled = true;
            }
        }
    }
}

