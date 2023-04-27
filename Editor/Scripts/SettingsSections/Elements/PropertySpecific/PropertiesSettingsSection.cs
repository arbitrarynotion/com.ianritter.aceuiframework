using Packages.com.ianritter.aceuiframework.Editor.Scripts.ACECore;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsGlobal.PropertySpecific;
using static Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.ElementBuilding.AceElementBuilder;

namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.SettingsSections.Elements.PropertySpecific
{
    public class PropertiesSettingsSection : SettingsSection
    {
        protected override string GetRelativePathVarName( string varName )
        {
            return AceTheme.GetPropertySpecificSettingsVarName  + "." + varName;
        }
    
        public override Element GetSection()
        {
            return GetGroupWithFoldoutHeading( nameof( AceTheme.propertySpecificSectionToggle ),
                "Property Elements", "Settings specific to property elements.", null,


                GetGroupWithFoldoutHeading( null, "Field widths for Column Properties", 
                    "The following settings control how much space is allocated to the two parts of an element: label and field." +
                    "When the assigned min width exceeds the width available, the element will draw its field on a second line.", null,

                    GetElement( GetRelativePathVarName( nameof( PropertySpecificSettings.showWideModeBoxes ) ), "Wide Mode Visuals",  
                        "Highlight elements using custom wide mode. Note: only applied to elements sharing an element line. Green = label, Orange = label end, Yellow = field." +
                        "Note that red line at top marks default wide mode trigger width for non-custom wide mode elements (just for reference)." ),
                    GetGroupWithLabelHeading( "Labels", string.Empty, null,
                        GetElement( GetRelativePathVarName( nameof( PropertySpecificSettings.propertyChildLabelWidth ) ), "Child Label Width", 
                            "Width reserved for a property's label when it shares a line with other elements." ),
                        GetElement( GetRelativePathVarName( nameof( PropertySpecificSettings.propertyLabelEndPadding ) ), "Label End", 
                            "Space between the end of the label and its field(s), where applicable." )
                    ),
                
                    GetGroupWithLabelHeading( "Fields", string.Empty, null,
                        GetElement( GetRelativePathVarName( nameof( PropertySpecificSettings.propertyNumberFieldsMinWidth ) ), "Numbers",
                            "Minimum width of the field element of a property. Used to move field to second line when there's not enough space on a single line." ),
                        GetElement( GetRelativePathVarName( nameof( PropertySpecificSettings.propertySlidersMinWidth ) ), "Sliders", string.Empty ),
                        GetElement( GetRelativePathVarName( nameof( PropertySpecificSettings.propertyStringFieldsMinWidth ) ), "Strings", string.Empty ),
                        // ( GetRelativePathVarName( nameof( PropertySpecificSettings.propertyBoolsMinWidth ) ), "Bools", string.Empty ),
                        GetElement( GetRelativePathVarName( nameof( PropertySpecificSettings.propertyColorsMinWidth ) ), "PresetColors", string.Empty ),
                        GetElement( GetRelativePathVarName( nameof( PropertySpecificSettings.propertyAnimationCurvesMinWidth ) ), "Animation Curves", string.Empty ),
                        GetElement( GetRelativePathVarName( nameof( PropertySpecificSettings.propertyDropDownsMinWidth ) ), "Drop Downs", string.Empty ),
                        GetElement( GetRelativePathVarName( nameof( PropertySpecificSettings.propertySetsMinWidth ) ), "Sets", string.Empty ),
                        GetElement( GetRelativePathVarName( nameof( PropertySpecificSettings.minMaxSliderMinWidth ) ), "Slider Min Width",
                            "The minimum width of the min/max slider including its float fields. Used to move the " +
                            "slider and its fields to a second line when there is not enough space for it and its label on a single line." )
                    )
                ),
                
            
                GetGroupWithFoldoutHeading( null,
                    "Min/Max Sliders", "Settings specific to min/max slider property elements.", null,

                    GetElement( GetRelativePathVarName( nameof( PropertySpecificSettings.minMaxSliderFloatFieldWidth ) ), "Float Field Widths",
                    "Hard float field width for min/max slider. The slider will scale with window width but the float fields won't." ),
                    GetElement( GetRelativePathVarName( nameof( PropertySpecificSettings.minMaxSliderSeparation ) ), "Field/Slider Gap",
                    "Spacing between the min/max slider and its float fields. Left and right are both always equal." ),
                    GetElement( GetRelativePathVarName( nameof( PropertySpecificSettings.minMaxDecimalPlace ) ), "Float Decimal Limit", "Rounding for the float values." )
                )

            );
        }
    }
}
