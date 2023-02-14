using ACEPackage.Editor.Scripts.ACECore;
using ACEPackage.Editor.Scripts.Elements;
using ACEPackage.Runtime.Scripts.SettingsGlobal.Elements.Groups.BasicGroups;
using static ACEPackage.Editor.Scripts.ElementBuilding.AceElementBuilder;

namespace ACEPackage.Editor.Scripts.SettingsSections.Elements.Groups.BasicGroups
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
                GetGroupWithFoldoutHeading( nameof(AceTheme.basicGroupsSectionDrawAreaToggle), "Draw Area Padding", string.Empty, null,
                    
                    GetPositionSection( "Total Area", string.Empty, basicGroupVarNames, false )
                ),

                GetFramesSection( "Frame", string.Empty, frameSettings, frameVarNames ),
                
                GetGroupWithFoldoutHeading( basicGroupVarNames.ShowLayoutTools, $"Layout Tools ( LVL: {level.ToString()} )", string.Empty, null,

                    GetLayoutToolsSection( 
                        
                        GetLayoutToolsSubsection( "Heading Group", basicGroupVarNames )
                    )
                )
            };
        }
    }
}
