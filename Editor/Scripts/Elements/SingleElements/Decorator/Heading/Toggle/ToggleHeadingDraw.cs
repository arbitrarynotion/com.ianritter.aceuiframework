using ACEPackage.Editor.Scripts.Elements.GroupElements.HeadingGroup;
using UnityEditor;
using UnityEngine;

namespace ACEPackage.Editor.Scripts.Elements.SingleElements.Decorator.Heading.Toggle
{
    public class ToggleHeadingDraw : HeadingElementDraw
    {
        private readonly ToggleHeading _toggleHeading;

        
        public ToggleHeadingDraw( ToggleHeading toggleHeading ) 
            : base( toggleHeading )
        {
            _toggleHeading = toggleHeading;
        }


        protected override void DrawElementContents()
        {
            var toggleGroup = (HeadingGroup) _toggleHeading.ParentElement;

            if ( toggleGroup.HeadingGroupSettings.hideFoldoutGroupElements )
                return;

            Rect headingDrawRect = Element.Layout.GetDrawRect();

            var toggleRect = new Rect( headingDrawRect );
            toggleRect.xMin += DefaultLeftPadding + _toggleHeading.HeadingElementFrameSettings.textHorizontalOffset + 2f;

            // The heading displays the toggle for the heading group but it uses the IsEnabled bool from its parent instead
            // of its own.
            // toggleGroup.IsEnabled = GUI.Toggle( headingDrawRect, toggleGroup.IsEnabled, toggleGroup.GUIContent, GetHeadingStyle( EditorStyles.toggle ) );

            toggleGroup.IsEnabled = GUI.Toggle( toggleRect, toggleGroup.IsEnabled, GUIContent.none,
                GetHeadingStyle( EditorStyles.toggle ) );

            headingDrawRect.xMin += 20f + _toggleHeading.HeadingElementFrameSettings.textHorizontalOffset;
            DrawLabelField( headingDrawRect, GetHeadingLabelStyle( toggleGroup.IsEnabled ) );

            if ( toggleGroup.HasProperty )
                toggleGroup.HeadingProperty.boolValue = toggleGroup.IsEnabled;
        }
    }
}
