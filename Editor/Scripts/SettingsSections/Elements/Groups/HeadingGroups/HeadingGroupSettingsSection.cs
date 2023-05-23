using System;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.ACECore;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.GroupElements.BasicGroup;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.GroupElements.CompositeGroup;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.SingleElements.Decorator.Label;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.SingleElements.Properties.Basic;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.Enums;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsCustom.Groups;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsCustom.SingleElements;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsGlobal;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsGlobal.Elements.Groups.ChildAreaGroups;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsGlobal.Elements.Groups.HeadingGroups;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsGlobal.Elements.SingleElements.Decorator;
using UnityEngine;
using static Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.ElementBuilding.AceElementBuilder;

namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.SettingsSections.Elements.Groups.HeadingGroups
{
    public class HeadingGroupSettingsSection : LevelSettingsSection
    {
        private HeadingGroupVarNames[] _headingGroupVarNames;
        private ChildAreaGroupVarNames[] _childAreaGroupVarNames;
        private HeadingElementFrameSettingsVarNames[] _headingElementFrameVarNames;
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
            _headingElementFrameVarNames = new HeadingElementFrameSettingsVarNames[Levels];
            _foldoutGroupVarNames = new HeadingGroupFrameVarNames[Levels];
            _toggleGroupVarNames = new HeadingGroupFrameVarNames[Levels];
            _labeledGroupVarNames = new HeadingGroupFrameVarNames[Levels];
            for (int i = 0; i < Levels; i++)
            {
                _headingGroupVarNames[i] = new HeadingGroupVarNames( AceTheme.GetHeadingGroupSettingsListVarName, i );
                _childAreaGroupVarNames[i] = new ChildAreaGroupVarNames( i );
                _headingElementFrameVarNames[i] = new HeadingElementFrameSettingsVarNames( i );
                _foldoutGroupVarNames[i] = new HeadingGroupFrameVarNames( AceTheme.GetFoldoutGroupSettingsListVarName, i );
                // _toggleGroupVarNames[i] = new HeadingGroupFrameVarNames( AceTheme.GetToggleGroupSettingsListVarName, i );
                // _labeledGroupVarNames[i] = new HeadingGroupFrameVarNames( AceTheme.GetLabeledGroupInfoListVarName, i );
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
            HeadingElementFrameSettingsVarNames headingElementFrameVarNames = _headingElementFrameVarNames[level];
            HeadingElementFrameSettings headingElementFrameSettings = AceTheme.GetHeadingFrameSettingsForLevel( InfoSelectionIndex );
            
            FrameSettings frameSettings = AceTheme.GetCurrentModeHeadingGroupFrameSettings( InfoSelectionIndex );
            HeadingGroupFrameVarNames frameVarNames = _foldoutGroupVarNames[level];

            
            // Heading type-specific settings are disabled for now.
            // Element activeSection = AceTheme.headingGroupSectionState switch
            // {
            //     HeadingGroupSectionState.FoldoutGroups => GetHeadingGroupSection( _foldoutGroupVarNames[level], headingElementFrameVarNames ),
            //     HeadingGroupSectionState.ToggleGroups => GetHeadingGroupSection( _toggleGroupVarNames[level], headingElementFrameVarNames ),
            //     HeadingGroupSectionState.LabeledGroups => GetHeadingGroupSection( _labeledGroupVarNames[level], headingElementFrameVarNames ),
            //     _ => throw new ArgumentOutOfRangeException()
            // };
            // Element headingsSection = AceTheme.GetHeadingGroupSettingsForLevel( level ).useSeparateHeadingSettings
            //     ? GetTabbedOptionsSection( activeSection,
            //         ( "Foldout", string.Empty, AceTheme.headingGroupSectionState == HeadingGroupSectionState.FoldoutGroups, OnFoldoutButtonPressed ),
            //         ( "Toggle", string.Empty, AceTheme.headingGroupSectionState == HeadingGroupSectionState.ToggleGroups, OnToggleButtonPressed ),
            //         ( "Labeled", string.Empty, AceTheme.headingGroupSectionState == HeadingGroupSectionState.LabeledGroups, OnLabeledButtonPressed )
            //     )
            //     : new CompositeGroup( new GroupCustomSettings() { CustomFrameSettings = NoFrame },
            //         new [] { activeSection } 
            //     );

            return new Element[]{
                GetGroupWithFoldoutHeading( nameof( AceTheme.headingGroupsSectionDrawAreaToggle ), "Draw Area Padding", string.Empty, 
                    null,
                    GetDrawAreaPaddingSection( "Total Area", string.Empty, headingGroupVarNames, false ),
                    GetDrawAreaPaddingSection( "Child Area", "Adjust the whole area used to draw the child elements of the group.", childAreaGroupVarNames, false )
                ),
                
                GetHeadingTextSection( headingElementFrameSettings, headingElementFrameVarNames ),
                
                // // Todo: Disabled the option to change frame settings by heading type for now as it may be a bit to fine-grained of an option.
                // GetCompositeGroup( new GroupCustomSettings() {NumberOfColumns = 2},
                //     new LabelElement( new GUIContent(""), new SingleCustomSettings() {UseIndentedDefaultLabelWidth = true, CustomFrameSettings = NoFrame} ),
                //     new BasicProperty( headingGroupVarNames.UseSeparateHeadingSettings, 
                //         new GUIContent( "Use Separate Heading Settings", "Will use foldout settings as a default." ),
                //         new SingleCustomSettings() { LabelAlignment = Alignment.Right, LabelEndPadding = 13f, CustomFrameSettings = NoFrame}, OnSeparateHeadingsToggleChanged)
                // ), 

                // headingsSection,

                GetGroupWithFoldoutHeading( null, "Frames", string.Empty, new GroupCustomSettings() {},
                    GetHeadingFrameSection( headingElementFrameSettings, headingElementFrameVarNames ),
                    GetGroupFrameSection( frameSettings, frameVarNames )

                    // GetFramesSection( "Heading Frame", string.Empty, headingElementFrameSettings, headingElementFrameVarNames, true ),
                    // GetFramesSection( "Group Frame", string.Empty, frameSettings, frameVarNames, true )
                ),
                
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

        private Element GetHeadingTextSection( 
            HeadingElementFrameSettings headingElementFrameSettings, 
            HeadingElementFrameSettingsVarNames headingElementFrameVarNames )
        {
            return GetGroupWithFoldoutHeading( null, "Heading Text", string.Empty, null,
                GetElement( headingElementFrameVarNames.TextHorizontalOffset, "Text Offset", string.Empty ),

                GetGroupWithLabelHeading( "Text Colors", string.Empty, null,
                    
                    AceTheme.GetColorSelectionElement( "Enabled", string.Empty,
                        headingElementFrameSettings.enabledTextColorName,
                        headingElementFrameVarNames.EnabledTextColorName, OnColorSelectionChanged,
                        GetMustHaveOutlineFilter( headingElementFrameVarNames.FrameType ) ),
                    
                    // AceTheme.GetColorSelectionElement( "Enabled", string.Empty,
                    //     headingElementFrameSettings.enabledTextColorIndex,
                    //     headingElementFrameVarNames.EnabledTextColorNameIndex, OnColorSelectionChanged,
                    //     GetMustHaveOutlineFilter( headingElementFrameVarNames.FrameType ) ),
                    
                    AceTheme.GetColorSelectionElement( "Disabled", string.Empty,
                        headingElementFrameSettings.disabledTextColorName,
                        headingElementFrameVarNames.DisabledTextColorName, OnColorSelectionChanged )
                    
                    // AceTheme.GetColorSelectionElement( "Disabled", string.Empty,
                    //     headingElementFrameSettings.disabledTextColorIndex,
                    //     headingElementFrameVarNames.DisabledTextColorNameIndex, OnColorSelectionChanged )
                )
            );
        }

        private Element GetGroupFrameSection( FrameSettings frameSettings, HeadingGroupFrameVarNames frameVarNames )
        {
            return GetGroupWithLabelHeading( "Group Frame", string.Empty, null,

                GetGroupWithToggleHeading( frameVarNames.IncludeBackground, "Background", string.Empty, null,
                    true,
                    
                    AceTheme.GetColorSelectionElement( "Active Color", string.Empty,
                        frameSettings.backgroundActiveColorName,
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
                    GetElement( frameVarNames.FrameOutlineThickness, "Line Thickness", string.Empty, null,
                        false, GetMustHaveOutlineFilter( frameVarNames.FrameType ) ),
                    
                    AceTheme.GetColorSelectionElement( "Color", string.Empty,
                        frameSettings.frameOutlineColorName,
                        frameVarNames.FrameOutlineColorName, OnColorSelectionChanged,
                        GetMustHaveOutlineFilter( frameVarNames.FrameType ) )
                    
                    // AceTheme.GetColorSelectionElement( "Color", string.Empty,
                    //     frameSettings.frameOutlineColorIndex,
                    //     frameVarNames.FrameOutlineColorIndex, OnColorSelectionChanged,
                    //     GetMustHaveOutlineFilter( frameVarNames.FrameType ) )
                )
            );
        }

        private Element GetHeadingFrameSection( 
            HeadingElementFrameSettings headingElementFrameSettings, 
            HeadingElementFrameSettingsVarNames headingElementFrameVarNames )
        {
            return GetGroupWithLabelHeading( "Heading Frame", string.Empty, null,
                GetElement( headingElementFrameVarNames.BoxHeight, "Height", string.Empty ),
                GetGroupWithToggleHeading( headingElementFrameVarNames.IncludeBackground, "Background", string.Empty, null,
                    true,
                    
                    AceTheme.GetColorSelectionElement( "Active Color", string.Empty,
                        headingElementFrameSettings.backgroundActiveColorName,
                        headingElementFrameVarNames.BackgroundColorName, OnColorSelectionChanged,
                        GetMustHaveBackgroundFilter( headingElementFrameVarNames.IncludeBackground ) ),
                    
                    // AceTheme.GetColorSelectionElement( "Active Color", string.Empty,
                    //     headingElementFrameSettings.backgroundColorIndex,
                    //     headingElementFrameVarNames.BackgroundColorIndex, OnColorSelectionChanged,
                    //     GetMustHaveBackgroundFilter( headingElementFrameVarNames.IncludeBackground ) ),
                    
                    AceTheme.GetColorSelectionElement( "Inactive Color", string.Empty,
                        headingElementFrameSettings.backgroundInactiveColorName,
                        headingElementFrameVarNames.BackgroundInactiveColorName, OnColorSelectionChanged,
                        GetMustHaveBackgroundFilter( headingElementFrameVarNames.IncludeBackground ) )
                    
                    // AceTheme.GetColorSelectionElement( "Inactive Color", string.Empty,
                    //     headingElementFrameSettings.backgroundInactiveColorIndex,
                    //     headingElementFrameVarNames.BackgroundInactiveColorNameIndex, OnColorSelectionChanged,
                    //     GetMustHaveBackgroundFilter( headingElementFrameVarNames.IncludeBackground ) )
                ),

                GetGroupWithToggleHeading( headingElementFrameVarNames.IncludeOutline, "Outline", string.Empty, null,
                    true,
                    GetElement( headingElementFrameVarNames.FrameType, "Style", string.Empty ),
                    GetElement( headingElementFrameVarNames.FrameOutlineThickness, "Line Thickness", string.Empty, null,
                        false, GetMustHaveOutlineFilter( headingElementFrameVarNames.FrameType ) ),
                    
                    AceTheme.GetColorSelectionElement( "Color", string.Empty,
                        headingElementFrameSettings.frameOutlineColorName,
                        headingElementFrameVarNames.FrameOutlineColorName, OnColorSelectionChanged,
                        GetMustHaveOutlineFilter( headingElementFrameVarNames.FrameType ) )
                    
                    // AceTheme.GetColorSelectionElement( "Color", string.Empty,
                    //     headingElementFrameSettings.frameOutlineColorIndex,
                    //     headingElementFrameVarNames.FrameOutlineColorIndex, OnColorSelectionChanged,
                    //     GetMustHaveOutlineFilter( headingElementFrameVarNames.FrameType ) )
                )
            );
        }
        
        // private Element GetHeadingGroupSection( ElementFrameVarNames frameVarNames, HeadingElementFrameSettingsVarNames headingFrameVarNames )
        // {
        //     FrameSettings frameSettings = AceTheme.GetCurrentModeHeadingGroupFrameSettings( InfoSelectionIndex );
        //     HeadingElementFrameSettings headingElementFrameSettings = AceTheme.GetHeadingFrameSettingsForLevel( InfoSelectionIndex );
        //
        //     return GetGroupWithFoldoutHeading( null, "Frames", string.Empty, new GroupCustomSettings() {},
        //         GetFramesSection( "Heading Frame", string.Empty, headingElementFrameSettings, headingFrameVarNames, true ),
        //         GetFramesSection( "Group Frame", string.Empty, frameSettings, frameVarNames, true )
        //     );
        // }
        //
        // private void OnSeparateHeadingsToggleChanged() => UIStateUpdatedNotify();
        //
        // private void OnFoldoutButtonPressed()
        // {
        //     AceTheme.headingGroupSectionState = HeadingGroupSectionState.FoldoutGroups;
        //     DataUpdatedNotify();
        // }
        //
        // private void OnToggleButtonPressed()
        // {
        //     AceTheme.headingGroupSectionState = HeadingGroupSectionState.ToggleGroups;
        //     DataUpdatedNotify();
        // }
        //
        // private void OnLabeledButtonPressed()
        // {
        //     AceTheme.headingGroupSectionState = HeadingGroupSectionState.LabeledGroups;
        //     DataUpdatedNotify();
        // }
    }
}
