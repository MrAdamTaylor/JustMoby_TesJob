using UnityEngine;

namespace DragAndDrop
{
    public struct DragInformation
    {
        public Vector2 LocalPoint;
        public RectTransform DropArea;
        public Sprite Sprite;
        public float Height;
        public IFlagSetter FlagSetter;


        public DragInformation(Vector2 localPoint, RectTransform dropArea, Sprite sprite, float height, IFlagSetter flagSetter)
        {
            FlagSetter = flagSetter;
            Height = height;
            Sprite = sprite;
            DropArea = dropArea;
            LocalPoint = localPoint;
        }
    }
}