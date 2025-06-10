using System;

namespace Infrastructure.DI.Tickable
{
    public interface ITickable : ITickMain
    {
        void Tick();
    }
    
    public interface ILateTickable : ITickMain
    {
        void LateTick();
    }
    
    public interface IFixedTickable : ITickMain
    {
        void FixedTick();
    }

    public interface ITickMain : IDisposable
    {
        
    }
}