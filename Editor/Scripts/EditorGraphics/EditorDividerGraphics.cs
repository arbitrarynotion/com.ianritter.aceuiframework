using UnityEditor;
using UnityEngine;

namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.EditorGraphics
{
    public static class EditorDividerGraphics
    {
        /// <summary>
        ///     Builds a divider in between vertical elements in the inspector window. Presence of scrollbar is not considered
        ///     since there is no straightforward way I could find to detect it.
        /// </summary>
        /// <param name="dividerThickness">Vertical thickness of the line drawn.</param>
        /// <param name="leftMargin"></param>
        /// <param name="rightMargin">Number of pixels of space between the end of the divider line and the right edge
        /// of the of the inspector window.</param>
        /// <param name="dividerColor">Color of the line.</param>
        /// <param name="topSpacing"></param>
        /// <param name="bottomSpacing"></param>
        public static void DrawDivider(
            Color dividerColor,
            int dividerThickness = 1,
            int leftMargin = 5,
            int rightMargin = 5,
            int topSpacing = 12,
            int bottomSpacing = 12
        )
        {
            EditorGUILayout.Space( topSpacing );

            Rect divider = EditorGUILayout.GetControlRect( GUILayout.Height( dividerThickness ) );

            float indentAdjustment = Mathf.Max( 2, ( EditorGUI.indentLevel * 15 ) );

            divider.x = indentAdjustment + leftMargin;
            divider.width += 15 - indentAdjustment + 4 - rightMargin - leftMargin;
            divider.height = dividerThickness;
            EditorGUI.DrawRect( divider, dividerColor );
            
            EditorGUILayout.Space( bottomSpacing );
        }
    }
}
