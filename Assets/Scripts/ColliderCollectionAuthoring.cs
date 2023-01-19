using System.Collections.Generic;
using Unity.Entities;
using Unity.Physics.Authoring;
using UnityEngine;

public class ColliderCollectionAuthoring : MonoBehaviour
{
    public List<PhysicsShapeAuthoring> colliders;
    public class Baker : Baker<ColliderCollectionAuthoring>
    {
        public override void Bake(ColliderCollectionAuthoring authoring)
        {
            var buffer = AddBuffer<ColliderCollectionData>();
            for (int i = 0; i < authoring.colliders.Count; i++)
            {
                buffer.Add(new ColliderCollectionData()
                {
                    collider = GetEntity(authoring.colliders[i])
                });
            }
        }
    }

    private void OnValidate()
    {
        this.colliders = new List<PhysicsShapeAuthoring>(GetComponentsInChildren<PhysicsShapeAuthoring>());
    }
}