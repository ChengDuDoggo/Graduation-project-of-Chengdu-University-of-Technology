using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MFarm.Inventory
{
    public class ItemBounce : MonoBehaviour
    {
        private Transform spriteTrans;
        private BoxCollider2D coll;
        public float gravity = -3.5f;//重力
        private bool isGround;//判断是否已经落地
        private float distance;//抛出的距离
        private Vector2 direction;//抛出的方向
        private Vector3 targetPos;//目标的坐标信息
        private void Awake()
        {
            spriteTrans = transform.GetChild(0);
            coll = GetComponent<BoxCollider2D>();
            coll.enabled = false;//物品被扔出去时关闭碰撞体,避免发生物体碰撞
        }
        private void Update()
        {
            Bounce();
        }
        /// <summary>
        /// 初始化弹射的物品
        /// </summary>
        /// <param name="target">目标位置</param>
        /// <param name="dir">方向</param>
        public void InitBounceItem(Vector3 target,Vector2 dir)
        {
            coll.enabled = false;
            direction = dir;
            targetPos = target;
            distance = Vector3.Distance(target, transform.position);
            spriteTrans.position += Vector3.up * 1.5f;
        }
        /// <summary>
        /// 弹射出去
        /// </summary>
        private void Bounce()
        {
            //判断是否着陆
            isGround = spriteTrans.position.y <= transform.position.y;
            if (Vector3.Distance(transform.position, targetPos) > 0.1f)//横向还没到达目标点
            {
                transform.position += (Vector3)direction * distance * -gravity * Time.deltaTime;
            }
            if (!isGround)//纵向还没到达地面
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

