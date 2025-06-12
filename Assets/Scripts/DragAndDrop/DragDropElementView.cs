using Core;
using DG.Tweening;
using Extension;
using Scroller;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DragAndDrop
{
    public class DragDropElementView : MonoBehaviour, IHideHandler, IFlagSetter
    {
        [SerializeField] private Image _image;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private CanvasGroup _canvasGroup;

        [Header("Animation")] [Space]
        [SerializeField] private Vector3 _scaleSize;
        [Range(0.1f, 2f)] [SerializeField] private float _animationSpeed;
        public event System.Action<PointerEventData> OnEndDragEvent;

        public Sprite DragSprite { get; private set; }

        public QuadItem QuadItem { get; private set; }
        
        private Sequence _sequenceChangeSize, _sequenceFadeImage;

        private Vector2 _offset;

        private bool _isOff;

        public void Initialize(QuadItem quadItem, Vector2 screenPosition)
        {
            QuadItem = quadItem;
            if (quadItem.QuadData.CubeSprite != null)
            {
                _image.sprite = quadItem.QuadData.CubeSprite;
                DragSprite = quadItem.QuadData.CubeSprite;
            }
            _rectTransform.position = screenPosition;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _image.color = new Color(1,1,1,1);
        
            if(!gameObject.activeSelf)
                gameObject.SetActive(true);
        
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _rectTransform.parent as RectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out _offset
            );
            _offset = _rectTransform.localPosition - new Vector3(_offset.x, _offset.y, 0);
            _canvasGroup.blocksRaycasts = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    _rectTransform.parent as RectTransform,
                    eventData.position,
                    eventData.pressEventCamera,
                    out Vector2 localPointerPos
                ))
            {
                _rectTransform.localPosition = localPointerPos + _offset;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _canvasGroup.blocksRaycasts = true;
            OnEndDragEvent?.Invoke(eventData);
        }

        public void HideHandler()
        {
            Observable.NextFrame()
                .Subscribe(_ => 
                {
                    if (_isOff)
                    {
                        Debug.Log("<color=red>Hide Without Animation</color>");
                        FadeSpite();
                        _isOff = false;
                    }
                    else
                    {
                        Debug.Log("<color=yellow>PlayAnimation</color>");
                        CallAnimation();
                        FadeSpite();
                    }
                })
                .AddTo(this);
        }

        public void SetFlag(bool flag) => _isOff = flag;

        private void CallAnimation()
        {
            GetImageWithComponents(out var cloneRect, out var cloneImage);

            _sequenceFadeImage?.Kill();
            _sequenceChangeSize?.Kill();
    
            _sequenceFadeImage = DOTween.Sequence()
                .Append(cloneImage.DOFade(0f, _animationSpeed));

            _sequenceChangeSize = DOTween.Sequence()
                .Append(cloneRect.DOScale(_scaleSize, _animationSpeed))
                .Insert(0f, _sequenceFadeImage);
        
       
        }

        private void GetImageWithComponents(out RectTransform cloneRect, out Image cloneImage)
        {
            GameObject dragClone = transform.CreateChildImageComponent("DragClone");
            cloneImage = dragClone.GetComponent<Image>();
            cloneRect = dragClone.GetComponent<RectTransform>();
        
            cloneImage.sprite = _image.sprite;
            cloneImage.color = _image.color;
            cloneRect.localScale = _rectTransform.localScale;
            cloneRect.sizeDelta = _rectTransform.sizeDelta;
        
            dragClone.AddComponent<SelfDestroyer>().Init(_animationSpeed * 1.5f);
        }

        private void FadeSpite()
        {
            _image.color = new Color(1,1,1,0);
        }

        private void OnDisable()
        {
            this.DOKill();
            _sequenceChangeSize?.Kill();
            _sequenceFadeImage?.Kill();
        }
    }
}