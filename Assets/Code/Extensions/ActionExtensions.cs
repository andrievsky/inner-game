using System;

namespace Code.Extensions
{
    public static class ActionExtensions
    {
        public static void Dispatch(this Action act)
        {
            if (act == null)
            {
                return;
            }

            act();
        }
        
        public static void Dispatch<T>(this Action<T> act, T arg)
        {
            if (act == null)
            {
                return;
            }

            act(arg);
        }
    }
}
