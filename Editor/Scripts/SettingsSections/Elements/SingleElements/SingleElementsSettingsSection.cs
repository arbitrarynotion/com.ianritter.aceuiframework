using Packages.com.ianritter.aceuiframework.Editor.Scripts.ACECore;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.SingleElements.Properties.Basic;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsCustom.SingleElements;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsGlobal.Elements.SingleElements;
using UnityEngine;
using static Packages.com.ianritter.aceuiframework.Editor.Scripts.ElementBuilding.AceElementBuilder;

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
                    GetPositionSection( "Total Area", string.Empty, varNames, false )
                ),
                
                GetFramesSection( "Frames", string.Empty, frameSettings, frameVarNames, false, 
                    new BasicProperty( frameVarNames.FramesSkipSingleLine, new GUIContent( "Skip Single Line" ), new SingleCustomSettings(), null )
                ),

                GetGroupWithToggleHeading( varNames.ShowLayoutTools, $"Layout Tools ( LVL: {level.ToString()} )", string.Empty, null,
                    
                    // GetElement( ( varNames.HideElements), "Hide Elements", 
                    //     string.Empty, new SingleCustomSettings() {ForceSingleLine = true} ),
                    
                    true,
                    GetLayoutToolsSection( 
                        GetLayoutToolsSubsection( "Single Element", varNames )
                    )
                )
            };
        }
    }
}
