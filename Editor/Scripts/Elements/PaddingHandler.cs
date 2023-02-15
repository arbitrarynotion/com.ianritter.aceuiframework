using UnityEngine;
using static ACEPackage.Editor.Scripts.EditorGraphics.EditorRectGraphics;

namespace ACEPackage.Editor.Scripts.Elements
{
    // Note: Padding is spaced added between the assigned position rect and the draw rect which is
    // derived from it.
    // The contents of the element are drawn in the draw rect.
    // An element's frame is drawn based on the position rect, modified by the frame position settings. This
    // is to increase the versatility of the frame offset settings.
    public class PaddingHandler
    {
#region Private Class Data

        private Element Element { get; }

#endregion


#region Constructors

        public PaddingHandler( Element element )
        {
            Element = element;
        }

#endregion


#region Public Methods

        public float GetLeftEdgePadding() => Element.Layout.GetLeftEdgeTypeBasedAdjustment() +
                                             Element.Layout.GetCustomLeftIndentAmount() +
                                             GetGlobalLeftEdgePadding() +
                                             GetFrameLeftPadding() +
                                             GetCustomLeftPaddingAmount();

        public float GetRightEdgePadding() => GetGlobalRightEdgePadding() +
                                              GetFrameRightPadding() +
                                              GetCustomRightPaddingAmount();

        public float GetTopEdgePadding() => GetGlobalTopEdgePadding() +
                                            GetFrameTopPadding() +
                                            GetCustomTopPaddingAmount();

        public float GetBottomEdgePadding() => GetGlobalBottomEdgePadding() +
                                               GetFrameBottomPadding() +
                                               GetCustomBottomPaddingAmount();


        public Rect ApplyCustomPadding( Rect baseRect )
        {
            baseRect.yMin += GetCustomTopPaddingAmount();
            baseRect.yMax -= GetCustomBottomPaddingAmount();
            baseRect.xMin += GetCustomLeftPaddingAmount();
            baseRect.xMax -= GetCustomRightPaddingAmount();
            return baseRect;
        }

        public Rect ApplyGlobalPadding( Rect baseRect )
        {
            baseRect.yMin += GetGlobalTopEdgePadding();
            baseRect.yMax -= GetGlobalBottomEdgePadding();
            baseRect.xMin += GetGlobalLeftEdgePadding();
            baseRect.xMax -= GetGlobalRightEdgePadding();
            return baseRect;
        }

#endregion


#region Private Methods

        private float GetColumnLeftPadding() =>
            Element.HasParent() && !Element.HasOwnLine() && !Element.Layout.IsFirstOnLine
                ? Element.GlobalSettings.columnGap / 2f
                : 0;

        private float GetColumnRightPadding() =>
            Element.HasParent() && !Element.HasOwnLine() && !Element.Layout.IsLastOnLine
                ? Element.GlobalSettings.columnGap / 2f
                : 0;

        // Frame auto padding.
        private float GetFrameLeftPadding() => ShouldApplyAnyFramePadding() ? GetLeftAutoPaddingAmount() : 0f;
        private float GetFrameRightPadding() => ShouldApplyAnyFramePadding() ? GetRightAutoPaddingAmount() : 0f;
        private float GetFrameTopPadding() => ShouldApplyAnyFramePadding() ? GetTopAutoPaddingAmount() : 0f;
        private float GetFrameBottomPadding() => ShouldApplyAnyFramePadding() ? GetBottomAutoPaddingAmount() : 0f;

        private bool ShouldApplyAnyFramePadding() =>
            Element.Layout.ShouldApplyFramePadding() ||
            Element.CustomSettings.OverrideFrame() && Element.FrameSettings.applyFraming;

        private float GetLeftAutoPaddingAmount() =>
            FrameHasLeft( Element.FrameSettings.frameType ) ? TotalFramePadding : 0;

        private float GetRightAutoPaddingAmount() =>
            FrameHasRight( Element.FrameSettings.frameType ) ? TotalFramePadding : 0;

        private float GetTopAutoPaddingAmount() =>
            FrameHasTop( Element.FrameSettings.frameType ) ? TotalFramePadding : 0;

        private float GetBottomAutoPaddingAmount() =>
            FrameHasBottom( Element.FrameSettings.frameType ) ? TotalFramePadding : 0;

        private float TotalFramePadding =>
            Element.FrameSettings.frameOutlineThickness + Element.FrameSettings.frameAutoPadding;

        private float GetCustomLeftPaddingAmount() => Element.CustomSettings.LeftPadding;
        private float GetCustomRightPaddingAmount() => Element.CustomSettings.RightPadding;
        private float GetCustomTopPaddingAmount() => Element.CustomSettings.TopPadding;
        private float GetCustomBottomPaddingAmount() => Element.CustomSettings.BottomPadding;

        // Global draw area padding
        private float GetGlobalLeftEdgePadding() =>
            Element.Layout.ShouldApplyGlobalLeftPadding() ? GetColumnLeftPadding() + Element.Settings.leftPadding : 0f;

        private float GetGlobalRightEdgePadding() =>
            Element.Layout.ShouldApplyGlobalRightPadding()
                ? GetColumnRightPadding() + Element.Settings.rightPadding
                : 0;

        private float GetGlobalTopEdgePadding() =>
            Element.Layout.ShouldApplyGlobalTopPadding() ? Element.Settings.topPadding : 0;

        private float GetGlobalBottomEdgePadding() =>
            Element.Layout.ShouldApplyGlobalBottomPadding() ? Element.Settings.bottomPadding : 0;

#endregion
    }
}