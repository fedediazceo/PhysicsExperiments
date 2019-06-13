using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddAndShowVelocity : MonoBehaviour
{
    public Vector3 velocity;
    public Text label;
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Rigidbody>().AddForce(velocity,ForceMode.Impulse);

    }

    public void Update() {
        label.text = this.GetComponent<Rigidbody>().velocity.ToString();
    }

}
