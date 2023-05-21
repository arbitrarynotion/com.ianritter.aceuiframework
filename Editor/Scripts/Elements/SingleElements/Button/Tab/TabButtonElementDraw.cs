using UnityEditor;
using UnityEngine;

namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.SingleElements.Button.Tab
{
    public class TabButtonElementDraw : ButtonElementDraw
    {
        private readonly ButtonElement _buttonElement;
        protected override SingleElement SingleElement => _buttonElement;
        
        
        public TabButtonElementDraw( ButtonElement buttonElement )
        {
            _buttonElement = buttonElement;
        }
        
        
        protected override void DrawElementContents()
        {
            var style = new GUIStyle( EditorStyles.centeredGreyMiniLabel ) {fontSize = 12};
            if (_buttonElement.Focused) 
                style = new GUIStyle( EditorStyles.toolbarButton ) {fontStyle = FontStyle.Bold };

            using (new EditorGUI.DisabledScope( _buttonElement.CustomSettings.ForceDisable))
            {
                if (GUI.Button( _buttonElement.Layout.GetDrawRect(), _buttonElement.GUIContent, style ))
                    _buttonElement.ButtonCallBack?.Invoke();
            }
        }
    }
}
