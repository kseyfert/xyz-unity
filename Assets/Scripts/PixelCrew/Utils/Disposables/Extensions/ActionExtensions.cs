using System;

namespace PixelCrew.Utils.Disposables
{
    public static class ActionExtensions
    {
        public static IDisposable Subscribe(this Action action, Action call)
        {
            action += call;
            return new ActionDisposable(() => action -= call);
        }
    }
}