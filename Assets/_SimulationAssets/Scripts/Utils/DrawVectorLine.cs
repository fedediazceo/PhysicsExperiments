using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DrawVectorLine : MonoBehaviour
{
    public LineRenderer vector;
    public bool worldCoordinates = false;
    public void Update() {
        if (vector != null) {
            vector.SetPosition(1, worldCoordinates? this.transform.position : this.transform.localPosition);
            if (worldCoordinates) {
                vector.SetPosition(0, worldCoordinates ? vector.gameObject.transform.position : vector.gameObject.transform.localPosition);
            }
        }    
    }
}
