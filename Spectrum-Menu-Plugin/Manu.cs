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
            var spectrumSettingsObject = new GameObject(settingsObjectName);
            var menuController = spectrumSettingsObject.AddComponent<SpectrumSettingsMenu>();
            menuController.SetManager(manager);

            var optionsLogic = Util.FindByName(optionsFrontRootName).GetComponent<OptionsMenuLogic>();
            var options = new List<OptionsSubmenu>();
            options.AddRange(optionsLogic.subMenus_);
            options.Add(menuController);
            optionsLogic.subMenus_ = options.ToArray();

            var mainMenuLogic = Util.FindByName(mainMenuFrontRootName).GetComponent<MainMenuLogic>();
            List<MenuButtonList.ButtonInfo> buttonInfos = mainMenuLogic.optionsButtons_.GetButtonInfos(optionsLogic, false);
            mainMenuLogic.optionsButtons_.Init(buttonInfos);
        }
    }
}
