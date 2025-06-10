using Infrastructure.DI.Container;

namespace Infrastructure.DI.Model
{
    public static class FluentContainerExtension
    {
        public static BindingBuilder<T> Bind<T>(this IContainer container)
        {
            return new BindingBuilder<T>(container, container.TickProvider, container.LifetimeManager);
        }
    }
}