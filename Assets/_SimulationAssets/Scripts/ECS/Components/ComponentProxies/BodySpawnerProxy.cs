using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;


namespace ECSRigidBodyPhysics {
    [DisallowMultipleComponent]
    [RequiresEntityConversion]
    public class BodySpawnerProxy : MonoBehaviour, IDeclareReferencedPrefabs, IConvertGameObjectToEntity {

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
            var spawnerData = new BodySpawner {
                spaceshipBoxEntity = conversionSystem.GetPrimaryEntity(RigidbodyData.Instance.rigidBodyPrefab)
            };
            dstManager.AddComponentData(entity, spawnerData);
        }

        public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs) {
            referencedPrefabs.Add(RigidbodyData.Instance.rigidBodyPrefab);
        }
    }
}