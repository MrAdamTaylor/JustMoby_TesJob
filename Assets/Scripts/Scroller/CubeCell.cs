using FancyScrollView;
using TMPro;
using UnityEngine;

public class CubeCell : FancyCell<ItemData>
{
    
    [SerializeField] Animator _animator;
    [SerializeField] TextMeshProUGUI _message;
    
    static class AnimatorHash
    {
        public static readonly int Scroll = Animator.StringToHash("scroll");
    }
    
    public override void UpdateContent(ItemData itemData)
    {
        _message.text = itemData.Message;
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
