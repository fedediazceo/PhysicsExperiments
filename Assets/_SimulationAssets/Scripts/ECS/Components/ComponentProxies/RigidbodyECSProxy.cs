using System;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace ECSRigidBodyPhysics {
    public class RigidbodyECSProxy : MonoBehaviour, IConvertGameObjectToEntity {
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
            var data = new RigidbodyECS { };
            dstManager.AddComponentData(entity, data);
        }
    }
}