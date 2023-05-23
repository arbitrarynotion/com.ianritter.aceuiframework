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
        
        [Header("Color Palette Settings")]
        [SerializeField] private ColorPaletteSettings colorsPaletteSettings;
        
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
        [SerializeField] private List<FrameSettings> headingGroupFrameSettingsList;
        // [SerializeField] private List<FrameSettings> toggleGroupSettingsList;
        // [SerializeField] private List<FrameSettings> labeledGroupInfoList;

        [Header("Single Element Settings")]
        [SerializeField] private List<SingleElementSettings> singleElementSettingsList;
        [SerializeField] private List<SingleElementFrameSettings> singleElementFrameSettingsList;
        
        [Header("Property Specific Settings")]
        [SerializeField] private PropertySpecificSettings propertySpecificSettings;

        // Section instances (Not Serializable).
        [SerializeField] private GlobalSettingsSection globalSettingsSection;
        [SerializeField] private ColorPaletteSection colorPaletteSection;
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
        [HideInInspector] public bool basicGroupsSectionDrawAreaToggle = true;
        [HideInInspector] public bool headingGroupsSectionDrawAreaToggle = true;
        [HideInInspector] public bool headingGroupsSectionHeadingTextToggle = true;
        [HideInInspector] public bool headingGroupsSectionFramesToggle = true;
        [HideInInspector] public bool singleElementsSectionDrawAreaToggle = true;

        private readonly Element _dividingLine = new DividingLineElement();
        private Element _sectionSelectionDropdown;

        private int _colorEntryListCount;
        
        
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

        protected override string GetLoggerName() => ThemeLoggerName;

        // public void Awake()
        // {
        //     InitializeSettings();
        //     InitializeSections();
        // }

        protected override void OnEnableLast()
        {
            if ( logger != null ) logger.LogStart();
            colorsPaletteSettings.Initialize( logger );
            SubscribeToAllColors();
            UpdateAllColorUseCounts();
            if ( logger != null ) logger.LogEnd();
        }


