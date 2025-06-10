using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Infrastructure.DI.Model;
using ServiceDescriptor = Infrastructure.DI.ServiceLocator.ServiceDescriptor;

namespace Infrastructure.DI.Container
{
    public class BuildActivator
    {
        private readonly ServiceDescriptor _serviceDescriptor;

        public BuildActivator(ServiceDescriptor serviceDescriptor)
        {
            _serviceDescriptor = serviceDescriptor;
        }

        public Func<IScope, object> BuildActivation(Type service, bool useDirectType = false)
        {
            Type implementationType;

            if (useDirectType)
            {
                implementationType = service;
            }
            else
            {
                var descriptor = _serviceDescriptor.TryGetType(service);
                if (descriptor is null)
                    throw new InvalidOperationException($"Service {service} is not registered.");

                if (descriptor is InstanceBasedServiceDescriptor ib)
                    return _ => ib.Instance;

                if (descriptor is FactoryBasedServiceDescriptor fb)
                    return _ => fb.Factory;

                implementationType = ((TypeBasedServiceDescriptor)descriptor).ImplementationType;
            }

            var ctor = implementationType.GetConstructors(BindingFlags.Public | BindingFlags.Instance).Single();
            var parameters = ctor.GetParameters();

            var scopeParam = Expression.Parameter(typeof(IScope), "scope");

            var argsExpressions = new Expression[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                var paramType = parameters[i].ParameterType;

                var containerExpr = Expression.Constant(this);
                var typeExpr = Expression.Constant(paramType, typeof(Type));
                var callExpr = Expression.Call(containerExpr,
                    typeof(Container).GetMethod("CreateInstance", BindingFlags.NonPublic | BindingFlags.Instance),
                    typeExpr, scopeParam);

                argsExpressions[i] = Expression.Convert(callExpr, paramType);
            }

            var newExpr = Expression.New(ctor, argsExpressions);
            var lambda = Expression.Lambda<Func<IScope, object>>(newExpr, scopeParam);

            return lambda.Compile();
        }
    }
}