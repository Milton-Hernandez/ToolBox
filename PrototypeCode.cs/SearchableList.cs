using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrototypeCode.cs
{
    public class SearchableList<T>
    {
        private readonly object _mutex = new object();
        private readonly Dictionary<T,int> _inner = new Dictionary<T,int>();
        private readonly List<T> _innList = new List<T>();

        public int Count
        {
            get
            {
                return _innList.Count;
            }
        }

        public void Reset()
        {
            lock(_mutex)
            {
                _inner.Clear();
                _innList.Clear();
            }
        }

        public IEnumerable<T> All()
        {
            foreach(var s in _inner.Keys)
               yield return (T) s;
        }


        public void Add(T arg)
        {
           lock(_mutex)
            {
                if (!_inner.ContainsKey(arg))
                {
                    _inner.Add(arg, _innList.Count);
                    _innList.Add(arg);
                }
            }
        }

        public int this[T arg]
        {
          get
            {
                if (_inner.ContainsKey(arg))
                    return (int)_inner[arg];
                return -1;
            }
        }

        public T this[int index]
        {
            get
            {
                return _innList[index];
            }
        }
    }
}
