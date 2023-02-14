using ACEPackage.Editor.Scripts.ElementConditions;
using ACEPackage.Runtime.Scripts;
using ACEPackage.Runtime.Scripts.SettingsCustom.SingleElements;
using UnityEngine;

namespace ACEPackage.Editor.Scripts.Elements.SingleElements.Decorator.DividingLine
{
    public class DividingLineElement : SingleElement
    {
        private DividingLineElementLayout _dividingLineElementLayout;
        private DividingLineElementDraw _dividingLineElementDraw;

        public override SingleElementLayout SingleElementLayout => _dividingLineElementLayout;
        public override SingleElementDraw SingleElementDraw => _dividingLineElementDraw;
        
        public readonly float DividerThickness;
        public readonly float BoxHeight;
        public readonly bool UseCustomColor;
        public readonly Color Color;
        

        // Todo: Add option to change divider color to settings window.
        /// <summary>
        /// An element which draws a divider as its inspector line entry. The color defaults to the global outline color
        /// settings.
        /// </summary>
        /// <param name="boxHeight">The amount of vertical space the divider will take up.</param>
        /// <param name="dividerThickness">The thickness of the line drawn in the vertical center of the box height.</param>
        public DividingLineElement( 
            float boxHeight = 5f, 
            float dividerThickness = 1f ) : 
            base( GUIContent.none, new SingleCustomSettings() {ForceSingleLine = true}, false, new ElementCondition[] {} )
        {
            BoxHeight = boxHeight;
            DividerThickness = dividerThickness;
            DividerThickness.AtMost( BoxHeight );
        }

        /// <summary>
        /// An element which draws a divider as its inspector line entry. The color defaults to the global outline color
        /// settings.
        /// </summary>
        /// <param name="color">Specify the color used to draw the dividing line.</param>
        /// <param name="boxHeight">The amount of vertical space the divider will take up.</param>
        /// <param name="dividerThickness">The thickness of the line drawn in the vertical center of the box height.</param>
        public DividingLineElement( Color color, float boxHeight = 5f, float dividerThickness = 1f ) : 
            base( GUIContent.none, new SingleCustomSettings() {ForceSingleLine = true}, false, new ElementCondition[] {} )
        {
            UseCustomColor = true;
            Color = color;
            BoxHeight = boxHeight;
            DividerThickness = dividerThickness;
            DividerThickness.AtMost( BoxHeight );
        }
        

        protected override void InitializeLayout() => _dividingLineElementLayout = new DividingLineElementLayout( this );

        protected override void InitializeDraw() => _dividingLineElementDraw = new DividingLineElementDraw( this );
    }
}
