using DG.Tweening;
using UnityEngine;

namespace Core
{
    public class MovingElement : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private RectTransform _startPosition;
        [SerializeField] private RectTransform _endPosition;
        [SerializeField] private UnityEngine.UI.Image _spriteImage;
    
        [Header("Animation Duration")]
        [SerializeField] private float _duration;
    
    
        private Sequence _sequence;

        public void Setup(Sprite sprite)
        {
            _spriteImage.sprite = sprite;
            _rectTransform.position = _startPosition.position;
            PlayAnimation();
        
        }
    
        private void PlayAnimation()
        {
            _sequence?.Kill();
            _sequence = DOTween.Sequence();
            _sequence.Append(_rectTransform.DOMove(_endPosition.position, _duration));
        }
    
        private void OnDestroy()
        {
            _sequence?.Kill();
        }
    }
}
