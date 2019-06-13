using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionsPhysicsEngine : MonoBehaviour {
    [SerializeField]
    private MovingSphere sphereOne;
    [SerializeField]
    private MovingSphere sphereTwo;
    [SerializeField]
    private MovingSphere collisionSphereOne;
    [SerializeField]
    private MovingSphere collisionSphereTwo;
    [SerializeField]
    private GameObject collisionPlane;

    [Range(0.0f, 1.0f)][SerializeField]
    private float cOR = 1.0f;

    private string collisionMessage;

    public MovingSphere SphereOne { get => sphereOne; set => sphereOne = value; }
    public MovingSphere SphereTwo { get => sphereTwo; set => sphereTwo = value; }
    public MovingSphere CollisionSphereOne { get => collisionSphereOne; set => collisionSphereOne = value; }
    public MovingSphere CollisionSphereTwo { get => collisionSphereTwo; set => collisionSphereTwo = value; }
    public GameObject CollisionPlane { get => collisionPlane; set => collisionPlane = value; }
    public string CollisionMessage { get => collisionMessage; set => collisionMessage = value; }
    public float COR { get => cOR; set => cOR = value; }

    public struct CollisionData {
        public float Time { get; set; }
        public Vector3 PositionAf { get; set; }
        public Vector3 PositionBf { get; set; }
        public Vector3 ContactPointNormal { get; set; }
        public Vector3 ContactPoint { get; set; }
        public Vector3 FinalVelocityA { get; set; }
        public Vector3 FinalVelocityB { get => FinalVelocityB1; set => FinalVelocityB1 = value; }
        public Vector3 FinalVelocityB1 { get; set; }

    }

    // Update is called once per frame
    void Update() {
        CollisionData collisionData = new CollisionData {
            Time = 0.0f,
            PositionAf = Vector3.zero,
            PositionBf = Vector3.zero,
            ContactPointNormal = Vector3.zero,
            ContactPoint = Vector3.zero,
            FinalVelocityA = Vector3.zero,
            FinalVelocityB = Vector3.zero
        };

        bool collisionDetected = ComputeCollision(SphereOne.transform.position, SphereTwo.transform.position,
            SphereOne.VelocityPointer.transform.localPosition, SphereTwo.VelocityPointer.transform.localPosition,
            SphereOne.Radius, SphereTwo.Radius,
            SphereOne.Mass, SphereTwo.Mass,
            ref collisionData);
        if (collisionDetected) {
            CollisionDetected(collisionData);
        } else {
            CollisionSphereOne.gameObject.SetActive(false);
            CollisionSphereTwo.gameObject.SetActive(false);
            CollisionPlane.SetActive(false);
            CollisionMessage = "No Collision Detected";
        }

    }

    private bool ComputeCollision(Vector3 positionA, Vector3 positionB,
        Vector3 velocityA, Vector3 velocityB,
        float radiusA, float radiusB,
        float massA, float massB,
        ref CollisionData collisionData) {


        Vector3 collisionDirection = positionA - positionB;
        Vector3 VelocitySub = velocityA - velocityB;
        float radiusSum = radiusA + radiusB;
        float DistanceVelocityDot = Vector3.Dot(collisionDirection, VelocitySub);
        float DistanceDistanceDot = Vector3.Dot(collisionDirection, collisionDirection);
        float VelocityVelocityDot = Vector3.Dot(VelocitySub, VelocitySub);

        //////////////////
        //Reference Math:
        //https://physics.stackexchange.com/questions/107648/what-are-the-general-solutions-to-a-hard-sphere-collision
        /////////////////

        float rootFactor = Mathf.Pow(DistanceVelocityDot, 2) - VelocityVelocityDot * (DistanceDistanceDot - Mathf.Pow(radiusSum, 2));
        collisionData.Time = 0.0f;
        if (rootFactor > 0) {
            collisionData.Time = -(DistanceVelocityDot + Mathf.Sqrt(rootFactor)) / VelocityVelocityDot;
            if (collisionData.Time > 0) {
                collisionData.PositionAf = positionA + velocityA * collisionData.Time;
                collisionData.PositionBf = positionB + velocityB * collisionData.Time;

                collisionData.ContactPointNormal = (collisionData.PositionBf - collisionData.PositionAf).normalized;
                collisionData.ContactPoint = collisionData.PositionAf + collisionData.ContactPointNormal * SphereOne.Radius;
                ////////
                //Reference Math: 
                //https://www.euclideanspace.com/physics/dynamics/collision/threed/index.htm
                ////////
                float Impulse = -(1 + COR) * Vector3.Dot(VelocitySub, collisionData.ContactPointNormal) / ((1 / massA) + (1 / massB));

                //The amount of impulse on the normal of the collision point will give the final velocity direction
                collisionData.FinalVelocityA = velocityA + collisionData.ContactPointNormal * (Impulse / massA);
                //opposite direction
                collisionData.FinalVelocityB = velocityB - collisionData.ContactPointNormal * (Impulse / massB);

                return true;

            } else {
                return false;
            }
        } else {
            return false;

        }
    }

    private void CollisionDetected(CollisionData collisionData) {
        CollisionSphereOne.transform.position = collisionData.PositionAf;
        CollisionSphereTwo.transform.position = collisionData.PositionBf;
        CollisionSphereOne.VelocityPointer.transform.localPosition = collisionData.FinalVelocityA;
        CollisionSphereTwo.VelocityPointer.transform.localPosition = collisionData.FinalVelocityB;

        CollisionPlane.transform.position = collisionData.ContactPoint;
        CollisionPlane.transform.rotation = Quaternion.FromToRotation(Vector3.up, collisionData.ContactPointNormal);

        CollisionSphereOne.gameObject.SetActive(true);
        CollisionSphereTwo.gameObject.SetActive(true);

        CollisionPlane.SetActive(true);

        CollisionMessage = 
            string.Format(
                "Collision Detected! Sphere1.position: {0} Sphere2.position: {1} "+
                "\nCollision Detected! Sphere1.finalVelocity: {2} Sphere2.finalVelocity: {3} "+
                "\nCollision Detected! Contact Point {4} "+
                "\nCollision Detected! Normal {5} "+
                "\nCollision Detected! Collision will happen at t: {6} ", 
                collisionData.PositionAf, collisionData.PositionBf, 
                collisionData.FinalVelocityA, collisionData.FinalVelocityB, 
                collisionData.ContactPoint, collisionData.ContactPointNormal, 
                collisionData.Time);

    }

}
