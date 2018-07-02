using SpectrumTestPlugin.UI;

namespace SpectrumTestPlugin
{
    public class SpectrumSettingsMenu : SpectrumMenu
    {
        public override string MenuTitleName_ => "Spectrum Extension System";
        public override string Name_ => "Spectrum";

        public override bool DisplayInMenu(bool isPauseMenu) => true;

        public override void InitializeVirtual()
        {
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

        public override void OnPanelPop()
        {
           
        }
    }
}
