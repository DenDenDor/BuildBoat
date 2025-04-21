using UnityEngine;
using UnityEngine.EventSystems;

namespace CMF
{
    public class CharacterMobileJumpButtonRectangle : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public bool IsPressed { get; private set; }
        
        public void OnPointerDown(PointerEventData eventData) => IsPressed = true;

        public void OnPointerUp(PointerEventData eventData) => IsPressed = false;
    }
}