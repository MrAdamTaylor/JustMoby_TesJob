using System.Collections.Generic;
using FancyScrollView;
using UnityEngine;

class ScrollView : FancyScrollView<ItemData>
{
    [SerializeField] private Scroller _scroller;
    [SerializeField] private GameObject _cellPrefab;
    
    protected override GameObject CellPrefab => _cellPrefab;

    protected override void Initialize()
    {
        base.Initialize();
        _scroller.OnValueChanged(UpdatePosition);
    }

    public void UpdateData(IList<ItemData> items)
    {
        UpdateContents(items);
        _scroller.SetTotalCount(items.Count);
    }
}