using Packages.com.ianritter.aceuiframework.Editor.Scripts.ACECore;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.SettingsSections.Elements.Groups.BasicGroups;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.SettingsSections.Elements.Groups.HeadingGroups;
using UnityEngine;
using static Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.ElementBuilding.AceElementBuilder;

namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.SettingsSections.Elements.Groups
{
    public class GroupSettingsSection : SettingsSection
    {
        private readonly BasicGroupSettingsSection _basicGroupSettingsSection;
        private readonly HeadingGroupSettingsSection _headingGroupSettingsSection;


        public GroupSettingsSection( AceTheme aceTheme, string myRelativeVarName )
        {
            AceTheme = aceTheme;
            MyRelativeVarName = myRelativeVarName;
            _basicGroupSettingsSection = new BasicGroupSettingsSection( aceTheme );
            _headingGroupSettingsSection = new HeadingGroupSettingsSection( aceTheme, nameof( _headingGroupSettingsSection ) );
            
            SubscribeToChildSections();
        }


        private void SubscribeToChildSections()
        {
            // Note that the Group section acts as a go between telling the Ace Theme editor when
            // the heading and basic group sections have updated.
            _basicGroupSettingsSection.OnDataUpdated += BasicGroupSectionDataChanged;
            _basicGroupSettingsSection.OnUIStateUpdated += BasicGroupSectionUIStateChanged;
            _basicGroupSettingsSection.OnColorUserModified += ColorUserModifiedNotify;
            
            _headingGroupSettingsSection.OnDataUpdated += HeadingGroupSectionDataChanged;
            _headingGroupSettingsSection.OnUIStateUpdated += HeadingGroupSectionDataChanged;
            _headingGroupSettingsSection.OnColorUserModified += ColorUserModifiedNotify;
        }

        public void UnsubscribeFromChildSections()
        {
            _basicGroupSettingsSection.OnDataUpdated -= BasicGroupSectionDataChanged;
            _basicGroupSettingsSection.OnUIStateUpdated += BasicGroupSectionDataChanged;
            _basicGroupSettingsSection.OnColorUserModified += ColorUserModifiedNotify;
            
            _headingGroupSettingsSection.OnDataUpdated -= HeadingGroupSectionDataChanged;
            _headingGroupSettingsSection.OnUIStateUpdated += HeadingGroupSectionUIChanged;
            _headingGroupSettingsSection.OnColorUserModified += ColorUserModifiedNotify;

            
            _basicGroupSettingsSection.ClearSubscriptions();
            _headingGroupSettingsSection.ClearSubscriptions();
        }

        private void BasicGroupSectionDataChanged() => DataUpdatedNotify();

        private void BasicGroupSectionUIStateChanged() => UIStateUpdatedNotify();

        private void HeadingGroupSectionDataChanged() => DataUpdatedNotify();

        private void HeadingGroupSectionUIChanged() => UIStateUpdatedNotify();


        protected override string GetRelativePathVarName( string varName ) => AceTheme.GetGroupSettingsSectionVarName + "." + varName;

        public override Element GetSection()
        {
            GroupSectionState groupSectionState = AceTheme.groupSectionState;

            Element activeSection = ( groupSectionState == GroupSectionState.BasicGroups )
                ? _basicGroupSettingsSection.GetSection()
                : _headingGroupSettingsSection.GetSection();
            
            return GetGroupWithFoldoutHeading( nameof( AceTheme.groupsSectionToggle ), "Group Elements",
                "Group of elements with a heading.",
                null,

                GetTabbedOptionsSection( activeSection,
                    ( "Heading Groups", string.Empty, groupSectionState == GroupSectionState.HeadingGroups, OnHeadingGroupButtonPressed ),
                    ( "Basic Groups", string.Empty, groupSectionState == GroupSectionState.BasicGroups, OnBasicGroupButtonPressed )
                )
            );
        }
        
        private void OnBasicGroupButtonPressed()
        {
            AceTheme.groupSectionState = GroupSectionState.BasicGroups;
            UIStateUpdatedNotify();
        }

        private void OnHeadingGroupButtonPressed()
        {
            AceTheme.groupSectionState = GroupSectionState.HeadingGroups;
            UIStateUpdatedNotify();
        }
    }
}
