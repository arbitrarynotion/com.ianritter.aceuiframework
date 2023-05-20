using UnityEditor;
using UnityEngine;

namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.SingleElements.Properties.Popup.CustomColorEntryPopup
{
    public class CustomColorEntryPopupElementDraw : PopupElementDraw
    {
        private readonly CustomColorEntryPopupElement _customColorEntryPopupElement;
        protected override PropertyElement PropertyElement => _customColorEntryPopupElement;
        
        public CustomColorEntryPopupElementDraw( CustomColorEntryPopupElement customColorEntryPopupElement ) 
            : base( customColorEntryPopupElement )
        {
            _customColorEntryPopupElement = customColorEntryPopupElement;
        }
    
        protected override void DrawPopup( Rect drawRect )
        {
            EditorGUI.BeginChangeCheck();
            _customColorEntryPopupElement.Property.intValue = EditorGUI.Popup( drawRect, _customColorEntryPopupElement.Property.intValue, _customColorEntryPopupElement.Options );
            if (EditorGUI.EndChangeCheck())
                _customColorEntryPopupElement.ChangeCallBack?.Invoke();
        }
    }
}
