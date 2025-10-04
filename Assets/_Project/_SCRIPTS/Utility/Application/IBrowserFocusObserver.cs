using System;

namespace Infrastructure.Application
{
    public interface IBrowserFocusObserver
    {
        event Action<bool> OnApplicationFocusChanged;
        event Action<bool> OnApplicationPauseChanged;
    }
}
