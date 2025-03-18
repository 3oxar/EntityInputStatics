using Unity.Entities;
using UnityEngine;

class FlyBulletAuthoring : MonoBehaviour
{
    public bool IsDestroy;
    public float BulletVelosity;
}

class FlyBulletAuthoringBaker : Baker<FlyBulletAuthoring>
{
    public override void Bake(FlyBulletAuthoring authoring)
    {
        Entity entity = GetEntity(authoring, TransformUsageFlags.Dynamic);

        AddComponent(entity, new FlyBulletComponent
        {
            IsDestroy = authoring.IsDestroy,
            BulletVelosity = authoring.BulletVelosity
        });
    }
}

struct FlyBulletComponent : IComponentData
{
    public bool IsDestroy;//����� �� ���� �������� ��� ������������ � ��������
    public float BulletVelosity;//�������� ������ ����

}
