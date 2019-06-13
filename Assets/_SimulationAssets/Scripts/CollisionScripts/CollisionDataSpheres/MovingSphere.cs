using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSphere : MonoBehaviour {
    [SerializeField]
    private GameObject velocityPointer;
    [SerializeField]
    private float mass = 1.0f;
    [SerializeField]
    private float radius = 0.5f;
    public GameObject VelocityPointer { get => velocityPointer; set => velocityPointer = value; }
    public float Radius { get => radius; set => radius = value; }
    public float Mass { get => mass; set => mass = value; }

    public void Update() {
        //I can use any scale xyz coordinate, as I am assuming a uniform scale on the sphere.
        this.transform.localScale = new Vector3(Radius*2.0f, Radius*2.0f, Radius*2.0f);
    }

    public void OnValidate() {
        Radius = Radius <= 0.0f ? 0.1f: Radius;

    }
}
