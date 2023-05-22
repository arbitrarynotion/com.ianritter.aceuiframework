using System;
using Packages.com.ianritter.unityscriptingtools.Runtime.Services.CustomLogger;
using UnityEngine;
using static Packages.com.ianritter.aceuiframework.Runtime.Scripts.AceEditorConstants;
using static Packages.com.ianritter.unityscriptingtools.Runtime.Services.TextFormatting.TextFormat;

namespace Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsGlobal.Colors
{
    [Serializable]
    public class CustomColorSettings
    {
        private const float OutlineBrightness = 0.32f;
        private const float HeadingRootBackgroundBrightness = 0.18f;
        private const float HeadingChildBackgroundBrightness = 0.25f;
        private const float HeadingTextEnabledBrightness = 1f;
        private const float HeadingTextDisabledBrightness = 0.5f;

        private CustomLogger _logger;
        
        [SerializeField] private CustomColorEntry[] customColorsList;
        // This will give the property field something to work with when there are no entries in the color list. 
        [SerializeField] private CustomColorEntry backupColor = new CustomColorEntry( "Backup Color", Color.magenta) {toggle = true};
        
        public CustomColorSettings()
        {
            
            InitializeCustomColors();
        }

        public void Initialize( CustomLogger logger )
        {
            _logger = logger;
        }

        public int GetColorListCount() => customColorsList.Length;

        private void InitializeCustomColors()
        {
            const int totalColors = 5;

            if ( customColorsList != null ) return;
            
            // Set up default colors.
            customColorsList = new CustomColorEntry[totalColors];

            int currentIndex = 0;
            customColorsList[currentIndex++] = new CustomColorEntry( CustomColorOutlinesName,
                new Color( OutlineBrightness, OutlineBrightness, OutlineBrightness ) );
            customColorsList[currentIndex++] = new CustomColorEntry( CustomColorRootBackgroundName,
                new Color( HeadingRootBackgroundBrightness, HeadingRootBackgroundBrightness, HeadingRootBackgroundBrightness ) );
            customColorsList[currentIndex++] = new CustomColorEntry( CustomColorChildBackgroundName,
                new Color( HeadingChildBackgroundBrightness, HeadingChildBackgroundBrightness, HeadingChildBackgroundBrightness ) );
            customColorsList[currentIndex++] = new CustomColorEntry( CustomColorEnabledTextName,
                new Color( HeadingTextEnabledBrightness, HeadingTextEnabledBrightness, HeadingTextEnabledBrightness ) );
            customColorsList[currentIndex++] = new CustomColorEntry( CustomColorDisabledTextName,
                new Color( HeadingTextDisabledBrightness, HeadingTextDisabledBrightness, HeadingTextDisabledBrightness ) );
            
            for (int i = currentIndex; i < totalColors; i++)
            {
                customColorsList[i] = new CustomColorEntry( ( "Color " + ( i + 1 ).ToString() ), 
                    new Color( HeadingChildBackgroundBrightness, HeadingChildBackgroundBrightness, HeadingChildBackgroundBrightness));
            }

            // SubscribeToAllColorEntries();
        }

        // private void SubscribeToAllColorEntries()
        // {
        //     foreach ( CustomColorEntry customColorEntry in customColorsList )
        //     {
        //         customColorEntry.OnNameUpdated += OnColorEntryUpdated;
        //     }
        // }
        //
        // private void OnColorEntryUpdated( string nameOfColorThatChanged )
        // {
        //     Debug.Log( $"CustomColorSettings: Notified that {GetColoredStringYellow(nameOfColorThatChanged)} was updated." );
        // }

        // public void SubscribeToNewColor()
        // {
        //     int listLength = customColorsList.Length;
        //     // The new element is always the last in the list.
        //     customColorsList[listLength - 1].OnNameUpdated += OnColorEntryUpdated;
        //     Debug.Log( $"CustomColorSettings: Subscribed to new color {customColorsList[listLength - 1]} at index {( listLength - 1 ).ToString()}." );
        // }

        
        public void ProcessNewColorEntry()
        {
            _logger.LogStart();
            // A color has been added to the list. As it was added via the + button on the reorderable list,
            // the CustomColorEntry's default constructor was called and Unity copied the serialized data
            // of the previous name onto the new entry. This bypasses the copy constructor so I can't intercept it there
            // either. My only option is to monitor the array size in AceTheme, then to modify the new entry
            // manually here before the AceTheme subscribes to respond to name changes.
            // There are two main cases:
            // 1. The list was empty so this color has no name (Default values are ignored).
            // 2. The list was not empty so the color will be an exact duplicate of the one before it.
            
            // Get new color.
            CustomColorEntry newColorEntry = customColorsList[customColorsList.Length - 1];

            bool entryIsBlank = newColorEntry.name.Equals( "" ) & newColorEntry.color == new Color( 0f, 0f, 0f, 0f );
            newColorEntry.name = entryIsBlank ? "New Color" : $"{newColorEntry.name}_copy";
            newColorEntry.color = entryIsBlank ? new Color( 0f, 0f, 0f, 1f ) : newColorEntry.color;
            newColorEntry.previousName = "";

            _logger.LogEnd();
        }

