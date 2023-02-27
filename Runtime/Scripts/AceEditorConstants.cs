namespace Packages.com.ianritter.aceuiframework.Runtime.Scripts
{
    public static class AceEditorConstants
    {
        public const string UsersSearchFolderName = "Assets";
        public const string DemosSearchFolderName = "Packages/com.ianritter.aceuiframework/Runtime/Scripts/Demos";
        public const string ThemesSearchFolderName = "Packages/com.ianritter.aceuiframework/Resources/ACEThemes";
        public const string LoggersSearchFolderName = "Packages/com.ianritter.aceuiframework/Runtime/Scripts/Services/Loggers";
        public const string ThemesResourceFolderName = "ACEThemes";
        public const string EditorWindowThemeName = "AceEditorWindowTheme";
        public const string ThemeCoreName = "AceTheme";
        public const string ThemeManagerCoreName = "AceThemeManager";
        public const string DefaultThemeName = "AceDefaultTheme";
        public const string ThemeLoggerName = "ThemeLogger";

        public const string ThemeAssetMenuName = "ACE Theme";
        public const string ThemeManagerAssetMenuName = "ACE Theme Manager";
        public const string LoggerAssetMenuName = "Custom Logger";
        public const string ThemeSettingsWindowMenuItemName = "Tools/ACE Theme Settings";
        public const string ThemeSettingsWindowTitle = "ACE Theme Settings";
        public const string ThemeManagerSettingsWindowMenuItemName = "Tools/ACE Theme Manager";
        public const string ThemeManagerSettingsWindowTitle = "ACE Theme Manager";
        public const string ColorPickerWindowTitle = "Color Picker";
        
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
