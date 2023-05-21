using Packages.com.ianritter.aceuiframework.Editor.Scripts.ACECore;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.SingleElements.Properties.Basic;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsCustom.SingleElements;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsGlobal.Elements.SingleElements;
using UnityEngine;
using static Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.ElementBuilding.AceElementBuilder;

namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.SettingsSections.Elements.SingleElements
{
    public class SingleElementsSettingsSection : LevelSettingsSection
    {
        private SingleElementVarNames[] _singleElementVarNames;
        private SingleElementFrameVarNames[] _singleElementFrameVarNames;


        public SingleElementsSettingsSection( AceTheme aceTheme )
        {
            AceTheme = aceTheme;
            LevelSettingsModeRelativeVarName = nameof( AceTheme.singleElementsLevelSettingsMode );
            Initialize();
        }


        protected override LevelSettingsMode GetLevelSettingsMode() => AceTheme.singleElementsLevelSettingsMode;
        
        protected override void InitializeSectionVarNamesList()
        {
            _singleElementVarNames = new SingleElementVarNames[AceTheme.Levels];
            for (int i = 0; i < AceTheme.Levels; i++)
            {
                _singleElementVarNames[i] = new SingleElementVarNames( i );
            }
            
            _singleElementFrameVarNames = new SingleElementFrameVarNames[AceTheme.Levels];
            for (int i = 0; i < AceTheme.Levels; i++)
            {
                _singleElementFrameVarNames[i] = new SingleElementFrameVarNames( i );
            }
        }
        
        protected override string GetRelativePathVarName( string varName ) => AceTheme.GetSingleElementSettingsSectionVarName + "." + varName;

        public override Element GetSection() =>
            GetLevelSettingsSectionWithTabs( nameof( AceTheme.singleElementsSectionToggle ), 
                "Single Elements", "All non-group elements.", null );

        protected override Element[] GetElementForLevel( int level )
        {
            SingleElementVarNames varNames = _singleElementVarNames[level];
            SingleElementFrameVarNames frameVarNames = _singleElementFrameVarNames[level];
            SingleElementFrameSettings frameSettings = AceTheme.GetSingleElementFrameSettingsForLevel( InfoSelectionIndex );

            return new []
            {
                GetGroupWithFoldoutHeading( nameof(AceTheme.singleElementsSectionDrawAreaToggle), "Draw Area", string.Empty, null,
                    GetDrawAreaPaddingSection( "Total Area", string.Empty, varNames, false )
                ),
                
                // GetFramesSection( "Frames", string.Empty, frameSettings, frameVarNames, false, 
                //     new BasicProperty( frameVarNames.FramesSkipSingleLine, new GUIContent( "Skip Single Line" ), new SingleCustomSettings(), null )
                // ),
                
                GetFrameSection( frameSettings, frameVarNames ),

                GetGroupWithToggleHeading( varNames.ShowLayoutVisualizations, $"Layout Visualizations ( LVL: {level.ToString()} )", string.Empty, null,
                    
                    // GetElement( ( varNames.HideElements), "Hide Elements", 
                    //     string.Empty, new SingleCustomSettings() {ForceSingleLine = true} ),
                    
                    true,
                    GetLayoutVisualizationSection( 
                        GetLayoutVisualizationSubsection( "Single Element", varNames )
                    )
                )
            };
        }
        
        private Element GetFrameSection( SingleElementFrameSettings frameSettings, SingleElementFrameVarNames frameVarNames )
        {
            return GetGroupWithLabelHeading( "Frame", string.Empty, null,
                GetElement( frameVarNames.FramesSkipSingleLine, "Skip Single Line" ),
                GetGroupWithToggleHeading( frameVarNames.IncludeBackground, "Background", string.Empty, null,
                    true,
                    
                    AceTheme.GetColorSelectionElement( "Active Color", string.Empty,
                        frameSettings.backgroundColorName,
                        frameVarNames.BackgroundColorName, OnColorSelectionChanged,
                        GetMustHaveBackgroundFilter( frameVarNames.IncludeBackground ) )
                    
                    // AceTheme.GetColorSelectionElement( "Active Color", string.Empty,
                    //     frameSettings.backgroundColorIndex,
                    //     frameVarNames.BackgroundColorIndex, OnColorSelectionChanged,
                    //     GetMustHaveBackgroundFilter( frameVarNames.IncludeBackground ) )
                ),

                GetGroupWithToggleHeading( frameVarNames.IncludeOutline, "Outline", string.Empty, null,
                    true,
                    GetElement( frameVarNames.FrameType, "Style", string.Empty ),
                    GetElement( frameVarNames.FramePadding, "Frame Padding", string.Empty, null, 
                        false, GetMustHaveOutlineFilter( frameVarNames.FrameType ) ),
                    GetElement( frameVarNames.FrameOutlineThickness, "Line Thickness", string.Empty, null,
                        false, GetMustHaveOutlineFilter( frameVarNames.FrameType ) ),
                    
                    // Link colors only to their name, independent of that color's index number.
                    AceTheme.GetColorSelectionElement( "Color", string.Empty,
                        frameSettings.frameOutlineColorName,
                        frameVarNames.FrameOutlineColorName, OnColorSelectionChanged,
                        GetMustHaveOutlineFilter( frameVarNames.FrameType ) )
                    
                    // Link colors to their respective index number.
                    // AceTheme.GetColorSelectionElement( "Color", string.Empty,
                    //     frameSettings.frameOutlineColorIndex,
                    //     frameVarNames.FrameOutlineColorIndex, OnColorSelectionChanged,
                    //     GetMustHaveOutlineFilter( frameVarNames.FrameType ) )
                )
            );
        }
    }
}
