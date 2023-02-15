using ACEPackage.Editor.Scripts.ElementConditions;
using ACEPackage.Runtime.Scripts.SettingsCustom.SingleElements;
using UnityEditor;
using UnityEngine;

namespace ACEPackage.Editor.Scripts.Elements.SingleElements.Decorator.Label
{
    public class LabelElement : SingleElement
    {
        private LabelElementLayout _labelElementLayout;
        private LabelElementDraw _labelElementDraw;

        public override SingleElementLayout SingleElementLayout => _labelElementLayout;
        public override SingleElementDraw SingleElementDraw => _labelElementDraw;

        public readonly bool Bold = false;
        

        public LabelElement( 
            GUIContent guiContent, 
            bool bold = false,
            SingleCustomSettings singleCustomSettings = null ) :
            base( guiContent, singleCustomSettings,false, new ElementCondition[] {} )
        {
            Bold = bold;
        }
        
        
        protected override void InitializeElement( SerializedObject targetScriptableObject ) {}

        protected override void InitializeLayout() => _labelElementLayout = new LabelElementLayout( this );
        protected override void InitializeDraw() => _labelElementDraw = new LabelElementDraw( this );
        
        /// <summary>
        /// Set the text displayed by the label. The initial value is set during declaration. Us this to update the
        /// label if it represents a value that changes.
        /// </summary>
        public void SetLabel( string labelText )
        {
            // Todo: I'll have to test this to see if it updates in the inspector. If not, I would have to get a listener in place to trigger redraws.
            GUIContent.text = labelText;
        }
    }
}
