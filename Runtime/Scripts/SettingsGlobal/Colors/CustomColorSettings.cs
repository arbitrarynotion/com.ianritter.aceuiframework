using System;
using UnityEngine;
using static Packages.com.ianritter.aceuiframework.Runtime.Scripts.AceEditorConstants;

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
        
        [SerializeField] private CustomColorEntry[] customColorsList;
        
        public CustomColorSettings() => Initialize();

        private void Initialize() => InitializeCustomColors();

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
        }

        public string[] GetCustomColorsOptionsList()
        {
            int colorsListLength = customColorsList.Length;
            string[] customColorOptions = new string[colorsListLength];
            for (int i = 0; i < colorsListLength; i++)
            {
                customColorOptions[i] = customColorsList[i].customColor.name;
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
            for (int i = 0; i < customColorsList.Length; i++)
            {
                if ( customColorsList[i].customColor.name.Equals( customColorName ) )
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
        
        public Color GetColorForIndex( int index )
        {
            if (index >= 0 && index < customColorsList.Length)
                return customColorsList[index].customColor.color;

            // Todo: Find a better workaround for when a color that is in use is deleted from the custom colors array.
            Debug.LogWarning( $"CT|GCFI: Error! Color index is out of range ({index.ToString()} out of {customColorsList.Length.ToString()})!" );
            return Color.magenta;
        }

        public static string GetCustomColorListVarName() => nameof( customColorsList );
    }
}
