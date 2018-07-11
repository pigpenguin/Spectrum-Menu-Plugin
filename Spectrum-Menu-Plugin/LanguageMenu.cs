using SpectrumTestPlugin.UI;
using System.Collections.Generic;

namespace SpectrumTestPlugin
{
    public enum Language
    {
        English,
        French,
        German,
        Japanese,
        Chinese
    }

    public class LanguageMenu : SpectrumMenu
    {
        public override string MenuTitleName_ => "Spectrum Extension System";
        public override string Name_ => "Language";

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
            TweakEnum<Language>("Language"
                               , () => Language.English /* This should get the currently set language*/
                               , (language) => {/* Code for setting the language should go here*/}
                               , displayNames);
        }

        // I assume this gets called when some one leaves the setting menu?
        public override void OnPanelPop()
        {

        }
    }
}
