using System;
using System.IO;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.ElementConditions;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsCustom.SingleElements;
using UnityEditor;
using UnityEngine;
using static Packages.com.ianritter.unityscriptingtools.Runtime.Services.TextFormatting.TextFormat;

namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.SingleElements.Properties.Popup.CustomColorEntryPopup
{
    public class CustomColorEntryPopupElement : PopupElement
    {
        public int SelectedIndex { get; set; }
        private CustomColorEntryPopupElementDraw _customColorEntryPopupElementDraw;
        
        private SerializedObject _serializedObject;

        protected override PropertyElementDraw PropertyElementDraw => _customColorEntryPopupElementDraw;

        
        public CustomColorEntryPopupElement( string varName, GUIContent guiContent, string[] options, SingleCustomSettings singleCustomSettings, Action changeCallBack, bool hideOnDisable = false, params ElementCondition[] conditions ) 
            : base( varName, guiContent, options, singleCustomSettings, changeCallBack, hideOnDisable, conditions )
        {
        }

        protected override void InitializeElement( SerializedObject targetScriptableObject )
        {
            base.InitializeElement( targetScriptableObject );
            _serializedObject = targetScriptableObject;
            // Have to look up the index by the name. Do this as rarely as possible since it's expensive.
            SelectedIndex = AceTheme.GetIndexForCustomColorName( Property.stringValue );
            // Debug.Log( $"CustomColorPopup index initialized to {GetColoredStringYellow(SelectedIndex.ToString())} for color {GetColoredStringOrange(Property.stringValue)}." );
        }

        protected override void InitializeDraw() => _customColorEntryPopupElementDraw = new CustomColorEntryPopupElementDraw( this );

        public void UpdateSelectedColorByIndex( int index )
        {
            SelectedIndex = index;
            // Will this stick or will I need to do an immediate call to DataUpdateRequired?
            Property.stringValue = AceTheme.GetColorNameForIndex( SelectedIndex );
            _serializedObject.ApplyModifiedProperties();
            
        }
    }
}
