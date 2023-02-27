using System.Collections.Generic;
using System.Reflection;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsGlobal.Colors;
using UnityEditor;
using UnityEngine;
using static Packages.com.ianritter.aceuiframework.Runtime.Scripts.Services.PresetColors;
using static Packages.com.ianritter.aceuiframework.Runtime.Scripts.Services.UIGraphics.UIRectGraphics;
using static Packages.com.ianritter.aceuiframework.Runtime.Scripts.Services.TextFormat;

namespace Packages.com.ianritter.aceuiframework.Runtime.Scripts.Services
{

    public class CustomColorPicker : PopupWindowContent
    {
        private class CustomColorButton
        {
            public readonly CustomColor CustomColor;
            public readonly float XPosOffset;
            public readonly float YPosOffset;
            public readonly Texture2D Texture2D;

            public CustomColorButton( CustomColor customColor,
                float xPosOffset, float yPosOffset, 
                Texture2D texture2D )
            {
                CustomColor = customColor;
                Texture2D = texture2D;
                XPosOffset = xPosOffset;
                YPosOffset = yPosOffset;
            }
        }
        
        
        private CustomColor[] _colorList;
        private readonly Vector2 _windowSize;
        private readonly int _buttonsPerLine;
        private Vector2 _buttonSize;
        private float _buttonWidth;
        
        private const float Separator = 2f;
        private const float VerticalSeparator = 2f;
        private const float EdgePadding = 5f;

        private Vector2 _scrollPosition;

        private readonly List<CustomColorButton> _buttons = new List<CustomColorButton>();

        private readonly CustomLogger _logger;

        public delegate void ButtonPressed( CustomColor color );
        public event ButtonPressed OnButtonPressed;
        private void OnButtonPressedNotify( CustomColor color )
        {
            OnButtonPressed?.Invoke( color );
        }

        public CustomColorPicker( CustomLogger logger, Vector2 windowSize, Vector2 buttonSize )
        {
            _logger = logger;
            _logger.Log( "CustomColorPicker constructor called." );
            _windowSize = windowSize;
            _buttonSize = buttonSize;
            
            BuildButtonList();
        }
        
        public CustomColorPicker( CustomLogger logger, Vector2 windowSize, int buttonsPerLine = 8 )
        {
            _logger = logger;
            _windowSize = windowSize;
            _buttonsPerLine = buttonsPerLine;
            
            BuildButtonList();
        }

        private void BuildButtonList()
        {
            _logger.LogStart( MethodBase.GetCurrentMethod(), true );
            _colorList = GetAllColors().ToArray();

            // Define the position available for this line.
            var firstLine = new Rect( 0f, 0f, _windowSize.x, _windowSize.y );
            firstLine.yMin += EdgePadding;
            firstLine.yMax -= EdgePadding;
            firstLine.xMin += EdgePadding;
            firstLine.xMax -= EdgePadding;

            // var labelRect = new Rect( firstLine ) { height = EditorGUIUtility.singleLineHeight };
            // GUI.Label( labelRect, new GUIContent( "Color Picker") );

            // Shift down beyond the label.
            firstLine.yMin += EditorGUIUtility.singleLineHeight + VerticalSeparator;
            
            PopulateButtons( GetSingleButtonRect( firstLine ) );
            _logger.LogEnd( MethodBase.GetCurrentMethod(), true );
        }
        
        private void PopulateButtons( Rect singleButtonRect )
        {
            _logger.LogStart( MethodBase.GetCurrentMethod() );
            
            // _logger.Log( $"Button size set to: {GetColoredStringGreen( $"{singleButtonRect.width.ToString()} x {singleButtonRect.height.ToString()}" )}" );
            // Draw each element in the position provided, adding a separator between each.
            var buttonRect = new Rect( singleButtonRect );
            int buttonCount = 1;
            foreach ( CustomColor customColor in _colorList )
            {
                // Todo: Not sure if there is a way to cache this as well to reduce per frame work.
                Texture2D buttonBackground = GenerateTexture( (int) _buttonWidth, (int) _buttonWidth, customColor.color );
                
                if ( buttonCount > _buttonsPerLine )
                {
                    buttonCount = 1;
                    buttonRect.x = singleButtonRect.x;
                    buttonRect.y += _buttonWidth + Separator;
                }
                
                _buttons.Add( new CustomColorButton( 
                    customColor, 
                    buttonRect.x, buttonRect.y, 
                    buttonBackground ) );
                
                buttonRect.x += _buttonWidth + Separator;
                buttonCount++;
            }
            _logger.LogEnd( MethodBase.GetCurrentMethod() );
        }
        
        public override Vector2 GetWindowSize()
        {
            return _windowSize;
        }
        

        public override void OnGUI(Rect position)
        {
            // Define the position available for this line.
            var firstLine = new Rect( 0f, 0f, _windowSize.x, _windowSize.y );
            firstLine.yMin += EdgePadding;
            firstLine.yMax -= EdgePadding;
            firstLine.xMin += EdgePadding;
            firstLine.xMax -= EdgePadding;
            
            var labelRect = new Rect( firstLine ) { height = EditorGUIUtility.singleLineHeight };
            GUI.Label( labelRect, new GUIContent( "Color Picker") );

            // Apply scrollbar if space required for buttons exceeds window size.
            var scrollArea = new Rect( position );
            scrollArea.yMin += EdgePadding + EditorGUIUtility.singleLineHeight + VerticalSeparator;
            _scrollPosition = GUI.BeginScrollView( scrollArea, _scrollPosition, GetTotalPositionRequired( scrollArea ) );
            {
                DrawColorButtons( GetSingleButtonRect( firstLine ) );
            }
            GUI.EndScrollView();
        }
        
        
        private void DrawColorButtons( Rect position )
        {
            foreach ( CustomColorButton customColorButton in _buttons )
            {
                var buttonRect = new Rect( position );
                buttonRect.x += customColorButton.XPosOffset;
                buttonRect.y += customColorButton.YPosOffset;
                
                DrawColorGUIButton( buttonRect, customColorButton );
            }
        }
        
        private void DrawColorGUIButton( Rect positionRect, CustomColorButton customColorButton )
        {
            Color cacheColor = GUI.color;
            GUI.color = customColorButton.CustomColor.color;
            if ( GUI.Button( positionRect, customColorButton.Texture2D ) )
                OnButtonPressedNotify( customColorButton.CustomColor );
            GUI.color = cacheColor;
        }
        
        
        private Rect GetTotalPositionRequired( Rect basePosition )
        {
            // Determine height required to fit all buttons.
            var returnRect = new Rect( basePosition );
            returnRect.xMax -= 15f;
            float totalHeightRequired = 0;
            for (int i = 0; i < ( _buttons.Count / _buttonsPerLine ); i++)
                totalHeightRequired += _buttonWidth + VerticalSeparator;

            returnRect.height = totalHeightRequired + ( 2 * EdgePadding );
            return new Rect( returnRect );
        }


        private Rect GetSingleButtonRect( Rect position )
        {
            // If buttons per line > -1, auto size to fit
            // if ( _buttonsPerLine > 0 )
            // {
            _buttonWidth = ( ( position.width - ( ( _colorList.Length - 1 ) * Separator ) ) / _buttonsPerLine );
            return new Rect( position ) { width = _buttonWidth, height = _buttonWidth };
            // }
            
            // Todo: Allow number of buttons be line to be determined by a set width.
            // Otherwise, set all buttons to the cached size and set buttons per line based on the set width vs the window width.
            
            
        }
    }
}