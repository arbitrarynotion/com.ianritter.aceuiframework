using System;
using UnityEngine;
using Packages.com.ianritter.unityscriptingtools.Runtime.Services.CustomLogger;
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
        // This color entry is used when a color can't be found.
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
            
            colorEntryList = new CustomColorEntry[totalColors];

            // Set up default colors.
            int currentIndex = 0;
            colorEntryList[currentIndex++] = new CustomColorEntry( CustomColorOutlinesName, new Color( OutlineBrightness, OutlineBrightness, OutlineBrightness ) );
            colorEntryList[currentIndex++] = new CustomColorEntry( CustomColorRootBackgroundName, new Color( HeadingRootBackgroundBrightness, HeadingRootBackgroundBrightness, HeadingRootBackgroundBrightness ) );
            colorEntryList[currentIndex++] = new CustomColorEntry( CustomColorChildBackgroundName, new Color( HeadingChildBackgroundBrightness, HeadingChildBackgroundBrightness, HeadingChildBackgroundBrightness ) );
            colorEntryList[currentIndex++] = new CustomColorEntry( CustomColorEnabledTextName, new Color( HeadingTextEnabledBrightness, HeadingTextEnabledBrightness, HeadingTextEnabledBrightness ) );
            colorEntryList[currentIndex++] = new CustomColorEntry( CustomColorDisabledTextName, new Color( HeadingTextDisabledBrightness, HeadingTextDisabledBrightness, HeadingTextDisabledBrightness ) );
            
            // If the totalColors is set to more than the colors provided above, this sets those extra colors do a default.
            for (int i = currentIndex; i < totalColors; i++)
            {
                colorEntryList[i] = new CustomColorEntry( ( "Color " + ( i + 1 ).ToString() ), 
                    new Color( HeadingChildBackgroundBrightness, HeadingChildBackgroundBrightness, HeadingChildBackgroundBrightness));
            }
        }

        public void ProcessNewColorEntries( int totalNewColorEntries )
        {
            // A color has been added to the list. As it was added via the + button on the reorderable list,
            // the CustomColorEntry's default constructor was called and Unity copied the serialized data
            // of the previous name onto the new entry. This bypasses the copy constructor so I can't intercept it there
            // either. My only option is to monitor the array size in AceTheme, then to modify the new entry
            // manually here before the AceTheme subscribes to respond to name changes.
            // There are two main cases:
            // 1. The list was empty so this color has no name (Default values are ignored).
            // 2. The list was not empty so the color will be an exact duplicate of the one before it.
            // 3. A color was deleted but the action was undone. As such, the list was not empty and this isn't a duplicate.

            // As the size of the list can be arbitrarily changed via the size field and that field can't be disabled in a reorderable list,
            // the total number of new entries will be 1 or more.
            int startIndex = colorEntryList.Length - totalNewColorEntries;
            int nameCount = 1;
            for (int i = startIndex; i < colorEntryList.Length; i++)
            {
                CustomColorEntry newColorEntry = colorEntryList[i];
                
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
            foreach ( CustomColorEntry customColorEntry in colorEntryList )
            {
                if ( !customColorEntry.wasUpdated ) continue;
                customColorEntry.wasUpdated = false;

                // Check to make sure the name isn't a duplicate of an existing name.
                if ( ListContainsName( customColorEntry ) )
                {
                    // The name is a duplicate. Revert it back to previousName and return true.
                    customColorEntry.name = customColorEntry.previousName;
                    continue;
                }

                customColorEntry.NameUpdatedNotify();
            }
        }

        private bool ListContainsName( CustomColorEntry changedColorEntry )
        {
            foreach ( CustomColorEntry customColorEntry in colorEntryList )
            {
                // Ignore self.
                if ( changedColorEntry == customColorEntry ) continue;
                if ( !changedColorEntry.name.Equals( customColorEntry.name ) ) continue;
                return true;
            }
            return false;
        }

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
            // Debug.LogWarning( $"CT|GCFI: Error! Color index is out of range ({index.ToString()} out of {colorEntryList.Length.ToString()})!" );
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
