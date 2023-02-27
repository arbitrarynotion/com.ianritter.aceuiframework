using System;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.ACECore;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.GroupElements.BasicGroup;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.Enums;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsCustom.Groups;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsGlobal;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsGlobal.Elements.Groups.ChildAreaGroups;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsGlobal.Elements.Groups.HeadingGroups;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsGlobal.Elements.SingleElements.Decorator;
using static Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.ElementBuilding.AceElementBuilder;

namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.SettingsSections.Elements.Groups.HeadingGroups
{
    public class HeadingGroupSettingsSection : LevelSettingsSection
    {
        private HeadingGroupVarNames[] _headingGroupVarNames;
        private ChildAreaGroupVarNames[] _childAreaGroupVarNames;
        private HeadingElementFrameSettingsVarNames[] _headingElementVarNames;
        private HeadingGroupFrameVarNames[] _foldoutGroupVarNames;
        private HeadingGroupFrameVarNames[] _toggleGroupVarNames;
        private HeadingGroupFrameVarNames[] _labeledGroupVarNames;
        
        
        public HeadingGroupSettingsSection( AceTheme aceTheme, string myRelativeVarName )
        {
            AceTheme = aceTheme;
            MyRelativeVarName = myRelativeVarName;
            LevelSettingsModeRelativeVarName = nameof( AceTheme.headingGroupLevelSettingsMode );
            Initialize();
        }
        
        
        protected override LevelSettingsMode GetLevelSettingsMode() => AceTheme.headingGroupLevelSettingsMode;

        protected override void InitializeSectionVarNamesList()
        {
            _headingGroupVarNames = new HeadingGroupVarNames[Levels];
            _childAreaGroupVarNames = new ChildAreaGroupVarNames[Levels];
            _headingElementVarNames = new HeadingElementFrameSettingsVarNames[Levels];
            _foldoutGroupVarNames = new HeadingGroupFrameVarNames[Levels];
            _toggleGroupVarNames = new HeadingGroupFrameVarNames[Levels];
            _labeledGroupVarNames = new HeadingGroupFrameVarNames[Levels];
            for (int i = 0; i < Levels; i++)
            {
                _headingGroupVarNames[i] = new HeadingGroupVarNames( AceTheme.GetHeadingGroupSettingsListVarName, i );
                _childAreaGroupVarNames[i] = new ChildAreaGroupVarNames( i );
                _headingElementVarNames[i] = new HeadingElementFrameSettingsVarNames( i );
                _foldoutGroupVarNames[i] = new HeadingGroupFrameVarNames( AceTheme.GetFoldoutGroupSettingsListVarName, i );
                _toggleGroupVarNames[i] = new HeadingGroupFrameVarNames( AceTheme.GetToggleGroupSettingsListVarName, i );
                _labeledGroupVarNames[i] = new HeadingGroupFrameVarNames( AceTheme.GetLabeledGroupInfoListVarName, i );
            }
        }
        
        protected override string GetRelativePathVarName( string varName ) =>
            AceTheme.GetGroupSettingsSectionVarName + "." + 
            MyRelativeVarName + "." + varName;

        public override Element GetSection() => GetLevelSettingsSectionWithTabs();
        
        protected override Element[] GetElementForLevel( int level )
        {
            HeadingGroupVarNames headingGroupVarNames = _headingGroupVarNames[level];
            ChildAreaGroupVarNames childAreaGroupVarNames = _childAreaGroupVarNames[level];
            HeadingElementFrameSettingsVarNames headingElementFrameVarNames = _headingElementVarNames[level];
            
            
            Element activeSection = AceTheme.headingGroupSectionState switch
            {
                HeadingGroupSectionState.FoldoutGroups => GetHeadingGroupSection( _foldoutGroupVarNames[level], headingElementFrameVarNames ),
                HeadingGroupSectionState.ToggleGroups => GetHeadingGroupSection( _toggleGroupVarNames[level], headingElementFrameVarNames ),
                HeadingGroupSectionState.LabeledGroups => GetHeadingGroupSection( _labeledGroupVarNames[level], headingElementFrameVarNames ),
                _ => throw new ArgumentOutOfRangeException()
            };

            Element headingsSection = AceTheme.GetHeadingGroupSettingsForLevel( level ).useSeparateHeadingSettings
                ? GetTabbedOptionsSection( activeSection,
                    ( "Foldout", string.Empty, AceTheme.headingGroupSectionState == HeadingGroupSectionState.FoldoutGroups, OnFoldoutButtonPressed ),
                    ( "Toggle", string.Empty, AceTheme.headingGroupSectionState == HeadingGroupSectionState.ToggleGroups, OnToggleButtonPressed ),
                    ( "Labeled", string.Empty, AceTheme.headingGroupSectionState == HeadingGroupSectionState.LabeledGroups, OnLabeledButtonPressed )
                )
                : new BasicGroup( new GroupCustomSettings() { CustomFrameSettings = NoFrame },
                    new [] { activeSection } 
                );

            
            return new Element[]{
                GetGroupWithFoldoutHeading( nameof( AceTheme.headingGroupsSectionDrawAreaToggle ), "Draw Area Padding", string.Empty, 
                    null,
                    
                    GetPositionSection( "Total Area", string.Empty, headingGroupVarNames, false ),
                    GetPositionSection( "Child Area", "Adjust the whole area used to draw the child elements of the group.", childAreaGroupVarNames, false )
                ),
                
                // Todo: Disabled the option to change frame settings by heading type for now as it may be a bit to fine-grained of an option.
                // GetGroup( new GroupCustomSettings() {numberOfColumns = 2},
                //     new LabelElement( string.Empty, string.Empty, new SingleCustomSettings() {UseIndentedDefaultLabelWidth = true, CustomFrameSettings = NoFrame} ),
                //     new BasicProperty( headingGroupVarNames.UseSeparateHeadingSettings, 
                //         "Use Separate Heading Settings", "Will use foldout settings as a default.", OnSeparateHeadingsToggleChanged,
                //         new SingleCustomSettings() { LabelAlignment = Alignment.Right, CustomFrameSettings = NoFrame})
                // ), 

                headingsSection,
                
                GetGroupWithToggleHeading( headingGroupVarNames.ShowLayoutVisualizations, 
                    $"Layout Visualizations ( LVL: {level.ToString()} )", 
                    "Show the outlines of the position boxes used to place each element. " +
                    "Use to see exactly what combination of padding is being applied to elements.", 
                    null,
                    // GetElement( headingGroupVarNames.HideFoldoutGroupElements, "Hide Heading", string.Empty, new SingleCustomSettings() {ForceSingleLine = true} ),
                    
                    true,
                    GetLayoutVisualizationSection( 
                        GetLayoutVisualizationSubsection( "Heading Group", headingGroupVarNames ),
                        GetLayoutVisualizationSubsection( "HG Child Area", childAreaGroupVarNames, true )
                    )
                )
            };
        }
        
        private Element GetHeadingGroupSection( ElementFrameVarNames frameVarNames, HeadingElementFrameSettingsVarNames headingFrameVarNames )
        {
            FrameSettings frameSettings = AceTheme.GetCurrentModeHeadingGroupFrameSettings( InfoSelectionIndex );
            HeadingElementFrameSettings headingFrameSettings = AceTheme.GetHeadingFrameSettingsForLevel( InfoSelectionIndex );
            HeadingElementFrameSettingsVarNames headingElementFrameVarNames = _headingElementVarNames[InfoSelectionIndex];

            return GetGroupWithFoldoutHeading( null, "Frames", string.Empty, null,
                GetGroupWithFoldoutHeading( null, "Heading", string.Empty, null,
                    GetElement( headingFrameVarNames.BoxHeight, "Height", string.Empty ),
                    GetElement( headingFrameVarNames.TextHorizontalOffset, "Text Offset", string.Empty ),
                    GetTextColorsSection( headingFrameSettings, headingElementFrameVarNames ),
                    GetFramesSection( "Heading Frame", string.Empty, headingFrameSettings, headingFrameVarNames, true )
                ),
                GetFramesSection( "Frame", string.Empty, frameSettings, frameVarNames, true )
            );
        }
        
        private void OnSeparateHeadingsToggleChanged() => UIStateUpdatedNotify();

        private void OnFoldoutButtonPressed()
        {
            AceTheme.headingGroupSectionState = HeadingGroupSectionState.FoldoutGroups;
            DataUpdatedNotify();
        }

        private void OnToggleButtonPressed()
        {
            AceTheme.headingGroupSectionState = HeadingGroupSectionState.ToggleGroups;
            DataUpdatedNotify();
        }

        private void OnLabeledButtonPressed()
        {
            AceTheme.headingGroupSectionState = HeadingGroupSectionState.LabeledGroups;
            DataUpdatedNotify();
        }
    }
}
