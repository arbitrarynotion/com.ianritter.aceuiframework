using System;
using Packages.com.ianritter.unityscriptingtools.Runtime.Services.CustomColors;
using UnityEngine;

namespace Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsGlobal.Colors
{
    [Serializable]
    public class CustomColorSettings
    {
        private const float OutlineBrightness = 0.32f;
        private const float HeadingRootBackgroundBrightness = 0.18f;
        private const float HeadingChildBackgroundBrightness = 0.25f;
        
        [SerializeField] private CustomColor[] customColorsList;
        
        public CustomColorSettings() => Initialize();

        private void Initialize() => InitializeCustomColors();

        private void InitializeCustomColors()
        {
            const int totalColors = 3;

            if ( customColorsList != null ) return;
            
            // Set up default colors.
            customColorsList = new CustomColor[totalColors];

            int currentIndex = 0;
            customColorsList[currentIndex++] = new CustomColor( ( "Outlines" ), 
                new Color( OutlineBrightness, OutlineBrightness, OutlineBrightness));
            customColorsList[currentIndex++] = new CustomColor( ( "Root Background" ), 
                new Color( HeadingRootBackgroundBrightness, HeadingRootBackgroundBrightness, HeadingRootBackgroundBrightness));
            customColorsList[currentIndex++] = new CustomColor( ( "Child Background" ), 
                new Color( HeadingChildBackgroundBrightness, HeadingChildBackgroundBrightness, HeadingChildBackgroundBrightness));
            
            for (int i = currentIndex; i < totalColors; i++)
            {
                customColorsList[i] = new CustomColor( ( "Color " + ( i + 1 ).ToString() ), 
                    new Color( HeadingChildBackgroundBrightness, HeadingChildBackgroundBrightness, HeadingChildBackgroundBrightness));
            }
        }

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
        
        public Color GetColorForIndex( int index )
        {
            if (index >= 0 && index < customColorsList.Length)
                return customColorsList[index].color;

            // Todo: Find a better workaround for when a color that is in use is deleted from the custom colors array.
            Debug.LogWarning( $"CT|GCFI: Error! Color index is out of range ({index.ToString()} out of {customColorsList.Length.ToString()})!" );
            return Color.magenta;
        }

        public static string GetCustomColorListVarName() => nameof( customColorsList );
    }
}
