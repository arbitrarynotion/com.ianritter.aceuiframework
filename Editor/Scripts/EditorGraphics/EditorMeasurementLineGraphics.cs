using UnityEditor;
using UnityEngine;
using static Packages.com.ianritter.aceuiframework.Editor.Scripts.EditorGraphics.EditorDividerGraphics;

namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.EditorGraphics
{
    public static class EditorMeasurementLineGraphics
    {
        /// <summary>
        /// Draws a series of vertical lines positioned at various indent points. Useful in ensuring inspector elements
        /// are lined up correctly.
        /// </summary>
        /// <param name="cetTheme"></param>
        /// <param name="guideLinesBrightness"></param>
        /// <param name="graphLinesBrightness"></param>
        public static void DrawVerticalGridLines( float guideLinesBrightness, float graphLinesBrightness, bool forEditorWindow = false )
        {
            const float indent = 18f;
            const float leftEdgePadding = 0;

            float initialIndent = forEditorWindow ? 0f : indent;
            float labelToFieldDividerPosition = initialIndent + leftEdgePadding + EditorGUIUtility.labelWidth;
            Color guideLinesColor = new Color( guideLinesBrightness, guideLinesBrightness, guideLinesBrightness );
            Color graphLinesColor = new Color( graphLinesBrightness, graphLinesBrightness, graphLinesBrightness );

            DrawVerticalGraphLines( 0, 6 );
            DrawVerticalGraphLines( labelToFieldDividerPosition, 5 );
        
            DrawVerticalLine( guideLinesColor, labelToFieldDividerPosition, 0f, 1f, 1500 );
            DrawVerticalLine( guideLinesColor, ( indent * 1 ) + leftEdgePadding, 0f, 1f, 1500 );
            DrawVerticalLine( guideLinesColor, EditorGUIUtility.currentViewWidth - 6f, 0f, 1f, 1500 );
            DrawVerticalLine( guideLinesColor, EditorGUIUtility.currentViewWidth - 6f - indent + 2f, 0f, 1f, 1500 );
        
            void DrawVerticalGraphLines( float startingPosition, int numberOfLines )
            {
                for (int indentLevel = 0; indentLevel < numberOfLines; indentLevel++)
                {
                    DrawVerticalLine( graphLinesColor, startingPosition + ( indent * indentLevel ) + leftEdgePadding, 0, 1f, 1500 );
                }
            }
        }
    
        /// <summary>
        /// Draws a series of vertical lines at the specified distance apart. Useful for ensuring inspector elements are
        /// being placed correctly.
        /// </summary>
        /// <param name="cetTheme"></param>
        /// <param name="numberOfLines"></param>
        /// <param name="distanceApart"></param>
        /// <param name="color"></param>
        public static void DrawMeasurementLines( int numberOfLines, float distanceApart, Color color )
        {
            const float indent = 15;
            const float leftEdgePadding = 0;

            for (int indentLevel = 0; indentLevel < numberOfLines; indentLevel++)
            {
                DrawVerticalLine( color, indent + leftEdgePadding + ( distanceApart * indentLevel ), 0, 1f, 1000 );
            }
        }

        /// <summary>
        /// Draw a series of indented horizontal lines in the inspector.
        /// </summary>
        /// <param name="levels"></param>
        /// <param name="thickness"></param>
        /// <param name="leftMargin"></param>
        /// <param name="rightMargin"></param>
        /// <param name="topSpacing"></param>
        /// <param name="bottomSpacing"></param>
        public static void DrawIndentedDividers(
            int levels,
            int thickness = 1,
            int leftMargin = 0,
            int rightMargin = 0,
            int topSpacing = 10,
            int bottomSpacing = 10
        )
        {
            DrawDivider( Color.white, thickness, leftMargin, rightMargin, topSpacing, bottomSpacing );
            for (int i = 0; i < levels; i++)
            {
                EditorGUI.indentLevel++;
                DrawDivider( Color.white, thickness, leftMargin, rightMargin, topSpacing, bottomSpacing );
            }

            for (int j = 0; j < levels; j++)
            {
                EditorGUI.indentLevel--;
            }
        }

        // For testing and just for the fuck of it. Looks neat.
        /// <summary>
        /// Draw horizontal lines in the inspector in the shape of an hour glass. 
        /// </summary>
        /// <param name="totalLines"></param>
        /// <param name="topSpace"></param>
        /// <param name="bottomSpace"></param>
        /// <param name="leftMargin"></param>
        /// <param name="rightMargin"></param>
        /// <param name="reductionPerLine"></param>
        /// <param name="distanceBetween"></param>
        public static void DrawHourGlassOfDividers(
            int totalLines = 10,
            int topSpace = 10,
            int bottomSpace = 10,
            int leftMargin = 15,
            int rightMargin = 0,
            int reductionPerLine = 15,
            int distanceBetween = 5 )
        {
            int halfWayPoint = totalLines / 2;

            DrawDivider( Color.white, 1, leftMargin, rightMargin, topSpace, 0 );
            for (int i = 1; i < ( totalLines ); i++)
            {
                int current = i;
                if (i >= halfWayPoint)
                {
                    current = halfWayPoint - ( i % halfWayPoint );
                }

                int margin = ( current * reductionPerLine );
                DrawDivider( Color.white, 1, leftMargin + margin, rightMargin + margin, distanceBetween, 0 );
            }

            DrawDivider( Color.white, 1, leftMargin, rightMargin, distanceBetween, bottomSpace );
        }

        public static void DrawVerticalLine( Color color, float leftMargin, float topMargin, float thickness,
            float length )
        {
            Rect rect = new Rect( leftMargin, topMargin, thickness, length );
            EditorGUI.DrawRect( rect, color );
        }
    }
}
