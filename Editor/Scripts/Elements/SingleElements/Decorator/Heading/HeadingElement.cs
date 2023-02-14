using ACEPackage.Editor.Scripts.ElementConditions;
using ACEPackage.Runtime.Scripts.SettingsCustom.SingleElements;
using ACEPackage.Runtime.Scripts.SettingsGlobal;
using ACEPackage.Runtime.Scripts.SettingsGlobal.Elements.SingleElements.Decorator;
using UnityEditor;
using UnityEngine;

namespace ACEPackage.Editor.Scripts.Elements.SingleElements.Decorator.Heading
{
    public abstract class HeadingElement : SingleElement
    {
        public HeadingElementFrameSettings HeadingElementFrameSettings => AceTheme.GetHeadingFrameSettingsForLevel( ElementLevel );
        public override FrameSettings GetElementFrameSettings() => HeadingElementFrameSettings;
        
        public abstract HeadingElementLayout HeadingElementLayout { get; }
        public abstract HeadingElementDraw HeadingElementDraw { get; }

        public override SingleElementLayout SingleElementLayout => HeadingElementLayout;
        public override SingleElementDraw SingleElementDraw => HeadingElementDraw;
        
        public override bool IsVisible { get => true; set{} }


        protected HeadingElement( GUIContent guiContent ) :
            base( guiContent, new SingleCustomSettings() {ForceSingleLine = true}, false, new ElementCondition[] {} )
        {
        }
        
        protected override void InitializeElement( SerializedObject targetScriptableObject ) {}
    }
}
