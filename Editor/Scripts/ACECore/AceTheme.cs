using System;
using System.Collections.Generic;
using System.Reflection;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.AceEditorRoots;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.ElementConditions;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.SingleElements.Decorator.DividingLine;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.SingleElements.Properties.Basic;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.SettingsSections;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.SettingsSections.Colors;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.SettingsSections.Elements.Groups;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.SettingsSections.Elements.PropertySpecific;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.SettingsSections.Elements.SingleElements;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.SettingsSections.Global;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.Enums;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsCustom.SingleElements;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsGlobal;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsGlobal.Colors;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsGlobal.Elements.Groups.BasicGroups;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsGlobal.Elements.Groups.ChildAreaGroups;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsGlobal.Elements.Groups.HeadingGroups;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsGlobal.Elements.SingleElements;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsGlobal.Elements.SingleElements.Decorator;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsGlobal.Global;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsGlobal.PropertySpecific;
using UnityEngine;
using static Packages.com.ianritter.aceuiframework.Runtime.Scripts.AceEditorConstants;
using static Packages.com.ianritter.aceuiframework.Editor.Scripts.ACECore.AceDelegates;
using static Packages.com.ianritter.unityscriptingtools.Runtime.Services.TextFormatting.TextFormat;

namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.ACECore
{
    /// <summary>
    ///     The theme houses both the theme settings and the element structure
    ///     which determines how they will be grouped in the editor window.
    /// </summary>
    
    [CreateAssetMenu(menuName = ThemeAssetMenuName)]
    public class AceTheme : AceScriptableObjectEditorRoot
    {
        [SerializeField] private SettingsSectionOptions selectedSection;
        
        public LevelSettingsSection.LevelSettingsMode basicGroupLevelSettingsMode = LevelSettingsSection.LevelSettingsMode.AllUseRootLevel;
        public LevelSettingsSection.LevelSettingsMode headingGroupLevelSettingsMode = LevelSettingsSection.LevelSettingsMode.AllUseRootLevel;
        public LevelSettingsSection.LevelSettingsMode singleElementsLevelSettingsMode = LevelSettingsSection.LevelSettingsMode.AllUseRootLevel;
        
        [Header("Global Settings")]
        [SerializeField] private GlobalSettings globalSettings;
        
        [Header("Color Settings")]
        [SerializeField] private CustomColorSettings customColorsSettings;
        
        [Header("Basic Group Settings")]
        [SerializeField] private List<BasicGroupSettings> basicGroupSettingsList;
        [SerializeField] private List<BasicGroupFrameSettings> basicGroupFrameSettingsList;
        
        [Header("Heading Group Sections States")]
        public HeadingGroupSectionState headingGroupSectionState = HeadingGroupSectionState.FoldoutGroups;
        public GroupSectionState groupSectionState = GroupSectionState.BasicGroups;
        
        [Header("Heading Group Settings")]
        [SerializeField] private List<HeadingGroupSettings> headingGroupSettingsList;
        [SerializeField] private List<ChildAreaGroupSettings> childAreaGroupSettingsList;
        [SerializeField] private List<HeadingElementFrameSettings> headingElementFrameSettingsList;
        [SerializeField] private List<FrameSettings> foldoutGroupFrameSettingsList;
        // [SerializeField] private List<FrameSettings> toggleGroupSettingsList;
        // [SerializeField] private List<FrameSettings> labeledGroupInfoList;

        [Header("Single Element Settings")]
        [SerializeField] private List<SingleElementSettings> singleElementSettingsList;
        [SerializeField] private List<SingleElementFrameSettings> singleElementFrameSettingsList;
        
        [Header("Property Specific Settings")]
        [SerializeField] private PropertySpecificSettings propertySpecificSettings;

        // Section instances (Not Serializable).
        [SerializeField] private GlobalSettingsSection globalSettingsSection;
        [SerializeField] private CustomColorsSection customColorsSection;
        [SerializeField] private GroupSettingsSection groupSettingsSection;
        [SerializeField] private SingleElementsSettingsSection singleElementSettingsSection;
        [SerializeField] private PropertiesSettingsSection propertiesSettingsSection;
        
        // Constants.
        public const int Levels = 5;

        // Section toggles
        [HideInInspector] public bool globalSettingsSectionToggle = true;
        [HideInInspector] public bool colorsSectionToggle = true;
        [HideInInspector] public bool groupsSectionToggle = true;
        [HideInInspector] public bool singleElementsSectionToggle = true;
        [HideInInspector] public bool propertySpecificSectionToggle = true;
        [HideInInspector] public bool basicGroupsSectionDrawAreaToggle;
        [HideInInspector] public bool headingGroupsSectionDrawAreaToggle;
        [HideInInspector] public bool singleElementsSectionDrawAreaToggle;

        private readonly Element _dividingLine = new DividingLineElement();
        private Element _sectionSelectionDropdown;

        private int _customColorsListCount;
        
        
        /// <summary>
        ///     This event is invoked when a data change occurs that justifies a repaint (element values have changed).
        /// </summary>
        public event ColorsUpdated OnColorsUpdated;
        
        /// <summary>
        ///     Used to notify when data that affects the UI data has occured, requiring a redraw.
        /// </summary>
        private void ColorsUpdatedNotify() => OnColorsUpdated?.Invoke();

        
        public AceTheme()
        {
            InitializeSettings();
            InitializeSections();
        }
        
        // public void Awake()
        // {
        //     InitializeSettings();
        //     InitializeSections();
        // }

        private void OnEnable()
        {
            if ( logger != null ) logger.LogStart();
            customColorsSettings.Initialize( logger );
            SubscribeToAllColors();
            if ( logger != null ) logger.LogEnd();
        }


#region Initialization

        private void InitializeSettings()
        {
            if (globalSettings == null) globalSettings = new GlobalSettings();
            if (customColorsSettings == null) customColorsSettings = new CustomColorSettings();
            _customColorsListCount = customColorsSettings.GetColorListCount();
            
            basicGroupSettingsList = InitializeList( basicGroupSettingsList );
            basicGroupFrameSettingsList = InitializeList( basicGroupFrameSettingsList );
            
            headingGroupSettingsList = InitializeList( headingGroupSettingsList );
            childAreaGroupSettingsList = InitializeList( childAreaGroupSettingsList );
            headingElementFrameSettingsList = InitializeList( headingElementFrameSettingsList );
            foldoutGroupFrameSettingsList = InitializeList( foldoutGroupFrameSettingsList );
            // toggleGroupSettingsList = InitializeList( toggleGroupSettingsList );
            // labeledGroupInfoList = InitializeList( labeledGroupInfoList );
            
            singleElementSettingsList = InitializeList( singleElementSettingsList );
            singleElementFrameSettingsList = InitializeList( singleElementFrameSettingsList );
            
            if (propertySpecificSettings == null) propertySpecificSettings = new PropertySpecificSettings();
        }

        private static List<T> InitializeList<T>( List<T> list )
            where T : new()
        {
            if ( list != null ) return list;

            list = new List<T>();
            for (int i = 0; i < Levels; i++)
            {
                list.Add( new T() );
            }

            return list;
        }
        
        // Have to use manual initialization instead of OnEnable because this must be ran AFTER the editor class has
        // had the chance to run its OnEnable - at which point it will call this method.
        private void InitializeSections()
        {
            _sectionSelectionDropdown = new BasicProperty( nameof( selectedSection ), 
                new GUIContent( "Selected Section" ), new SingleCustomSettings(), SectionSelectionChanged );
            
            // Instantiate basic sections.
            globalSettingsSection = new GlobalSettingsSection();
            propertiesSettingsSection = new PropertiesSettingsSection();
            
            // Initialize sections which implement element level settings.
            customColorsSection = new CustomColorsSection( this, customColorsSettings, nameof( customColorsSettings) );
            groupSettingsSection = new GroupSettingsSection( this, nameof( groupSettingsSection) );
            singleElementSettingsSection = new SingleElementsSettingsSection( this );

            // Establish Callbacks so the theme can tell its editor when the window
            // needs to be redrawn or rebuilt.
            globalSettingsSection.OnDataUpdated += RepaintSettingsWindow;
            globalSettingsSection.OnUIStateUpdated += RebuildSettingsWindow;
            
            customColorsSection.OnDataUpdated += RepaintSettingsWindow;
            customColorsSection.OnUIStateUpdated += RebuildSettingsWindow;
            
            groupSettingsSection.OnDataUpdated += RepaintSettingsWindow;
            groupSettingsSection.OnUIStateUpdated += RebuildSettingsWindow;
            
            singleElementSettingsSection.OnDataUpdated += RepaintSettingsWindow;
            singleElementSettingsSection.OnUIStateUpdated += RebuildSettingsWindow;
            
            propertiesSettingsSection.OnDataUpdated += RepaintSettingsWindow;
            propertiesSettingsSection.OnUIStateUpdated += RebuildSettingsWindow;

            customColorsSection.OnDataUpdated += ColorsUpdatedNotify;
            customColorsSection.OnUIStateUpdated += RebuildSettingsWindow;
        }

        private void OnDisable()
        {
            // Clear Callbacks
            globalSettingsSection.OnDataUpdated -= RepaintSettingsWindow;
            globalSettingsSection.OnUIStateUpdated -= RebuildSettingsWindow;
            
            customColorsSection.OnDataUpdated -= RepaintSettingsWindow;
            customColorsSection.OnUIStateUpdated -= RebuildSettingsWindow;
            
            groupSettingsSection.OnDataUpdated -= RepaintSettingsWindow;
            groupSettingsSection.OnUIStateUpdated -= RebuildSettingsWindow;
            groupSettingsSection.UnsubscribeFromChildSections();

            singleElementSettingsSection.OnDataUpdated -= RepaintSettingsWindow;
            singleElementSettingsSection.OnUIStateUpdated -= RebuildSettingsWindow;
            
            propertiesSettingsSection.OnDataUpdated -= RepaintSettingsWindow;
            propertiesSettingsSection.OnUIStateUpdated -= RebuildSettingsWindow;
        }
        
        
        // Section Changed callbacks
        // These should only be called when data is changed that affects the UI layout.
        private void SectionSelectionChanged()
        {
            DataUpdateRequiredNotify();
            RebuildSettingsWindow();
        }

        private void RebuildSettingsWindow() => UIStateUpdatedNotify();

        private void RepaintSettingsWindow()
        {
            DataUpdateRequiredNotify();
            // Todo: Make updates more efficient. All repainting set to rebuilding while ironing out updating issues.
            UIStateUpdatedNotify();
            // DataUpdatedNotify();
        }




#endregion

        private void OnValidate()
        {
            if ( logger != null) logger.LogStart();
            
            // If the customColorSettings list was changed, need to know.
            int newColorListCount = customColorsSettings.GetColorListCount();
            if ( newColorListCount != _customColorsListCount )
            {
                logger.Log( $"Color list count has changed from {_customColorsListCount.ToString()} to {newColorListCount.ToString()}!" );
                _customColorsListCount = newColorListCount;
                SubscribeToNewColor();
            }
            
            customColorsSettings.ScanForListUpdates();

            if ( logger != null) logger.LogEnd();
        }

        private void SubscribeToAllColors()
        {
            // Debug.Log( "AceTheme: Subscribing to all colors." );
            
            for (int i = 0; i < customColorsSettings.GetColorListCount(); i++)
            {
                CustomColorEntry currentEntry = customColorsSettings.GetColorEntryForIndex( i );
                // Debug.Log( $"    Subscribing to {currentEntry.customColor.name}." );
                currentEntry.OnNameUpdated -= ReplaceColorNameWithNewName;
                currentEntry.OnNameUpdated += ReplaceColorNameWithNewName;
            }
        }

        private void SubscribeToNewColor()
        {
            logger.LogStart();
            CustomColorEntry newColorEntry = customColorsSettings.GetColorEntryForIndex( customColorsSettings.GetColorListCount() - 1 );
            newColorEntry.OnNameUpdated += ReplaceColorNameWithNewName;
            logger.LogEnd();
        }
        
        private void ReplaceColorNameWithNewName( string oldName, string newName )
        {
            logger.LogStart();
            // Look at colors in all settings groups to see if the old name is registered.
            // If found, replace it with the new one.
            logger.Log( $"Replacing all instances of {GetColoredStringAquamarine( oldName )} with {GetColoredStringCyan( newName )}." );
            
            // Names used by all:
            // public string frameOutlineColorName = CustomColorOutlinesName;
            // public string backgroundColorName = CustomColorRootBackgroundName;
            // headingElementFrameSettingsList only names:
            // public string enabledTextColorName = CustomColorEnabledTextName;
            // public string disabledTextColorName = CustomColorDisabledTextName;
            // public string backgroundInactiveColorName = CustomColorDisabledTextName;
            
            // Loop through all lists to deal with the universal names.
            UpdateFrameSettingsNames( basicGroupFrameSettingsList, oldName, newName );
            UpdateFrameSettingsNames( foldoutGroupFrameSettingsList, oldName, newName );
            UpdateFrameSettingsNames( singleElementFrameSettingsList, oldName, newName );
            UpdateFrameSettingsNames( headingElementFrameSettingsList, oldName, newName );
            // Loop through foldoutGroupFrameSettingsList to deal with its unique names.
            UpdateHeadingFrameSettingsNames( headingElementFrameSettingsList, oldName, newName );
            
            logger.LogEnd();
        }

        private void UpdateFrameSettingsNames( IReadOnlyList<FrameSettings> frameSettings, string oldName, string newName )
        {
            for (int i = 0; i < Levels; i++)
            {
                if ( frameSettings[i].frameOutlineColorName.Equals( oldName ) ) 
                    frameSettings[i].frameOutlineColorName = newName;
                if ( frameSettings[i].backgroundColorName.Equals( oldName ) ) 
                    frameSettings[i].backgroundColorName = newName;
            }
        }
         
        private void UpdateHeadingFrameSettingsNames( IReadOnlyList<HeadingElementFrameSettings> frameSettings, string oldName, string newName )
        {
            for (int i = 0; i < Levels; i++)
            {
                // if ( frameSettings[i].frameOutlineColorName.Equals( oldName ) ) 
                //     frameSettings[i].frameOutlineColorName = newName;
                // if ( frameSettings[i].backgroundColorName.Equals( oldName ) ) 
                //     frameSettings[i].backgroundColorName = newName;
                if ( frameSettings[i].enabledTextColorName.Equals( oldName ) ) 
                    frameSettings[i].enabledTextColorName = newName;
                if ( frameSettings[i].disabledTextColorName.Equals( oldName ) ) 
                    frameSettings[i].disabledTextColorName = newName;
                if ( frameSettings[i].backgroundInactiveColorName.Equals( oldName ) ) 
                    frameSettings[i].backgroundInactiveColorName = newName;
            }
        }


#region GettingElements
        
        public override Element[] GetElementList()
        {
            if (selectedSection == SettingsSectionOptions.AllSections )
            {
                return new []
                {
                    _dividingLine,
                    _sectionSelectionDropdown,
                    _dividingLine,
                    GetGlobalSettingsElement(),
                    GetCustomColorsElement(),
                    GetGroupsElement(),
                    GetSinglesElement(),
                    GetPropertiesElement()
                };
            }

            return new []
            {
                _dividingLine,
                _sectionSelectionDropdown,
                _dividingLine,
                selectedSection switch
                {
                    SettingsSectionOptions.GlobalAdjustments => GetGlobalSettingsElement(),
                    SettingsSectionOptions.GroupElements => GetGroupsElement(),
                    SettingsSectionOptions.Colors => GetCustomColorsElement(),
                    SettingsSectionOptions.SingleElements => GetSinglesElement(),
                    SettingsSectionOptions.PropertyElements => GetPropertiesElement(),
                    _ => throw new ArgumentOutOfRangeException()
                }
            };
        }

        private Element GetGlobalSettingsElement() => globalSettingsSection.GetSection();
        private Element GetCustomColorsElement() => customColorsSection.GetSection();
        private Element GetGroupsElement() => groupSettingsSection.GetSection();
        private Element GetSinglesElement() => singleElementSettingsSection.GetSection();
        private Element GetPropertiesElement() => propertiesSettingsSection.GetSection();
        
#endregion
        

#region AccessingData

        // Variable names are collected so that their associated serialized properties can be retrieved.
        public static string GetGlobalSettingsVarName => nameof( globalSettings );
        
        public static string GetGroupSettingsSectionVarName => nameof( groupSettingsSection );
        
        public static string GetBasicGroupSettingsListVarName => nameof( basicGroupSettingsList );
        public static string GetBasicGroupFrameSettingsListVarName => nameof( basicGroupFrameSettingsList );
        
        public static string GetHeadingGroupSettingsListVarName => nameof( headingGroupSettingsList );
        public static string GetHeadingElementFrameSettingsListVarName => nameof( headingElementFrameSettingsList );
        public static string GetChildAreaGroupSettingsListVarName => nameof( childAreaGroupSettingsList );
        public static string GetFoldoutGroupSettingsListVarName => nameof( foldoutGroupFrameSettingsList );
        // public static string GetToggleGroupSettingsListVarName => nameof( toggleGroupSettingsList );
        // public static string GetLabeledGroupInfoListVarName => nameof( labeledGroupInfoList );
        
        public static string GetSingleElementSettingsSectionVarName => nameof( singleElementSettingsSection );
        public static string GetSingleElementSettingsListVarName => nameof( singleElementSettingsList );
        public static string GetSingleElementFrameSettingsListVarName => nameof( singleElementFrameSettingsList );
        
        public static string GetPropertySpecificSettingsVarName => nameof( propertySpecificSettings );

        public static string GetCustomColorListVarName => CustomColorSettings.GetCustomColorListVarName();


        public static int GetLevelBasedOnMode( int level, LevelSettingsSection.LevelSettingsMode levelSettingsMode ) =>
            levelSettingsMode switch
            {
                LevelSettingsSection.LevelSettingsMode.AllUseRootLevel => 0,
                LevelSettingsSection.LevelSettingsMode.AllChildrenUseLevel1 => ( ( level > 1 ) ? 1 : level ),
                _ => level
            };

        // /// <summary>
        // ///     Use to link colors to their respective index numbers within the custom colors list.
        // /// </summary>
        // public Element GetColorSelectionElement(
        //     string title, string tooltip,
        //     int selectedIndex, 
        //     string selectedIndexRelativeVarName,
        //     Action callback,
        //     params ElementCondition[] filter )
        // {
        //     CustomColorEntry colorEntry = customColorsSettings.GetColorEntryForIndex( selectedIndex );
        //     return customColorsSection.GetColorSelectionElement( title, tooltip, selectedIndex,
        //         selectedIndexRelativeVarName,
        //         colorEntry.toggle,
        //         callback, filter );
        // }
        
        /// <summary>
        ///     Use to link colors by their names to keep the link independent of the color's position in the custom
        ///     colors list.
        /// </summary>
        public Element GetColorSelectionElement(
            string title, string tooltip,
            string colorName, 
            string selectedIndexRelativeVarName,
            Action callback,
            params ElementCondition[] filter )
        {
            return customColorsSection.GetColorSelectionElement( title, tooltip, colorName,
                selectedIndexRelativeVarName,
                callback, filter );
        }
        
        // public Element GetColorSelectionElement(
        //     string title, string tooltip,
        //     int selectedIndex, string selectedIndexRelativeVarName,
        //     params ElementCondition[] filter )
        // {
        //     return customColorsSection.GetColorSelectionElement( title, tooltip, selectedIndex,
        //         selectedIndexRelativeVarName,
        //         filter );
        // }

        public GlobalSettings GetGlobalSettings() => globalSettings;

        public int GetTotalSettingsLevels() => Levels;
        

        public HeadingGroupSettings GetHeadingGroupSettingsForLevel( int level ) => 
            headingGroupSettingsList[GetLevelBasedOnMode( level, headingGroupLevelSettingsMode )];
        
        public ChildAreaGroupSettings GetChildAreaGroupSettingsForLevel( int level ) => 
            childAreaGroupSettingsList[GetLevelBasedOnMode( level, headingGroupLevelSettingsMode )];

        public HeadingElementFrameSettings GetHeadingFrameSettingsForLevel( int level ) =>
            headingElementFrameSettingsList[GetLevelBasedOnMode( level, headingGroupLevelSettingsMode )];

        
        public FrameSettings GetFoldoutGroupFrameSettingsForLevel( int level ) =>
            GetHeadingGroupSettingsForLevel( level ).useSeparateHeadingSettings 
                ? foldoutGroupFrameSettingsList[GetLevelBasedOnMode( level, headingGroupLevelSettingsMode )]
                : GetDefaultGroupFrameSettingsForLevel( level );

        // public FrameSettings GetToggleGroupFrameSettingsForLevel( int level ) =>
        //     GetHeadingGroupSettingsForLevel( level ).useSeparateHeadingSettings 
        //         ? toggleGroupSettingsList[GetLevelBasedOnMode( level, headingGroupLevelSettingsMode )]
        //         : GetDefaultGroupFrameSettingsForLevel( level );
        //
        // public FrameSettings GetLabeledGroupFrameSettingsForLevel( int level ) =>
        //     GetHeadingGroupSettingsForLevel( level ).useSeparateHeadingSettings 
        //         ? labeledGroupInfoList[GetLevelBasedOnMode( level, headingGroupLevelSettingsMode )]
        //         : GetDefaultGroupFrameSettingsForLevel( level );

        private FrameSettings GetDefaultGroupFrameSettingsForLevel( int level )
        {
            // Heading group type defaults to foldout group settings.
            headingGroupSectionState = HeadingGroupSectionState.FoldoutGroups;
            DataUpdateRequiredNotify();
            return foldoutGroupFrameSettingsList[GetLevelBasedOnMode( level, headingGroupLevelSettingsMode )];
        }
        
        public FrameSettings GetCurrentModeHeadingGroupFrameSettings( int level )
        {
            return headingGroupSectionState switch
            {
                HeadingGroupSectionState.FoldoutGroups => foldoutGroupFrameSettingsList[level],
                // HeadingGroupSectionState.ToggleGroups => toggleGroupSettingsList[level],
                // HeadingGroupSectionState.LabeledGroups => labeledGroupInfoList[level],
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        
        public BasicGroupSettings GetBasicGroupSettingsForLevel( int level ) => 
            basicGroupSettingsList[GetLevelBasedOnMode( level, basicGroupLevelSettingsMode )];
        
        public BasicGroupFrameSettings GetBasicGroupFrameSettingsForLevel( int level ) => 
            basicGroupFrameSettingsList[GetLevelBasedOnMode( level, basicGroupLevelSettingsMode )];



        public SingleElementSettings GetSingleElementSettingsForLevel( int level ) => 
            singleElementSettingsList[GetLevelBasedOnMode( level, singleElementsLevelSettingsMode )];


        public SingleElementFrameSettings GetSingleElementFrameSettingsForLevel( int level ) => 
            singleElementFrameSettingsList[GetLevelBasedOnMode( level, singleElementsLevelSettingsMode )];


        // public Color GetColorForIndex( int index ) => customColorsSettings.GetColorForIndex( index );
        public Color GetColorForColorName( string colorName ) => customColorsSettings.GetColorForColorName( colorName );
        public string[] GetCustomColorOptions() => customColorsSettings.GetCustomColorsOptionsList();
        // public CustomColorSettings GetCustomColorSettings() => customColorsSettings;
        public int GetIndexForCustomColorName( string customColorName ) => customColorsSettings.GetIndexForCustomColorName( customColorName );
        public string GetColorNameForIndex( int index ) => customColorsSettings.GetColorNameForIndex( index );

        public PropertySpecificSettings GetPropertySpecificSettings() => propertySpecificSettings;

#endregion

    }
}
