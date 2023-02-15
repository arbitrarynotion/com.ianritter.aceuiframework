using UnityEngine;

namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.SingleElements.Decorator.Heading.Labeled
{
    public class LabeledHeading : HeadingElement
    {
        private LabeledHeadingLayout _labeledHeadingLayout;
        private LabeledHeadingDraw _labeledHeadingDraw;
        
        public override HeadingElementLayout HeadingElementLayout => _labeledHeadingLayout;
        public override HeadingElementDraw HeadingElementDraw => _labeledHeadingDraw;

        
        public LabeledHeading( GUIContent guiContent ) 
            : base( guiContent )
        {
        }
        

        protected override void InitializeLayout() => _labeledHeadingLayout = new LabeledHeadingLayout( this );
        protected override void InitializeDraw() => _labeledHeadingDraw = new LabeledHeadingDraw( this );
    }
}
