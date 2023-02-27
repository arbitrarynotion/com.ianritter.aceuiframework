using System;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.ElementConditions;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsCustom.SingleElements;
using UnityEngine;

namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.SingleElements.Button.Tab
{
    public class TabButtonElement : ButtonElement
    {
        private ButtonElementLayout _buttonElementLayout;
        private ButtonElementDraw _buttonElementDraw;

        public override ButtonElementLayout ButtonElementLayout => _buttonElementLayout;
        public override ButtonElementDraw ButtonElementDraw => _buttonElementDraw;
        
        
        public TabButtonElement( 
            GUIContent guiContent, 
            bool focused, 
            SingleCustomSettings singleCustomSettings,
            Action buttonCallBack, 
            bool hideOnDisable = false,
            params ElementCondition[] conditions ) 
            : base( guiContent, focused, singleCustomSettings, buttonCallBack, hideOnDisable, conditions )
        {
        }

        
        protected override void InitializeLayout() => _buttonElementLayout = new TabButtonElementLayout( this );
        protected override void InitializeDraw() => _buttonElementDraw = new TabButtonElementDraw( this );
    }
}
