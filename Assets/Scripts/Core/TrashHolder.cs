using DragAndDrop;
using UnityEditor.VersionControl;

namespace Core
{
    public class TrashHolder
    {
        private TrashSlot _trashSlot;
        private MovingElement _movingElement;
        private MessageOutput _message;

        public TrashHolder(TrashSlot trashSlot, MovingElement movingElement, MessageOutput message)
        {
            _message = message;
            _trashSlot = trashSlot;
            _trashSlot.OnDragStart += Execute;
            _movingElement = movingElement;
        }

        private void Execute(DragInformation obj)
        {
            obj.FlagSetter.SetFlag(true);
            _movingElement.Setup(obj.Sprite);
            _message.OutputByKey(MessagesKey.TROW_TO_TRASH);
        }

        ~TrashHolder()
        {
            _trashSlot.OnDragStart -= Execute;
        }
    }
}