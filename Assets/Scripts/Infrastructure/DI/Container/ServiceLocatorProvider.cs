using System;
using Infrastructure.DI.ServiceLocator;

namespace Infrastructure.DI.Container
{
    public class ServiceLocatorProvider
    {
        private readonly IServiceLocator _defaultLocator;
        private readonly IServiceLocator _monoLocator;
        private readonly IServiceLocator _scriptableLocator;
        private readonly IServiceLocator _componentsLocator;
        
        public ServiceLocatorProvider(
            IServiceLocator defaultLocator = null,
            IServiceLocator monoLocator = null,
            IServiceLocator scriptableLocator = null,
            IServiceLocator componentLocator = null)
        {
            _defaultLocator = defaultLocator ?? new DictionaryServiceLocator();
            _monoLocator = monoLocator ?? new MonoDictionaryServiceLocator();
            _scriptableLocator = scriptableLocator ?? new DictionaryServiceLocator();
            _componentsLocator = componentLocator ?? new ComponentDictionaryServiceLocator();
        }

        public IServiceLocator Default => _defaultLocator;

        public IServiceLocator Mono => _monoLocator;

        public IServiceLocator Scriptable => _scriptableLocator;

        public IServiceLocator Component => _scriptableLocator;

        
        public IServiceLocator GetLocator(LocatorType type)
        {
            return type switch
            {
                LocatorType.Mono => _monoLocator,
                LocatorType.Scriptable => _scriptableLocator,
                LocatorType.Component => _componentsLocator,
                _ => _defaultLocator,
            };
        }

        public bool ContainType(Type type)
        {
            return _defaultLocator.IsGetData(type)
                   || _monoLocator.IsGetData(type)
                   || _scriptableLocator.IsGetData(type)
                   || _componentsLocator.IsGetData(type);
        }

        public bool TryResolveService(Type type, out object service)
        {
            service = null;
            
            if (_defaultLocator.IsGetData(type))
            {
                service = _defaultLocator.GetData(type);
                return true;
            }

            if (_monoLocator.IsGetData(type))
            {
                service = _monoLocator.GetData(type);
                return true;
            }

            if (_scriptableLocator.IsGetData(type))
            {
                service = _scriptableLocator.GetData(type);
                return true;
            }

            if (_componentsLocator.IsGetData(type))
            {
                service = _componentsLocator.GetData(type);
                return true;
            }

            return false;
        }
        
    }
}