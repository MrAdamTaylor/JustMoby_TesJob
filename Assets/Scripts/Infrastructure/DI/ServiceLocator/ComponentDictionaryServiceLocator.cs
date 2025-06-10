using System;
using System.Collections.Generic;
using UnityEngine;

namespace Infrastructure.DI.ServiceLocator
{
    public class ComponentDictionaryServiceLocator : IServiceLocator
    {
        private readonly Dictionary<Type, Component> _componentDictionary = new();

        public object GetData(Type type) => _componentDictionary[type];

        public bool IsGetData(Type type) => _componentDictionary.ContainsKey(type);

        public void BindData(Type type, object obj)
        {
            if (obj is Component mono)
            {
                _componentDictionary[type] = mono;
            }
            else
            {
                Debug.LogWarning($"Object {obj} is not MonoBehaviour");
            }
        }
    }
}