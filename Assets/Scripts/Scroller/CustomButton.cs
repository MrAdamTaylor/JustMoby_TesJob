using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomButton : Button
{
    public event Action OnPointerAction;
    public override void OnPointerDown(PointerEventData eventData)
    {
        OnPointerAction?.Invoke();
    }
}