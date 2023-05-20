using System;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.ElementConditions;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsCustom.SingleElements;
using UnityEngine;

namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.SingleElements.Properties.Popup.CustomColorEntryPopup
{
    public class CustomColorEntryPopupElement : PopupElement
    {
        private CustomColorEntryPopupElementDraw _customColorEntryPopupElementDraw;

        protected override PropertyElementDraw PropertyElementDraw => _customColorEntryPopupElementDraw;

        
        public CustomColorEntryPopupElement( string varName, GUIContent guiContent, string[] options, SingleCustomSettings singleCustomSettings, Action changeCallBack, bool hideOnDisable = false, params ElementCondition[] conditions ) 
            : base( varName, guiContent, options, singleCustomSettings, changeCallBack, hideOnDisable, conditions )
        {
        }
        
        protected override void InitializeDraw() => _customColorEntryPopupElementDraw = new CustomColorEntryPopupElementDraw( this );
    }
}
