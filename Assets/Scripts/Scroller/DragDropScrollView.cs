using System.Collections.Generic;
using FancyScrollView;
using UnityEngine;

namespace Scroller
{
    public class DragDropScrollView : FancyScrollView<ItemData>
    {
        [SerializeField] private FancyScrollView.Scroller _scroller;
        [SerializeField] private GameObject _cellPrefab;
    
        protected override GameObject CellPrefab => _cellPrefab;

        protected override void Initialize()
        {
            base.Initialize();
            _scroller.OnValueChanged(UpdatePosition);
        }

        private void StopScrolling()
        {
            _scroller.enabled = false;
        }

        public void UpdateData(IList<ItemData> items)
        {
            for (int i = 0; i < items.Count; i++)
            {
                ItemData item = items[i];
                item.AddAction(StopScrolling);
            }

            UpdateContents(items);
            _scroller.SetTotalCount(items.Count);
        }
    }
}