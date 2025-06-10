using System;
using System.Collections.Generic;
using UnityEngine;

namespace Infrastructure.DI.ServiceLocator
{
    public class DictionaryServiceLocator : IServiceLocator
    {
        private readonly Dictionary<Type, object> _servicesDataBase = new();

        public object GetData(Type type) => _servicesDataBase[type];

        public bool IsGetData(Type type) => _servicesDataBase.ContainsKey(type);

        public void BindData(Type type, object obj)
        {
            if (_servicesDataBase.ContainsKey(type))
            {
                _servicesDataBase[type] = obj;
            }
            else
            {
                _servicesDataBase.Add(type, obj);
            }
        }
    }
}