using System;
using System.Collections.Generic;

namespace Assets.Scripts.Models.Events
{
    public class SimpleEvents
    {
        readonly private static MonoState _monoState;

        static SimpleEvents()
        {
            _monoState = new MonoState();
        }

        internal class MonoState
        {
            readonly Dictionary<string, List<Action<object>>> _items = new Dictionary<string, List<Action<object>>>();

            public void Attach(string category, Action<object> method)
            {
                List<Action<object>> list;
                if (_items.TryGetValue(category, out list))
                    list.Add(method);
                else
                    _items.Add(category, new List<Action<object>> { method });
            }

            public void Detach(string category, Action<object> method)
            {
                List<Action<object>> list;
                if (!_items.TryGetValue(category, out list)) return;
                list.Remove(method);
            }

            public void Call(string category, object obj)
            {
                List<Action<object>> list;
                if (!_items.TryGetValue(category, out list)) return;
                if (list.Count == 0) return;
                if (list.Count == 1)
                    list[0](obj);
                else
                {
                    var copy = new Action<object>[list.Count];
                    list.CopyTo(copy, 0);
                    foreach (var method in copy)
                        method(obj);
                }
            }

            public void DetachCategory(string category)
            {
                _items.Remove(category);
            }

        }

        public void Attach(string category, Action<object> method)
        {
            lock (_monoState)
            {
                _monoState.Attach(category, method);
            }
        }

        public void Detach(string category, Action<object> method)
        {
            lock (_monoState)
            {
                _monoState.Detach(category, method);
            }
        }

        public void DetachCategory(string category)
        {
            _monoState.DetachCategory(category);
        }

        public void Call(string category, object obj)
        {
            _monoState.Call(category, obj);
        }
    }
}
