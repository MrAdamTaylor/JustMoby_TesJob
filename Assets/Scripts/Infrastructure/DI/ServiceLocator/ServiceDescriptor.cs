using System;
using System.Collections.Generic;

namespace Infrastructure.DI.ServiceLocator
{
    public class ServiceDescriptor 
    {
        private readonly Dictionary<Type, Model.ServiceDescriptor> _descriptors;

        public ServiceDescriptor(Dictionary<Type, Model.ServiceDescriptor> descriptors)
        {
            _descriptors = descriptors;
        }
        
        public ServiceDescriptor()
        {
            _descriptors = new Dictionary<Type, Model.ServiceDescriptor>();
        }

        public void BindData(Type type, object obj)
        {
            if (_descriptors.ContainsKey(type))
                return;
            
            _descriptors.Add(type,(Model.ServiceDescriptor)obj);
        }

        public Model.ServiceDescriptor GetDescriptor(Type type)
        {
            return _descriptors[type];
        }
        
        public Model.ServiceDescriptor TryGetDescriptor(Type type)
        {
            return _descriptors.ContainsKey(type) ? _descriptors[type] : null;
        }

        public Model.ServiceDescriptor TryGetType(Type service)
        {
            _descriptors.TryGetValue(service, out var result);
            return result;
        }
        
    }
}
