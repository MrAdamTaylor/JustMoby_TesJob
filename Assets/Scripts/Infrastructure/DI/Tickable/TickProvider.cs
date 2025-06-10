using System.Collections.Generic;

namespace Infrastructure.DI.Tickable
{
    public class TickProvider
    {
        private readonly List<ITickable> _tickables = new();
        private readonly List<ILateTickable> _lateTickables = new();
        private readonly List<IFixedTickable> _fixedTickables = new();

        public void Register(object obj)
        {
            if (obj is ITickable tickable)
                _tickables.Add(tickable);

            if (obj is ILateTickable lateTickable)
                _lateTickables.Add(lateTickable);

            if (obj is IFixedTickable fixedTickable)
                _fixedTickables.Add(fixedTickable);
        }

        public void Tick()
        {
            for (int i = 0; i < _tickables.Count; i++)
            {
                var tickService = _tickables[i];
                tickService.Tick();
            }
        }

        public void LateTick()
        {
            for (int i = 0; i < _lateTickables.Count; i++)
            {
                var lateTickable = _lateTickables[i];
                lateTickable.LateTick();
            }
        }

        public void FixedTick()
        {
            for (int i = 0; i < _fixedTickables.Count; i++)
            {
                var fixedTickable = _fixedTickables[i];
                fixedTickable.FixedTick();
            }
        }

        public void RemoveAllTicksBy(IEnumerable<ITickMain> items)
        {
            foreach (var item in items)
            {
                if (item is ITickable tickable)
                    _tickables.Remove(tickable);

                if (item is ILateTickable lateTickable)
                    _lateTickables.Remove(lateTickable);

                if (item is IFixedTickable fixedTickable)
                    _fixedTickables.Remove(fixedTickable);
            }
        }
        
        public void ClearAll()
        {
            _tickables.Clear();
            _lateTickables.Clear();
            _fixedTickables.Clear();
        }

    }
}