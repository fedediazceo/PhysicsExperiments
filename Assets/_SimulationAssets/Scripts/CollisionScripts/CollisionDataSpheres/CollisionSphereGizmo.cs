using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MovingSphere))]
public class CollisionSphereGizmo : MonoBehaviour {
    [SerializeField]
    private MovingSphere sphere;

    private MovingSphere gizmoSphere;

    public MovingSphere Sphere { get => sphere; set => sphere = value; }

    public void Start() {
        gizmoSphere = this.GetComponent<MovingSphere>();
    }
    void Update() {
        gizmoSphere.Radius = Sphere.Radius;
    }
}
