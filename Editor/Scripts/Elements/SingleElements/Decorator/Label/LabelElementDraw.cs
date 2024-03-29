using UnityEditor;
using UnityEngine;

namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.SingleElements.Decorator.Label
{
    public class LabelElementDraw : SingleElementDraw
    {
        private GUIStyle _style;
        private readonly LabelElement _labelElement;
        protected override SingleElement SingleElement => _labelElement;


        public LabelElementDraw( LabelElement labelElement )
        {
            _labelElement = labelElement;
        }


        protected override void DrawElementContents()
        {
            if ( _style == null )
            {
                _style = new GUIStyle( EditorStyles.label );
                if ( Element.CustomSettings.BoldLabel )
                {
                    _style.richText = true;
                    _style.fontStyle = FontStyle.Bold;
                    _style.normal.textColor = _labelElement.GlobalSettings.headingTextEnabledColor;
                }
            }

            DrawAlignedLabelField( _style );
        }
    }
}