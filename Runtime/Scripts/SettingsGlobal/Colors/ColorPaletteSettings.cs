using System;
using Packages.com.ianritter.unityscriptingtools.Runtime.Services.CustomLogger;
using UnityEngine;
using static Packages.com.ianritter.aceuiframework.Runtime.Scripts.AceEditorConstants;
using static Packages.com.ianritter.unityscriptingtools.Runtime.Services.TextFormatting.TextFormat;

namespace Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsGlobal.Colors
{
    [Serializable]
    public class ColorPaletteSettings
    {
        private const float OutlineBrightness = 0.32f;
        private const float HeadingRootBackgroundBrightness = 0.18f;
        private const float HeadingChildBackgroundBrightness = 0.25f;
        private const float HeadingTextEnabledBrightness = 1f;
        private const float HeadingTextDisabledBrightness = 0.5f;

        private CustomLogger _logger;
        
        [SerializeField] private CustomColorEntry[] colorEntryList;
        // This will give the property field something to work with when there are no entries in the color list. 
        [SerializeField] private CustomColorEntry backupColor = new CustomColorEntry( "Backup Color", Color.magenta) {locked = true};
        
        public ColorPaletteSettings()
        {
            
            InitializeCustomColors();
        }

        public void Initialize( CustomLogger logger )
        {
            _logger = logger;
        }

        public int GetColorListCount() => colorEntryList.Length;

        private void InitializeCustomColors()
        {
            const int totalColors = 5;

            if ( colorEntryList != null ) return;
            
            // Set up default colors.
            colorEntryList = new CustomColorEntry[totalColors];

            int currentIndex = 0;
            colorEntryList[currentIndex++] = new CustomColorEntry( CustomColorOutlinesName, new Color( OutlineBrightness, OutlineBrightness, OutlineBrightness ) );
            colorEntryList[currentIndex++] = new CustomColorEntry( CustomColorRootBackgroundName, new Color( HeadingRootBackgroundBrightness, HeadingRootBackgroundBrightness, HeadingRootBackgroundBrightness ) );
            colorEntryList[currentIndex++] = new CustomColorEntry( CustomColorChildBackgroundName, new Color( HeadingChildBackgroundBrightness, HeadingChildBackgroundBrightness, HeadingChildBackgroundBrightness ) );
            colorEntryList[currentIndex++] = new CustomColorEntry( CustomColorEnabledTextName, new Color( HeadingTextEnabledBrightness, HeadingTextEnabledBrightness, HeadingTextEnabledBrightness ) );
            colorEntryList[currentIndex++] = new CustomColorEntry( CustomColorDisabledTextName, new Color( HeadingTextDisabledBrightness, HeadingTextDisabledBrightness, HeadingTextDisabledBrightness ) );
            
            for (int i = currentIndex; i < totalColors; i++)
            {
                colorEntryList[i] = new CustomColorEntry( ( "Color " + ( i + 1 ).ToString() ), 
                    new Color( HeadingChildBackgroundBrightness, HeadingChildBackgroundBrightness, HeadingChildBackgroundBrightness));
            }

            // SubscribeToAllColorEntries();
        }

        // private void SubscribeToAllColorEntries()
        // {
        //     foreach ( CustomColorEntry customColorEntry in colorEntryList )
        //     {
        //         customColorEntry.OnNameUpdated += OnColorEntryUpdated;
        //     }
        // }
        //
        // private void OnColorEntryUpdated( string nameOfColorThatChanged )
        // {
        //     Debug.Log( $"ColorPaletteSettings: Notified that {GetColoredStringYellow(nameOfColorThatChanged)} was updated." );
        // }

        // public void SubscribeToNewColor()
        // {
        //     int listLength = colorEntryList.Length;
        //     // The new element is always the last in the list.
        //     colorEntryList[listLength - 1].OnNameUpdated += OnColorEntryUpdated;
        //     Debug.Log( $"ColorPaletteSettings: Subscribed to new color {colorEntryList[listLength - 1]} at index {( listLength - 1 ).ToString()}." );
        // }

        
        public void ProcessNewColorEntry( int totalNewColorEntries )
        {
            if ( _logger != null ) _logger.LogStart();
            // A color has been added to the list. As it was added via the + button on the reorderable list,
            // the CustomColorEntry's default constructor was called and Unity copied the serialized data
            // of the previous name onto the new entry. This bypasses the copy constructor so I can't intercept it there
            // either. My only option is to monitor the array size in AceTheme, then to modify the new entry
            // manually here before the AceTheme subscribes to respond to name changes.
            // There are two main cases:
            // 1. The list was empty so this color has no name (Default values are ignored).
            // 2. The list was not empty so the color will be an exact duplicate of the one before it.
            // 3. A color was deleted but the action was undone. As such, the list was not empty and this isn't a duplicate.
            
            // Get new color.
            // CustomColorEntry newColorEntry = colorEntryList[colorEntryList.Length - 1];

            // As the size of the list can be arbitrarily changed via the size field and that field can't be disabled in a reorderable list,
            // the total number of new entries will be 1 or more.
            int startIndex = colorEntryList.Length - totalNewColorEntries;
            int nameCount = 1;
            // Debug.Log( $"Checking new entries. List size: {colorEntryList.Length.ToString()}, new entry count: {totalNewColorEntries.ToString()}, starting index: {startIndex.ToString()}" );
            for (int i = startIndex; i < colorEntryList.Length; i++)
            {
                CustomColorEntry newColorEntry = colorEntryList[i];
                // Debug.Log( $"Processing entry {i.ToString()}: '{newColorEntry.name}'" );
                
                // if ( newColorEntry.name.Equals( "" ) & ( newColorEntry.color == new Color( 0f, 0f, 0f, 0f ) ) )
                if ( newColorEntry.name.Equals( "" ) & ( newColorEntry.color == new Color( 0f, 0f, 0f, 0f ) ) )
                {
                    newColorEntry.name = $"New Color {nameCount.ToString()}";
                    newColorEntry.color = new Color( 0f, 0f, 0f, 1f );
                }
            
                // Edge case: a color named "New Color" exists and the user duplicates a color with a blank name and color.
                // The "New Color" name assigned would then be a duplicate name. So a check for a duplicate is done to catch this case.
            
                if ( ListContainsName( newColorEntry ) ) newColorEntry.name = $"{newColorEntry.name}_copy {nameCount.ToString()}";
            
                newColorEntry.previousName = "";
                nameCount++;
            }
            
            if ( _logger != null ) _logger.LogEnd();
        }

