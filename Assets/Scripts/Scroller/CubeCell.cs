using FancyScrollView;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CubeCell : FancyCell<ItemData>, IPointerDownHandler
{
    [SerializeField] private Image _image;
    [SerializeField] private Animator _animator;
    [SerializeField] private TextMeshProUGUI _message;
    [SerializeField] private CustomButton _button;
    
    private ItemData _itemData;
    private 
    
    static class AnimatorHash
    {
        public static readonly int Scroll = Animator.StringToHash("scroll");
    }
    
    public override void UpdateContent(ItemData itemData)
    {
        _itemData = itemData;
        _message.text = itemData.Message;
        _image.sprite = itemData.CubeData.CubeSprite;

        _button.OnPointerAction += ()=> _itemData.DragDropManager.StartDrag(_itemData, Input.mousePosition);
        _button.OnPointerAction += _itemData.AdditionalAction;
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
        _itemData.DragDropManager.StartDrag(_itemData, Input.mousePosition);
        _itemData.AdditionalAction?.Invoke();
    }
}