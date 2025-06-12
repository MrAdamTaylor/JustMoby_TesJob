using System.Collections.Generic;

namespace DragAndDrop
{
    public class OnDroppedHandler
    {
        private List<IOnDropped> OnDropped = new();

        public void Add(IOnDropped onDropped)
        {
            OnDropped.Add(onDropped);
        }

        public void Subscribe(DragDropElementView elementView)
        {
            for (int i = 0; i < OnDropped.Count; i++)
            {
                elementView.OnEndDragEvent += OnDropped[i].OnDropped;
            }
        }

        public void Unsubscribe(DragDropElementView elementView)
        {
            for (int i = 0; i < OnDropped.Count; i++)
            {
                elementView.OnEndDragEvent -= OnDropped[i].OnDropped;
            }
        }
    }
}