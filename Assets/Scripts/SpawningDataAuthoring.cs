using Unity.Entities;
using UnityEngine;

public class SpawningDataAuthoring : MonoBehaviour
{
    public GameObject staticPrefab;
    public GameObject dynamicPrefab;
    public class Baker : Baker<SpawningDataAuthoring>
    {
        public override void Bake(SpawningDataAuthoring authoring)
        {
            AddComponent(new SpawningData()
            {
                staticPrefab = GetEntity(authoring.staticPrefab),
                dynamicPrefab = GetEntity(authoring.dynamicPrefab)
            });
        }
    }
}
