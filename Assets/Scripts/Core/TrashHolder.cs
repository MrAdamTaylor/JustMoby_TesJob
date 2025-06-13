using DragAndDrop;

namespace Core
{
    public class TrashHolder
    {
        private TrashSlot _trashSlot;
        private MovingElement _movingElement;

        public TrashHolder(TrashSlot trashSlot, MovingElement movingElement)
        {
            _trashSlot = trashSlot;
            _trashSlot.OnDragStart += Execute;
            _movingElement = movingElement;
        }

        private void Execute(DragInformation obj)
        {
            obj.FlagSetter.SetFlag(true);
            _movingElement.Setup(obj.Sprite);
        }

        ~TrashHolder()
        {
            _trashSlot.OnDragStart -= Execute;
        }
    }
}