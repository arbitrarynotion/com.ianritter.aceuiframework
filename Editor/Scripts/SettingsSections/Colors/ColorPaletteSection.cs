using System;
using UnityEngine;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.ACECore;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.ElementConditions;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.SingleElements.Decorator.Label;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.SingleElements.Properties.Basic;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.SingleElements.Properties.CustomColor;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.SingleElements.Properties.Popup.CustomColorEntryPopup;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsCustom;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsCustom.Groups;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsCustom.SingleElements;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsGlobal.Colors;
using static Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.ElementBuilding.AceElementBuilder;
using static Packages.com.ianritter.unityscriptingtools.Runtime.Services.TextFormatting.TextFormat;

namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.SettingsSections.Colors
{
    public class ColorPaletteSection : SettingsSection
    {
        private string[] _customColorOptions;
        private readonly string _customColorSettingsVarName;
        private readonly ColorPaletteSettings _colorPaletteSettings;
        
        
        public ColorPaletteSection( AceTheme aceTheme, ColorPaletteSettings colorPaletteSettings, string customColorSettingsVarName )
        {
            AceTheme = aceTheme;
            _colorPaletteSettings = colorPaletteSettings;
            _customColorSettingsVarName = customColorSettingsVarName;
        }

        
        protected override string GetRelativePathVarName( string varName ) => _customColorSettingsVarName + "." + varName;
        
        public override Element GetSection()
        {
            return GetGroupWithFoldoutHeading( nameof( AceTheme.colorsSectionToggle ), "Color Palette", string.Empty,
                new GroupCustomSettings() {CustomFrameSettings = NoFrame},

                // Note that the ColorEntry
                new BasicProperty( GetRelativePathVarName( AceTheme.GetCustomColorListVarName ), new GUIContent( "Color Palette" ), new SingleCustomSettings(), OnColorsChanged )
            );
        }

        public Element GetColorSelectionElement( 
            string title, 
            string tooltip, 
            string colorName, 
            string selectedIndexRelativeVarName, 
            Action callback,
            params ElementCondition[] filter )
        {
            int selectedIndex = _colorPaletteSettings.GetIndexForColorName( colorName );
            string selectedColorVarName = $"{GetRelativePathVarName( AceTheme.GetCustomColorListVarName )}.Array.data[{selectedIndex.ToString()}].color";
            if ( selectedIndex == -1 )
            {
                // Debug.LogWarning( $"Warning! Could not find custom color {GetColoredStringYellow(colorName)}" );
                selectedColorVarName = $"{GetRelativePathVarName( AceTheme.GetBackupColorVarName )}.color";
            }
            CustomColorEntry customColorEntry = _colorPaletteSettings.GetColorEntryForIndex( selectedIndex );

            _customColorOptions = AceTheme.GetColorEntryOptions();
            
            Element popupField = new CustomColorEntryPopupElement( selectedIndexRelativeVarName, GUIContent.none, _customColorOptions, new SingleCustomSettings(), callback, false, filter );
            
            // If the color is locked, draw a disabled color field. Otherwise, draw the color field with a color picker button.
            Element colorField = customColorEntry.locked 
                ? (Element) new BasicProperty( selectedColorVarName, GUIContent.none, new SingleCustomSettings() {ConstantWidth = 80f, ForceDisable =  true}, callback, false, filter)
                : new ColorPickerElement( selectedColorVarName, GUIContent.none, new SingleCustomSettings() { ConstantWidth = 80f }, callback, false, filter );

            return GetCompositeGroup( 
                new GroupCustomSettings() {CustomFrameSettings = new CustomFrameSettings() {applyFraming = false}, NumberOfColumns = 3},
                new LabelElement( 
                    new GUIContent( title, tooltip ), 
                    new SingleCustomSettings() {CustomFrameSettings = NoFrame, UseIndentedDefaultLabelWidth = true} 
                ), 
                popupField, 
                colorField 
            );
        }

        private void OnColorsChanged() => DataUpdatedNotify();
    }
}
