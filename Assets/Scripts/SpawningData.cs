using Unity.Entities;

public struct SpawningData : IComponentData
{
    public Entity staticPrefab;
    public Entity dynamicPrefab;
}
