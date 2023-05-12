using Packages.com.ianritter.aceuiframework.Runtime.Scripts.Enums;
using UnityEditor;
using UnityEngine;

namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements
{
    public abstract class ElementLayout
    {
#region Public Class Data

        // Position info.
        public float Width { get; set; } = -1;
        public float Height { get; set; } = -1;
        public float ExtraVerticalSpace { get; set; } = 0;

        // Position in the lines list.
        public int Line { get; private set; } = -1;

        // Todo: Column value is never getting used. Is it needed?
        public int Column { get; private set; } = -1;

        // Cached data to speed up list position inquiries.
        public int NumberOfNeighbors { get; set; } = 0;
        public bool IsLastOnLine { get; set; }
        public bool IsFirstOnLine { get; set; }

        public PaddingHandler PaddingHandler { get; }

#endregion


#region Protected Abstract Class Data

        protected abstract Element Element { get; }

#endregion


#region Protected Virtual Class Data

        /// <summary>
        ///     Used by the settings to adjust the column width priority balance per element type.
        /// </summary>
        protected virtual float ColumnWidthPriorityAdjustment => 0;

#endregion


#region Private Class Data

        private Rect _positionRect;

        /// <summary>
        ///     This is the total indent level including the single indent level applied by default. That is, the root
        ///     element will have an indent level of 1 plus its custom indent level increase.
        /// </summary>
        private float TotalIndentLevel
        {
            get
            {
                if ( Element.IsRootElement() )
                    return 0f;

                if ( Element.ParentElement.Layout == null )
                {
                    Debug.LogError( $"E|TIL: Error! {Element.GetName()} can't get its parent's ({Element.ParentElement.GetName()}) indent level because the parent's layout is null!" );
                    return 0;
                }

                if ( !Element.ParentElement.GroupCustomSettings.IndentChildren )
                    return 0;


                // return parentsTotalIndent;
                return Element.ParentElement.GroupElementLayout.ChildIndentAmount;
            }
        }

#endregion


#region Constructors

        protected ElementLayout( Element element )
        {
            PaddingHandler = new PaddingHandler( element );
        }

#endregion


#region Public Methods

        /// <summary>
        ///     Set the cached position information for this element.
        /// </summary>
        public void SetListPositionInfo( int line, int column )
        {
            Line = line;
            Column = column;
        }

        /// <summary>
        ///     Set the cached position information for this element including its position in line.
        /// </summary>
        public void SetListPositionInfo( int line, int column, bool isFirstOnLine, bool isLastOnLine )
        {
            Line = line;
            Column = column;
            IsFirstOnLine = isFirstOnLine;
            IsLastOnLine = isLastOnLine;
        }

        /// <summary>
        ///     Gets the height required by the element. The height is used to allocate the element's vertical space. Any
        ///     vertical padding should be included here.
        /// </summary>
        public float GetRequiredHeight( bool updating ) =>
            GetElementHeight( updating ) + PaddingHandler.GetTopEdgePadding() + PaddingHandler.GetBottomEdgePadding();

        /// <summary>
        ///     Column width priority is used to control the amount of horizontal space assigned to this element when it
        ///     shares the line with other elements. This value is translated to the percentage of the sum of all the
        ///     other column width priorities on this line - which is also the percentage of the usable width thi element
        ///     will be assigned.
        /// </summary>
        public float ColumnWidthPriority =>
            1f + ColumnWidthPriorityAdjustment + Element.CustomSettings.ColumnWidthPriority;

        /// <summary>
        ///     Returns the unmodified position rect. Primarily only used to determine outline and in debugging.
        /// </summary>
        public Rect GetPositionRect() => _positionRect;

        /// <summary>
        ///     Returns the drawable area of the position rect. This is the area remaining after edge padding has been applied.
        ///     Generally, all drawing is done with this rect.
        /// </summary>
        public Rect GetDrawRect()
        {
            // Apply individual element custom settings here before returning the rect.
            // Note that these adjustments can't change the amount of space allocated to the element, just where the element is drawn.
            Rect customPosRect = GetPositionRect();

            if ( Element.CustomSettings.CenterInFullHeightOfLine )
                customPosRect.yMin += ExtraVerticalSpace / 2.0f;

            customPosRect.yMin += PaddingHandler.GetTopEdgePadding();
            // Todo: This should be using GetBottomEdgePadding...
            customPosRect.height = GetElementHeight();
            customPosRect.xMin += PaddingHandler.GetLeftEdgePadding();
            customPosRect.xMax -= PaddingHandler.GetRightEdgePadding();
            return customPosRect;
        }

        /// <summary>
        ///     Returns a modified version of the draw rect to align the label to the left, center, or right as specified
        ///     in its custom settings.
        /// </summary>
        public Rect GetAlignedLabelDrawRect() => GetAlignedLabelDrawRect( GetDrawRect() );

        /// <summary>
        ///     Returns a modified version of the provided rect to align the label to the left, center, or right as specified
        ///     in its custom settings.
        /// </summary>
        public Rect GetAlignedLabelDrawRect( Rect drawRect )
        {
            if ( Element.CustomSettings.LabelAlignment == Alignment.Left ) return drawRect;
            
            float labelWidth = CalcPrefixLabelWidth( Element.GUIContent ) + 4f;
            if ( Element.CustomSettings.BoldLabel ) labelWidth += 5f;

            // Left aligned by default so only needs to adjusted if center or right is selected.
            switch ( Element.CustomSettings.LabelAlignment )
            {
                case Alignment.Center:
                {
                    float halfExtraWidth = ( drawRect.width - labelWidth ) / 2f;
                    drawRect.xMin += halfExtraWidth;
                    drawRect.width -= halfExtraWidth;
                    break;
                }

                case Alignment.Right:
                {
                    float extraWidth = drawRect.width - labelWidth;
                    drawRect.xMin += extraWidth;
                    break;
                }
            }

            return drawRect;
        }

        /// <summary>
        ///     The width of the position rect is stored separately as a work around to Unity's quirky layout system. The
        ///     width of all position rects must be based on this reliable value or they will get super twitchy.
        /// </summary>
        public float GetUsableWidth() =>
            Width - PaddingHandler.GetLeftEdgePadding() - PaddingHandler.GetRightEdgePadding();

        public abstract bool ShouldApplyGlobalLeftPadding();
        public abstract bool ShouldApplyGlobalRightPadding();
        public abstract bool ShouldApplyGlobalTopPadding();
        public abstract bool ShouldApplyGlobalBottomPadding();

        /// <summary>
        ///     Set this to false when using a group element that contains a child area group. In that case, the parent
        ///     group should not use padding as it should be applied to the child group. The child area group takes care
        ///     of that implementation.
        /// </summary>
        public abstract bool ShouldApplyFramePadding();

        public float GetCustomLeftIndentAmount() =>
            Element.CustomSettings.IndentLevelIncrease * Element.GlobalSettings.leftIndentUnitAmount;

        /// <summary>
        ///     This adjustment is applying before all checks so it will be applied regardless.
        /// </summary>
        public virtual float GetLeftEdgeTypeBasedAdjustment() => 0;

#endregion


#region Public Abstract Methods

        /// <summary>
        ///     Get the draw height of the element. Should NOT include the default element vertical padding.
        /// </summary>
        public abstract float GetElementHeight( bool updating = false );

        /// <summary>
        ///     Assigns a new position rect for a root element.
        /// </summary>
        public abstract void AssignNewPositionRect( bool updateRequired );

        /// <summary>
        ///     Returns true if element frame is allowed. This is based on the custom settings, the presence of a parent,
        ///     and that parent's custom settings - in that order.
        /// </summary>
        public abstract bool ShouldShowFrame();

#endregion


#region Public Virtual Methods

        /// <summary>
        ///     Assign a child element's position rect and width.
        /// </summary>
        public virtual void SetPositionRect( Rect newPositionRect, float width )
        {
            _positionRect = newPositionRect;
            Width = width;
        }

        /// <summary>
        ///     The rect used to draw the outline around an element.
        /// </summary>
        public virtual Rect GetFrameRect() => new Rect( GetPositionRectWithGlobalAndCustomPadding() );

        /// <summary>
        ///     This is the height of the element when it is not visible. Will be zero for all but heading elements.
        /// </summary>
        /// <returns></returns>
        public virtual float GetCollapsedHeight() => 0;

#endregion


#region Protected Methods

        protected Rect GetPositionRectWithGlobalAndCustomPadding()
        {
            var customRect = new Rect( _positionRect );
            customRect.xMin += GetCustomLeftIndentAmount();
            customRect = PaddingHandler.ApplyGlobalPadding( customRect );
            customRect = PaddingHandler.ApplyCustomPadding( customRect );
            customRect = ApplyFullHeightFrameAdjustment( customRect );

            return customRect;
        }

        protected Rect ApplyFullHeightFrameAdjustment( Rect baseRect )
        {
            if ( Element.CustomSettings.FullHeightFrame ) return baseRect;

            float halfOfExtraSpace = ExtraVerticalSpace / 2.0f;
            baseRect.yMin += halfOfExtraSpace;
            baseRect.yMax -= halfOfExtraSpace;

            return baseRect;
        }

#endregion


#region Protected Virtual Methods

        /// <summary>
        ///     Get this element's indent level in pixels including the level inherited by its parent.
        ///     This is recursive so each parent also inherits from their parents and so on.
        /// </summary>
        protected float GetTotalLeftIndentAmount() => TotalIndentLevel * Element.GlobalSettings.leftIndentUnitAmount;

#endregion


#region Private Methods

        /// <summary>
        ///     Copied from internal source code. Gets the width required to fill string. Useful for both minimizing the space
        ///     a label takes up.
        /// </summary>
        private float CalcPrefixLabelWidth( GUIContent label, GUIStyle style = null )
        {
            if ( style == null )
                style = EditorStyles.label;
            return style.CalcSize( label ).x;
        }

#endregion
    }
}