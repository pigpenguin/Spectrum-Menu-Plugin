using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SpectrumTestPlugin
{
    public abstract class OptionsSubmenuButton
    {
        protected string _text;

        protected GameObject _blueprint;
        protected GameObject _button;

        protected UILabel _buttonLabel;
        protected UITable _buttonsTable;
        protected UIEventListener _uiEventListener;
        protected UIKeyNavigation _uiKeyNavigation;

        public string Text
        {
            get { return _text; }
            set
            {
                if (_buttonLabel != null)
                    _buttonLabel.text = value;

                _text = value;
            }
        }

        public GameObject GameObject => _button;

        public OptionsSubmenuButton(string text)
        {
            _blueprint = GetBlueprint();

            if (_blueprint == null)
                return;

            _button = UIExBlueprint.Duplicate(_blueprint);
            _buttonLabel = _button.transform.Find("UILabel")?.GetComponent<UILabel>();

            _buttonLabel.text = text;
            Text = text;

            _button.SetActive(true);
            _buttonsTable.repositionNow = true;
        }

        public OptionsSubmenuButton(string text, Action onClick) : this(text)
        {
            SetClickEventHandler(onClick);
        }

        public void SetClickEventHandler(Action action)
        {
            if (_uiEventListener == null)
                _uiEventListener = UIEventListener.Get(_button);

            _uiEventListener.onClick = (sender) => action();
        }

        protected abstract GameObject GetBlueprint();
    }
}
