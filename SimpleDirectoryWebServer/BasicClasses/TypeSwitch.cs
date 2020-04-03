using System;
using System.Collections.Generic;

namespace SimpleDirectoryWebServer.BasicClasses {
    public class TypeSwitch {
        private Dictionary<Type, Action<object>> matches = new Dictionary<Type, Action<object>>();
        private Action<object> _defaultAction;

        public TypeSwitch Case<T>(Action<T> action) {
            matches.Add(typeof(T), x => action((T)x)); 
            return this;
        }

        public TypeSwitch DefaultAction(Action<object> action) {
            _defaultAction = action;
            return this;
        }
        public void Switch(object x) {
            try { 
                matches[x.GetType()](x);
            }
            catch (KeyNotFoundException) {
                _defaultAction(x);
            }
        }
    }
}