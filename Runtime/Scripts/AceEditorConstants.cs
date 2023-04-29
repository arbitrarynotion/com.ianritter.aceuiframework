namespace Packages.com.ianritter.aceuiframework.Runtime.Scripts
{
    public static class AceEditorConstants
    {
        // Asset paths.
        public const string UsersSearchFolderName = "Assets";
        public const string ThemesSearchFolderName = "Packages/com.ianritter.aceuiframework/ScriptableObjects/AceThemes/User";
        public const string SystemCoreSearchFolderName = "Packages/com.ianritter.aceuiframework/ScriptableObjects/ACECore";
        public const string DemosSearchFolderName = "Packages/com.ianritter.aceuiframework/Runtime/Scripts/Demos";
        public const string LoggersSearchFolderName = "Packages/com.ianritter.aceuiframework/Runtime/Scripts/Services/Loggers";
        
        // System theme names.
        public const string EditorWindowThemeName = "AceEditorWindowTheme";
        public const string DefaultThemeName = "AceDefaultTheme";
        
        // Script names.
        public const string ThemeManagerCoreName = "AceThemeManager";
        public const string MonobehaviourRootEditorLoggerName = "MonobehaviourRootEditorLogger";
        public const string ThemeCoreName = "AceTheme";

        // Text displayed in the editor.
        public const string ThemeAssetMenuName = "ACE Theme";
        public const string ThemeSettingsWindowTitle = "ACE Theme Settings";
        public const string ThemeManagerAssetMenuName = "ACE Theme Manager";
        public const string ThemeManagerWindowTitle = "ACE Theme Manager";
        public const string LoggerAssetMenuName = "Custom Logger";
        public const string ColorPickerWindowTitle = "Color Picker";

        // Menu Item Paths
        public const string ThemeSettingsWindowMenuItemName = "Tools/ACE UI/Theme Settings";
        public const string ThemeManagerSettingsWindowMenuItemName = "Tools/ACE UI/Theme Manager";
        
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
