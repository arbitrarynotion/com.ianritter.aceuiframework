using Packages.com.ianritter.aceuiframework.Editor.Scripts.ACECore;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsGlobal.Global;
using static Packages.com.ianritter.aceuiframework.Editor.Scripts.ElementBuilding.AceElementBuilder;

namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.SettingsSections.Global
{
    public class GlobalSettingsSection : SettingsSection
    {
        protected override string GetRelativePathVarName( string varName )
        {
            return AceTheme.GetGlobalSettingsVarName + "." + varName;
        }
        
        public override Element GetSection()
        {
            return GetGroupWithFoldoutHeading( nameof( AceTheme.globalSettingsSectionToggle ), 
                "Global", 
                "Global settings are applied to all non-root UI elements. A root UI element is one that is not grouped with any other element.", 
                null,
                GetElement( GetRelativePathVarName( nameof(GlobalSettings.leftIndentUnitAmount) ), "Indent Unit Amount", "Used for all indents. This is the amount for a single indent." ),
                GetElement( GetRelativePathVarName( nameof( GlobalSettings.elementVerticalPadding ) ), "Vertical Spacing", "Vertical space between each element." ),
                GetElement( GetRelativePathVarName( nameof( GlobalSettings.columnGap ) ), "Column Gap", "Gap between elements that share a line." ),
                
                // Todo: This debug options section will probably get removed later if it doesn't prove to be useful from a user's perspective.
                GetGroupWithFoldoutHeading( GetRelativePathVarName( nameof(GlobalSettings.debugOptionsToggle) ), 
                    "Debugging", string.Empty, null,
                    // This is for debugging as width truncating can mask layout bugs.
                    GetElement( GetRelativePathVarName( nameof(GlobalSettings.widthTruncating) ), "Width Truncating",
                        "Feature that limits the width of an element to the width of a control rect. The" +
                        "truncating takes place to reduce width when a scrollbar is present."
                    ),
                    // Grid lines help visually verify that positioning is correct.
                    GetElement( GetRelativePathVarName( nameof( GlobalSettings.showGridLines ) ), "Grid Lines",
                            "Use grid lines to check element levels." ),
                    // Measurement lines provide a ruler of sorts to help compare the width of elements.
                    GetElement( GetRelativePathVarName( nameof( GlobalSettings.showMeasurementLines ) ), "Measurement Lines",
                        "Vertical lines starting from the default indent and extending out in intervals " +
                        "of 100 pts. Useful for verifying the width of drawn elements."
                    )
                )
            );
        }
    }
}
