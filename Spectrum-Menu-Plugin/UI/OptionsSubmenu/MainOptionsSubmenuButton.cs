using System;
using UnityEngine;

namespace SpectrumTestPlugin
{
    public class MainOptionsSubmenuButton : OptionsSubmenuButton
    {
        public MainOptionsSubmenuButton(string text) : base(text) { }
        public MainOptionsSubmenuButton(string text, Action onClick) : base(text, onClick) { }

        protected override GameObject GetBlueprint()
        {
            var optionButtonsPanel = Util.FindByName("OptionsButtonsPanel");
            var opbLogic = optionButtonsPanel?.GetComponent<UIPanel>();

            var optionButtonsTable = optionButtonsPanel?.transform.Find("OptionsButtonsTable");
            _buttonsTable = optionButtonsTable?.GetComponent<UITable>();

            return optionButtonsTable.transform.Find("ButtonBlueprint")?.gameObject;
        }
    }
}