        /// <summary>
        ///     Scan through the custom colors list and fire the NameUpdated event on any that were changed.
        /// </summary>
        public void ScanForListUpdates()
        {
            // _logger.LogStart();
            
            foreach ( CustomColorEntry customColorEntry in colorEntryList )
            {
                if ( !customColorEntry.wasUpdated ) continue;
                // _logger.Log( $"A name change was detected in {customColorEntry.customColor.name}. Notifying subscribers." );
                customColorEntry.wasUpdated = false;
                // _logger.Log( $"{customColorEntry.name} was updated from {customColorEntry.previousName}." );

                // Check to make sure the name isn't a duplicate of an existing name.
                if ( ListContainsName( customColorEntry ) )
                {
                    // The name is a duplicate. Revert it back to previousName and return true.
                    // _logger.LogEnd( $"{changedColorEntry.name} is a duplicate. Reverting name to {changedColorEntry.previousName}." );
                    customColorEntry.name = customColorEntry.previousName;
                    continue;
                }

                customColorEntry.NameUpdatedNotify();
            }

            // _logger.LogEnd();
        }

        private bool ListContainsName( CustomColorEntry changedColorEntry )
        {
            // _logger.LogStart();
            foreach ( CustomColorEntry customColorEntry in colorEntryList )
            {
                // Ignore self.
                if ( changedColorEntry == customColorEntry ) continue;
                if ( !changedColorEntry.name.Equals( customColorEntry.name ) ) continue;
                return true;
            }
            // _logger.LogEnd();
            return false;
        }

        // public void SubscribeToColorEntryByName( string colorName, Func<string> callback )
        // {
        //     foreach ( CustomColorEntry customColorEntry in colorEntryList )
        //     {
        //         if ( !customColorEntry.customColor.name.Equals( colorName ) ) continue;
        //         customColorEntry.OnNameUpdated += callback;
        //     }
        // }

        public string[] GetCustomColorsOptionsList()
        {
            int colorsListLength = colorEntryList.Length;
            string[] customColorOptions = new string[colorsListLength];
            for (int i = 0; i < colorsListLength; i++)
            {
                customColorOptions[i] = colorEntryList[i].name;
            }

            return customColorOptions;
        }
        
        // public string[] GetCustomColorsOptionsList()
        // {
        //     int colorsListLength = colorEntryList.Length;
        //     string[] customColorOptions = new string[colorsListLength + 1];
        //     customColorOptions[0] = "Custom";
        //     for (int i = 1; i < colorsListLength; i++)
        //     {
        //         customColorOptions[i] = colorEntryList[i].name;
        //     }
        //
        //     return customColorOptions;
        // }

        public int GetIndexForColorName( string customColorName )
        {
            // What happens when the name can't be found?
            for (int i = 0; i < colorEntryList.Length; i++)
            {
                if ( colorEntryList[i].name.Equals( customColorName ) )
                    return i;
            }

            return -1;
        }
        
        public CustomColorEntry GetColorEntryForIndex( int index )
        {
            if (index >= 0 && index < colorEntryList.Length)
                return colorEntryList[index];

            // Todo: Find a better workaround for when a color that is in use is deleted from the custom colors array.
            Debug.LogWarning( $"CT|GCFI: Error! Color index is out of range ({index.ToString()} out of {colorEntryList.Length.ToString()})!" );
            return new CustomColorEntry( "Failed to Load", Color.magenta );
        }

        private Color GetColorForIndex( int index )
        {
            if (index >= 0 && index < colorEntryList.Length)
                return colorEntryList[index].color;

            // Debug.LogWarning( $"CT|GCFI: Error! Color index is out of range ({index.ToString()} out of {colorEntryList.Length.ToString()})!" );
            return Color.magenta;
        }

        public static string GetCustomColorListVarName() => nameof( colorEntryList );
        public static string GetBackupColorVarName() => nameof( backupColor );

        public string GetColorNameForIndex( int index )
        {
            if ( index >= colorEntryList.Length ) throw new IndexOutOfRangeException();

            return colorEntryList[index].name;
        }

        public Color GetColorForColorName( string colorName )
        {
            int index = GetIndexForColorName( colorName );
            
            // Note that the color name is not overwritten if the color name is not found.
            // This is to allow the user to recover from an accidental color deletion. As long as a matching name is
            // provided without changing the color selected, the correct color will return.
            return ( index == -1 ) ? Color.magenta : GetColorForIndex( index );
        }

        public int GetColorListLength() => colorEntryList.Length;

        public CustomColorEntry GetColorEntryAtIndex( int index ) => index >= colorEntryList.Length ? null : colorEntryList[index];
    }
}
