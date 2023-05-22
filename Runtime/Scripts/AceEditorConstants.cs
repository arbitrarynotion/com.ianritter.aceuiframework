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
        
        // Custom Editor text.
        public const string ThemeCustomEditorHelpInfoText = "For the sake of your sanity, use the settings window at \"Tools / ACE UI / Theme Settings\".\n" +
                                                            "If you want to modify the settings of the ACE UI Settings windows, use the debug option 'Theme Self-edit'" +
                                                            "in the Theme Manager window.";
        
        // System theme names.
        public const string EditorWindowThemeName = "Ghost_Sys";
        // public const string EditorWindowThemeName = "AceEditorWindowTheme";
        public const string DefaultThemeName = "AceDefaultTheme";
        
        // Logger names.
        public const string DefaultElementLoggerName = "_ElementLogger";
        public const string DefaultMbRootLoggerName = "_MBRootLogger";
        public const string DefaultSoLoggerName = "_SORootLogger";
        public const string ThemeLoggerName = "ThemeLogger";
        public const string ThemeManagerLoggerName = "ThemeManagerLogger";
        public const string MonobehaviourRootEditorLoggerName = "MonobehaviourRootEditorLogger";
        public const string ElementLoggerName = "ElementLogger";
        
        // Script names.
        /// <summary>
        /// This is both the script name and the scriptable object instance name.
        /// </summary>
        public const string ThemeManagerCoreName = "AceThemeManager";
        public const string ThemeCoreClassName = "AceTheme";
        
        // Theme custom color names.
        public const string CustomColorOutlinesName = "Outlines";
        public const string CustomColorRootBackgroundName = "Root Background";
        public const string CustomColorChildBackgroundName = "Child Background";
        public const string CustomColorEnabledTextName = "Enabled Text";
        public const string CustomColorDisabledTextName = "Disabled Text";

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
