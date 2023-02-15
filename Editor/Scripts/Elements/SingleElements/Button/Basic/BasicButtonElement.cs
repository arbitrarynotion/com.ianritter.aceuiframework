using System;
using ACEPackage.Editor.Scripts.ElementConditions;
using ACEPackage.Runtime.Scripts.SettingsCustom.SingleElements;
using UnityEngine;

namespace ACEPackage.Editor.Scripts.Elements.SingleElements.Button.Basic
{
    public class BasicButtonElement : ButtonElement
    {
        private ButtonElementLayout _buttonElementLayout;
        private ButtonElementDraw _buttonElementDraw;
        
        public override ButtonElementLayout ButtonElementLayout => _buttonElementLayout;
        public override ButtonElementDraw ButtonElementDraw => _buttonElementDraw;
        

        public BasicButtonElement( 
            GUIContent guiContent, 
            bool focused, 
            SingleCustomSettings singleCustomSettings,
            Action buttonCallBack, 
            bool hideOnDisable = false,
            params ElementCondition[] conditions ) 
            : base( guiContent, focused, singleCustomSettings, buttonCallBack, hideOnDisable, conditions )
        {
        }
        

        protected override void InitializeLayout() => _buttonElementLayout = new BasicButtonElementLayout( this );
        protected override void InitializeDraw() => _buttonElementDraw = new BasicButtonElementDraw( this );
    }
}
