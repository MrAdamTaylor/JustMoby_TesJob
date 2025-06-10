using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.DI.Injector;
using Infrastructure.DI.Model;
using Infrastructure.DI.ServiceLocator;
using Infrastructure.DI.Tickable;
using UnityEngine;
using ServiceDescriptor = Infrastructure.DI.ServiceLocator.ServiceDescriptor;

namespace Infrastructure.DI.Container
{
    public class Container : IContainer
    {
        private class Scope : IScope
        {
            private readonly Container _container;
            private readonly ConcurrentDictionary<Type, object> _scopedInstance = new();
            private readonly ServiceLocatorProvider _serviceLocator;

            public Scope(Container container, ServiceLocatorProvider serviceLocator=null)
            {
                _container = container;
                _serviceLocator = serviceLocator;
            }

            public object Resolve(Type service)
            {
                if (_serviceLocator != null)
                {
                    _serviceLocator.TryResolveService(service, out object instance);
                    if (instance != null)
                        return instance;
                }
                
                var descriptor = _container.FindDescriptors(service);
                if (descriptor.LifeTime == LifeTime.Transient)
                    return _container.CreateInstance(service, this);
                else if (descriptor.LifeTime == LifeTime.Scoped || _container._rootScope == this)
                {
                    return _scopedInstance.GetOrAdd(service, s => _container.CreateInstance(s, this));
                }
                else
                {
                    return _container._rootScope.Resolve(service);
                }
            }
        }

        public GameStateLifetimeManager LifetimeManager => _gameStateLifeManager;
        public TickProvider TickProvider => _tickProvider;
        
        private readonly ServiceDescriptor _serviceDescriptor;
        private readonly DescriptorDependencyInjector _dependencyInjector;
        private readonly ConcurrentDictionary<Type,Func<IScope, object>> _cachedBuildActivators = new();
        private readonly ServiceLocatorProvider _locatorProvider;
        private readonly GameStateLifetimeManager _gameStateLifeManager;
        private readonly BuildActivator _buildActivator;
        
        private readonly TickProvider _tickProvider;
        private bool _initializedTickable;
        
        private readonly Scope _rootScope;

        public Container(IEnumerable<Model.ServiceDescriptor> descriptors, IServiceLocator serviceLocator = null)
        {
            Dictionary<Type, Model.ServiceDescriptor> dictionaryDescriptors = descriptors.ToDictionary(x => x.ServiceType);
            _serviceDescriptor = new ServiceDescriptor(dictionaryDescriptors);
            _locatorProvider = new ServiceLocatorProvider();
            _tickProvider = new TickProvider();
            _gameStateLifeManager = new GameStateLifetimeManager(_tickProvider);
            _dependencyInjector = new DescriptorDependencyInjector(_serviceDescriptor, _locatorProvider, _gameStateLifeManager, this);
            _buildActivator = new BuildActivator(_serviceDescriptor);
            
            _rootScope = new Scope(this);
        }
        
        public Container(IServiceLocator serviceLocator = null)
        {
            _serviceDescriptor = new ServiceDescriptor();
            _locatorProvider = new ServiceLocatorProvider();
            _tickProvider = new TickProvider();
            _gameStateLifeManager = new GameStateLifetimeManager(_tickProvider);
            _dependencyInjector = new DescriptorDependencyInjector(_serviceDescriptor, _locatorProvider, _gameStateLifeManager, this);
            _buildActivator = new BuildActivator(_serviceDescriptor);
            
            _rootScope = new Scope(this, _locatorProvider);
        }

        public void InitializeITickable()
        {
            _initializedTickable = true;
        }

        public void FixedUpdate()
        {
            if(_initializedTickable) 
                _tickProvider.FixedTick();
        }

        public void LateUpdate()
        {
            if (_initializedTickable)
                _tickProvider.LateTick();
        }

        public void Update()
        {
            if (_initializedTickable)
                _tickProvider.Tick();
        }

        public void StopITickable()
        {
            _initializedTickable = false;
        }

        public object ReturnInjectArgument(Model.ServiceDescriptor descriptor, Type type)
        {
            object service = descriptor.LifeTime == LifeTime.Transient ? CreateInstance(type, _rootScope) : _rootScope.Resolve(type);
            return service;
        }

        public IScope CreateScope()
        {
            return new Scope(this);
        }

        public void BindData(Type type, Type service, LifeTime lifetime)
        {
            TypeBasedServiceDescriptor serviceDescriptor = new TypeBasedServiceDescriptor() 
                {ImplementationType = service, ServiceType = type, LifeTime = lifetime };
            _serviceDescriptor.BindData(type,serviceDescriptor);
        }

        public void Construct(object target)
        {
            _dependencyInjector.Inject(target);
        }

        public void CacheType(Type type, object instance)
        {
            _locatorProvider.Default.BindData(type, instance);
        }

        public void CacheMono<T>(Type type, T instance) where T : MonoBehaviour
        {
            _locatorProvider.Mono.BindData(type, instance);
        }

        public void CacheScriptableObject<T>(Type type, T instance) where T : ScriptableObject
        {
            _locatorProvider.Scriptable.BindData(type, instance);
        }

        public void RegisterTransientWithTimeState(Type type, Type type1, DisposeEvent disposeEvent)
        {
            var instance = _buildActivator.BuildActivation(type, true)(_rootScope);
            _gameStateLifeManager.Track(DisposeEvent.AfterBootstrap, instance);
        }

        public void CacheComponent<T>(Type type, T instance) where T : Component
        {
            _locatorProvider.Component.BindData(type, instance);
        }

        public void ClearBy(DisposeEvent bootStateLife)
        {
            _gameStateLifeManager.Clear(bootStateLife);
        }

        private Model.ServiceDescriptor FindDescriptors(Type service)
        {
            if (!_locatorProvider.TryResolveService(service, out var instance))
                return _serviceDescriptor.TryGetType(service);
            InstanceBasedServiceDescriptor serviceDescriptor = new(
                service, instance );
            return serviceDescriptor;
        }

        private object CreateInstance(Type service, IScope scope)
        {
            return _cachedBuildActivators.GetOrAdd(service, key => _buildActivator.BuildActivation(key))(scope);
        }
    }
}