using UnityEngine;
using UnityEngine.UI;

namespace EnterpriceLogic.Extension
{
    public static class UiCreatorExtension 
    {
        public static GameObject CreateChildImageComponent(this Transform parentTransform, string nameChild)
        {
            GameObject child = new GameObject(nameChild);
            Image image = child.AddComponent<Image>();
            child.transform.SetParent(parentTransform, false);
        
            RectTransform rectTransform = child.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(175, 175);
        
            rectTransform.anchoredPosition = Vector2.zero;
            return child;
        }
    }
}
