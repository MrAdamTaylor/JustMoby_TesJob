using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.DI.Model;
using Infrastructure.DI.Tickable;

namespace Infrastructure.DI.Container
{
    public class GameStateLifetimeManager
    {
        private readonly Dictionary<DisposeEvent, List<object>> _stateInstances = new();
        private TickProvider _tickProvider;

        public GameStateLifetimeManager(TickProvider tickProvider)
        {
            _tickProvider = tickProvider;
        }
        
        public void Track(DisposeEvent state, object instance)
        {
            if (instance == null)
                return;

            if (!_stateInstances.TryGetValue(state, out var list))
            {
                list = new List<object>();
                _stateInstances[state] = list;
            }

            if (instance is not ITickMain)
                list.Insert(0, instance);
            else
                list.Add(instance);
        }

        public void Clear(DisposeEvent state)
        {
            if (!_stateInstances.TryGetValue(state, out var list)) 
                return;
            
            List<ITickMain> tickablesToRemove = new();
            
            for (int i = 0; i < list.Count; i++)
            {
                var instance = list[i];
                if (instance is not IDisposable disposable) 
                    continue;
                
                if (instance is ITickMain tick)
                    tickablesToRemove.Add(tick);
                
                disposable.Dispose();
            }
            
            _tickProvider.RemoveAllTicksBy(tickablesToRemove);
            
            list.Clear();
        }

        public bool IsGetData(Type type)
        {
            var states = _stateInstances.Keys.ToList();
            for (int i = 0; i < states.Count; i++)
            {
                var state = states[i];
                var list = _stateInstances[state];

                for (int j = 0; j < list.Count; j++)
                {
                    if (list[j] != null && type.IsInstanceOfType(list[j]))
                        return true;
                }
            }

            return false;
        }

        public object ReturnService(Type type)
        {
            var states = _stateInstances.Keys.ToList();
            for (int i = 0; i < states.Count; i++)
            {
                var state = states[i];
                var list = _stateInstances[state];

                for (int j = 0; j < list.Count; j++)
                {
                    if (list[j] != null && type.IsInstanceOfType(list[j]))
                        return list[j];
                }
            }

            return null;
        }

        private void CheckOnDispose(object instance)
        {
            if (instance is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }
}