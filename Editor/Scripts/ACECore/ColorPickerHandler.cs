using UnityEditor;
using UnityEngine;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.Services;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsGlobal.Colors;
using static Packages.com.ianritter.aceuiframework.Runtime.Scripts.Services.TextFormat;

namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.ACECore
{
    public class ColorPickerHandler
    {
        private readonly CustomColorPicker _customColorPicker;
        private readonly CustomLogger _logger;
        private readonly Rect _position;
        
        public delegate void ColorSelected( CustomColor color );
        /// <summary>
        ///     Subscribe to this event with the method that will receive the chosen custom color.
        /// </summary>
        public event ColorSelected OnColorSelected;
        private void OnColorSelectedNotify( CustomColor color )
        {
            OnColorSelected?.Invoke( color );
        }

        private ColorPickerHandler( CustomLogger logger, Rect position )
        {
            _logger = logger;
            _position = position;
        }

        public ColorPickerHandler( CustomLogger logger, Rect position, Vector2 windowSize, int buttonsPerLine = 8 ) : this( logger, position )
        {
            _customColorPicker = new CustomColorPicker( logger, windowSize, buttonsPerLine );
            _customColorPicker.OnButtonPressed += OnColorSelection;
        }

        public ColorPickerHandler( CustomLogger logger, Rect position, Vector2 windowSize, Vector2 buttonSize ) : this( logger, position )
        {
            _customColorPicker = new CustomColorPicker( logger, windowSize, buttonSize );
            _customColorPicker.OnButtonPressed += OnColorSelection;
        }

        public void Close() => _customColorPicker.editorWindow.Close();

        /// <summary>
        ///     Use this as the button callback to trigger the color picker popup window.
        /// </summary>
        public void ColorPickerButtonPressed()
        {
            _logger.Log( "Popup button pressed." );
            PopupWindow.Show( _position, _customColorPicker );
        }

        private void OnColorSelection( CustomColor color )
        {
            _logger.Log( $"Color picker returned color: {GetColoredString( color.name, color.GetHex() )}" );
            _customColorPicker.editorWindow.Close();
            // _customColorPicker.OnButtonPressed -= OnColorSelection;
            OnColorSelectedNotify( color );
        }

        // public override Element[] GetElementList()
        // {
        //     return new Element[]
        //     {
        //         new LabeledGroup( 
        //             new GUIContent( "Color Picker"), 
        //             new GroupCustomSettings(), 
        //             _colorList.
        //                 Select( ( t, i ) => nameof( _colorList ) + ".Array.data[" + ( i.ToString() ) + "]." ).
        //                 Select( arrayLookupString => new BasicProperty( arrayLookupString, GUIContent.none, new SingleCustomSettings() ) ).
        //                 Cast<Element>().ToArray()  ), 
        //     };
        // }
    }
}
