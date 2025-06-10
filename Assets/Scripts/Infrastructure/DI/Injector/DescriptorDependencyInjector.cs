using System;
using System.Reflection;
using Infrastructure.DI.Container;
using Infrastructure.DI.ServiceLocator;

namespace Infrastructure.DI.Injector
{
    public class DescriptorDependencyInjector : IInjector
    {
        private ServiceDescriptor _serviceDescriptor;
        private ServiceLocatorProvider _locatorProvider;
        private  GameStateLifetimeManager _gameStateLifeManager;
        private Container.Container _container;

        public DescriptorDependencyInjector(ServiceDescriptor service, ServiceLocatorProvider provider,  GameStateLifetimeManager lifeManager,Container.Container container)
        {
            _locatorProvider = provider;
            _gameStateLifeManager = lifeManager;
            _serviceDescriptor = service;
            _container = container;
        }
        
        
        public void Inject(object target)
        {
            var type = target.GetType();

            ProcessMembers(
                type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy),
                m => m.IsDefined(typeof(InjectAttribute), true),
                m => InvokeConstruct((MethodInfo)m, target)
            );

            ProcessMembers(
                type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy),
                f => f.IsDefined(typeof(InjectAttribute), true),
                f => InvokeField((FieldInfo)f, target)
            );

            ProcessMembers(
                type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy),
                f => f.IsDefined(typeof(OptionalInjectAttribute), true),
                f => InvokeOptionalField((FieldInfo)f, target)
            );

            ProcessMembers(
                type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy),
                p => p.IsDefined(typeof(InjectAttribute), true) && ((PropertyInfo)p).CanWrite && ((PropertyInfo)p).GetSetMethod(true) != null,
                p => InvokeProperty((PropertyInfo)p, target)
            );

            ProcessMembers(
                type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy),
                p => p.IsDefined(typeof(OptionalInjectAttribute), true) && ((PropertyInfo)p).CanWrite && ((PropertyInfo)p).GetSetMethod(true) != null,
                p => InvokeOptionalProperty((PropertyInfo)p, target)
            );
        }
        
        private void ProcessMembers<T>(T[] members, Func<T, bool> filter, Action<T> handler) where T : MemberInfo
        {
            for (int i = 0; i < members.Length; i++)
            {
                T member = members[i];
                if (filter(member))
                {
                    handler(member);
                }
            }
        }
        
        
        /// <summary>
        /// Can be return null in field
        /// </summary>
        /// <param name="field"></param>
        /// <param name="target"></param>
        private void InvokeOptionalField(FieldInfo field, object target)
        {
            var type = field.FieldType;
            var value = TryResolveService(type);
            field.SetValue(target, value);
        }

        
        /// <summary>
        /// Can be return null in Property
        /// </summary>
        /// <param name="property"></param>
        /// <param name="target"></param>
        private void InvokeOptionalProperty(PropertyInfo property, object target)
        {
            var type = property.PropertyType;
            var value = TryResolveService(type); 
            property.SetValue(target, value);
        }

        private void InvokeField(FieldInfo field, object target)
        {
            var type = field.FieldType;
            var value = ResolveService(type);
            field.SetValue(target, value);
        }

        private void InvokeProperty(PropertyInfo property, object target)
        {
            if (!property.CanWrite || property.GetSetMethod(true) == null)
                return;

            var type = property.PropertyType;
            var value = ResolveService(type);
            property.SetValue(target, value);
        }

        private void InvokeConstruct(MethodInfo method, object target)
        {
            ParameterInfo[] parameters = method.GetParameters();
            int count = parameters.Length;
            object[] args = new object[count];

            for (int i = 0; i < count; i++)
            {
                Type type = parameters[i].ParameterType;
                args[i] = ResolveService(type);
            }

            method.Invoke(target, args);
        }
        
        private object ResolveService(Type type)
        {
            if (_locatorProvider.TryResolveService(type, out var service))
                return service;

            if (_gameStateLifeManager.IsGetData(type))
                return _gameStateLifeManager.ReturnService(type);

            var descriptor = _serviceDescriptor.GetDescriptor(type);
            return _container.ReturnInjectArgument(descriptor, type);
        }
        
        private object TryResolveService(Type type)
        {
            if (_locatorProvider.TryResolveService(type, out var service))
                return service;

            if (_gameStateLifeManager.IsGetData(type))
                return _gameStateLifeManager.ReturnService(type);

            var descriptor = _serviceDescriptor.TryGetDescriptor(type);
            if (descriptor != null)
            {
                return _container.ReturnInjectArgument(descriptor, type);
            }
            return null;
        }
    }
}