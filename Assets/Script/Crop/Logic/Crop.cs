using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//����ű���������
public class Crop : MonoBehaviour
{
    public CropDetails cropDetails;
    public TileDetails tileDetails;
    private int harvestActionCount;
    public bool canHarvest => tileDetails.growthDays >= cropDetails.TotalGrowthDays;//�ж��Ƿ��ܹ��ջ�
    private Animator anim;
    private Transform PlayerTransform => FindObjectOfType<Player>().transform;
    public void ProcessToolAction(ItemDetails tool,TileDetails tile)//ִ�й��ߵ���Ϊ
    {
        tileDetails = tile;
        //����ʹ�ô���
        int requireActionCount = cropDetails.GetTotalRequireCount(tool.itemID);
        if (requireActionCount == -1) return;
        anim = GetComponentInChildren<Animator>();
        //���������
        if (harvestActionCount < requireActionCount)//��Ĳ���(������ľ��Ҫ��5�²��ܿ���)
        {
            harvestActionCount++;
            //�ж��Ƿ��ж��� ��ľ
            if (anim != null&&cropDetails.hasAnimation)
            {
                if (PlayerTransform.position.x < transform.position.x)
                {
                    anim.SetTrigger("RotateRight");
                }
                else
                {
                    anim.SetTrigger("RotateLeft");
                }
            }
            //��������
            if (cropDetails.hasParticalEffect)
            {
                EventHandler.CallParticaleEffectEvent(cropDetails.effectType, transform.position + cropDetails.effectPos);
            }

            //��������
            if (cropDetails.soundEffect != SoundName.none)
            {
                EventHandler.CallPlaySoundEvent(cropDetails.soundEffect);
            }
        }
        if (harvestActionCount >= requireActionCount)
        {
            if (cropDetails.generateAtPlayerPostion||!cropDetails.hasAnimation)//�жϴӵص�����(ũ����)����ͷ������(��ľ)
            {
                //����ũ����
                SpawnHarvestItems();
            }
            else if(cropDetails.hasAnimation)
            {
                if (PlayerTransform.position.x < transform.position.x)
                {
                    anim.SetTrigger("FallingRight");
                }
                else
                {
                    anim.SetTrigger("FallingLeft");
                }
                EventHandler.CallPlaySoundEvent(SoundName.TreeFalling);
                StartCoroutine(HarvestAfterAnimator());
            }
        }
    }
    /// <summary>
    /// �ڶ����������֮�����ɹ�ʵ��Э�̷���
    /// </summary>
    /// <returns></returns>
    private IEnumerator HarvestAfterAnimator()
    {
        while (!anim.GetCurrentAnimatorStateInfo(0).IsName("END"))//��"END"����û�в��������һֱѭ��Ϊnullֱ��"END"����������ϲ����ɹ�ʵ
        {
            yield return null;
        }
        SpawnHarvestItems();
        //ת��Ϊ������
        if (cropDetails.transferItemID > 0)//���жϵ�ǰ������ת������Ʒ
        {
            CreateTransferCrop();
        }

    }
    /// <summary>
    /// ����ת��������
    /// </summary>
    private void CreateTransferCrop()
    {
        tileDetails.seedItemID = cropDetails.transferItemID;//ת����Ʒ
        tileDetails.daysSinceLastHarvest = -1;//���ڲ������ո���
        tileDetails.growthDays = 0;
        EventHandler.CallRefreshCurrentMap();//ˢ��һ�µ�ǰ��Ʒ����Ϣ
    }
    /// <summary>
    /// ����ũ���ﺯ��
    /// </summary>
    public void SpawnHarvestItems()
    {
        for (int i = 0; i < cropDetails.producedItemID.Length; i++)
        {
            int amountToProduce;
            if (cropDetails.produceMinAmount[i] == cropDetails.produceMaxAmount[i])
            {
                //����ֻ����ָ��������
                amountToProduce = cropDetails.produceMinAmount[i];
            }
            else
            {
                //��Ʒ�������
                amountToProduce = Random.Range(cropDetails.produceMinAmount[i], cropDetails.produceMaxAmount[i] + 1);
            }
            //ִ������ָ����������Ʒ
            for (int j = 0; j < amountToProduce; j++)
            {
                if (cropDetails.generateAtPlayerPostion)
                {
                    EventHandler.CallHarvestAtPlayerPostion(cropDetails.producedItemID[i]);
                }
                else//�������ͼ��������Ʒ
                {
                    //�ж�Ӧ�����ɵ���Ʒ����
                    var dirX = transform.position.x > PlayerTransform.position.x ? 1 : -1;
                    //һ����Χ�����
                    var spawnPos = new Vector3(transform.position.x + Random.Range(dirX, cropDetails.spawnRadius.x * dirX), transform.position.y + Random.Range(-cropDetails.spawnRadius.y, cropDetails.spawnRadius.y), 0);
                    //�����糡����������Ʒ
                    EventHandler.CallInstantiateItemInScene(cropDetails.producedItemID[i], spawnPos);
                }
            }
        }
        if (tileDetails != null)
        {
            tileDetails.daysSinceLastHarvest++;//�����ظ��ջ���һ��
            //�ж��Ƿ�����ظ�����
            if (cropDetails.daysToRegrow > 0 && tileDetails.daysSinceLastHarvest < cropDetails.regrowTimes)
            {
                tileDetails.growthDays = cropDetails.TotalGrowthDays - cropDetails.daysToRegrow;
                //ˢ������
                EventHandler.CallRefreshCurrentMap();//����ˢ�����ӽ׶���ֱ��ˢ�����ŵ�ͼ,������ֻ����ˢ���������
                /*����ֻ�Ǹı��˵�ǰ���ӵ������׶ε�ʱ��,����ˢ�����ŵ�ͼ�Ļ�,ֻ�е�ǰ�������ݷ����仯��,������Ӱ���ͼ�ϵ�������Ʒ*/
            }
            else//�����ظ�����
            {
                tileDetails.daysSinceLastHarvest = -1;
                tileDetails.seedItemID = -1;
            }
            Destroy(gameObject);
        }
    }
}
