using JetBrains.Annotations;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.SingleElements.Decorator.Heading.Foldout;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsCustom.Groups;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsGlobal;
using UnityEngine;

namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.GroupElements.HeadingGroup.FoldOut
{
    public class FoldoutGroup : HeadingGroup
    {
        public override FrameSettings HeadingGroupFrameSettings => AceTheme.GetFoldoutGroupFrameSettingsForLevel( ElementLevel );

        protected override HeadingGroupLayout HeadingGroupLayout => FoldoutGroupLayout;
        protected override HeadingGroupDraw HeadingGroupDraw => FoldoutGroupDraw;

        private FoldoutGroupLayout FoldoutGroupLayout { get; set; }
        private FoldoutGroupDraw FoldoutGroupDraw { get; set; }


        public FoldoutGroup(
            [CanBeNull] string foldoutVarName,
            GUIContent guiContent,
            [CanBeNull] GroupCustomSettings groupCustomSettings,
            params Element[] newElements )
            : base( guiContent, foldoutVarName, new FoldoutHeading( guiContent ), groupCustomSettings, newElements )
        {
        }

        protected override void ApplyPropertyState() => IsVisible = HeadingProperty.boolValue;

        protected override void InitializeLayout() => FoldoutGroupLayout = new FoldoutGroupLayout( this );

        protected override void InitializeDraw() => FoldoutGroupDraw = new FoldoutGroupDraw( this );
    }
}