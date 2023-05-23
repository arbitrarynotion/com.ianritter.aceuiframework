using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.ElementConditions;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.GroupElements.HeadingGroup;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsCustom.SingleElements;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsGlobal;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsGlobal.Elements.SingleElements.Decorator;
using UnityEditor;
using UnityEngine;

namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.SingleElements.Decorator.Heading
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
        
        public HeadingGroup HeadingGroup;


        protected HeadingElement( GUIContent guiContent ) :
            base( guiContent, new SingleCustomSettings() {ForceSingleLine = true}, false, new ElementCondition[] {} )
        {
        }

        protected override void InitializeElement( SerializedObject targetScriptableObject )
        {
            // HeadingElementFrameSettings.EnabledTextColorNameIndex = AceTheme.GetIndexForColorName( HeadingElementFrameSettings.enabledTextColorName );
            // HeadingElementFrameSettings.DisabledTextColorNameIndex = AceTheme.GetIndexForColorName( HeadingElementFrameSettings.disabledTextColorName );
            // HeadingElementFrameSettings.BackgroundInactiveColorNameIndex = AceTheme.GetIndexForColorName( HeadingElementFrameSettings.backgroundInactiveColorName );
        }
    }
}
