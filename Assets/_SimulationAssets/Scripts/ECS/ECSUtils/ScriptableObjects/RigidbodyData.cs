using Unity.Mathematics;
using UnityEngine;

// Add an entry to the Assets menu for creating an asset of this type
[CreateAssetMenu(menuName = "ECSPhysics/RigidbodyData")]
public class RigidbodyData : SingletonScriptableObject<RigidbodyData> {
    public GameObject rigidBodyPrefab;
    //forces components
    public float3 forceOrigin;
    public float3 forceTip;
    //mass and inertia components
    public float mass;
    public float3 scale;
    public bool shouldReset = false;
    public bool engageThrust = false;
}