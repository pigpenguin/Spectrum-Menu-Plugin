using System;
using UnityEngine;

namespace SpectrumTestPlugin
{
    public class PauseOptionsSubmenuButton : OptionsSubmenuButton
    {
        public PauseOptionsSubmenuButton(string text) : base(text) { }
        public PauseOptionsSubmenuButton(string text, Action onClick) : base(text, onClick) { }

        protected override GameObject GetBlueprint()
        {
            var optionButtonsPanel = Util.FindByName("NewOptionsButtonsPanel");
            var opbLogic = optionButtonsPanel?.GetComponent<UIPanel>();

            var optionButtonsTable = optionButtonsPanel?.transform.Find("NewOptionsButtons");
            _buttonsTable = optionButtonsTable?.GetComponent<UITable>();

            return optionButtonsTable?.transform.Find("ButtonBlueprint")?.gameObject;
        }
    }
}