#region Initialization

        private void InitializeSettings()
        {
            if (globalSettings == null) globalSettings = new GlobalSettings();
            if (colorsPaletteSettings == null) colorsPaletteSettings = new ColorPaletteSettings();
            _colorEntryListCount = colorsPaletteSettings.GetColorListCount();
            
            basicGroupSettingsList = InitializeList( basicGroupSettingsList );
            basicGroupFrameSettingsList = InitializeList( basicGroupFrameSettingsList );
            
            headingGroupSettingsList = InitializeList( headingGroupSettingsList );
            childAreaGroupSettingsList = InitializeList( childAreaGroupSettingsList );
            headingElementFrameSettingsList = InitializeList( headingElementFrameSettingsList );
            headingGroupFrameSettingsList = InitializeList( headingGroupFrameSettingsList );
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
            colorPaletteSection = new ColorPaletteSection( this, colorsPaletteSettings, nameof( colorsPaletteSettings) );
            groupSettingsSection = new GroupSettingsSection( this, nameof( groupSettingsSection) );
            singleElementSettingsSection = new SingleElementsSettingsSection( this );

            // Establish Callbacks so the theme can tell its editor when the window
            // needs to be redrawn or rebuilt.
            globalSettingsSection.OnDataUpdated += RepaintSettingsWindow;
            globalSettingsSection.OnUIStateUpdated += RebuildSettingsWindow;
            
            colorPaletteSection.OnDataUpdated += RepaintSettingsWindow;
            colorPaletteSection.OnUIStateUpdated += RebuildSettingsWindow;
            
            groupSettingsSection.OnDataUpdated += RepaintSettingsWindow;
            groupSettingsSection.OnUIStateUpdated += RebuildSettingsWindow;
            groupSettingsSection.OnColorUserModified += OnColorUsersModified;
            
            singleElementSettingsSection.OnDataUpdated += RepaintSettingsWindow;
            singleElementSettingsSection.OnUIStateUpdated += RebuildSettingsWindow;
            singleElementSettingsSection.OnColorUserModified += OnColorUsersModified;
            
            propertiesSettingsSection.OnDataUpdated += RepaintSettingsWindow;
            propertiesSettingsSection.OnUIStateUpdated += RebuildSettingsWindow;

            colorPaletteSection.OnDataUpdated += ColorsUpdatedNotify;
            colorPaletteSection.OnUIStateUpdated += RebuildSettingsWindow;
        }

        private void OnDisable()
        {
            // Clear Callbacks
            globalSettingsSection.OnDataUpdated -= RepaintSettingsWindow;
            globalSettingsSection.OnUIStateUpdated -= RebuildSettingsWindow;
            
            colorPaletteSection.OnDataUpdated -= RepaintSettingsWindow;
            colorPaletteSection.OnUIStateUpdated -= RebuildSettingsWindow;
            
            groupSettingsSection.OnDataUpdated -= RepaintSettingsWindow;
            groupSettingsSection.OnUIStateUpdated -= RebuildSettingsWindow;
            groupSettingsSection.OnColorUserModified -= OnColorUsersModified;
            groupSettingsSection.UnsubscribeFromChildSections();

            singleElementSettingsSection.OnDataUpdated -= RepaintSettingsWindow;
            singleElementSettingsSection.OnUIStateUpdated -= RebuildSettingsWindow;
            singleElementSettingsSection.OnColorUserModified -= OnColorUsersModified;
            
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
            logger.LogStart();
            logger.Log( "Calling Data Update Required Notify." );
            DataUpdateRequiredNotify();
            
            // UpdateAllColorUseCounts();
            
            // Todo: Make updates more efficient. All repainting set to rebuilding while ironing out updating issues.
            logger.Log( "Calling UI State Updated Notify." );
            UIStateUpdatedNotify();
            // DataUpdatedNotify();
            logger.LogEnd();
        }




