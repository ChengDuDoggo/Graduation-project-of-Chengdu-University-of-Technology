using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//��Ʒҡ��Ч���ű�
public class ItemInterActive : MonoBehaviour
{
    private bool isAnimating;//�ж��Ƿ����ڲ��Ŷ���
    private WaitForSeconds pause = new WaitForSeconds(0.04f);//�������ʱ��
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isAnimating)
        {
            if (other.transform.position.x < transform.position.x)
            {
                //�Է������ ����ҡ��
                StartCoroutine(RotateRight());
            }
            else
            {
                //�Է����Ҳ� ����ҡ��
                StartCoroutine(RotateLeft());
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (!isAnimating)
        {
            if (other.transform.position.x > transform.position.x)
            {
                //�Է������ ����ҡ��
                StartCoroutine(RotateRight());
            }
            else
            {
                //�Է����Ҳ� ����ҡ��
                StartCoroutine(RotateLeft());
            }
        }
    }
    private IEnumerator RotateLeft()//ʹ��Я������ɶ���Ч��,�������̳�ͻ
    {
        isAnimating = true;
        for (int i = 0; i < 4; i++)
        {
            transform.GetChild(0).Rotate(0, 0, 2);
            yield return pause;
        }
        for (int i = 0; i < 5; i++)
        {
            transform.GetChild(0).Rotate(0, 0, -2);
            yield return pause;
        }
        transform.GetChild(0).Rotate(0, 0, 2);
        yield return pause;
        isAnimating = false;
    }
    private IEnumerator RotateRight()//ʹ��Я������ɶ���Ч��,�������̳�ͻ
    {
        isAnimating = true;
        for (int i = 0; i < 4; i++)
        {
            transform.GetChild(0).Rotate(0, 0, -2);
            yield return pause;
        }
        for (int i = 0; i < 5; i++)
        {
            transform.GetChild(0).Rotate(0, 0, 2);
            yield return pause;
        }
        transform.GetChild(0).Rotate(0, 0, -2);
        yield return pause;
        isAnimating = false;
    }
}
