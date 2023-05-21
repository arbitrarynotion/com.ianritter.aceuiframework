using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsGlobal;
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
            // Entries as saved as a string customColorName instead of an index.
            
            EditorGUI.BeginChangeCheck();
            int returnIndex = EditorGUI.Popup( drawRect, _customColorEntryPopupElement.SelectedIndex, _customColorEntryPopupElement.Options );
            if ( !EditorGUI.EndChangeCheck() ) return;
            
            // When the popup returns, look up the name and write it back to the string property.
            _customColorEntryPopupElement.UpdateSelectedColorByIndex( returnIndex );
            
            _customColorEntryPopupElement.ChangeCallBack?.Invoke();
            
            // EditorGUI.BeginChangeCheck();
            // _customColorEntryPopupElement.Property.intValue = EditorGUI.Popup( drawRect, _customColorEntryPopupElement.Property.intValue, _customColorEntryPopupElement.Options );
            // if (EditorGUI.EndChangeCheck())
            //     _customColorEntryPopupElement.ChangeCallBack?.Invoke();
        }
    }
}
