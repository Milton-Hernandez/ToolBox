using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolBox
{
    public class FlexEnum : DynamicObject
    {
        private IDictionary<string, ushort> _map = new Dictionary<string, ushort>();
        private IList<string> _list = new List<string>();

        public FlexEnum()
        {
            _map["NONE"] = 0;
        }

        public string TypeName
        {
            get
            {
                return this.GetType().Name;
            }
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var key = binder.Name.ToUpper();
            if(!_map.ContainsKey(key))
            {
                if (_list.Count >= ushort.MaxValue)
                {
                    result = (ushort) 0;
                    return false;
                }
                else
                {
                    _list.Add(key);
                    _map[key] = (ushort) _list.Count;
                }
            }
            result = _map[key];
            return true;
        }

        public ushort Parse(string arg)
        {
            var val = arg.ToUpper();
            if (!_map.ContainsKey(val))
            {
                if (_list.Count >= ushort.MaxValue)
                    return (ushort)0;
                else
                {
                    _list.Add(val);
                    _map[val] = (ushort)_list.Count;
                }
            }
            return _map[val];
        }

        public string this[ushort arg]
        {            
            get {
                if (arg == 0)
                    return "NONE";
                else if (arg > _list.Count)
                    return "NONE";
                return _list[arg-1];
            }
        }

        public ushort this[string arg]
        {
            get
            {
                var key = arg.ToUpper();
                if (!_map.ContainsKey(key))
                    return 0;
                return _map[key];
            }
        }


        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
           return false;
        }
    }
}
