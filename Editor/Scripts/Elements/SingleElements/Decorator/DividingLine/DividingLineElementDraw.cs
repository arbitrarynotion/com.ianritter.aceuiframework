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
            var drawRect = new Rect(_dividingLineElement.SingleElementLayout.GetDrawRect());
            // DrawRectOutline( drawRect, Color.grey );
            
            drawRect = ApplyEdgePadding( drawRect );
            // DrawRectOutline( drawRect, Color.yellow );

            drawRect.y += padding / 2f;
            drawRect.height = _dividingLineElement.DividerThickness;
            DrawSolidRect( drawRect, color );
        }

        private Rect ApplyEdgePadding( Rect drawRect )
        {
            if ( !_dividingLineElement.UseCustomSettings ) return drawRect;
            
            float elementHeightRectWidth = drawRect.width;
            
            float leftTrimPadding = elementHeightRectWidth * _dividingLineElement.LeftTrimPercent;
            float rightTrimPadding = elementHeightRectWidth * _dividingLineElement.RightTrimPercent;
            
            if ( _dividingLineElement.SettingsAreLive )
            {
                leftTrimPadding =  elementHeightRectWidth * _dividingLineElement.LeftTrimPercentProperty.floatValue;
                rightTrimPadding = elementHeightRectWidth * _dividingLineElement.RightTrimPercentProperty.floatValue;
            }

            drawRect.xMin += leftTrimPadding;
            drawRect.xMax -= elementHeightRectWidth - rightTrimPadding;

            return drawRect;

        }
        
    }
}
