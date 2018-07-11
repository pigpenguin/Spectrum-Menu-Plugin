using SpectrumTestPlugin.UI;
using System.Collections.Generic;

namespace SpectrumTestPlugin
{

    public class SpectrumSettingsMenu : SpectrumMenu
    {
        public override string MenuTitleName_ => "Spectrum Extension System";
        public override string Name_ => "Spectrum";

        public override bool DisplayInMenu(bool isPauseMenu) => true;

        private readonly KeyValuePair<string, Language>[] displayNames = 
            new KeyValuePair<string, Language>[] { KVP<string,Language>("English",Language.English)
                                                 , KVP<string,Language>("Français",Language.French)
                                                 , KVP<string,Language>("Deutsch",Language.German)
                                                 , KVP<string,Language>("日本語",Language.Japanese)
                                                 , KVP<string,Language>("中文",Language.Chinese)
                                                 };

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
        
        // I assume this gets called when some one leaves the setting menu?
        public override void OnPanelPop()
        {
           
        }
    }
}
