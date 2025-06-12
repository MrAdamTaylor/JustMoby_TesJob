using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Scroller
{
    public class CustomButton : Button
    {
        public event Action OnPointerDownAction;
        public event Action OnPointerUpAction;

        public override void OnPointerDown(PointerEventData eventData)
        {
            OnPointerDownAction?.Invoke();
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            OnPointerUpAction?.Invoke();
        }
    }
}