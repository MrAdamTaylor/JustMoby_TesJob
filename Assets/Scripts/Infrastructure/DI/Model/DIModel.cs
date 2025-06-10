using System;
using Infrastructure.DI.Container;
using Infrastructure.DI.Tickable;
using UnityEngine;

namespace Infrastructure.DI.Model
{
    
        public interface IContainerBuilder
        {
            public void Register(ServiceDescriptor descriptor);

            public IContainer Build();
        }
        public interface IContainer
        {
            public GameStateLifetimeManager LifetimeManager { get; }
            
            public TickProvider TickProvider { get; }
            
            public IScope CreateScope();

            public void BindData(Type type, Type service, LifeTime lifeTime);
            
            void Construct(object behaviour);
            void CacheType(Type type, object instance);
            void CacheMono<T>(Type type, T instance) where T : MonoBehaviour;
            
            void CacheScriptableObject<T>(Type type, T instance) where T : ScriptableObject;
            void RegisterTransientWithTimeState(Type type, Type type1, DisposeEvent disposeEvent);
            void CacheComponent<T>(Type type, T instance) where T : Component;
        }
    
        public interface IScope 
        {
            public object Resolve(Type service);
        }
    
}
