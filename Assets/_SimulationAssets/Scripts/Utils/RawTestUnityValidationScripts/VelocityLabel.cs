using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VelocityLabel : MonoBehaviour {
    public Transform velocity;
    // Start is called before the first frame update

    public void Update() {
        this.GetComponent<Text>().text = velocity.localPosition.ToString();
    }

}
