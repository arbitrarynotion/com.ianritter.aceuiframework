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
        // [SerializeField]
        // public CustomColor customColor;
        [SerializeField] [Delayed]
        public string name = "Custom Color";
        [SerializeField]
        public Color color = new Color( 0f, 0f, 0f, 1f );
        
        // private string _hexColor;
        
        /// <summary>
        /// When the name is changed in the reorderable list, the previous name is stored here (from property drawer) so that it can be
        /// passed, along with the new name, to subscribers of the NameUpdated event. Those subscribers can then update their stored
        /// name so their connection to this color is not lost.
        /// </summary>
        public string previousName;
        
        /// <summary>
        /// Used to track when changes are made to serialized data within the property drawer. This will be set back to false in CustomColorSettings
        /// when its NameUpdated event has been triggered.
        /// </summary>
        public bool wasUpdated = false;
        
        /// <summary>
        /// When a color's name is changed, any users of it must be notified so that they can update their name references.
        /// </summary>
        public delegate void NameUpdated( string nameOfColorThatChanged, string newName );
        public event NameUpdated OnNameUpdated;
        public void NameUpdatedNotify()
        {
            // Debug.Log( $"CustomColorEntry: {name}'s NameUpdated event was triggered." );
            OnNameUpdated?.Invoke( previousName, name );
        }

        public CustomColorEntry()
        {
            // This is called every time an object is created in the editor but the default values are only kept if
            // this is the first item created in an empty array. In all other cases, the data is subsequently overwritten
            // during deserialization.
            // I'm explicitly declaring the default constructor mainly so intellisense stops complaining about 
            // the data members having default values.
        }
        
        public CustomColorEntry( string colorName, Color customColor )
        {
            // Debug.Log( $"CustomColorEntry constructor called for {GetColoredStringLightSalmon( colorName )}." );
            // this.customColor = new CustomColor( colorName, customColor );
            name = colorName;
            color = customColor;
            // UpdateHexColor();
        }

        // public string GetHexColor() => _hexColor;
        // public void UpdateHexColor() => _hexColor = customColor.GetHex();
        //
        // public string GetSymbol() => toggle ? customColor.name : string.Empty;
    }
}
