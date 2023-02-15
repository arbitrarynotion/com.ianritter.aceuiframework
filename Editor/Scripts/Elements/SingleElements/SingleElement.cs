using ACEPackage.Editor.Scripts.ElementConditions;
using ACEPackage.Runtime.Scripts.SettingsCustom;
using ACEPackage.Runtime.Scripts.SettingsCustom.SingleElements;
using ACEPackage.Runtime.Scripts.SettingsGlobal;
using ACEPackage.Runtime.Scripts.SettingsGlobal.Elements.SingleElements;
using ACEPackage.Runtime.Scripts.SettingsGlobal.PropertySpecific;
using UnityEngine;

namespace ACEPackage.Editor.Scripts.Elements.SingleElements
{
    public abstract class SingleElement : Element
    {
        public PropertySpecificSettings PropertiesSettings => AceTheme.GetPropertySpecificSettings();
        
        // Single Element specific settings.
        public SingleElementSettings SingleElementSettings => AceTheme.GetSingleElementSettingsForLevel( ElementLevel );
        public SingleElementFrameSettings SingleElementFrameSettings => AceTheme.GetSingleElementFrameSettingsForLevel( ElementLevel );
        
        public override FrameSettings GetElementFrameSettings() => SingleElementFrameSettings;

        // Single Element level references.
        public SingleCustomSettings SingleCustomSettings { get; private set; }
        public abstract SingleElementDraw SingleElementDraw { get; }
        public abstract SingleElementLayout SingleElementLayout { get; }

        // Element level overrides.
        public override Settings Settings => SingleElementSettings;
        
        public override CustomSettings CustomSettings => SingleCustomSettings;
        public override ElementLayout Layout => SingleElementLayout;
        public override ElementDraw Draw => SingleElementDraw;
        
        public override bool IsVisible { get; set; } = true;
        

        protected SingleElement( 
            GUIContent guiContent, 
            SingleCustomSettings singleCustomSettings,
            bool hideOnDisable, 
            ElementCondition[] conditions ) : 
            base( guiContent, hideOnDisable, conditions  )
        {
            SingleCustomSettings = singleCustomSettings ?? new SingleCustomSettings();
        }

        
        public virtual bool ElementIsValid() => true;
    }
}