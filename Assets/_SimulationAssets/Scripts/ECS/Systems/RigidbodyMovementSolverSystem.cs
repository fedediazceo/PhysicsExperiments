using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace ECSRigidBodyPhysics {
    public class RigidbodyMovementSolverSystem : JobComponentSystem {

        EntityCommandBufferSystem m_EntityCommandBufferSystem;

        protected override void OnCreateManager() {
            m_EntityCommandBufferSystem = World.GetOrCreateSystem<EntityCommandBufferSystem>();

        }

        //[BurstCompile]
        struct PlanetMovementSystemJob : IJobForEachWithEntity<Translation, Rotation, RigidbodyECS> {
            public float deltaTime;
            public bool activateThrust;
            public void Execute(Entity entity, int index, [ReadOnly]ref Translation currentPosition, [ReadOnly]ref Rotation currentRotation, ref RigidbodyECS rigidbodyECS) {
                //update the position of the force on the rigid body with the corresponding rotation
                float3 forceTip = RotateFloat3Towards(rigidbodyECS.forceTip, currentRotation.Value) + currentPosition.Value;
                float3 forceOrigin = RotateFloat3Towards(rigidbodyECS.forceOrigin, currentRotation.Value) + currentPosition.Value;

                float3 forceVector = forceTip - forceOrigin;

                //my very SIMPLE and custom version of a "reactive" system
                if (activateThrust) {
                    float3 acceleration = forceVector / rigidbodyECS.mass;
                    rigidbodyECS.velocity += acceleration * deltaTime;
                    rigidbodyECS.torque = math.cross(forceOrigin - currentPosition.Value, forceVector);
                    rigidbodyECS.angularMomentum += rigidbodyECS.torque * deltaTime;

                    rigidbodyECS.angularVelocity = new Vector3(
                        rigidbodyECS.angularMomentum.x * rigidbodyECS.inverseInertiaTensor.x,
                        rigidbodyECS.angularMomentum.y * rigidbodyECS.inverseInertiaTensor.y,
                        rigidbodyECS.angularMomentum.z * rigidbodyECS.inverseInertiaTensor.z
                        );

                    rigidbodyECS.angularVelocityQuaternion = new Quaternion(
                        rigidbodyECS.angularMomentum.x * rigidbodyECS.inverseInertiaTensor.x,
                        rigidbodyECS.angularMomentum.y * rigidbodyECS.inverseInertiaTensor.y,
                        rigidbodyECS.angularMomentum.z * rigidbodyECS.inverseInertiaTensor.z,
                        0.0f
                        );
                }
            }

            private float3 RotateFloat3Towards(float3 vector, quaternion rotation) {
                //using hamilton product to obtain rotation of the quaternion
                quaternion rotationConjugate = new quaternion(
                            -rotation.value.x,
                            -rotation.value.y,
                            -rotation.value.z,
                            rotation.value.w);
                quaternion vectorQuaternion = new quaternion(vector.x, vector.y, vector.z, 0.0f);
                quaternion tempResult = math.mul(math.mul(rotation,vectorQuaternion),rotationConjugate);
                return new float3(tempResult.value.x, tempResult.value.y, tempResult.value.z);
            }
        }
        protected override JobHandle OnUpdate(JobHandle inputDependencies) {

            var job = new PlanetMovementSystemJob { activateThrust = RigidbodyData.Instance.engageThrust, deltaTime = Time.fixedDeltaTime };
            //a bit of cheating, I know there is only one sun for now

            return job.Schedule(this, inputDependencies);
        }


    }
}