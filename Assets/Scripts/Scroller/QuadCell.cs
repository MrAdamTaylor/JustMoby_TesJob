using FancyScrollView;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Scroller
{
    public class QuadCell : FancyCell<QuadItem>, IPointerDownHandler
    {
        [SerializeField] private Image _image;
        [SerializeField] private Animator _animator;
        [SerializeField] private CustomButton _button;
    
        private QuadItem _quadItem;
    
        static class AnimatorHash
        {
            public static readonly int Scroll = Animator.StringToHash("scroll");
        }
    
        public override void UpdateContent(QuadItem quadItem)
        {
            _quadItem = quadItem;
            _image.sprite = quadItem.QuadData.CubeSprite;

            _button.OnPointerDownAction += ()=> _quadItem.DragDropManager.StartDrag(_quadItem, Input.mousePosition);
            _button.OnPointerDownAction += _quadItem.AdditionalAction;
        }

        public override void UpdatePosition(float position)
        {
            currentPosition = position;

            if (_animator.isActiveAndEnabled)
            {
                _animator.Play(AnimatorHash.Scroll, -1, position);
            }
        
            _animator.speed = 0;
        }
    
        float currentPosition = 0;

        void OnEnable() => UpdatePosition(currentPosition);

        private void OnDestroy()
        {
            _button.onClick.RemoveAllListeners();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _quadItem.DragDropManager.StartDrag(_quadItem, Input.mousePosition);
            _quadItem.AdditionalAction?.Invoke();
        }
    }
}