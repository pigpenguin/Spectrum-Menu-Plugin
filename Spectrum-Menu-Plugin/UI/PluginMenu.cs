using Spectrum.API.Interfaces.Systems;
using Spectrum.API.IPC;
using Spectrum_Menu_Plugin;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace SpectrumTestPlugin.UI
{
    public class PluginMenu : SuperMenu
    {
        public override string MenuTitleName_ => "[NAME GOES HERE]";
        public override string Name_ => "[MENU BUTTON TEXT]";

        public IManager Manager { get; private set; }
        public MenuItem[] Layout { get; private set; }
        public MenuData Data { get; private set; }
        public String Source { get; private set; }

        public override bool DisplayInMenu(bool isPauseMenu) => true;

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

        public void SetState(MenuData data)
        {
            Data = data;
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
            Console.WriteLine("Initialize called!");
            foreach (var item in Layout)
            {
                RenderItem(item);
            }
        }

        private void RenderItem(MenuItem item)
        {
            Console.WriteLine("Attempting to render item");
            var type        = item["type"];
            var name        = item.Get<string>("name");
            var description = item.Get<string>("description");

            if (item.Get<bool>("restart"))
                description = description + "\n[FF0000]Requires game restart.[-]";

            if (type == typeof(bool))
            {
                Console.WriteLine("Hey, it's a bool!");
                TweakBool(name, Data.Get<bool>(name), DataReporter<bool>(name), description);
            }

            if (type == typeof(int))
            {
                TweakInt(name, 0, item.Get<int>("min"), item.Get<int>("max"), Data.Get<int>(name), DataReporter<int>(name), description);
            }

            if (type == typeof(float))
            {
                TweakFloat(name, Data.Get<float>(name), item.Get<float>("min"), item.Get<float>("max"), item.Get<float>("default"), DataReporter<float>(name), description);
            }
            if (((Type)type).IsEnum)
            {
                typeof(PluginMenu).GetMethod("RenderEnum" , BindingFlags.NonPublic | BindingFlags.Instance)
                                  .MakeGenericMethod((Type)type)
                                  .Invoke(this, new Object[] { item });

            }
        }

        private void RenderEnum<T>(MenuItem item)
        {
            Console.WriteLine("We are trying to draw the enum menu ;_;");
            var name = item.Get<string>("name");
            var description = item.Get<string>("description");
            var displayList = item.Get<KeyValuePair<String,T>[]>("displayList");

            if (item.Get<bool>("restart"))
                description = description + "\n[FF0000]Requires game restart.[-]";

            TweakEnum(name, () => Data.Get<T>(name), DataReporter<T>(name), description, displayList);
        }

        public override void OnPanelPop()
        {
            
        }
    }
}
