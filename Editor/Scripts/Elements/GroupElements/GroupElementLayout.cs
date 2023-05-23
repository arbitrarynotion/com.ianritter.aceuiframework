using UnityEditor;
using UnityEngine;
using static Packages.com.ianritter.unityscriptingtools.Runtime.Services.UIGraphics.UIRectGraphics;

namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.GroupElements
{
    /// <summary>
    ///     Manages the position rect for the child elements of a section. Due to Unity layout limitations, width is managed
    ///     separately with a usable width that is based on the inspector's width.
    /// </summary>
    public abstract class GroupElementLayout : ElementLayout
    {
        // Note that 18 represents the default indent of 15, the start of a root element's position, plus 3 extra
        // points of padding to ensure the element's content doesn't draw directly on the left edge. This is the default
        // for all elements in the inspector by default in Unity.
        private const float DefaultLeftEdgePadding = 18;
        private const float DefaultRightEdgePadding = 5;

        protected abstract GroupElement GroupElement { get; }
        // protected GroupElementList GroupElementList => GroupElement.GroupElementList;
        protected override Element Element => GroupElement;

        public abstract float ChildIndentAmount { get; }
        protected abstract float HeightWithChildren { get; set; }


        protected GroupElementLayout( Element element )
            : base( element )
        {
        }


        public override bool ShouldApplyGlobalLeftPadding() => true;
        public override bool ShouldApplyGlobalRightPadding() => true;
        public override bool ShouldApplyGlobalTopPadding() => true;
        public override bool ShouldApplyGlobalBottomPadding() => true;

        public override bool ShouldApplyFramePadding() => true;

        /// <summary>
        ///     Gets the width a label needs to be to respect the label/field separation line. This includes the label's
        ///     reduction due to any indenting applied either by the element's parent or by custom settings.
        /// </summary>
        protected abstract float GetSingleLineChildLabelWidth();

        /// <summary>
        ///     Updating will be true either when AssignNewPositionRect is called on a root group element or when this method is
        ///     called during AssignElementAndLineHeights on a group element.
        /// </summary>
        public override float GetElementHeight( bool updating = false )
        {
            if ( updating ) UpdateWidthsAndHeights();

            return HeightWithChildren;
        }

        public override void AssignNewPositionRect( bool updateRequired )
        {
            // Check if the window width has changed and, if so, update widths and heights of all elements and set group element's height with children.
            if ( updateRequired ) UpdateWidthsAndHeights();

            // Note that this is called only for the root element. Settings that affect the whole root section go here.
            Rect newPositionRect = EditorGUILayout.GetControlRect( false, GetRequiredHeight( false ) );
            SetPositionRect( newPositionRect, GetUsableWindowWidth() );
            SetChildrenPositionRects();
        }

        public override void SetPositionRect( Rect newPositionRect, float width )
        {
            base.SetPositionRect( newPositionRect, width );
            SetChildrenPositionRects();
        }

        /// <summary>
        ///     Returns the exact width of inspector window.
        /// </summary>
        private float GetUsableWindowWidth() =>
            EditorGUIUtility.currentViewWidth -
            ( ( Element.DrawnInInspector ? DefaultLeftEdgePadding : 0 ) + DefaultRightEdgePadding );

        /// <summary>
        ///     Get the height required to fit all elements of a group including the vertical separation (from global
        ///     settings) between them.
        /// </summary>
        private float GetHeightOfGroup()
        {
            float groupHeight = 0;
            for (int line = 0; line < GroupElement.GetNumberOfLines(); line++)
            {
                groupHeight += GroupElement.GetHeightOfLine( line );

                // Add vertical separation between elements.
                if ( line != GroupElement.GetNumberOfLines() - 1 )
                    groupHeight += GroupElement.GlobalSettings.elementVerticalPadding;
            }

            return groupHeight;
        }

        /// <summary>
        ///     Debug method for visualizing the position rect and it's center vertical line.
        /// </summary>
        private void DrawDebugRectOutlineWithVerticalMiddleLine( Rect lineRect )
        {
            // untouched position rect.
            DrawRectOutline( lineRect, Color.black );
            // Draw center line.
            var centerLineRect = new Rect( lineRect );
            float halfWayPoint = centerLineRect.width / 2f;
            centerLineRect.xMin += halfWayPoint;
            centerLineRect.width = 1f;
            DrawRectOutline( centerLineRect, Color.black );
        }

#region OnlyRunWhenNecessary

        // The methods in this region are focused on caching data to reduce the performance impact of the work that will
        // be done every frame. Ideally, these are run only when a change is required such as when the width of the
        // editor window is changed or the height of a child element such as an array is changed via an interaction.

        /// <summary>
        ///     Assigns widths and heights to all child elements then assigns this group's height with children.
        /// </summary>
        private void UpdateWidthsAndHeights()
        {
            float usableWindowWidth = GroupElement.HasParent() ? Width : GetUsableWindowWidth();
            usableWindowWidth -= PaddingHandler.GetLeftEdgePadding() + PaddingHandler.GetRightEdgePadding();

            AssignWidths( usableWindowWidth );
            AssignElementAndLineHeights();
            SetHeightWithChildren( GetHeightOfGroup() );
        }

        /// <summary>
        ///     This is used to modify the height required to draw all elements in this group before saving to the
        ///     group's HeightWithChildren field.
        /// </summary>
        protected abstract void SetHeightWithChildren( float heightOfGroup );

        /// <summary>
        ///     Determine and assign a width to each element in the element list.
        /// </summary>
        private void AssignWidths( float usableWindowWidth )
        {
            for (int line = 0; line < GroupElement.GetNumberOfLines(); line++)
            {
                // Calculate the remaining scalable width.
                float scalableWidth = usableWindowWidth;
                scalableWidth -= GroupElement.GetConstantWidthTotalForLine( line );

                // Check if the first element on this line has opted to use default label width.
                int startingIndex = 0;
                Element firstElement = GroupElement.GetElement( line, 0 );
                if ( firstElement.CustomSettings.UseIndentedDefaultLabelWidth )
                {
                    // Get the default label width and subtract if from the usable window width.
                    // Todo: UseIndentedDefaultLabelWidth Issue: Not sure why this 4f reduction is necessary to get the field lined up with the label/field separator. Test element was grandchild.
                    float indentedLabelWidth = GetSingleLineChildLabelWidth();
                    // float indentedLabelWidth = GetSingleLineChildLabelWidth() - 4f;

                    scalableWidth -= indentedLabelWidth;
                    firstElement.Layout.Width = indentedLabelWidth;

                    // Increment the starting index so the first element isn't processed twice.
                    startingIndex++;
                }

                // Finally, assign the widths.
                for (int column = startingIndex; column < GroupElement.GetNumberOfElementsOnLine( line ); column++)
                {
                    Element element = GroupElement.GetElement( line, column );

                    if ( element.CustomSettings.ConstantWidth > 0 ) continue;

                    float percentageOfWidth = element.Layout.ColumnWidthPriority / GroupElement.GetWidthPriorityTotalForLine( line );
                    // This will allow priorities to be changed in realtime, however, the cost is recalculating width priority ever frame so it's not ideal.
                    // float percentageOfWidth = element.Layout.ColumnWidthPriority / GroupElementList.GetNewWidthPriorityTotalForLine( line );
                    element.Layout.Width = scalableWidth * percentageOfWidth;
                }
            }
        }

        /// <summary>
        ///     Determine and assign both the height of each element and the height of the line they are one, which is
        ///     set to the height of the tallest element on the line. Widths must have already been assigned.
        /// </summary>
        private void AssignElementAndLineHeights()
        {
            for (int line = 0; line < GroupElement.GetNumberOfLines(); line++)
            {
                // bool secondRunRequired = false;
                float heightOfTallestElement = 0;
                for (int column = 0; column < GroupElement.GetNumberOfElementsOnLine( line ); column++)
                {
                    Element element = GroupElement.GetElement( line, column );
                    // secondRunRequired |= element.CustomSettings.UseFullHeightOfLine;
                    ElementLayout elementLayout = element.Layout;

                    // Note that if this element is a group, this is recursive so that it includes the height of all
                    // grandchildren and so forth.
                    elementLayout.Height = elementLayout.GetRequiredHeight( true );

                    heightOfTallestElement = Mathf.Max( heightOfTallestElement, elementLayout.Height );
                }

                // Cache the line height.
                GroupElement.SetHeightForLine( line, heightOfTallestElement );

                // if ( !secondRunRequired ) return;

                // Check if any elements have opted to utilize the full height of the line.
                for (int column = 0; column < GroupElement.GetNumberOfElementsOnLine( line ); column++)
                {
                    Element element = GroupElement.GetElement( line, column );
                    if ( !( element.CustomSettings.CenterInFullHeightOfLine ||
                            element.CustomSettings.FullHeightFrame ) ) continue;

                    ElementLayout elementLayout = element.Layout;
                    // elementLayout.ExtraVerticalSpace = ( GroupElementList.GetHeightOfLine( line ) - elementLayout.Height );
                    elementLayout.ExtraVerticalSpace = Mathf.Max( heightOfTallestElement - elementLayout.Height, 0 );
                    element.Layout.Height = GroupElement.GetHeightOfLine( line );
                    // element.Layout.Height = GroupElementList.GetHeightOfLine( line );
                }
            }
        }

#endregion


#region RunEveryFrame

        /// <summary>
        ///     Assign position rects for all elements using existing values. The position rect for the parent group element
        ///     must already be established.
        /// </summary>
        protected virtual void SetChildrenPositionRects()
        {
            if ( GroupElement.GetNumberOfElements() == 0 || !GroupElement.IsVisible )
                return;

            var groupDrawRect = new Rect( GetDrawRect() );
            for (int currentLine = 0; currentLine < GroupElement.GetNumberOfLines(); currentLine++)
            {
                groupDrawRect.height = GroupElement.GetHeightOfLine( currentLine );
                SetChildPositionRectsForLine( currentLine, groupDrawRect );
                groupDrawRect.yMin += groupDrawRect.height + GroupElement.GlobalSettings.elementVerticalPadding;
            }
        }

        /// <summary>
        ///     Partitions space on a line of the section into rects for each element on the line. The width assigned to
        ///     each rect depends on the line's element's constantWidth and columnWidthPriority settings.
        /// </summary>
        private void SetChildPositionRectsForLine( int line, Rect lineRect )
        {
            // Any settings that edit the layout of a section line go here.
            float totalHorizontalOffset = lineRect.x;
            for (int column = 0; column < GroupElement.GetNumberOfElementsOnLine( line ); column++)
            {
                totalHorizontalOffset += SetChildPositionRect( line, column, lineRect, totalHorizontalOffset );
            }
        }

        /// <summary>
        ///     Calculates the position rect for the child element on line, at column based on the lineRect. Returns
        ///     the width of the child element for keeping track of the current horizontal offset on the line.
        /// </summary>
        protected float SetChildPositionRect( int line, int column, Rect lineRect, float totalHorizontalOffset )
        {
            Element element = GroupElement.GetElement( line, column );
            ElementLayout currentChildLayout = element.Layout;

            var elementPositionRect = new Rect( lineRect )
            {
                x = totalHorizontalOffset,
                width = currentChildLayout.Width,
                height = currentChildLayout.Height
            };

            // Limit last rect on line to the length of the root control rect. This is how we can have both a variable
            // height and a width that respects the presence of a scrollbar.
            if ( currentChildLayout.IsLastOnLine && GroupElement.GlobalSettings.widthTruncating )
                elementPositionRect.xMax = Mathf.Min( elementPositionRect.xMax, lineRect.xMax );

            currentChildLayout.SetPositionRect( elementPositionRect, currentChildLayout.Width );

            return currentChildLayout.Width;
        }

#endregion
    }
}