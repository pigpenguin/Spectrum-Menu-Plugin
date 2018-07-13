using Spectrum.API.Interfaces.Systems;
using Spectrum.API.IPC;
using Spectrum_Menu_Plugin;
using System;
using System.Collections.Generic;

namespace SpectrumTestPlugin.UI
{
    public abstract class PluginMenu : SuperMenu
    {
        public override string MenuTitleName_ => "[NAME GOES HERE]";
        public override string Name_ => "[MENU BUTTON TEXT]";

        public IManager Manager { get; private set; }
        public MenuItem[] Layout { get; private set; }
        public MenuData Data { get; private set; }
        public String Source { get; private set; }

        public override bool DisplayInMenu(bool isPauseMenu) => isPauseMenu;

        public PluginMenu()
        {
            menuBlueprint_ = Util.FindByName("SuperMenuBlueprint");
        }

        public void SetManager(IManager manager)
        {
            Manager = manager;
        }

        public void SetLayout(MenuItem[] layout)
        {
            Layout = layout;
        }

        public void SetPlugin(String ipcname)
        {
            Source = ipcname;
        }

        public void SetData(MenuData data)
        {
            Data = data;
        }

        private T GetData<T>(String name)
        {
            return Data.Get<T>(name);
        }

        public Action<T> DataReporter<T>(String name)
        {
            var ipcData = new IPCData("Menu");
            return data =>
            {
                ipcData[name] = data;
                Manager.SendIPC(Source, ipcData);
            };
        }

        public override void InitializeVirtual()
        {
            foreach (var item in Layout)
            {
                RenderItem(item);
            }
        }

        private void RenderItem(MenuItem item)
        {
            var type        = item.Get<Type>("type");
            var name        = item.Get<string>("name");
            var description = item.Get<string>("description");

            if (item.Get<bool>("restart"))
                description = description + "\n[FF0000]Requires game restart.[-]";

            if (type == typeof(bool))
            {
                TweakBool(name, false, DataReporter<bool>(name), description);
            }

            if (type == typeof(int))
            {
                TweakInt(name, 0, item.Get<int>("min"), item.Get<int>("max"), Data.Get<int>(name), DataReporter<int>(name), description);
            }

            if (type == typeof(float))
            {
                TweakFloat(name, 0, item.Get<float>("min"), item.Get<float>("max"), Data.Get<float>(name), DataReporter<float>(name), description);
            }

            if (type.IsEnum)
            {
                var m = typeof(PluginMenu).GetMethod("RenderEnum");
                var g = m.MakeGenericMethod(type);
                Object[] arguments = [item];
                g.Invoke(this, arguments);
            }
        }

        private void RenderEnum<T>(MenuItem item)
        {
            var name = item.Get<string>("name");
            var description = item.Get<string>("description");
            var displayList = item.Get<KeyValuePair<String,T>[]>("displayList");

            if (item.Get<bool>("restart"))
                description = description + "\n[FF0000]Requires game restart.[-]";

            TweakEnum(name, () => Data.Get<T>(name), DataReporter<T>(name),description,displayList);
        }

        public override void OnPanelPop()
        {
            
        }
    }
}
