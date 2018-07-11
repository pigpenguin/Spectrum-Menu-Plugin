using Spectrum.API;
using Spectrum.API.Interfaces.Plugins;
using Spectrum.API.Interfaces.Systems;
using Spectrum.API.IPC;
using Spectrum.API.Logging;
using SpectrumTestPlugin.UI;

using UnityEngine;

using System;
using SpectrumTestPlugin;
using System.Collections.Generic;

namespace Spectrum_Menu_Plugin
{
    public class Entry : IPlugin, IIPCEnabled
    {
        private IManager _manager;

        public Spectrum.API.Logging.Logger Log { get; set; }

        public void Initialize(IManager manager, string ipcIdentifier)
        {
            _manager = manager;
            Console.WriteLine("Hello, world.");
            Log = new Spectrum.API.Logging.Logger("IPCData.log")
            {
                WriteToConsole = true
            };

            Events.Scene.LoadFinish.Subscribe((data) =>
            {
                if (data.sceneName == "MainMenu")
                {
                    CreateMenu(manager, "SpectrumSettingsObject", "OptionsFrontRoot", "MainMenuFrontRoot");
                    CreateLanguageMenu(manager, "LanguageSettingsObject", "OptionsFrontRoot", "MainMenuFrontRoot");
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
            var menuController = spectrumSettingsObject.AddComponent<SpectrumSettingsMenu>();

            // Feed it the plugin manager so this menu can modify those settings
            menuController.SetManager(manager);

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

        //CreateMenu(manager, "LanguageSettingsObject", "OptionsFrontRoot", "MainMenuFrontRoot");
        private void CreateLanguageMenu(IManager manager, string settingsObjectName, string optionsFrontRootName, string mainMenuFrontRootName)
        {
            // Create a new object
            var spectrumSettingsObject = new GameObject(settingsObjectName);

            // Put the spectrum settings menu into that object
            var menuController = spectrumSettingsObject.AddComponent<LanguageMenu>();

            // Feed it the plugin manager so this menu can modify those settings
            menuController.SetManager(manager);

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
    }
}
