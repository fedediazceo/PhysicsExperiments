using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectsCenterOfMass : MonoBehaviour
{
    public MeshCenterOfMass[] centerOfMassObjects;
    public Text COMLabel;

    // Update is called once per frame
    void Update()
    {
        string COMMessage = "COM of: \n";
        foreach(MeshCenterOfMass COM in centerOfMassObjects) {
            COMMessage += COM.gameObject.name +" "+COM.CenterOfMass+"\n";
        }
        COMLabel.text = COMMessage;
    }
}
