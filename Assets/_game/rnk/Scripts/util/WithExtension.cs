using System;

namespace _game.rnk.Scripts.util
{
    public static class WithExtension
    {
        public static T With<T>(this T item, Action<T> action)
        {
            action(item);
            return item;
        }
    }
}