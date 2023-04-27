using System;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.ElementConditions;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsCustom.SingleElements;
using UnityEngine;

namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.SingleElements.Properties.Popup
{
    public class PopupElement : PropertyElement
    {
        private PopupElementLayout _popupElementLayout;
        private PopupElementDraw _popupElementDraw;
        
        public override PropertyElementLayout PropertyElementLayout => _popupElementLayout;
        protected override PropertyElementDraw PropertyElementDraw => _popupElementDraw;
        
        public readonly string[] Options;
        

        public PopupElement( 
            string varName, 
            GUIContent guiContent,
            string[] options,  
            SingleCustomSettings singleCustomSettings,
            Action changeCallBack, 
            
            bool hideOnDisable = false, 
            params ElementCondition[] conditions ) 
            : base( varName, guiContent, singleCustomSettings, changeCallBack, hideOnDisable, conditions )
        {
            Options = options;
        }
        
        
        protected override void InitializeLayout() => _popupElementLayout = new PopupElementLayout( this );

        protected override void InitializeDraw() => _popupElementDraw = new PopupElementDraw( this );
    }
}
