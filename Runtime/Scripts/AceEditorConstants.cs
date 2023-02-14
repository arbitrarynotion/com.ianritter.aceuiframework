namespace ACEPackage.Runtime.Scripts
{
    public static class AceEditorConstants
    {
        public const string UsersSearchFolderName = "Assets";
        public const string ThemesResourceFolderName = "ACEThemes";
        public const string EditorWindowThemeName = "AceEditorWindowTheme";
        public const string ThemeCoreName = "AceTheme";
        public const string ThemeManagerDatabaseCoreName = "AceThemeManagerDatabase";
        public const string DefaultThemeName = "AceDefaultTheme";

        public const string ThemeAssetMenuName = "ACE Theme";
        public const string ThemeDatabaseAssetMenuName = "ACE Script Database";
        public const string ThemeSettingsWindowMenuItemName = "Tools/ACE Theme Settings";
        public const string ThemeSettingsWindowTitle = "ACE Theme Settings";
        public const string ThemeManagerSettingsWindowMenuItemName = "Tools/ACE Theme Manager";
        public const string ThemeManagerSettingsWindowTitle = "ACE Theme Manager";
        
        
        // Can not access the var names directly because the AceTheme is in an Editor folder.
        // This ugly workaround was the only way to retain this framework.
        public const string GetGlobalSettingsVarName = "globalSettings";
        
        public const string GetGroupSettingsSectionVarName = "groupSettingsSection";
        
        public const string GetBasicGroupSettingsListVarName = "basicGroupSettingsList";
        public const string GetBasicGroupFrameSettingsListVarName = "basicGroupFrameSettingsList";
        
        public const string GetHeadingGroupSettingsListVarName = "headingGroupSettingsList";
        public const string GetHeadingElementFrameSettingsListVarName = "headingElementFrameSettingsList";
        public const string GetChildAreaGroupSettingsListVarName = "childAreaGroupSettingsList";
        public const string GetFoldoutGroupSettingsListVarName = "foldoutGroupSettingsList";
        public const string GetToggleGroupSettingsListVarName = "toggleGroupSettingsList";
        public const string GetLabeledGroupInfoListVarName = "labeledGroupInfoList";
        
        public const string GetSingleElementSettingsSectionVarName = "singleElementSettingsSection";
        public const string GetSingleElementSettingsListVarName = "singleElementSettingsList";
        public const string GetSingleElementFrameSettingsListVarName = "singleElementFrameSettingsList";
        
        public const string GetPropertySpecificSettingsVarName = "propertySpecificSettings";
    }
}
