using Packages.com.ianritter.aceuiframework.Editor.Scripts.ACECore;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsCustom.SingleElements;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsGlobal.Elements.Groups.BasicGroups;
using static Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.ElementBuilding.AceElementBuilder;

namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.SettingsSections.Elements.Groups.BasicGroups
{
    public class BasicGroupSettingsSection : LevelSettingsSection
    {
        private BasicGroupVarNames[] _basicGroupVarNames;
        private BasicGroupFrameVarNames[] _basicGroupFrameVarNames;

        
        public BasicGroupSettingsSection( AceTheme aceTheme )
        {
            AceTheme = aceTheme;
            LevelSettingsModeRelativeVarName = nameof( AceTheme.basicGroupLevelSettingsMode );
            Initialize();
        }

        
        protected override LevelSettingsMode GetLevelSettingsMode() => AceTheme.basicGroupLevelSettingsMode;
        
        protected override void InitializeSectionVarNamesList()
        {
            _basicGroupVarNames = new BasicGroupVarNames[Levels];
            for (int i = 0; i < Levels; i++)
            {
                _basicGroupVarNames[i] = new BasicGroupVarNames( i );
            }
            
            _basicGroupFrameVarNames = new BasicGroupFrameVarNames[Levels];
            for (int i = 0; i < Levels; i++)
            {
                _basicGroupFrameVarNames[i] = new BasicGroupFrameVarNames( i );
            }
        }

        protected override string GetRelativePathVarName( string varName ) => AceTheme.GetBasicGroupSettingsListVarName + "." + varName;
        
        public override Element GetSection() => GetLevelSettingsSectionWithTabs();
        
        protected override Element[] GetElementForLevel( int level )
        {
            BasicGroupVarNames basicGroupVarNames = _basicGroupVarNames[level];
            BasicGroupFrameVarNames frameVarNames = _basicGroupFrameVarNames[level];

            BasicGroupFrameSettings frameSettings = AceTheme.GetBasicGroupFrameSettingsForLevel( level );
            
            return new Element[]{
                GetGroupWithFoldoutHeading( nameof(AceTheme.basicGroupsSectionDrawAreaToggle), "Draw Area Padding", string.Empty, null, OnColorRelatedBoolToggled,
                    
                    GetDrawAreaPaddingSection( "Total Area", string.Empty, basicGroupVarNames, false )
                ),

                // GetFramesSection( "Frame", string.Empty, frameSettings, frameVarNames ),
                GetGroupFrameSection( frameSettings, frameVarNames ),
                
                GetGroupWithToggleHeading( basicGroupVarNames.ShowLayoutVisualizations, $"Layout Visualizations ( LVL: {level.ToString()} )", string.Empty, null,

                    true,
                    GetLayoutVisualizationSection( 
                        
                        GetLayoutVisualizationSubsection( "Heading Group", basicGroupVarNames )
                    )
                )
            };
        }
        
        private Element GetGroupFrameSection( BasicGroupFrameSettings frameSettings, BasicGroupFrameVarNames frameVarNames )
        {
            return GetGroupWithLabelHeading( "Group Frame", string.Empty, null,

                GetGroupWithToggleHeading( frameVarNames.IncludeBackground, "Background", string.Empty, null,
                    OnColorRelatedBoolToggled,
                    true,
                    
                    AceTheme.GetColorSelectionElement( "Color", string.Empty,
                        frameSettings.backgroundActiveColorName,
                        frameVarNames.BackgroundColorName, 
                        OnColorSelectionChanged,
                        GetMustHaveBackgroundFilter( frameVarNames.IncludeBackground ) )
                    
                    // AceTheme.GetColorSelectionElement( "Active Color", string.Empty,
                    //     frameSettings.backgroundColorIndex,
                    //     frameVarNames.BackgroundColorIndex, 
                    //     OnColorSelectionChanged,
                    //     GetMustHaveBackgroundFilter( frameVarNames.IncludeBackground ) )
                ),

                GetGroupWithToggleHeading( frameVarNames.IncludeOutline, "Outline", string.Empty, null,
                    OnColorRelatedBoolToggled,
                    true,
                    GetElement( frameVarNames.FrameType, "Style", string.Empty ),
                    GetElement( frameVarNames.FramePadding, "Frame Padding", string.Empty, null, 
                        false, GetMustHaveOutlineFilter( frameVarNames.FrameType ) ),
                    GetElement( frameVarNames.FrameOutlineThickness, "Line Thickness", string.Empty, null,
                        false, GetMustHaveOutlineFilter( frameVarNames.FrameType ) ),
                    
                    AceTheme.GetColorSelectionElement( "Color", string.Empty,
                        frameSettings.frameOutlineColorName,
                        frameVarNames.FrameOutlineColorName, 
                        OnColorSelectionChanged,
                        GetMustHaveOutlineFilter( frameVarNames.FrameType ) )
                    
                    // AceTheme.GetColorSelectionElement( "Color", string.Empty,
                    //     frameSettings.frameOutlineColorIndex,
                    //     frameVarNames.FrameOutlineColorIndex, 
                    //     OnColorSelectionChanged,
                    //     GetMustHaveOutlineFilter( frameVarNames.FrameType ) )
                )
            );
        }
    }
}
