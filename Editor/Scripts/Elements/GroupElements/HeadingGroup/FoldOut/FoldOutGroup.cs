using ACEPackage.Editor.Scripts.Elements.SingleElements.Decorator.Heading.Foldout;
using ACEPackage.Runtime.Scripts.SettingsCustom.Groups;
using ACEPackage.Runtime.Scripts.SettingsGlobal;
using JetBrains.Annotations;
using UnityEngine;

namespace ACEPackage.Editor.Scripts.Elements.GroupElements.HeadingGroup.FoldOut
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
            : base( GUIContent.none, foldoutVarName, new FoldoutHeading( guiContent ), groupCustomSettings, newElements )
        {
        }

        protected override void InitializeLayout() => FoldoutGroupLayout = new FoldoutGroupLayout( this );

        protected override void InitializeDraw() => FoldoutGroupDraw = new FoldoutGroupDraw( this );
    }
}