//ר�Ŵ洢ö�ٵ�һ����
public enum ItemType //ͨ��ö�ٿ��Ժܷ�����г���ͬ���ͣ���û��Ҫÿ�����ʹ���һ����
{
    Seed,Commodity/*��Ʒ*/,Furniture,
    HolTool/*���صĹ���*/,ChopTool/*�����Ĺ���*/,BreakTool/*��ʯͷ�Ĺ���*/,ReapTool/*��ݵĹ���*/,WaterTool/*��ˮ�Ĺ���*/,CollectTool/*�Ѽ��˵Ĺ���*/,
    ReapableScenery/*���Ա�����Ӳ�*/
}
public enum SlotType//UI�зŶ����ĸ����ڲ�ͬ�ط��ǲ�ͬ������(�������ϣ��̵��������ȵ�)
{
    Bag,Shop,Box
}
public enum InventoryLocation//�ж�UI���µ�����Ǹ����������ϵĸ��ӻ���������ĸ���
{
    Player,Box
}
public enum PartType//�ж��õ����ϵĹ�������
{
    None,Carry,Hoe,Break,Water,Collect,Chop,Reap
}
public enum PartName//������Ҫ�����Ǹ���λ��ȡ
{
    Body,Hair,Arm,Tool
}
public enum Season//��������
{
    ����,����,����,����
}
public enum GridType//���������
{
    Digable,DropItem,PlaceFurniture,NPCObstacle//NPC���ߵ�·��
}
public enum ParticaleEffectType//��Ч������
{
    None,LeavesFalling01,LeavesFalling02,Rock,ReapableScenery/*�����Ч*/
}
public enum GameState//��Ϸ״̬ö��
{
    Gameplay/*��Ϸ��������״̬*/,Pause/*��Ϸ��ͣ״̬*/
}
public enum LightShift//��Ϸ�ƹ�(���ϻ�������)
{
    Morning,Night
}
public enum SoundName
{
    none,FootStepSoft,FootStepHard,
    Axe,Pickaxe,Hoe,Reap,Water,Basket,Chop,
    Pickup,Plant,TreeFalling,Rustle,
    AmbientCountryside1,AmbientCountryside2,MusicCalm1, MusicCalm2, MusicCalm4, MusicCalm5, MusicCalm6,MusicCalm3, AmbientIndoor1
}
