using UnityEditor;
using UnityEngine;

namespace ACEPackage.Editor.Scripts.Elements.SingleElements.Button.Basic
{
    public class BasicButtonElementDraw : ButtonElementDraw
    {
        private readonly ButtonElement _buttonElement;
        protected override SingleElement SingleElement => _buttonElement;
        
        
        public BasicButtonElementDraw( ButtonElement buttonElement )
        {
            _buttonElement = buttonElement;
        }
        
        
        protected override void DrawElementContents()
        {
            using (new EditorGUI.DisabledScope( _buttonElement.CustomSettings.ForceDisable))
            {
                if (GUI.Button( _buttonElement.Layout.GetDrawRect(), _buttonElement.GUIContent ))
                    _buttonElement.ButtonCallBack?.Invoke();
            }
        }
    }
}