        /// <summary>
        ///     Scan through the custom colors list and fire the NameUpdated event on any that were changed.
        /// </summary>
        public void ScanForListUpdates()
        {
            _logger.LogStart();
            
            foreach ( CustomColorEntry customColorEntry in customColorsList )
            {
                if ( !customColorEntry.wasUpdated ) continue;
                // _logger.Log( $"A name change was detected in {customColorEntry.customColor.name}. Notifying subscribers." );
                customColorEntry.wasUpdated = false;
                _logger.Log( $"{customColorEntry.name} was updated from {customColorEntry.previousName}." );

                // Check to make sure the name isn't a duplicate of an existing name.
                if ( UpdatedNameIsADuplicate( customColorEntry ) ) continue;

                customColorEntry.NameUpdatedNotify();
            }

            _logger.LogEnd();
        }

        private bool UpdatedNameIsADuplicate( CustomColorEntry changedColorEntry )
        {
            _logger.LogStart();
            foreach ( CustomColorEntry customColorEntry in customColorsList )
            {
                if ( changedColorEntry == customColorEntry ) continue;
                if ( !changedColorEntry.name.Equals( customColorEntry.name ) ) continue;
                
                // The name is a duplicate. Revert it back to previousName and return true.
                _logger.LogEnd( $"{changedColorEntry.name} is a duplicate. Reverting name to {changedColorEntry.previousName}." );
                changedColorEntry.name = changedColorEntry.previousName;
                return true;
            }
            _logger.LogEnd();
            return false;
        }

        // public void SubscribeToColorEntryByName( string colorName, Func<string> callback )
        // {
        //     foreach ( CustomColorEntry customColorEntry in customColorsList )
        //     {
        //         if ( !customColorEntry.customColor.name.Equals( colorName ) ) continue;
        //         customColorEntry.OnNameUpdated += callback;
        //     }
        // }

        public string[] GetCustomColorsOptionsList()
        {
            int colorsListLength = customColorsList.Length;
            string[] customColorOptions = new string[colorsListLength];
            for (int i = 0; i < colorsListLength; i++)
            {
                customColorOptions[i] = customColorsList[i].name;
            }

            return customColorOptions;
        }
        
        // public string[] GetCustomColorsOptionsList()
        // {
        //     int colorsListLength = customColorsList.Length;
        //     string[] customColorOptions = new string[colorsListLength + 1];
        //     customColorOptions[0] = "Custom";
        //     for (int i = 1; i < colorsListLength; i++)
        //     {
        //         customColorOptions[i] = customColorsList[i].name;
        //     }
        //
        //     return customColorOptions;
        // }

        public int GetIndexForCustomColorName( string customColorName )
        {
            // What happens when the name can't be found?
            for (int i = 0; i < customColorsList.Length; i++)
            {
                if ( customColorsList[i].name.Equals( customColorName ) )
                    return i;
            }

            return -1;
        }
        
        public CustomColorEntry GetColorEntryForIndex( int index )
        {
            if (index >= 0 && index < customColorsList.Length)
                return customColorsList[index];

            // Todo: Find a better workaround for when a color that is in use is deleted from the custom colors array.
            Debug.LogWarning( $"CT|GCFI: Error! Color index is out of range ({index.ToString()} out of {customColorsList.Length.ToString()})!" );
            return new CustomColorEntry( "Failed to Load", Color.magenta );
        }

        private Color GetColorForIndex( int index )
        {
            if (index >= 0 && index < customColorsList.Length)
                return customColorsList[index].color;

            // Debug.LogWarning( $"CT|GCFI: Error! Color index is out of range ({index.ToString()} out of {customColorsList.Length.ToString()})!" );
            return Color.magenta;
        }

        public static string GetCustomColorListVarName() => nameof( customColorsList );
        public static string GetBackupColorVarName() => nameof( backupColor );

        public string GetColorNameForIndex( int index )
        {
            if ( index >= customColorsList.Length ) throw new IndexOutOfRangeException();

            return customColorsList[index].name;
        }

        public Color GetColorForColorName( string colorName )
        {
            int index = GetIndexForCustomColorName( colorName );
            
            // Note that the color name is not overwritten if the color name is not found.
            // This is to allow the user to recover from an accidental color deletion. As long as a matching name is
            // provided without changing the color selected, the correct color will return.
            return ( index == -1 ) ? Color.magenta : GetColorForIndex( index );
        }
    }
}
