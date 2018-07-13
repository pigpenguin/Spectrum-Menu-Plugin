﻿using System;
using System.Collections.Generic;

namespace Spectrum_Menu_Plugin
{
    public class MenuItem : Dictionary<string, object>
    {
        public new object this[string key]
        {
            get
            {
                if (ContainsKey(key))
                    return base[key];

                return null;
            }

            set
            {
                if (!ContainsKey(key))
                    Add(key, value);
                else
                    base[key] = value;
            }
        }

        public T Get<T>(string key)
        {
            try
            {
                return (T)Convert.ChangeType(base[key], typeof(T));
            }
            catch
            {
                return default(T);
            }
        }
    }
}