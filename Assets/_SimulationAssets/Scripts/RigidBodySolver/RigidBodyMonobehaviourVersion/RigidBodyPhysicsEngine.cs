using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RigidBodyPhysicsEngine : MonoBehaviour {
    public Transform forceOrigin;
    public Transform forcePointer;

    public Vector3 forceVector;

    public Vector3 velocity = Vector3.zero;
    public Vector3 torque = Vector3.zero;
    public Vector3 angularMomentum = Vector3.zero;
    public float mass;
    public Vector3 inverseInertiaTensor = Vector3.zero;
    public Quaternion angularVelocityQuaternion;
    public Vector3 angularVelocity;
    // Update is called once per frame
    public void Start() {
        inverseInertiaTensor = new Vector3(
            1.0f / (1.0f / 12.0f * mass * (this.transform.localScale.y * this.transform.localScale.y + this.transform.localScale.z * this.transform.localScale.z)),
            1.0f / (1.0f / 12.0f * mass * (this.transform.localScale.x * this.transform.localScale.x + this.transform.localScale.z * this.transform.localScale.z)),
            1.0f / (1.0f / 12.0f * mass * (this.transform.localScale.x * this.transform.localScale.x + this.transform.localScale.y * this.transform.localScale.y))
            );
    }

    void FixedUpdate() {
        forceVector = forcePointer.transform.position - forceOrigin.transform.position;
        if (Input.GetKey(KeyCode.Space)) {
            Vector3 acceleration = forceVector / mass;
            velocity += acceleration * Time.fixedDeltaTime;
            torque = Vector3.Cross(forceOrigin.transform.position - this.transform.position, forceVector);
            angularMomentum += torque * Time.fixedDeltaTime;

            angularVelocityQuaternion = new Quaternion(
                angularMomentum.x * inverseInertiaTensor.x,
                angularMomentum.y * inverseInertiaTensor.y,
                angularMomentum.z * inverseInertiaTensor.z,
                0.0f
                );
        }
        Quaternion rotation = angularVelocityQuaternion * this.transform.rotation;
        //now multiply by 0.5f 
        rotation = new Quaternion(0.5f * rotation.x, 0.5f * rotation.y, 0.5f * rotation.z, 0.5f * rotation.w);
        //create a normalized 4 dimensional vector that will become the rotation quaternion
        Vector4 normalizedRotationResult = new Vector4(
            this.transform.rotation.x + rotation.x * Time.fixedDeltaTime,
            this.transform.rotation.y + rotation.y * Time.fixedDeltaTime,
            this.transform.rotation.z + rotation.z * Time.fixedDeltaTime,
            this.transform.rotation.w + rotation.w * Time.fixedDeltaTime
            ).normalized;

        this.transform.rotation = new Quaternion(
            normalizedRotationResult.x,
            normalizedRotationResult.y,
            normalizedRotationResult.z,
            normalizedRotationResult.w
            );

        this.transform.position += velocity * Time.fixedDeltaTime;
    }

}
