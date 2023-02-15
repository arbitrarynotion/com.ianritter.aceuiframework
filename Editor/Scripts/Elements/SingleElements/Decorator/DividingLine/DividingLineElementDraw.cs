using UnityEngine;
using static Packages.com.ianritter.aceuiframework.Editor.Scripts.EditorGraphics.EditorRectGraphics;

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
            DrawDivider( _dividingLineElement.UseCustomColor ?  _dividingLineElement.Color : _dividingLineElement.GlobalSettings.frameOutlineColor );

        private void DrawDivider( Color color )
        {
            float padding = _dividingLineElement.BoxHeight - _dividingLineElement.DividerThickness;
            var elementHeightRect = new Rect(_dividingLineElement.SingleElementLayout.GetDrawRect());
            elementHeightRect.y += padding / 2f;
            elementHeightRect.height = _dividingLineElement.DividerThickness;
            DrawSolidRect( elementHeightRect, color );
        }
    }
}
