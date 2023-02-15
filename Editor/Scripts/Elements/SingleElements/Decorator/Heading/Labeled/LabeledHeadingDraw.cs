using ACEPackage.Editor.Scripts.Elements.GroupElements.HeadingGroup;
using UnityEngine;

namespace ACEPackage.Editor.Scripts.Elements.SingleElements.Decorator.Heading.Labeled
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
            HeadingGroup labeledGroup = (HeadingGroup) _labeledHeading.ParentElement;

            if (labeledGroup.HeadingGroupSettings.hideFoldoutGroupElements)
                return;

            Rect headingDrawRect = _labeledHeading.HeadingElementLayout.GetDrawRect();

            var labelRect = new Rect( headingDrawRect );
            labelRect.xMin += DefaultLeftPadding + 2f +_labeledHeading.HeadingElementFrameSettings.textHorizontalOffset;
            
            DrawLabelField( labelRect, GetHeadingLabelStyle( true ) );
        }
    }
}
