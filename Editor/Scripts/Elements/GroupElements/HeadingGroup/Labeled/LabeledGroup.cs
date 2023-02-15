using ACEPackage.Editor.Scripts.Elements.SingleElements.Decorator.Heading.Labeled;
using ACEPackage.Runtime.Scripts.SettingsCustom.Groups;
using ACEPackage.Runtime.Scripts.SettingsGlobal;
using JetBrains.Annotations;
using UnityEngine;

namespace ACEPackage.Editor.Scripts.Elements.GroupElements.HeadingGroup.Labeled
{
    public class LabeledGroup : HeadingGroup
    {
        public override FrameSettings HeadingGroupFrameSettings => AceTheme.GetLabeledGroupFrameSettingsForLevel( ElementLevel );
        protected override HeadingGroupLayout HeadingGroupLayout => _labeledGroupLayout;
        protected override HeadingGroupDraw HeadingGroupDraw => _labeledGroupDraw;
        private LabeledGroupLayout _labeledGroupLayout;
        private LabeledGroupDraw _labeledGroupDraw;
        

        public LabeledGroup( 
            GUIContent guiContent, 
            [CanBeNull] GroupCustomSettings groupCustomSettings, 
            params Element[] newElements ) 
            : base( GUIContent.none, null, new LabeledHeading( guiContent ), groupCustomSettings, newElements )
        {
        }
        
        
        protected override void InitializeLayout() => _labeledGroupLayout = new LabeledGroupLayout( this );
        protected override void InitializeDraw() => _labeledGroupDraw = new LabeledGroupDraw( this );
    }
}
