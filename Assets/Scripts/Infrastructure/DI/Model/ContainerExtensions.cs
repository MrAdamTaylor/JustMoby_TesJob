using UnityEngine;

namespace Infrastructure.DI.Model
{
    public static class ContainerExtensions
    {
        public static void RegisterScoped<TService, TImplementation>(this IContainer container)
            where TImplementation : TService
        {
            container.BindData(typeof(TService), typeof(TImplementation), LifeTime.Scoped);
        }

        public static void RegisterTransient<TService, TImplementation>(this IContainer container)
            where TImplementation : TService
        {
            container.BindData(typeof(TService), typeof(TImplementation), LifeTime.Transient);
        }

        public static void RegisterSingleton<TService, TImplementation>(this IContainer container)
            where TImplementation : TService
        {
            container.BindData(typeof(TService), typeof(TImplementation), LifeTime.Singleton);
        }

        public static void RegisterSingleton<T>(this IContainer container, object instance)
        {
            container.BindData(typeof(T), instance.GetType(), LifeTime.Singleton);
        }
        
        public static void RegisterConfigs<T>(this IContainer container, object instance) 
        {
            container.CacheType(typeof(T), instance);
        }

        public static void RegisterMono<T>(this IContainer container, T instance) where T :  MonoBehaviour
        {
            container.CacheMono(typeof(T), instance);
        }

        public static void RegisterScriptable<T>(this IContainer container, T instance) where T : ScriptableObject
        {
            container.CacheScriptableObject(typeof(T), instance);
        }
        
        public static void RegisterComponent<T>(this IContainer container, T instance) where T : Component
        {
            container.CacheComponent(typeof(T), instance);
        }

        public static void CacheReadyClass<T>(this IContainer container, T instance) where T :  class
        {
            container.CacheType(typeof(T), instance);
        }
    }
}