#endregion

        private void OnValidate()
        {
            // logger.LogStart();
            
            // If the colorPaletteSettings list was changed, need to know.
            int newColorListCount = colorsPaletteSettings.GetColorListCount();
            if ( newColorListCount > _colorEntryListCount )
            {
                // logger.Log( $"Color list count has changed from {_colorEntryListCount.ToString()} to {newColorListCount.ToString()}!" );
                // logger.Log( $"Color list count has increased from {_colorEntryListCount.ToString()} to {newColorListCount.ToString()}!" );
                
                // Inform colorPaletteSettings of the change so the duplicate name can be modified.
                int totalNewColorEntries = newColorListCount - _colorEntryListCount;
                colorsPaletteSettings.ProcessNewColorEntry( totalNewColorEntries );
                
                _colorEntryListCount = newColorListCount;
                SubscribeToNewColor();
                UpdateAllColorUseCounts();
            }

            if ( newColorListCount < _colorEntryListCount )
            {
                // logger.Log( $"Color list count has decreased from {_colorEntryListCount.ToString()} to {newColorListCount.ToString()}!" );
                _colorEntryListCount = newColorListCount;
                UpdateAllColorUseCounts();
            }
            
            colorsPaletteSettings.ScanForListUpdates();

            // logger.LogEnd();
        }

        private void SubscribeToAllColors()
        {
            // Debug.Log( "AceTheme: Subscribing to all colors." );
            
            for (int i = 0; i < colorsPaletteSettings.GetColorListCount(); i++)
            {
                CustomColorEntry currentEntry = colorsPaletteSettings.GetColorEntryForIndex( i );
                // Debug.Log( $"    Subscribing to {currentEntry.customColor.name}." );
                currentEntry.OnNameUpdated -= ReplaceColorNameWithNewName;
                currentEntry.OnNameUpdated += ReplaceColorNameWithNewName;
            }
        }

        private void SubscribeToNewColor()
        {
            // logger.LogStart();
            CustomColorEntry newColorEntry = colorsPaletteSettings.GetColorEntryForIndex( colorsPaletteSettings.GetColorListCount() - 1 );
            // Note that this event is only triggered if the color passes the name check in ColorPaletteSettings.
            newColorEntry.OnNameUpdated += ReplaceColorNameWithNewName;
            // logger.LogEnd();
        }
        
        private void ReplaceColorNameWithNewName( string oldName, string newName )
        {
            logger.LogStart();
            // Look at colors in all settings groups to see if the old name is registered.
            // If found, replace it with the new one.
            logger.Log( $"Replacing all instances of {GetColoredStringAquamarine( oldName )} with {GetColoredStringCyan( newName )}." );

            // Loop through all lists to deal with the universal names.
            UpdateFrameSettingsNames( headingGroupFrameSettingsList, oldName, newName );
            UpdateFrameSettingsNames( basicGroupFrameSettingsList, oldName, newName );
            UpdateFrameSettingsNames( singleElementFrameSettingsList, oldName, newName );
            UpdateFrameSettingsNames( headingElementFrameSettingsList, oldName, newName );
            // Loop through headingElementFrameSettingsList to deal with its unique names.
            UpdateHeadingFrameSettingsNames( headingElementFrameSettingsList, oldName, newName );
            
            UpdateAllColorUseCounts();
            
            logger.LogEnd();
        }

        private void UpdateFrameSettingsNames( IReadOnlyList<FrameSettings> frameSettings, string oldName, string newName )
        {
            for (int i = 0; i < Levels; i++)
            {
                if ( frameSettings[i].frameOutlineColorName.Equals( oldName ) ) 
                    frameSettings[i].frameOutlineColorName = newName;
                if ( frameSettings[i].backgroundActiveColorName.Equals( oldName ) ) 
                    frameSettings[i].backgroundActiveColorName = newName;
            }
        }
         
        private void UpdateHeadingFrameSettingsNames( IReadOnlyList<HeadingElementFrameSettings> frameSettings, string oldName, string newName )
        {
            for (int i = 0; i < Levels; i++)
            {
                if ( frameSettings[i].enabledTextColorName.Equals( oldName ) ) 
                    frameSettings[i].enabledTextColorName = newName;
                if ( frameSettings[i].disabledTextColorName.Equals( oldName ) ) 
                    frameSettings[i].disabledTextColorName = newName;
                if ( frameSettings[i].backgroundInactiveColorName.Equals( oldName ) ) 
                    frameSettings[i].backgroundInactiveColorName = newName;
            }
        }

        public void OnColorUsersModified()
        {
            UpdateAllColorUseCounts();
            UIStateUpdatedNotify();
        }
        
        public void UpdateAllColorUseCounts()
        {
            // Get the size of the custom colors list.
            int colorListSize = colorsPaletteSettings.GetColorListLength();
            for (int i = 0; i < colorListSize; i++)
            {
                CustomColorEntry currentColorEntry = colorsPaletteSettings.GetColorEntryForIndex( i );
                currentColorEntry.userCount = 0;
                currentColorEntry.userList = "Users:";
                GetNumberOfUsesForColor( currentColorEntry );
            }
        }

        /// <summary>
        ///     Returns the total count of all uses of the color.
        /// </summary>
        /// <param name="colorEntry"></param>
        /// <returns></returns>
        private void GetNumberOfUsesForColor( CustomColorEntry colorEntry )
        {
            colorEntry.userList = "Users:";
            GetNumberOfUniversalUsesForColor( "Heading Group", headingGroupFrameSettingsList, headingGroupLevelSettingsMode, colorEntry );
            GetNumberOfUniversalUsesForColor( "Basic Group", basicGroupFrameSettingsList, basicGroupLevelSettingsMode, colorEntry );
            GetNumberOfUniversalUsesForColor( "Single Element", singleElementFrameSettingsList, singleElementsLevelSettingsMode, colorEntry );
            GetNumberOfUniversalUsesForColor( "Heading Element", headingElementFrameSettingsList, headingGroupLevelSettingsMode, colorEntry );
            if ( colorEntry.userList.Equals( "Users:" ) ) colorEntry.userList += " none";
        }
        
        /// <summary>
        ///     Records all color uses on all active settings level for this settings set.
        /// </summary>
        private void GetNumberOfUniversalUsesForColor( 
            string settingsName, 
            IReadOnlyList<FrameSettings> frameSettings, 
            LevelSettingsSection.LevelSettingsMode levelSettingsMode, 
            CustomColorEntry colorEntry )
        {
            // Stores the list of users for this color entry. If none are found, this string will still be empty by the end of the method.
            string listOfUsers = "";

            for (int i = 0; i < Levels; i++)
            {
                // Stores the list of users for this level. If none are found, this string will still be empty by the end of this loop.
                string currentLevelOutput = "";
                
                // Only count the active settings levels.
                if ( levelSettingsMode == LevelSettingsSection.LevelSettingsMode.AllUseRootLevel && i > 0 ) continue;
                if ( levelSettingsMode == LevelSettingsSection.LevelSettingsMode.AllChildrenUseLevel1 && i > 1 ) continue;

                FrameSettings currentFrameSettings = frameSettings[i];
                
                // All heading element frame settings only.
                if ( currentFrameSettings.GetType() == typeof( HeadingElementFrameSettings ) )
                {
                    var currentHeadingElementFrameSetting = (HeadingElementFrameSettings) frameSettings[i];
                    
                    if ( currentHeadingElementFrameSetting.enabledTextColorName.Equals( colorEntry.name ) ) 
                        currentLevelOutput = UpdateColorEntry( currentLevelOutput, colorEntry, "Enabled Text" );

                    if ( currentHeadingElementFrameSetting.disabledTextColorName.Equals( colorEntry.name ) ) 
                        currentLevelOutput = UpdateColorEntry( currentLevelOutput, colorEntry, "Disabled Text" );
                }

                // If framing is not used for this level of settings, stop here.
                if ( !currentFrameSettings.applyFraming )
                {
                    listOfUsers = ProcessColorUsersLine( currentLevelOutput, listOfUsers, i );
                    continue;
                }
                
                if ( currentFrameSettings.includeOutline && currentFrameSettings.frameOutlineColorName.Equals( colorEntry.name ) )
                    currentLevelOutput = UpdateColorEntry( currentLevelOutput, colorEntry, "Outlines" );

                if ( currentFrameSettings.includeBackground && currentFrameSettings.backgroundActiveColorName.Equals( colorEntry.name ) )
                    currentLevelOutput = UpdateColorEntry( currentLevelOutput, colorEntry, "Background" );

                // All heading element frame settings only.
                if ( currentFrameSettings.GetType() == typeof( HeadingElementFrameSettings ) )
                {
                    var currentHeadingElementFrameSetting = (HeadingElementFrameSettings) frameSettings[i];
                    if ( currentHeadingElementFrameSetting.backgroundInactiveColorName.Equals( colorEntry.name ) )
                    {
                        currentLevelOutput = UpdateColorEntry( currentLevelOutput, colorEntry, "Inactive Background" );
                    }
                }
                
                listOfUsers = ProcessColorUsersLine( currentLevelOutput, listOfUsers, i );
            }

            // If any users were recorded for this color, append the list to the color entry.
            if ( listOfUsers.Equals( "" ) ) return;
            
            if ( !listOfUsers.Equals( "" ) ) listOfUsers += "\n";
            colorEntry.userList += $"\n    {settingsName}:{listOfUsers}";
        }
        
        private string UpdateColorEntry( string currentLevelOutput, CustomColorEntry colorEntry, string colorUseName )
        {
            // Add new line char if this isn't the first entry this loop.
            if ( !currentLevelOutput.Equals( "" ) ) currentLevelOutput += "\n";
                    
            colorEntry.userCount++;
            currentLevelOutput += $"            {colorUseName}";
            return currentLevelOutput;
        }

        private string ProcessColorUsersLine( string currentLevelOutput, string listOfUsers, int settingsLevel )
        {
            // If there were any users of the current color on this level, add the level entries to the output string.
            if ( currentLevelOutput.Equals( "" ) ) return listOfUsers;
            
            string levelName = ( settingsLevel == 0 ) ? "Root LVL" : $"LVL{settingsLevel.ToString()}";
            listOfUsers += $"\n        {levelName}:\n{currentLevelOutput}";

            return listOfUsers;
        }

        // private void GetNumberOfHeadingUsesForColor( 
        //     IReadOnlyList<HeadingElementFrameSettings> frameSettings, 
        //     LevelSettingsSection.LevelSettingsMode levelSettingsMode, 
        //     CustomColorEntry colorEntry )
        // {
        //     
        //     for (int i = 0; i < Levels; i++)
        //     {
        //         if ( frameSettings[i].GetType() == typeof( HeadingElementFrameSettings) ) Debug.Log( "Found heading element frame settings." );
        //
        //         // Only count the active settings levels.
        //         if ( levelSettingsMode == LevelSettingsSection.LevelSettingsMode.AllUseRootLevel && i > 0 ) continue;
        //         if ( levelSettingsMode == LevelSettingsSection.LevelSettingsMode.AllChildrenUseLevel1 && i > 1 ) continue;
        //
        //         HeadingElementFrameSettings currentFrameSettings = frameSettings[i];
        //         
        //         if ( currentFrameSettings.enabledTextColorName.Equals( colorEntry.name ) )
        //         {
        //             colorEntry.userCount++;
        //             colorEntry.userList += $"\nHeading Group / Heading Enabled Text LVL{i.ToString()}";
        //         }
        //         
        //         if ( currentFrameSettings.disabledTextColorName.Equals( colorEntry.name ) )
        //         {
        //             colorEntry.userCount++;
        //             colorEntry.userList += $"\nHeading Group / Heading Disabled Text LVL{i.ToString()}";
        //         }
        //
        //         if ( !currentFrameSettings.applyFraming ) continue;
        //         
        //         if ( currentFrameSettings.includeOutline && currentFrameSettings.frameOutlineColorName.Equals( colorEntry.name ) )
        //         {
        //             colorEntry.userCount++;
        //             colorEntry.userList += $"\nHeading Group / Heading Frame Outline LVL{i.ToString()}";
        //         }
        //         
        //         if ( !currentFrameSettings.includeBackground ) continue;
        //         
        //         if ( currentFrameSettings.backgroundActiveColorName.Equals( colorEntry.name ) )
        //         {
        //             colorEntry.userCount++;
        //             colorEntry.userList += $"\nHeading Group / Heading Active Background LVL{i.ToString()}";
        //         }
        //         
        //         if ( currentFrameSettings.backgroundInactiveColorName.Equals( colorEntry.name ) )
        //         {
        //             colorEntry.userCount++;
        //             colorEntry.userList += $"\nHeading Group / Heading Inactive Background LVL{i.ToString()}";
        //         }
        //     }
        // }




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
                    GetColorPaletteElement(),
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
                    SettingsSectionOptions.Colors => GetColorPaletteElement(),
                    SettingsSectionOptions.SingleElements => GetSinglesElement(),
                    SettingsSectionOptions.PropertyElements => GetPropertiesElement(),
                    _ => throw new ArgumentOutOfRangeException()
                }
            };
        }

        private Element GetGlobalSettingsElement() => globalSettingsSection.GetSection();
        private Element GetColorPaletteElement()
        {
            UpdateAllColorUseCounts();
            return colorPaletteSection.GetSection();
        }

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
        public static string GetFoldoutGroupSettingsListVarName => nameof( headingGroupFrameSettingsList );
        // public static string GetToggleGroupSettingsListVarName => nameof( toggleGroupSettingsList );
        // public static string GetLabeledGroupInfoListVarName => nameof( labeledGroupInfoList );
        
        public static string GetSingleElementSettingsSectionVarName => nameof( singleElementSettingsSection );
        public static string GetSingleElementSettingsListVarName => nameof( singleElementSettingsList );
        public static string GetSingleElementFrameSettingsListVarName => nameof( singleElementFrameSettingsList );
        
        public static string GetPropertySpecificSettingsVarName => nameof( propertySpecificSettings );

        public static string GetCustomColorListVarName => ColorPaletteSettings.GetCustomColorListVarName();
        public static string GetBackupColorVarName => ColorPaletteSettings.GetBackupColorVarName();


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
        //     CustomColorEntry colorEntry = colorsPaletteSettings.GetColorEntryForIndex( selectedIndex );
        //     return colorPaletteSection.GetColorSelectionElement( title, tooltip, selectedIndex,
        //         selectedIndexRelativeVarName,
        //         colorEntry.locked,
        //         callback, filter );
        // }
        
        // /// <summary>
        // ///     Use to link colors by their names to keep the link independent of the color's position in the custom
        // ///     colors list.
        // /// </summary>
        // public Element GetColorSelectionElement(
        //     string title, string tooltip,
        //     string colorName, 
        //     string selectedIndexRelativeVarName,
        //     Action callback,
        //     params ElementCondition[] filter )
        // {
        //     return colorPaletteSection.GetColorSelectionElement( title, tooltip, colorName,
        //         selectedIndexRelativeVarName,
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
            // Hijacked the callback here to include the update all color use counts call.
            // The callbacks just led back to this to call data update required notify and rebuild settings window.
            return colorPaletteSection.GetColorSelectionElement( title, tooltip, colorName,
                selectedIndexRelativeVarName,
                OnColorDropdownChanged, filter );
        }

        private void OnColorDropdownChanged()
        {
            DataUpdateRequiredNotify();
            UpdateAllColorUseCounts();
            RebuildSettingsWindow();
        }
        
        // public Element GetColorSelectionElement(
        //     string title, string tooltip,
        //     int selectedIndex, string selectedIndexRelativeVarName,
        //     params ElementCondition[] filter )
        // {
        //     return colorPaletteSection.GetColorSelectionElement( title, tooltip, selectedIndex,
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
                ? headingGroupFrameSettingsList[GetLevelBasedOnMode( level, headingGroupLevelSettingsMode )]
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
            return headingGroupFrameSettingsList[GetLevelBasedOnMode( level, headingGroupLevelSettingsMode )];
        }
        
        public FrameSettings GetCurrentModeHeadingGroupFrameSettings( int level )
        {
            return headingGroupSectionState switch
            {
                HeadingGroupSectionState.FoldoutGroups => headingGroupFrameSettingsList[level],
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


        // public Color GetColorForIndex( int index ) => colorsPaletteSettings.GetColorForIndex( index );
        public string[] GetColorEntryOptions() => colorsPaletteSettings.GetCustomColorsOptionsList();
        
        /// <summary>
        /// This is used by draw classes to convert the name into a usable color. Note that the index is never used.
        /// </summary>
        public Color GetColorForColorName( string colorName ) => colorsPaletteSettings.GetColorForColorName( colorName );

        /// <summary>
        /// This should be used only for when a popup element needs to translate the color into a selection number.
        /// </summary>
        public int GetIndexForColorName( string colorEntryName ) => colorsPaletteSettings.GetIndexForColorName( colorEntryName );
        /// <summary>
        /// This should be used only for when a popup needs to convert the selected index back into a color name.
        /// </summary>
        public string GetColorNameForIndex( int index ) => colorsPaletteSettings.GetColorNameForIndex( index );

        public PropertySpecificSettings GetPropertySpecificSettings() => propertySpecificSettings;

#endregion

    }
}
