using UnityEditor;
using UnityEngine;

namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.GroupElements.HeadingGroup
{
    public abstract class HeadingGroupLayout : GroupElementLayout
    {
        public override float ChildIndentAmount => 0f;
        protected override GroupElement GroupElement => HeadingGroup;
        protected override float ColumnWidthPriorityAdjustment => HeadingGroup.GlobalSettings.sectionWidthPriorityAdjustment;
        protected override float HeightWithChildren { get; set; }
        
        private HeadingGroup HeadingGroup { get; }
        

        protected HeadingGroupLayout( HeadingGroup element )
            : base( element )
        {
            HeadingGroup = element;
        }


        public override bool ShouldShowFrame()
        {
            // Case 1: Element custom settings forbid frame.
            if ( Element.CustomSettings.BlockFrame() )
                return false;

            // Case 2: Heading group is collapsed so there is no child area on which to draw a frame.
            if ( !Element.IsVisible )
                return false;

            // Case 3: Element has parent who's custom settings forbid it's children from having frames.
            if ( Element.HasParent() &&
                 !Element.ParentElement.GroupCustomSettings.ChildGroupElementsHaveFrames )
                return false;

            // Default: Follow global frame settings for heading groups.
            return Element.FrameSettings.applyFraming;
        }


        public override bool ShouldApplyGlobalBottomPadding() => !HeadingGroup.ShouldHideChildren();

        public override bool ShouldApplyFramePadding() => false;

        public override float GetCollapsedHeight() =>
            HeadingGroup.HeadingElement.HeadingElementLayout.GetElementHeight() +
            Element.GlobalSettings.elementVerticalPadding;

        /// <summary>
        ///     The heading group's frame rect has its top vertically shifted down so that it starts at the bottom of
        ///     of its heading.
        /// </summary>
        public override Rect GetFrameRect()
        {
            var outlineRect = new Rect( GetDrawRect() );

            // Remove Heading area from top of draw area.
            outlineRect.yMin += HeadingGroup.HeadingElement.HeadingElementLayout.GetRequiredHeight( false );

            // Counter padding to hold outline in position.
            // outlineRect.yMax += HeadingGroup.Settings.bottomPadding;

            return outlineRect;
        }

        protected override float GetSingleLineChildLabelWidth() =>
            EditorGUIUtility.labelWidth - ( GetTotalLeftIndentAmount() + PaddingHandler.GetLeftEdgePadding() );

        protected override void SetHeightWithChildren( float heightOfGroup )
        {
            if ( HeadingGroup.GetNumberOfElements() == 0 || HeadingGroup.ShouldHideChildren() )
            {
                HeightWithChildren = HeadingGroup.GroupElementLayout.GetCollapsedHeight();
                return;
            }

            HeightWithChildren = heightOfGroup;
        }

        /// <summary>
        ///     Heading group always contains exactly two elements: a heading element and a child area group.
        /// </summary>
        protected override void SetChildrenPositionRects()
        {
            Rect currentLineRect = GetDrawRect();

            currentLineRect.height = HeadingGroup.HeadingElement.Layout.GetRequiredHeight( false );
            SetChildPositionRect( 0, 0, currentLineRect, currentLineRect.x );

            currentLineRect.yMin += currentLineRect.height + Element.GlobalSettings.elementVerticalPadding;
            if ( GroupElement.GetNumberOfElements() == 0 || !HeadingGroup.ChildAreaGroup.IsVisible ) return;

            SetChildPositionRect( 1, 0, currentLineRect, currentLineRect.x );
        }
    }
}