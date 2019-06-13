using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ApplyForce : MonoBehaviour
{
    public Transform forceOrigin;
    public Transform forcePointer;

    public Vector3 forceVector;
    public Text uiForceVectorLabel;

    // Update is called once per frame
    void FixedUpdate()
    {
        forceVector = forcePointer.transform.position - forceOrigin.transform.position;
        uiForceVectorLabel.text = forceVector.ToString();
        Debug.Log("inertia tensor: " + this.GetComponent<Rigidbody>().inertiaTensor);
        Debug.Log("inertia tensor rotation: " + this.GetComponent<Rigidbody>().inertiaTensorRotation);
        Debug.Log("angular velocity: " + this.GetComponent<Rigidbody>().angularVelocity);
        if (Input.GetKey(KeyCode.Space)){
            this.GetComponent<Rigidbody>().AddForceAtPosition(forceVector, forceOrigin.transform.position, ForceMode.Force);
        }
    }
}
