using System;
using DragAndDrop;
using Scroller;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Tower
{
    public class TowerQuadElement : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [Range(0f, 1f)] 
        [SerializeField] private float _transparentAfterClick = 0.5f;

        public bool IsNotTrashed { get; private set; }
        public QuadItem QuadItem => _quadItem;
    
        private QuadItem _quadItem;
        private CustomButton _customButton;
        private IDragDropManager _dragDropManager;
        private Action<GameObject> _hideAction;

        public void Init(QuadItem quadItem, CustomButton customButton, IDragDropManager dragDropManager)
        {
            _dragDropManager = dragDropManager;
            _customButton = customButton;
            _quadItem = quadItem;
            _customButton.OnPointerDownAction += SetCustomTransparent;
            _customButton.OnPointerUpAction += SetDefaultTransparent;
        }

        public void Set(Sprite sprite, string colorName, Action<GameObject> hideAction)
        {
            _hideAction = hideAction;
            _customButton.OnPointerUpAction += ExecuteHideAction;
            _quadItem.QuadData.CubeSprite = sprite;
            _quadItem.QuadData.ColorName = colorName;
        }

        private void SetCustomTransparent()
        {
            _image.color = new Color(1,1,1,_transparentAfterClick);
        }

        private void SetDefaultTransparent()
        {
            _image.color = new Color(1,1,1,1);
        }

        private void ExecuteHideAction()
        {
            _hideAction?.Invoke(gameObject);
        }

        public void OnDestroy()
        {
            // ReSharper disable once EventUnsubscriptionViaAnonymousDelegate
            _customButton.OnPointerDownAction -= ()=>_dragDropManager.StartDrag(_quadItem, Input.mousePosition);
            _customButton.OnPointerUpAction -= SetDefaultTransparent;
            _customButton.OnPointerDownAction -= SetCustomTransparent;
            _customButton.OnPointerUpAction -= ExecuteHideAction;
        }
    }
}
