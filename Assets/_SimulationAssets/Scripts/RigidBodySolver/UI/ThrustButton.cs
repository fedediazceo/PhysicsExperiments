
using UnityEngine;
using UnityEngine.EventSystems;

public class ThrustButton : MonoBehaviour, IPointerDownHandler, IPointerExitHandler
{
    public void OnPointerDown(PointerEventData eventData) {
        RigidbodyData.Instance.engageThrust = true;
    }

    public void OnPointerExit(PointerEventData eventData) {
        RigidbodyData.Instance.engageThrust = false;
    }
}
