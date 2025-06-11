using System;
using Infrastructure.DI.Model;
using Infrastructure.DI.Tickable;
using UnityEngine;

namespace Infrastructure.DI.Container
{
    public ref struct BindingBuilder<T>
    {
        private readonly IContainer _container;
        private readonly TickProvider _tickProvider;
        private readonly GameStateLifetimeManager _gameStateLifeManager;
        
        private static bool _asUpdate = false;
        private static DisposeEvent _disposeEvent;
        private static LifeTime _lifeTime = LifeTime.Transient;
        private static bool _asCached = false;
        private static bool _asMono = false;
        private static bool _asScriptable;
        private readonly IScope _scope;
        private static object _tempObject;
        private static Type _tempObjectType;
        

        public BindingBuilder<T> AsSingleton()
        {
            _lifeTime = LifeTime.Singleton;
            return this;
        }


        public BindingBuilder(IContainer container, TickProvider tickProvider, GameStateLifetimeManager gameStateLifeManager)
        {
            _container = container;
            _tickProvider = tickProvider;
            _gameStateLifeManager = gameStateLifeManager;
            _scope = _container.CreateScope();
            
            _asUpdate = false;
            _disposeEvent = DisposeEvent.OnDestroy;
            _lifeTime = LifeTime.Transient;
            _asCached = false;
            _asMono = false;
            _asScriptable = false;
            _tempObject = null;
            _tempObjectType = null;
        }

        public BindingBuilder<T> AsUpdate<TTick>() where TTick : class
        {
            _asUpdate = true;
            return this;
        }
        
        public BindingBuilder<T> AsDispose(DisposeEvent evt)
        {
            _disposeEvent = evt;
            return this;
        }

        public BindingBuilder<T> AsMono(object obj)
        {
            _asMono = true;
            _tempObject = obj;
            _asCached = true;
            return this;
        }

        public BindingBuilder<T> AsCached(object obj, Type objType = null)
        {
            _asCached = true;
            _tempObject = obj;
            if(objType != null)
                _tempObjectType = objType;
            return this;
        }

        public BindingBuilder<T> AsMonoCached(object obj)
        {
            _asMono = true;
            _tempObject = obj;
            _asCached = true;
            return this;
        }

        public  BindingBuilder<T> AsScriptable(object obj)
        {
            _asScriptable = true;
            _tempObject = obj;
            _asCached = true;
            return this;
        }

        public void Registration()
        {
            if (!_asCached)
                _container.BindData(typeof(T), typeof(T), _lifeTime);
            else if (_asMono)
                _container.CacheMono(_tempObject.GetType(), _tempObject as MonoBehaviour);
            else if(_asScriptable)
                _container.CacheScriptableObject(_tempObject.GetType(), _tempObject as ScriptableObject);
            else
                _container.CacheType(_tempObjectType ?? typeof(T), _tempObject);

            object instance = _scope.Resolve(_tempObjectType ?? typeof(T));
            
            if(_asUpdate)
                _tickProvider.Register(instance);
            
            if(_disposeEvent != DisposeEvent.OnDestroy)
                _gameStateLifeManager.Track(_disposeEvent, instance);
            
            _asUpdate = false;
            _disposeEvent = DisposeEvent.OnDestroy;
            _lifeTime = LifeTime.Transient;
            _asCached = false;
            _asMono = false;
            _asScriptable = false;
            _tempObject = null;
            _tempObjectType = null;
        }
    }

    
}