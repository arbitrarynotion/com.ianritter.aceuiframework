using UnityEditor;
using UnityEngine;

namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.SingleElements.Properties.Popup
{
    public class PopupElementDraw : PropertyElementDraw
    {
        private readonly PopupElement _popupElement;
        protected override PropertyElement PropertyElement => _popupElement;

        
        public PopupElementDraw( PopupElement popupElement )
        {
            _popupElement = popupElement;
        }
        
        
        protected override void DrawPropertyFieldWithOutLabel( Rect fieldRect ) => DrawPopup( fieldRect );

        protected override void DrawPropertyFieldWithLabel( Rect drawRect )
        {
            // Label width has already been handled. Just need to draw the label with the EditorGUIUtility.LabelWidth
            // and draw the min/max slider with float fields in the remaining width of the draw rect.
            
            // Build label rect.
            var labelRect = new Rect( drawRect )
            {
                width = EditorGUIUtility.labelWidth
            };
            
            DrawLabelField( labelRect );

            // Build field rect.
            var fieldRect = new Rect( drawRect );
            fieldRect.xMin += EditorGUIUtility.labelWidth;
            
            DrawPopup( fieldRect );
        }
        
        protected virtual void DrawPopup( Rect drawRect )
        {
            EditorGUI.BeginChangeCheck();
            _popupElement.Property.intValue = EditorGUI.Popup( drawRect, _popupElement.Property.intValue, _popupElement.Options );
            if (EditorGUI.EndChangeCheck())
                _popupElement.ChangeCallBack?.Invoke();
        }
    }
}
