using System;
using Packages.com.ianritter.unityscriptingtools.Runtime.Services.CustomColors;
using UnityEngine;
using static Packages.com.ianritter.unityscriptingtools.Runtime.Services.TextFormatting.TextFormat;

namespace Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsGlobal.Colors
{
    [Serializable]
    public class CustomColorEntry
    {
        [SerializeField]
        public bool toggle = true;
        [SerializeField]
        public CustomColor customColor;
        
        private string _hexColor;
        
        
        public CustomColorEntry( string colorName, Color customColor )
        {
            Debug.Log( $"CustomColorEntry constructored called for {GetColoredStringLightSalmon( colorName )}." );
            this.customColor = new CustomColor( colorName, customColor );
            UpdateHexColor();
        }
        
        public string GetHexColor() => _hexColor;
        public void UpdateHexColor() => _hexColor = customColor.GetHex();

        public string GetSymbol() => toggle ? customColor.name : string.Empty;

        
    }
}
