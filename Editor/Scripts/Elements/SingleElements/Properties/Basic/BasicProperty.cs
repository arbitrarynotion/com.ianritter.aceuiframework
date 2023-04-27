using System;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.ElementConditions;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsCustom.SingleElements;
using UnityEngine;

namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.SingleElements.Properties.Basic
{
    public class BasicProperty : PropertyElement
    {
        public override PropertyElementLayout PropertyElementLayout => _basicPropertyLayout;
        protected override PropertyElementDraw PropertyElementDraw => _basicPropertyDraw;
        private BasicPropertyLayout _basicPropertyLayout;
        private BasicPropertyDraw _basicPropertyDraw;
        

        public BasicProperty(
            string varName,
            GUIContent guiContent,
            SingleCustomSettings singleCustomSettings,
            Action changeCallBack = null,
            bool hideOnDisable = false,
            params ElementCondition[] conditions )
            : base( varName, guiContent, singleCustomSettings, changeCallBack, hideOnDisable, conditions )
        {
        }
        

        protected override void InitializeLayout() => _basicPropertyLayout = new BasicPropertyLayout( this );

        protected override void InitializeDraw() => _basicPropertyDraw = new BasicPropertyDraw( this );
    }
}
