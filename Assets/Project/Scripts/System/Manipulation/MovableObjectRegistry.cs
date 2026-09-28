using System.Collections.Generic;

namespace DarkPixelGD.Systems.Manipulation
{
    public static class MovableObjectRegistry
    {
        private static readonly List<MovableObject> _all = new();
        public static IReadOnlyList<MovableObject> All => _all;

        public static void Register(MovableObject obj)
        {
            if (!_all.Contains(obj)) _all.Add(obj);
        }
        public static void Unregister(MovableObject obj)
        {
            _all.Remove(obj);
        }
    }
}