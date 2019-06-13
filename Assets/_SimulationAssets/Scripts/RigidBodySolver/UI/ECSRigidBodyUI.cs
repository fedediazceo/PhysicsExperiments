using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using ECSRigidBodyPhysics;
using UnityEngine.EventSystems;

public class ECSRigidBodyUI : MonoBehaviour
{
    [SerializeField]
    private InputField forceOriginX, forceOriginY, forceOriginZ;
    [SerializeField]
    private InputField forceVectorX, forceVectorY, forceVectorZ;
    [SerializeField]
    private InputField mass;
    [SerializeField]
    private InputField sizeX, sizeY, sizeZ;
    [SerializeField]
    private RigidbodyData data;

    public void Start() {
        mass.text = data.mass.ToString();
        sizeX.text = data.scale.x.ToString();
        sizeY.text = data.scale.y.ToString();
        sizeZ.text = data.scale.z.ToString();
        forceOriginX.text = data.forceOrigin.x.ToString();
        forceOriginY.text = data.forceOrigin.y.ToString();
        forceOriginZ.text = data.forceOrigin.z.ToString();
        forceVectorX.text = data.forceTip.x.ToString();
        forceVectorY.text = data.forceTip.y.ToString();
        forceVectorZ.text = data.forceTip.z.ToString();

    }

    public void RandomizeAndReset() {
        data.mass = UnityEngine.Random.Range(1.0f,20f);
        data.scale = new float3(
            UnityEngine.Random.Range(1.0f, 10f), 
            UnityEngine.Random.Range(1.0f, 10f), 
            UnityEngine.Random.Range(1.0f, 10f));
        data.forceOrigin = new float3(
            UnityEngine.Random.Range(-data.scale.x, data.scale.x), 
            UnityEngine.Random.Range(-data.scale.y, data.scale.y), 
            UnityEngine.Random.Range(-data.scale.z, data.scale.z));
        data.forceTip = new float3(
            UnityEngine.Random.Range(-10f, 10f),
            UnityEngine.Random.Range(-10f, 10f),
            UnityEngine.Random.Range(-10f, 10f));

        data.rigidBodyPrefab.gameObject.transform.localScale = data.scale;

        mass.text = data.mass.ToString();
        sizeX.text = data.scale.x.ToString();
        sizeY.text = data.scale.y.ToString();
        sizeZ.text = data.scale.z.ToString();
        forceOriginX.text = data.forceOrigin.x.ToString();
        forceOriginY.text = data.forceOrigin.y.ToString();
        forceOriginZ.text = data.forceOrigin.z.ToString();
        forceVectorX.text = data.forceTip.x.ToString();
        forceVectorY.text = data.forceTip.y.ToString();
        forceVectorZ.text = data.forceTip.z.ToString();

        data.shouldReset = true;
        data.engageThrust = false;
    }

    public void ResetSystem() {
        data.mass = float.Parse(mass.text);
        data.scale = new float3(float.Parse(sizeX.text), float.Parse(sizeY.text), float.Parse(sizeZ.text));
        data.forceOrigin = new float3(float.Parse(forceOriginX.text), float.Parse(forceOriginY.text), float.Parse(forceOriginZ.text));
        data.forceTip = new float3(float.Parse(forceVectorX.text), float.Parse(forceVectorY.text), float.Parse(forceVectorZ.text));
        data.rigidBodyPrefab.gameObject.transform.localScale = data.scale;
        data.shouldReset = true;
    }
}
