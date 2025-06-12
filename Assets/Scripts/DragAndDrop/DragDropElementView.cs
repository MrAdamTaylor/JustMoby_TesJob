using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragDropElementView : MonoBehaviour, IHideHandler
{
    [SerializeField] private Image _image;
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private CanvasGroup _canvasGroup;
    
    public event System.Action<PointerEventData> OnEndDragEvent;

    public Sprite DragSprite { get; private set; }

    private Vector2 _offset;

    private bool _isOff;

    public void Initialize(ItemData itemData, Vector2 screenPosition)
    {
        _image.sprite = itemData.CubeData.CubeSprite;
        DragSprite = itemData.CubeData.CubeSprite;
        _rectTransform.position = screenPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
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
        if (_isOff)
        {
            //Play Animation (Maybe Coroutine)
        }
        else
        {
            //DisableGameObject
        }
    }
}
