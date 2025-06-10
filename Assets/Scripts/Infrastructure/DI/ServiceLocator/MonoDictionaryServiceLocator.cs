using System;
using System.Collections.Generic;
using UnityEngine;

namespace Infrastructure.DI.ServiceLocator
{
    public class MonoDictionaryServiceLocator : IServiceLocator
    {
        private readonly Dictionary<Type, MonoBehaviour> _monoDict = new();

        public object GetData(Type type) => _monoDict[type];

        public bool IsGetData(Type type) => _monoDict.ContainsKey(type);

        public void BindData(Type type, object obj)
        {
            if (obj is MonoBehaviour mono)
            {
                _monoDict[type] = mono;
            }
            else
            {
                Debug.LogWarning($"Object {obj} is not MonoBehaviour");
            }
        }
    }
}