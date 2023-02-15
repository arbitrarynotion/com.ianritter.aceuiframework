using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.GroupElements.HeadingGroup;
using UnityEditor;
using UnityEngine;

namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.SingleElements.Decorator.Heading.Foldout
{
    public class FoldoutHeadingDraw : HeadingElementDraw
    {
        private readonly FoldoutHeading _foldoutHeading;

        
        public FoldoutHeadingDraw( FoldoutHeading foldoutHeading ) 
            : base( foldoutHeading )
        {
            _foldoutHeading = foldoutHeading;
        }

        
        protected override void DrawElementContents()
        {
            var foldoutGroup = (HeadingGroup) _foldoutHeading.ParentElement;

            if (foldoutGroup.HeadingGroupSettings.hideFoldoutGroupElements)
                return;

            Rect headingDrawRect = _foldoutHeading.HeadingElementLayout.GetDrawRect();

            // Foldouts are offset horizontally by -15 by default which aligns their label to others but leaves their arrow sticking out
            // of their draw area on the left. This undoes that shift, though removing the full 15 wasn't necessary.
            float defaultDropdownArrowOffset = _foldoutHeading.DrawnInInspector ? 13 : 0;
            
            var foldoutRect = new Rect( headingDrawRect );
            foldoutRect.xMin += defaultDropdownArrowOffset + DefaultLeftPadding + _foldoutHeading.HeadingElementFrameSettings.textHorizontalOffset;
            
            // foldoutGroup.IsVisible = EditorGUI.Foldout( headingRect, foldoutGroup.IsVisible, foldoutGroup.GUIContent, true, GetHeadingStyle( EditorStyles.foldout ) );
            
            foldoutGroup.IsVisible = EditorGUI.Foldout( foldoutRect, foldoutGroup.IsVisible, GUIContent.none, true, GetHeadingStyle( EditorStyles.foldout ) );
            headingDrawRect.xMin += 15f + _foldoutHeading.HeadingElementFrameSettings.textHorizontalOffset;
            // DrawLabelField( headingDrawRect );
            DrawLabelField( headingDrawRect, GetHeadingLabelStyle( foldoutGroup.IsVisible ) );
            
            if (foldoutGroup.HasProperty)
                foldoutGroup.HeadingProperty.boolValue = foldoutGroup.IsVisible;
        }
    }
}
