using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Unity.Rendering;

namespace ECSRigidBodyPhysics {
    [UpdateAfter(typeof(RigidbodyMovementSolverSystem))]
    public class RigidbodyTransformUpdateSystem : JobComponentSystem {

        EntityCommandBufferSystem m_EntityCommandBufferSystem;

        protected override void OnCreateManager() {
            m_EntityCommandBufferSystem = World.GetOrCreateSystem<EntityCommandBufferSystem>();

        }

        //[BurstCompile]
        struct RigidbodyTransformUpdateSystemJob : IJobForEachWithEntity<Translation, Rotation, RigidbodyECS, LocalToWorld> {
            public float deltaTime;
            public bool activateThrust;
            public void Execute(Entity entity, int index,ref Translation currentPosition, ref Rotation currentRotation, ref RigidbodyECS rigidbodyECS, ref LocalToWorld location) {
                float3 positionResult = currentPosition.Value + rigidbodyECS.velocity * deltaTime;

                // nice litte optimization for quaternion rotation
                // from here: https://stackoverflow.com/questions/24197182/efficient-quaternion-angular-velocity
                //
                quaternion rotationResult;
                float3 halfW = rigidbodyECS.angularVelocity * deltaTime * 0.5f; // vector of half angle
                float l = math.length(halfW); // magnitude
                if (l > 0) {
                    float ss = math.sin(l) / l;
                    rotationResult = new quaternion(halfW.x * ss, halfW.y * ss, halfW.z * ss, math.cos(l));
                } else {
                    rotationResult = new quaternion(halfW.x, halfW.y, halfW.z, 1.0f);
                }

                currentPosition.Value = positionResult;
                currentRotation.Value = math.mul(currentRotation.Value, rotationResult);

            }
        }
        protected override JobHandle OnUpdate(JobHandle inputDependencies) {

            var job = new RigidbodyTransformUpdateSystemJob { deltaTime = Time.fixedDeltaTime };

            return job.Schedule(this, inputDependencies);
        }
    }
}