using UnityEngine;

namespace DragAndDrop
{
    public struct DragInformation
    {
        public Vector2 LocalPoint;
        public RectTransform DropArea;
        public Sprite Sprite;
        public float Height;


        public DragInformation(Vector2 localPoint, RectTransform dropArea, Sprite sprite, float height)
        {
            Height = height * 0.9f;
            Sprite = sprite;
            DropArea = dropArea;
            LocalPoint = localPoint;
        }
    }
}