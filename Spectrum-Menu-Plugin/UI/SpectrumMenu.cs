using Spectrum.API.Interfaces.Systems;

namespace SpectrumTestPlugin.UI
{
    public abstract class SpectrumMenu : SuperMenu
    {
        public override string MenuTitleName_ => "[NAME GOES HERE]";
        public override string Name_ => "[MENU BUTTON TEXT]";

        public IManager Manager { get; private set; }

        public override bool DisplayInMenu(bool isPauseMenu) => isPauseMenu;

        public SpectrumMenu()
        {
            menuBlueprint_ = Util.FindByName("SuperMenuBlueprint");
        }
        
        public void SetManager(IManager manager)
        {
            Manager = manager;
        }

        public override void InitializeVirtual()
        {

        }

        public override void OnPanelPop()
        {
            
        }
    }
}
