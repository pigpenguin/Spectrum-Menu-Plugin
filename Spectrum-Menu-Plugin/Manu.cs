using Spectrum.API.Interfaces.Plugins;
using Spectrum.API.Interfaces.Systems;
using Spectrum.API.IPC;

using UnityEngine;

using System;
using SpectrumTestPlugin;
using System.Collections.Generic;

namespace Spectrum_Menu_Plugin
{
    public enum Language
    {
        English,
        French,
        German,
        Japanese,
        Chinese
    }

    public class Entry : IPlugin, IIPCEnabled
    {
        private IManager _manager;

        private MenuData menuData;
        private MenuItem[] menuLayout;

        public Spectrum.API.Logging.Logger Log { get; set; }

        public void Initialize(IManager manager, string ipcIdentifier)
        {
            _manager = manager;
            Log = new Spectrum.API.Logging.Logger("IPCData.log")
            {
                WriteToConsole = true
            };

            Events.Scene.LoadFinish.Subscribe((data) =>
            {
                if (data.sceneName == "MainMenu")
                {
                    CreateMenu(manager, "SpectrumSettingsObject", "OptionsFrontRoot", "MainMenuFrontRoot");
                }
            });
        }

        public void HandleIPCData(IPCData data)
        {
            if (data.ContainsKey("Message"))
            {
                Console.WriteLine($"Message received. Contents: {data.Get<string>("Message")}");
                var ipcData = new IPCData("Menu");
                ipcData["Message"] = "Message Recieved";
                _manager.SendIPC(data.SourceIdentifier, ipcData);
            }
        }

        //CreateMenu(manager, "SpectrumSettingsObject", "OptionsFrontRoot", "MainMenuFrontRoot");
        private void CreateMenu(IManager manager, string settingsObjectName, string optionsFrontRootName, string mainMenuFrontRootName)
        {
            // Create a new object
            var spectrumSettingsObject = new GameObject(settingsObjectName);

            // Put the spectrum settings menu into that object
            var menuController = spectrumSettingsObject.AddComponent<SpectrumTestPlugin.UI.PluginMenu>();

            // Feed it the plugin manager so this menu can modify those settings
            menuController.SetManager(manager);
            menuController.SetState(menuData);
            menuController.SetLayout(menuLayout);

            // Lists all the items currently in the options menu
            var optionsLogic = Util.FindByName(optionsFrontRootName).GetComponent<OptionsMenuLogic>();

            // A new list to represent our new options menu
            var options = new List<OptionsSubmenu>();

            // Add all our old options to our new options
            options.AddRange(optionsLogic.subMenus_);

            // Add our spectrum menu too
            options.Add(menuController);

            // Write our new list to the options menu
            optionsLogic.subMenus_ = options.ToArray();

            // ??? No idea what these do yet
            var mainMenuLogic = Util.FindByName(mainMenuFrontRootName).GetComponent<MainMenuLogic>();
            List<MenuButtonList.ButtonInfo> buttonInfos = mainMenuLogic.optionsButtons_.GetButtonInfos(optionsLogic, false);
            mainMenuLogic.optionsButtons_.Init(buttonInfos);
        }
        /**
        public override void InitializeVirtual()
        {
            // These just bind functions to menu objects... not much to really understand
            TweakBool("SYSTEM ENABLED", Manager.GetConfig<bool>("Enabled"), (value) =>
            {
                Manager.SetConfig("Enabled", value);
            }, "Enable or disable Spectrum.\n[FF0000]Requires game restart.[-]");

            TweakBool("SHOW WATERMARK", Manager.GetConfig<bool>("ShowWatermark"), (value) =>
            {
                Manager.SetConfig("ShowWatermark", value);
            }, "Show/hide text in the upper-right corner of the screen.\n[FF0000]Requires game restart.[-]");

            TweakBool("LOG TO CONSOLE", Manager.GetConfig<bool>("LogToConsole"), (value) =>
            {
                Manager.SetConfig("LogToConsole", value);
            }, "Log all messages to the debug console (if visible).\n[FF0000]Requires game restart.[-]");
        }
        **/
    }
}
