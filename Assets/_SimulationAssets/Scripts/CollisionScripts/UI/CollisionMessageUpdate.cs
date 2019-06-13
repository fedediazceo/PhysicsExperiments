using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class CollisionMessageUpdate : MonoBehaviour
{
    [SerializeField]
    private CollisionsPhysicsEngine collisionsEngine = null;
    private Text messageText;

    public void Start() {
        messageText = this.GetComponent<Text>();
    }

    void Update()
    {
        if (collisionsEngine) {
            messageText.text = collisionsEngine.CollisionMessage;
        }    
    }
}
