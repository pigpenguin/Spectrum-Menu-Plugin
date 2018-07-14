using Spectrum.API.Configuration;
using Spectrum.API.IPC;
using System;
using System.Collections.Generic;

using MenuData = System.Collections.Generic.Dictionary<string, object>;
using MenuItem = System.Collections.Generic.Dictionary<string, object>;

namespace Helper
{
    public class SettingHelper
    {
        private Settings settings;
        private LinkedList<MenuItem> layout;
        private string IPCIdentifier, MenuTitle, MenuButton;

        public SettingHelper(string _IPCIdentifier, ref Settings _settings, string menuTitle, string menuButton)
        {
            settings = _settings;
            IPCIdentifier = _IPCIdentifier;
            MenuTitle = menuTitle;
            MenuButton = menuButton;
            layout = new LinkedList<MenuData>();
        }

        private MenuData ReadSettings()
        {
            var menuData = new MenuData();
            foreach (MenuItem item in layout)
            {
                var type = (Type)item["type"];
                var name = (String)item["name"];
                Object value;
                if (type.IsEnum)
                {
                    value = Enum.Parse(type, settings.GetItem<string>(name));
                }
                else
                {
                    value = settings[name];
                }
                menuData.Add(name, value);
            }
            return menuData;
        }

        public void AddBool(String name, String description, bool restart, bool defaultValue)
        {
            if (!settings.ContainsKey(name))
                settings.Add(name, defaultValue);

            var menuItem = new MenuItem
             {
                 { "name", name },
                 { "type", typeof(bool) },
                 { "description", description },
                 { "restart", restart }
             };

            layout.AddLast(menuItem);
        }

        public void AddInt(String name, String description, bool restart, int defaultValue, int min, int max)
        {
            if (!settings.ContainsKey(name))
                settings.Add(name, defaultValue);

            var menuItem = new MenuItem
             {
                 { "name", name },
                 { "type", typeof(int) },
                 { "description", description },
                 { "restart", restart },
                 { "default", defaultValue },
                 { "max", max },
                 { "min", min }
             };


            layout.AddLast(menuItem);
        }

        public void AddFloat(String name, String description, bool restart, float defaultValue, float min, float max)
        {
            if (!settings.ContainsKey(name))
                settings.Add(name, defaultValue);

            var menuItem = new MenuItem
             {
                 { "name", name },
                 { "type", typeof(float) },
                 { "description", description },
                 { "restart", restart },
                 { "default", defaultValue },
                 { "max", max },
                 { "min", min }
             };

            layout.AddLast(menuItem);
        }

        public void AddEnum<T>(String name, String description, bool restart, T defaultValue, KeyValuePair<String,T>[] displayNames)
        {
            if (!settings.ContainsKey(name))
                settings.Add(name, defaultValue);

            var menuItem = new MenuItem
            {
                { "name", name },
                { "type", typeof(T) },
                { "description", description },
                { "restart", restart },
                { "displayList", displayNames }
            };

            layout.AddLast(menuItem);
        }

        public IPCData InitialMessage()
        {
            var menuData = ReadSettings();
            var ipcdata = new IPCData(IPCIdentifier)
            {
                { "title", MenuTitle },
                { "button", MenuButton },
                { "layout", layout },
                { "data", menuData }
            };
            return ipcdata;
        }
        public IPCData GetLayout()
        {
            var ipcdata = new IPCData(IPCIdentifier)
            {
                { "layout", layout }
            };
            return ipcdata;
        }
        public IPCData GetData()
        {
            var menuData = ReadSettings();
            var ipcdata = new IPCData(IPCIdentifier)
            {
                { "data", menuData }
            };
            return ipcdata;
        }
    }
}
