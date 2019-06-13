using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace ECSRigidBodyPhysics {
    public struct RigidbodyECS : IComponentData {
        //forces components
        public float3 forceOrigin;
        public float3 forceTip;
        //mass and inertia components
        public float mass;
        public float3 inverseInertiaTensor;

        //dynamic linear and angular movement components
        public float3 velocity;
        public float3 torque;
        public float3 angularMomentum;
        public quaternion angularVelocityQuaternion;
        public float3 angularVelocity;
    }
}