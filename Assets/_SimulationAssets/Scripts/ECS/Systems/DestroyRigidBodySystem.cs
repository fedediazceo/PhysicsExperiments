using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using static Unity.Mathematics.math;


namespace ECSRigidBodyPhysics {
    [UpdateBefore(typeof(BodySpawnerSystem))]
    public class DestroyRigidBodySystem : JobComponentSystem {

        EndSimulationEntityCommandBufferSystem m_EntityCommandBufferSystem;
        protected override void OnCreateManager() {
            m_EntityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        [BurstCompile]
        struct DestroyRigidBodySystemJob : IJobForEachWithEntity<RigidbodyECS> {
            public EntityCommandBuffer.Concurrent commandBuffer;
            public bool shouldDestroy;
            public void Execute(Entity entity, int index, [ReadOnly]ref RigidbodyECS bodySpawner) {
                if (shouldDestroy) {
                    commandBuffer.DestroyEntity(index, entity);
                }
            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDependencies) {

            var job = new DestroyRigidBodySystemJob {
                commandBuffer = m_EntityCommandBufferSystem.CreateCommandBuffer().ToConcurrent(),
                shouldDestroy = RigidbodyData.Instance.shouldReset 
                //as this runs before the spawner, is the spawner job to disable this bool
                // I don't like this, and a better responsive system should be used, but this took a bit longer than planned already
            };
            var jobHandle = job.ScheduleSingle(this, inputDependencies);
            m_EntityCommandBufferSystem.AddJobHandleForProducer(jobHandle);

            return jobHandle;
        }
    }
}