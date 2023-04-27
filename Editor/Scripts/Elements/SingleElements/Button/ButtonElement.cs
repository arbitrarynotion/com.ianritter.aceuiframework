using System;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.ElementConditions;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsCustom.SingleElements;
using UnityEngine;

namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.SingleElements.Button
{
    public abstract class ButtonElement : SingleElement
    {
        public abstract ButtonElementLayout ButtonElementLayout { get; }
        public abstract ButtonElementDraw ButtonElementDraw { get; }
        
        public override SingleElementLayout SingleElementLayout => ButtonElementLayout;
        public override SingleElementDraw SingleElementDraw => ButtonElementDraw;
        
        public readonly Action ButtonCallBack;

        /// <summary>
        ///     When focused is true, button is drawn with normal theme. When false, it is drawn faded. This is
        ///     useful when using buttons to select the state of a menu so the selected state's associated button
        ///     stands out (example: tabbed section).
        /// </summary>
        public bool Focused { get; }


        protected ButtonElement( 
            GUIContent guiContent,
            bool focused, 
            SingleCustomSettings singleCustomSettings, 
            Action buttonCallBack,
            
            bool hideOnDisable,
            ElementCondition[] conditions ) 
            : base( guiContent, singleCustomSettings, hideOnDisable, conditions )
        {
            Focused = focused;
            ButtonCallBack = buttonCallBack;
        }
    }
}
