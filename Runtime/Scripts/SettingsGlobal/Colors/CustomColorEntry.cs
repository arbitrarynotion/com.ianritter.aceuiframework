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

        // If the custom color's name is changed in the editor, properties using that custom color will ask for it
        // by its old name. This allows such calls to be match back up with their custom color so they can update
        // their stored name.
        public string previousName;
        public bool wasUpdated = false;
        
        // When a color's name is changed, any users of it must be notified so that they can update their name references.
        public delegate void NameUpdated( string nameOfColorThatChanged, string newName );
        public event NameUpdated OnNameUpdated;
        public void NameUpdatedNotify()
        {
            Debug.Log( $"CustomColorEntry: {customColor.name}'s NameUpdated event was triggered." );
            OnNameUpdated?.Invoke( previousName, customColor.name );
        }


        public CustomColorEntry( string colorName, Color customColor )
        {
            // Debug.Log( $"CustomColorEntry constructored called for {GetColoredStringLightSalmon( colorName )}." );
            this.customColor = new CustomColor( colorName, customColor );
            UpdateHexColor();
        }
        
        public string GetHexColor() => _hexColor;
        public void UpdateHexColor() => _hexColor = customColor.GetHex();

        public string GetSymbol() => toggle ? customColor.name : string.Empty;

        
    }
}
