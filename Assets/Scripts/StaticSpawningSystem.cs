using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;



public partial struct StaticSpawningSystem : ISystem
{
    public enum SimulationType
    {
        Offset,
        Filter,
        FilterOffset,
        World
    }

    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<SpawningData>();
    }

    public void OnDestroy(ref SystemState state)
    {
        
    }
    
    private void ApplyOffset(EntityManager entityManager, Entity colliderEntity, uint filterID)
    {
        float offset = 20;
        var transformAspect = entityManager.GetAspect<TransformAspect>(colliderEntity);
        transformAspect.WorldPosition += new float3(offset * filterID, 0, 0);
    }

    private void ApplyFilter(EntityManager entityManager, Entity colliderEntity, uint filterID)
    {
        var collider = entityManager.GetComponentData<PhysicsCollider>(colliderEntity);
        var colliderBlobPtr = collider.Value.Value.Clone();

        colliderBlobPtr.Value.SetCollisionFilter(new CollisionFilter()
        {
            BelongsTo = 1U << (int)(filterID),
            CollidesWith = 1U << (int)(filterID),
            GroupIndex = 0,
        });
        collider.Value = colliderBlobPtr;
        entityManager.SetComponentData(colliderEntity, collider);
    }
    
    private void ApplyWorldIndex(EntityManager entityManager, Entity colliderEntity, uint filterID)
    {
        entityManager.SetSharedComponent(colliderEntity, new PhysicsWorldIndex()
        {
            Value = filterID
        });
    }
    
    private void SeperationFunction(EntityManager entityManager, Entity groupEntity, uint filterID)
    {
        SimulationType simulationType = SimulationType.FilterOffset; // Enable systems below if setting to world
        
        var collection = entityManager.GetBuffer<ColliderCollectionData>(groupEntity);
        for (int j = 0; j < collection.Length; j++)
        {
            var colliderEntity = collection[j].collider;
            switch (simulationType)
            {
                case SimulationType.Offset:
                {
                    ApplyOffset(entityManager, colliderEntity, filterID);
                    break;
                }
                case SimulationType.Filter:
                {
                    ApplyFilter(entityManager, colliderEntity, filterID);
                    break;
                }
                case SimulationType.FilterOffset:
                {
                    ApplyOffset(entityManager, colliderEntity, filterID);
                    ApplyFilter(entityManager, colliderEntity, filterID);
                    break;
                }
                case SimulationType.World:
                {
                    ApplyWorldIndex(entityManager, colliderEntity, filterID);
                    break;
                }
            }
            collection = entityManager.GetBuffer<ColliderCollectionData>(groupEntity);
        }
    }

    public void OnUpdate(ref SystemState state)
    {
        var spawningData = SystemAPI.GetSingleton<SpawningData>();

        var entityManager = state.EntityManager;
        for (uint i = 0; i < 32; i++)
        {
            var staticEntity = entityManager.Instantiate(spawningData.staticPrefab);
            SeperationFunction(entityManager, staticEntity, i);
            var dynamicEntity = entityManager.Instantiate(spawningData.dynamicPrefab);
            SeperationFunction(entityManager, dynamicEntity, i);
        }

        state.Enabled = false;
    }
}
//
// [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))] public class MultiWorldPhysicsGroup1 : CustomPhysicsSystemGroup { public MultiWorldPhysicsGroup1() : base(1, false) { } }
// [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))] public class MultiWorldPhysicsGroup2 : CustomPhysicsSystemGroup { public MultiWorldPhysicsGroup2() : base(2, false) { } }
// [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))] public class MultiWorldPhysicsGroup3 : CustomPhysicsSystemGroup { public MultiWorldPhysicsGroup3() : base(3, false) { } }
// [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))] public class MultiWorldPhysicsGroup4 : CustomPhysicsSystemGroup { public MultiWorldPhysicsGroup4() : base(4, false) { } }
// [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))] public class MultiWorldPhysicsGroup5 : CustomPhysicsSystemGroup { public MultiWorldPhysicsGroup5() : base(5, false) { } }
// [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))] public class MultiWorldPhysicsGroup6 : CustomPhysicsSystemGroup { public MultiWorldPhysicsGroup6() : base(6, false) { } }
// [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))] public class MultiWorldPhysicsGroup7 : CustomPhysicsSystemGroup { public MultiWorldPhysicsGroup7() : base(7, false) { } }
// [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))] public class MultiWorldPhysicsGroup8 : CustomPhysicsSystemGroup { public MultiWorldPhysicsGroup8() : base(8, false) { } }
// [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))] public class MultiWorldPhysicsGroup9 : CustomPhysicsSystemGroup { public MultiWorldPhysicsGroup9() : base(9, false) { } }
// [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))] public class MultiWorldPhysicsGroup10 : CustomPhysicsSystemGroup { public MultiWorldPhysicsGroup10() : base(10, false) { } }
// [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))] public class MultiWorldPhysicsGroup11 : CustomPhysicsSystemGroup { public MultiWorldPhysicsGroup11() : base(11, false) { } }
// [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))] public class MultiWorldPhysicsGroup12 : CustomPhysicsSystemGroup { public MultiWorldPhysicsGroup12() : base(12, false) { } }
// [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))] public class MultiWorldPhysicsGroup13 : CustomPhysicsSystemGroup { public MultiWorldPhysicsGroup13() : base(13, false) { } }
// [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))] public class MultiWorldPhysicsGroup14 : CustomPhysicsSystemGroup { public MultiWorldPhysicsGroup14() : base(14, false) { } }
// [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))] public class MultiWorldPhysicsGroup15 : CustomPhysicsSystemGroup { public MultiWorldPhysicsGroup15() : base(15, false) { } }
// [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))] public class MultiWorldPhysicsGroup16 : CustomPhysicsSystemGroup { public MultiWorldPhysicsGroup16() : base(16, false) { } }
// [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))] public class MultiWorldPhysicsGroup17 : CustomPhysicsSystemGroup { public MultiWorldPhysicsGroup17() : base(17, false) { } }
// [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))] public class MultiWorldPhysicsGroup18 : CustomPhysicsSystemGroup { public MultiWorldPhysicsGroup18() : base(18, false) { } }
// [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))] public class MultiWorldPhysicsGroup19 : CustomPhysicsSystemGroup { public MultiWorldPhysicsGroup19() : base(19, false) { } }
// [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))] public class MultiWorldPhysicsGroup20 : CustomPhysicsSystemGroup { public MultiWorldPhysicsGroup20() : base(20, false) { } }
// [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))] public class MultiWorldPhysicsGroup21 : CustomPhysicsSystemGroup { public MultiWorldPhysicsGroup21() : base(21, false) { } }
// [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))] public class MultiWorldPhysicsGroup22 : CustomPhysicsSystemGroup { public MultiWorldPhysicsGroup22() : base(22, false) { } }
// [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))] public class MultiWorldPhysicsGroup23 : CustomPhysicsSystemGroup { public MultiWorldPhysicsGroup23() : base(23, false) { } }
// [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))] public class MultiWorldPhysicsGroup24 : CustomPhysicsSystemGroup { public MultiWorldPhysicsGroup24() : base(24, false) { } }
// [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))] public class MultiWorldPhysicsGroup25 : CustomPhysicsSystemGroup { public MultiWorldPhysicsGroup25() : base(25, false) { } }
// [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))] public class MultiWorldPhysicsGroup26 : CustomPhysicsSystemGroup { public MultiWorldPhysicsGroup26() : base(26, false) { } }
// [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))] public class MultiWorldPhysicsGroup27 : CustomPhysicsSystemGroup { public MultiWorldPhysicsGroup27() : base(27, false) { } }
// [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))] public class MultiWorldPhysicsGroup28 : CustomPhysicsSystemGroup { public MultiWorldPhysicsGroup28() : base(28, false) { } }
// [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))] public class MultiWorldPhysicsGroup29 : CustomPhysicsSystemGroup { public MultiWorldPhysicsGroup29() : base(29, false) { } }
// [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))] public class MultiWorldPhysicsGroup30 : CustomPhysicsSystemGroup { public MultiWorldPhysicsGroup30() : base(30, false) { } }
// [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))] public class MultiWorldPhysicsGroup31 : CustomPhysicsSystemGroup { public MultiWorldPhysicsGroup31() : base(31, false) { } }
// [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))] public class MultiWorldPhysicsGroup32 : CustomPhysicsSystemGroup { public MultiWorldPhysicsGroup32() : base(32, false) { } }
