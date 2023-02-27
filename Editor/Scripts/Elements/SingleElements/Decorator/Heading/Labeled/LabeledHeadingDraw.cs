using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.GroupElements.HeadingGroup;
using UnityEngine;

namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.SingleElements.Decorator.Heading.Labeled
{
    public class LabeledHeadingDraw : HeadingElementDraw
    {
        private readonly LabeledHeading _labeledHeading;

        
        public LabeledHeadingDraw( LabeledHeading labeledHeading ) 
            : base( labeledHeading )
        {
            _labeledHeading = labeledHeading;
        }
        

        protected override void DrawElementContents()
        {
            Rect headingDrawRect = _labeledHeading.HeadingElementLayout.GetDrawRect();

            var labelRect = new Rect( headingDrawRect );
            labelRect.xMin += DefaultLeftPadding + 2f +_labeledHeading.HeadingElementFrameSettings.textHorizontalOffset;
            
            DrawLabelField( labelRect, GetHeadingLabelStyle( true ) );
        }
    }
}
