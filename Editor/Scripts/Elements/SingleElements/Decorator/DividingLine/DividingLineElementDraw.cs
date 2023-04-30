using UnityEngine;
using static Packages.com.ianritter.unityscriptingtools.Runtime.Services.UIGraphics.UIRectGraphics;

namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.SingleElements.Decorator.DividingLine
{
    public class DividingLineElementDraw : SingleElementDraw
    {
        private readonly DividingLineElement _dividingLineElement;
        protected override SingleElement SingleElement => _dividingLineElement;
        
        
        public DividingLineElementDraw( DividingLineElement dividingLineElement )
        {
            _dividingLineElement = dividingLineElement;
        }
        

        protected override void DrawElementContents() => 
            DrawDivider( _dividingLineElement.UseCustomSettings ? _dividingLineElement.Color : _dividingLineElement.GlobalSettings.frameOutlineColor );

        private void DrawDivider( Color color )
        {
            float padding = _dividingLineElement.BoxHeight - _dividingLineElement.DividerThickness;
            var elementHeightRect = new Rect(_dividingLineElement.SingleElementLayout.GetDrawRect());
            // DrawRectOutline( elementHeightRect, Color.grey );

            // Calculate trim values.
            
            float leftTrim = _dividingLineElement.LeftTrimPercent;
            float rightTrim = _dividingLineElement.RightTrimPercent;
            
            if ( _dividingLineElement.SettingsAreLive )
            {
                leftTrim =  _dividingLineElement.LeftTrimPercentProperty.floatValue;
                rightTrim = _dividingLineElement.RightTrimPercentProperty.floatValue;
                // Debug.Log( $"DividerElement: LeftTripProp: {leftTrimPadding.ToString()}, RightTrimProp: {rightTrimPadding.ToString()}" );
            }
            
            float elementHeightRectWidth = elementHeightRect.width;
            float leftTrimPadding = elementHeightRectWidth * leftTrim;
            float rightTrimPadding = elementHeightRectWidth * rightTrim;

            elementHeightRect.y += padding / 2f;
            elementHeightRect.height = _dividingLineElement.DividerThickness;
            elementHeightRect.xMin += leftTrimPadding;
            elementHeightRect.xMax -= elementHeightRectWidth - rightTrimPadding;
            DrawSolidRect( elementHeightRect, color );
        }
    }
}
