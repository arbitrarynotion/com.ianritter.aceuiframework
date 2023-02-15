using ACEPackage.Runtime.Scripts.SettingsCustom.Groups;
using ACEPackage.Runtime.Scripts.SettingsGlobal;
using ACEPackage.Runtime.Scripts.SettingsGlobal.Elements.SingleElements;
using UnityEditor;
using UnityEngine;

namespace ACEPackage.Editor.Scripts.Elements.GroupElements.CompositeGroup
{
    /// <summary>
    ///     Use a composite group when you want to create a hierarchy of elements that behave as a single element.
    ///     The root composite group will use global Single Element settings while all of its child elements will ignore
    ///     their respective global settings. Customize the child elements of a composite group with Custom Settings.
    /// </summary>
    public class CompositeGroup : GroupElement
    {
        public override GroupElementLayout GroupElementLayout => _compositeGroupLayout;
        public override ElementDraw Draw => _compositeGroupDraw;
        
        public SingleElementFrameSettings SingleElementFrameSettings =>
            AceTheme.GetSingleElementFrameSettingsForLevel( ElementLevel );
        
        public override Settings Settings => SingleElementSettings;
        
        public bool UseSingleElementSettings { get; private set; }

        public override bool IsVisible { get; set; } = true;

        private CompositeGroupDraw _compositeGroupDraw;
        private CompositeGroupLayout _compositeGroupLayout;

        // When UseSingleElementSettings is false, composite group responds to no global settings. When true, it 
        // uses the global single element settings.
        private SingleElementSettings SingleElementSettings => AceTheme.GetSingleElementSettingsForLevel( ElementLevel );
        
        
        public CompositeGroup( GroupCustomSettings groupCustomSettings, Element[] newElements ) :
            base( GUIContent.none, groupCustomSettings )
        {
            InstantiateGroupData( newElements );
        }
        

        public override FrameSettings GetElementFrameSettings() => SingleElementFrameSettings;
        
        public override int GetElementLevel() => IsRootElement() ? 0 : ParentElement.GetElementLevel();

        protected override void InitializeElement( SerializedObject targetScriptableObject )
        {
            UseSingleElementSettings = HasParent() && !ParentIsCompositeGroup();
            InitializeElementsList( targetScriptableObject, AceTheme, GroupCustomSettings );
        }

        protected override void InitializeLayout() => _compositeGroupLayout = new CompositeGroupLayout( this );

        protected override void InitializeDraw() => _compositeGroupDraw = new CompositeGroupDraw( this );
    }
}