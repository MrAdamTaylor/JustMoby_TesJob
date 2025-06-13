using Common;
using Scroller;
using UnityEngine;

namespace DragAndDrop
{
    public struct DragInformation
    {
        public Vector2 LocalPoint;
        public RectTransform DropArea;
        public readonly Sprite Sprite;
        public readonly float Height;
        public readonly IFlagSetter FlagSetter;
        public readonly string ColorName;


        public DragInformation(Vector2 localPoint, RectTransform dropArea, QuadItem quadItem, float height, IFlagSetter flagSetter)
        {
            FlagSetter = flagSetter;
            Height = height;
            Sprite = quadItem.QuadData.CubeSprite;
            DropArea = dropArea;
            LocalPoint = localPoint;
            ColorName = quadItem.QuadData.ColorName;
        }
    }
}