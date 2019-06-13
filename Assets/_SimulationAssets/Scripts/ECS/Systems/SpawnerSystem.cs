using Unity.Burst;
using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using Unity.Rendering;

namespace ECSRigidBodyPhysics {
    public class BodySpawnerSystem : JobComponentSystem {

        EndSimulationEntityCommandBufferSystem m_EntityCommandBufferSystem;

        protected override void OnCreateManager() {
            m_EntityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        struct SpawnJob : IJobForEachWithEntity<BodySpawner, LocalToWorld> {
            public EntityCommandBuffer.Concurrent commandBuffer;
            public bool resetBody;
            public float _mass;
            public float3 _scale;
            public float3 _forceOrigin;
            public float3 _forceTip;
            public void Execute(Entity entity, int index, [ReadOnly]ref BodySpawner bodySpawner, ref LocalToWorld location) {

                if (resetBody) {

                    var instance = commandBuffer.Instantiate(index, bodySpawner.spaceshipBoxEntity);

                    var inverseInertiaTensor = new float3(
                        1.0f / (1.0f / 12.0f * _mass * (_scale.y * _scale.y + _scale.z * _scale.z)),
                        1.0f / (1.0f / 12.0f * _mass * (_scale.x * _scale.x + _scale.z * _scale.z)),
                        1.0f / (1.0f / 12.0f * _mass * (_scale.x * _scale.x + _scale.y * _scale.y))
                    );


                    commandBuffer.SetComponent(index, instance, new Translation { Value = new float3(0.0f) });
                    commandBuffer.SetComponent(index, instance, new Rotation { Value = quaternion.identity });
                    commandBuffer.SetComponent(index, instance, new NonUniformScale { Value = new float3(_scale.x, _scale.y,_scale.z) });
                    commandBuffer.SetComponent(index, instance, new RigidbodyECS {
                        forceOrigin = _forceOrigin,
                        forceTip = _forceTip,
                        mass = _mass,
                        inverseInertiaTensor = inverseInertiaTensor,
                        velocity = float3.zero,
                        torque = float3.zero,
                        angularMomentum = float3.zero,
                        angularVelocityQuaternion = quaternion.identity,
                        angularVelocity = float3.zero
                    });
                    resetBody = false;
                }
            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps) {
            bool shouldReset = RigidbodyData.Instance.shouldReset;
            if (shouldReset) {
                RigidbodyData.Instance.shouldReset = false;
            }

            var job = new SpawnJob {
                commandBuffer = m_EntityCommandBufferSystem.CreateCommandBuffer().ToConcurrent(),
                resetBody = shouldReset,
                _mass = RigidbodyData.Instance.mass,
                _scale = RigidbodyData.Instance.scale,
                _forceOrigin = RigidbodyData.Instance.forceOrigin,
                _forceTip = RigidbodyData.Instance.forceTip
            };
            var jobHandle = job.ScheduleSingle(this, inputDeps);
            m_EntityCommandBufferSystem.AddJobHandleForProducer(jobHandle);

            return jobHandle;
        }
    }
}