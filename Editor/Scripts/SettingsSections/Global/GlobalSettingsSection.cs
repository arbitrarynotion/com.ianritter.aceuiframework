using ACEPackage.Editor.Scripts.ACECore;
using ACEPackage.Editor.Scripts.Elements;
using ACEPackage.Runtime.Scripts.SettingsGlobal.Global;
using static ACEPackage.Editor.Scripts.ElementBuilding.AceElementBuilder;

namespace ACEPackage.Editor.Scripts.SettingsSections.Global
{
    public class GlobalSettingsSection : SettingsSection
    {
        protected override string GetRelativePathVarName( string varName )
        {
            return AceTheme.GetGlobalSettingsVarName + "." + varName;
        }
        
        public override Element GetSection()
        {
            return GetGroupWithFoldoutHeading( nameof( AceTheme.globalSettingsSectionToggle ), "Global", "Group of elements with a heading.", null,
                GetElement( GetRelativePathVarName( nameof(GlobalSettings.leftIndentUnitAmount) ), "Indent Unit Amount", "Used for all indents. This is the amount for a single indent." ),
                GetElement( GetRelativePathVarName( nameof( GlobalSettings.elementVerticalPadding ) ), "Vertical Spacing", "Vertical space between each element." ),
                GetElement( GetRelativePathVarName( nameof( GlobalSettings.columnGap ) ), "Column Gap", "Gap between elements that share a line." ),
                
                // This is for debugging as width truncating can mask layout bugs.
                GetGroupWithLabelHeading( "Debugging", string.Empty, null,
                    GetElement( GetRelativePathVarName( nameof(GlobalSettings.widthTruncating) ), "Width Truncating",
                        "Feature that limits the width of an element to the width of a control rect. The" +
                        "truncating takes place to reduce width when a scrollbar is present."
                    ),
                    GetElement( GetRelativePathVarName( nameof( GlobalSettings.showGridLines ) ), "Grid Lines",
                            "Use grid lines to check element levels." ),
                    GetElement( GetRelativePathVarName( nameof( GlobalSettings.showMeasurementLines ) ), "Measurement Lines",
                        "Vertical lines starting from the default indent and extending out in intervals " +
                        "of 100 pts. Useful for verifying the length of drawn elements."
                    )
                )
            );
        }
    }
}
