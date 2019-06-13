using Unity.Entities;
using Unity.Mathematics;
namespace ECSRigidBodyPhysics {
    public struct BodySpawner : IComponentData {
        public Entity spaceshipBoxEntity;
    }
}