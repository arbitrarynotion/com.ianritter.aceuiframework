using UnityEngine;

namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.SingleElements.Decorator.Heading.Toggle
{
    public class ToggleHeading : HeadingElement
    {
        private ToggleHeadingLayout _toggleHeadingLayout;
        private ToggleHeadingDraw _toggleHeadingDraw;
        
        public override HeadingElementLayout HeadingElementLayout => _toggleHeadingLayout;
        public override HeadingElementDraw HeadingElementDraw => _toggleHeadingDraw;
        
        
        public ToggleHeading( GUIContent guiContent ) 
            : base( guiContent )
        {
        }
        

        protected override void InitializeLayout() => _toggleHeadingLayout = new ToggleHeadingLayout( this );
        protected override void InitializeDraw() => _toggleHeadingDraw = new ToggleHeadingDraw( this );
    }
}
