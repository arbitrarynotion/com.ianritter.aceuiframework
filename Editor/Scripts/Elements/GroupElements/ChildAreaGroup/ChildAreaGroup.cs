using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsCustom.Groups;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsGlobal;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsGlobal.Elements.Groups.ChildAreaGroups;
using UnityEditor;
using UnityEngine;

namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.GroupElements.ChildAreaGroup
{
    public class ChildAreaGroup : GroupElement
    {
#region Public Override Class Data
        
        public override GroupElementLayout GroupElementLayout => ChildAreaGroupLayout;
        public override ElementDraw Draw => ChildAreaGroupDraw;


        public override Settings Settings => ChildAreaGroupSettings;
        public override FrameSettings GetElementFrameSettings() => ParentElement.FrameSettings;
        
        public override bool IsVisible { get; set; } = true;

#endregion
        
#region Private Class Data

        private bool Indent { get; }
        
        private ChildAreaGroupSettings ChildAreaGroupSettings =>
            AceTheme.GetChildAreaGroupSettingsForLevel( ElementLevel );
        
        private ChildAreaGroupLayout ChildAreaGroupLayout { get; set; }
        private ChildAreaGroupDraw ChildAreaGroupDraw { get; set; }

#endregion


        
#region Constructors

        public ChildAreaGroup( bool indent, Element[] newElements, GroupCustomSettings groupCustomSettings )
            : base( GUIContent.none, groupCustomSettings ) 
        {
            Indent = indent;

            InstantiateGroupData( newElements );
        }

#endregion
        
        
        
#region Public Override Methods
        
        public override int GetElementLevel() => IsRootElement() ? 0 : ParentElement.GetElementLevel() + 0;

#endregion
        
        
#region Protected Override Methods

        protected override void InitializeElement( SerializedObject targetScriptableObject ) =>
            InitializeElementsList( targetScriptableObject, AceTheme, GroupCustomSettings );

        protected override void InitializeLayout() => ChildAreaGroupLayout = new ChildAreaGroupLayout( this, Indent );

        protected override void InitializeDraw() =>
            ChildAreaGroupDraw = new ChildAreaGroupDraw( this );

#endregion
    }
}