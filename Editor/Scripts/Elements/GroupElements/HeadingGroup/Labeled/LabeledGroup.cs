using System;
using JetBrains.Annotations;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.SingleElements.Decorator.Heading.Labeled;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsCustom.Groups;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsGlobal;
using UnityEngine;

namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.GroupElements.HeadingGroup.Labeled
{
    public class LabeledGroup : HeadingGroup
    {
        public override FrameSettings HeadingGroupFrameSettings => AceTheme.GetFoldoutGroupFrameSettingsForLevel( ElementLevel );
        // public override FrameSettings HeadingGroupFrameSettings => AceTheme.GetLabeledGroupFrameSettingsForLevel( ElementLevel );
        protected override HeadingGroupLayout HeadingGroupLayout => _labeledGroupLayout;
        protected override HeadingGroupDraw HeadingGroupDraw => _labeledGroupDraw;
        private LabeledGroupLayout _labeledGroupLayout;
        private LabeledGroupDraw _labeledGroupDraw;
        

        public LabeledGroup( 
            GUIContent guiContent, 
            [CanBeNull] GroupCustomSettings groupCustomSettings, 
            params Element[] newElements ) 
            : base( GUIContent.none, null, new LabeledHeading( guiContent ), groupCustomSettings, null, newElements )
        {
        }
        
        
        protected override void ApplyPropertyState() {}

        protected override void InitializeLayout() => _labeledGroupLayout = new LabeledGroupLayout( this );
        
        protected override void InitializeDraw() => _labeledGroupDraw = new LabeledGroupDraw( this );
    }
}
