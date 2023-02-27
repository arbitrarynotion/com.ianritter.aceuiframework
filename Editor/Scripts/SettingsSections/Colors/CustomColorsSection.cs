using System;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.ACECore;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.ElementConditions;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.SingleElements.Decorator.Label;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.SingleElements.Properties.Basic;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.SingleElements.Properties.Popup;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsCustom;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsCustom.Groups;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsCustom.SingleElements;
using UnityEngine;
using static Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.ElementBuilding.AceElementBuilder;

namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.SettingsSections.Colors
{
    public class CustomColorsSection : SettingsSection
    {
        private string[] _customColorOptions;
        private readonly string _customColorSettingsVarName;
        
        public CustomColorsSection( AceTheme aceTheme, string customColorSettingsVarName )
        {
            AceTheme = aceTheme;
            _customColorSettingsVarName = customColorSettingsVarName;
        }

        protected override string GetRelativePathVarName( string varName ) => 
            _customColorSettingsVarName + "." + varName;
        
        public override Element GetSection()
        {
            return GetGroupWithFoldoutHeading( nameof( AceTheme.colorsSectionToggle ), "PresetColors", string.Empty,
                new GroupCustomSettings() {CustomFrameSettings = NoFrame},

                // Custom colors array. Note that the Custom Color class has a property drawer applied to it
                // which determines how each element in the array is drawn.
                new BasicProperty( GetRelativePathVarName( AceTheme.GetCustomColorListVarName ), new GUIContent( "Custom PresetColors" ), new SingleCustomSettings(), OnColorsChanged )
            );
        }

        public Element GetColorSelectionElement( 
            string title, 
            string tooltip, 
            int selectedIndex, 
            string selectedIndexRelativeVarName, 
            Action callback,
            params ElementCondition[] filter )
        {
            _customColorOptions = AceTheme.GetCustomColorOptions();
            
            string selectedColorVarName = 
                $"{GetRelativePathVarName( AceTheme.GetCustomColorListVarName )}.Array.data[{selectedIndex.ToString()}].color";

            Element popupField = new PopupElement( selectedIndexRelativeVarName, GUIContent.none, _customColorOptions, new SingleCustomSettings(), callback, false, filter );
            Element colorField = new BasicProperty( selectedColorVarName, GUIContent.none, new SingleCustomSettings() {ConstantWidth = 60f}, callback, false, filter);

            return GetGroup( new GroupCustomSettings() {CustomFrameSettings = new CustomFrameSettings() {applyFraming = false}, NumberOfColumns = 3}, 
                new LabelElement( new GUIContent( title, tooltip ), false, new SingleCustomSettings() {CustomFrameSettings = NoFrame, UseIndentedDefaultLabelWidth = true} ), 
                popupField, 
                colorField );
        }
        
        private void OnColorsChanged() => DataUpdatedNotify();
    }
}
