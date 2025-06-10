using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RectTransformExtension 
{
    public static bool CheckByEdgesFromPosition(this RectTransform rectTransform, Vector2 position, float width, float height)
    {
        Rect rect = rectTransform.rect;
        
        float halfWidth = width * 0.5f;
        float halfHeight = height * 0.5f;
        
        bool isInsideHorizontally = 
            (position.x - halfWidth >= rect.xMin) && 
            (position.x + halfWidth <= rect.xMax);

        bool isInsideVertically = 
            (position.y - halfHeight >= rect.yMin) && 
            (position.y + halfHeight <= rect.yMax);

        return isInsideHorizontally && isInsideVertically;
    }
}
