using System;
using Infrastructure.DI.Tickable;
using UnityEngine;
using ITickable = Zenject.ITickable;

namespace Common
{
    public class InputHandler : ITickable
    {
        public event Action OnMouseUp;

        public void Tick()
        {
            if (Input.GetMouseButtonUp(0))
                OnMouseUp?.Invoke();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}