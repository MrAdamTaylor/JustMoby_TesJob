using FancyScrollView;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CubeCell : FancyCell<ItemData>
{
    [SerializeField] private Image _image;
    [SerializeField] private Animator _animator;
    [SerializeField] private TextMeshProUGUI _message;
    
    static class AnimatorHash
    {
        public static readonly int Scroll = Animator.StringToHash("scroll");
    }
    
    public override void UpdateContent(ItemData itemData)
    {
        _message.text = itemData.Message;
        _image.sprite = itemData.CubeData.CubeSprite;
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
}
