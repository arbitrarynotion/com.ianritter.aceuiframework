using System;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.ACECore;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.SingleElements.Properties.Basic;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.Enums;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsCustom.Groups;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsCustom.SingleElements;
using UnityEngine;
using static Packages.com.ianritter.aceuiframework.Editor.Scripts.ElementBuilding.AceElementBuilder;

namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.SettingsSections
{
    /// <summary>
    ///     This is a specialized setting section that facilitates the element level tabs menu.
    ///     Use for any settings section that will utilize element levels.
    /// </summary>
    public abstract class LevelSettingsSection : SettingsSection
    {
        [Serializable]
        public enum LevelSettingsMode
        {
            // All levels use the same settings.
            AllUseRootLevel,
            // The root has its own settings but the rest share level 1 settings.
            AllChildrenUseLevel1,
            // Every level has its own settings.
            NoSharedLevels
        }
        
        // Don't make this readonly! Rider says you should but doing so will break things!
        protected string LevelSettingsModeRelativeVarName;
        protected int InfoSelectionIndex = 0;

        private Element[][] _infoElementsList;
        
        // Display strings for the info list.
        private string[] _infoOptionsNoSharedLevels;
        private string[] _infoOptionsAllChildrenUseLevel1;
        private string[] _infoOptionsAllUseRoot;
        
        protected static int Levels => AceTheme.Levels;
        
        /// <summary>
        ///     Get the collection of elements that represent the section for this element level.
        /// </summary>
        protected abstract Element[] GetElementForLevel( int level );

        
        
#region Initialization
        
        protected void Initialize()
        {
            InitializeSectionVarNamesList();
            
            InitializeSettingsElementList();
            InitializeSettingsOptions();
            
            EstablishSubscriptions();
        }

        protected abstract void InitializeSectionVarNamesList();
        
        private void InitializeSettingsElementList()
        {
            _infoElementsList = new Element[Levels][];
            for (int i = 0; i < _infoElementsList.Length; i++)
            {
                _infoElementsList[i] = GetElementForLevel( i );
            }
        }

        private void InitializeSettingsOptions()
        {
            // No shared levels contains all level options.
            _infoOptionsNoSharedLevels = new string[Levels];
            _infoOptionsNoSharedLevels[0] = "Root Level Settings";
            for (int i = 1; i < _infoOptionsNoSharedLevels.Length; i++)
            {
                _infoOptionsNoSharedLevels[i] = $"Level {i.ToString()} Settings";
            }

            // All children use level 1 contains the root and level 1 options.
            _infoOptionsAllChildrenUseLevel1 = new string[2];
            for (int i = 0; i < _infoOptionsAllChildrenUseLevel1.Length; i++)
            {
                _infoOptionsAllChildrenUseLevel1[i] = _infoOptionsNoSharedLevels[i];
            }

            // All use root contains only the root level option.
            _infoOptionsAllUseRoot = new[] {_infoOptionsNoSharedLevels[0]};
        }
        
#endregion
        
        
        protected abstract LevelSettingsMode GetLevelSettingsMode();

        protected Element GetLevelSettingsSectionWithTabs( string foldoutVarName, string title, string tooltip, GroupCustomSettings customSettings )
        {
            return GetGroupWithFoldoutHeading( foldoutVarName, title, tooltip, customSettings, GetLevelSettingsSectionWithTabs() );
        }
        
        protected Element GetLevelSettingsSectionWithTabs()
        {
            InfoSelectionIndex = AceTheme.GetLevelBasedOnMode( InfoSelectionIndex, GetLevelSettingsMode() );

            string[] infoOptions = GetLevelSettingsMode() switch
            {
                LevelSettingsMode.AllUseRootLevel => _infoOptionsAllUseRoot,
                LevelSettingsMode.AllChildrenUseLevel1 => _infoOptionsAllChildrenUseLevel1,
                LevelSettingsMode.NoSharedLevels => _infoOptionsNoSharedLevels,
                _ => throw new ArgumentOutOfRangeException()
            };
            
            _infoElementsList[InfoSelectionIndex] = GetElementForLevel( InfoSelectionIndex );

            // Get the level settings section for this level index.
            // The title and level mode and level index drop downs are updated every time. 
            // The actual settings section is only updated above when an update is required.
            return GetLevelSettingsSectionWithTabs(
                LevelSettingsModeRelativeVarName,
                infoOptions,
                _infoElementsList[InfoSelectionIndex]
            );
        }
        
        private Element GetLevelSettingsSectionWithTabs(
            string levelModeVarName,
            string[] infoOptions,
            Element[] sectionSelected
        )
        {
            if (infoOptions == null || infoOptions.Length == 0)
            {
                Debug.LogWarning( "LSS|GLSS: Error! infoOptions list is null or empty!" );
                return GetLabelElement( "Error! Info Options list is null!" );
            }
        
            if (sectionSelected == null || sectionSelected.Length == 0)
            {
                Debug.LogWarning( "LSS|GLSS: Error! sectionSelected element list is null or empty!" );
                return GetLabelElement( "Error! Section Selected list is null!" );
            }
            
            // Todo: Disabled this for now as having separate sections for each heading type doesn't seem entirely necessary.
            // Get active section.
            // Element activeSection = headingGroupSectionState switch
            // {
            //     HeadingGroupSectionState.FoldoutGroups => GetHeadingGroupSection( foldoutGroupVarNames ),
            //     HeadingGroupSectionState.ToggleGroups => GetHeadingGroupSection( toggleGroupVarNames ),
            //     HeadingGroupSectionState.LabeledGroups => GetHeadingGroupSection( labeledGroupVarNames ),
            //     _ => throw new ArgumentOutOfRangeException()
            // };

            string[] levelNames = new[]
            {
                "Root Element Level",
                $"Element Level {1.ToString()}",
                $"Element Level {2.ToString()}",
                $"Element Level {3.ToString()}",
                $"Element Level {4.ToString()}",
            };
            
            string[] levelNamesShort = new[]
            {
                "Root",
                $"LVL {1.ToString()}",
                $"LVL {2.ToString()}",
                $"LVL {3.ToString()}",
                $"LVL {4.ToString()}",
            };

            var buttonInfoList = new (string name, string tooltip, bool focused, Action callback)[Levels];
            buttonInfoList[0] = ( levelNamesShort[0], string.Empty, InfoSelectionIndex == 0, OnLevel0ButtonPressed );
            buttonInfoList[1] = ( levelNamesShort[1], string.Empty, InfoSelectionIndex == 1, OnLevel1ButtonPressed );
            buttonInfoList[2] = ( levelNamesShort[2], string.Empty, InfoSelectionIndex == 2, OnLevel2ButtonPressed );
            buttonInfoList[3] = ( levelNamesShort[3], string.Empty, InfoSelectionIndex == 3, OnLevel3ButtonPressed );
            buttonInfoList[4] = ( levelNamesShort[4], string.Empty, InfoSelectionIndex == 4, OnLevel4ButtonPressed );

            // Determine levels to show.
            int visibleLevels = GetLevelSettingsMode() switch
            {
                LevelSettingsMode.AllUseRootLevel => 1,
                LevelSettingsMode.AllChildrenUseLevel1 => 2,
                LevelSettingsMode.NoSharedLevels => Levels,
                _ => throw new ArgumentOutOfRangeException()
            };

            var returnButtonInfoList = new (string name, string tooltip, bool focused, Action callback)[visibleLevels];
            for (int i = 0; i < visibleLevels; i++)
            {
                returnButtonInfoList[i] = buttonInfoList[i];
            }
            
            Element[] labeledSection = new Element[ sectionSelected.Length + 1 ];
            InsertLabelIntoSelectedSection( labeledSection, sectionSelected, levelNames );
            
            Element tabbedOptionsSection = ( visibleLevels == 1 ) 
                ? GetGroup( null, sectionSelected )
                : GetTabbedOptionsSection( GetGroup( null, labeledSection ), returnButtonInfoList );

            return GetGroup(
                new GroupCustomSettings()
                {
                    NumberOfColumns = 1,
                    CustomFrameSettings = NoFrame,
                    IndentChildren = false,
                },
                
                new BasicProperty( levelModeVarName, new GUIContent( "Level Settings Mode" ),
                    new SingleCustomSettings() { BottomPadding = 2f }, OnLevelModeChanged 
                ),
                
                tabbedOptionsSection 
            );
        }

        private void InsertLabelIntoSelectedSection( Element[] labeledSection, Element[] selectedSection, string[] levelNames )
        {
            labeledSection[0] = GetLabelElement( $"{levelNames[InfoSelectionIndex]}", string.Empty, new SingleCustomSettings() { LabelAlignment = Alignment.Center } );
            for (int i = 1; i < labeledSection.Length; i++)
            {
                labeledSection[i] = selectedSection[i - 1];
            }
        }
        
        private void OnLevel0ButtonPressed() => OnLevelTabButtonPressed( 0 );
        private void OnLevel1ButtonPressed() => OnLevelTabButtonPressed( 1 );
        private void OnLevel2ButtonPressed() => OnLevelTabButtonPressed( 2 );
        private void OnLevel3ButtonPressed() => OnLevelTabButtonPressed( 3 );
        private void OnLevel4ButtonPressed() => OnLevelTabButtonPressed( 4 );
        
        private void OnLevelTabButtonPressed( int level )
        {
            InfoSelectionIndex = level;
            UIStateUpdatedNotify();
        }

        private void OnLevelModeChanged() => UIStateUpdatedNotify();
    }
}
