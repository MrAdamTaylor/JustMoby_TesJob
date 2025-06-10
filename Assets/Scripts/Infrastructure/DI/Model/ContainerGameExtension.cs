namespace Infrastructure.DI.Model
{
    public static class ContainerGameExtension
    {
        public static void RegisterLifeCircleService<TService, TImplementation>(this IContainer container, DisposeEvent disposeEvent)
            where TImplementation : TService
        {
            container.RegisterTransientWithTimeState(typeof(TService), typeof(TImplementation),  disposeEvent);
        }
    }
}