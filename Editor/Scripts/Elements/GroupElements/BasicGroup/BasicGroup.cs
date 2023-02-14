using ACEPackage.Runtime.Scripts.SettingsCustom.Groups;
using ACEPackage.Runtime.Scripts.SettingsGlobal;
using UnityEditor;
using UnityEngine;

namespace ACEPackage.Editor.Scripts.Elements.GroupElements.BasicGroup
{
    public class BasicGroup : GroupElement
    {
        public override GroupElementLayout GroupElementLayout => BasicGroupLayout;
        public override ElementDraw Draw => BasicGroupDraw;
        public override Settings Settings => AceTheme.GetBasicGroupSettingsForLevel( ElementLevel );
        public override FrameSettings GetElementFrameSettings() => AceTheme.GetBasicGroupFrameSettingsForLevel( ElementLevel );
        public override bool IsVisible { get; set; } = true;
        private BasicGroupLayout BasicGroupLayout { get; set; }
        private BasicGroupDraw BasicGroupDraw { get; set; }


        public BasicGroup( GroupCustomSettings groupCustomSettings, Element[] newElements ) :
            base( GUIContent.none, groupCustomSettings )
        {
            InstantiateGroupData( newElements );
        }
        
        
        public override int GetElementLevel() => IsRootElement() ? 0 : ParentElement.GetElementLevel();
        
        protected override void InitializeElement( SerializedObject targetScriptableObject ) =>
            InitializeElementsList( targetScriptableObject, AceTheme, GroupCustomSettings );

        protected override void InitializeLayout() => BasicGroupLayout = new BasicGroupLayout( this );
        
        protected override void InitializeDraw() => BasicGroupDraw = new BasicGroupDraw( this );
        
    }
}