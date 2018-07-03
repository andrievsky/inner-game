using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Input
{
    public interface IKeyBindMap
    {
        ICollection<KeyBindInfo> Binds();
        void Add(KeyCode key, Action callback);
    }

    public class KeyBindInfo
    {
        public KeyCode Key;
        public Action Callback;
    }

    public class KeyBindMap : IKeyBindMap, IDisposable
    {
        private readonly ICollection<KeyBindInfo> _binds = new List<KeyBindInfo>();

        public ICollection<KeyBindInfo> Binds()
        {
            return _binds;
        }

        public void Add(KeyCode key, Action callback)
        {
            _binds.Add(new KeyBindInfo {Key = key, Callback = callback});
        }

        public void Dispose()
        {
            _binds.Clear();
        }
    }
}