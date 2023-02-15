using ACEPackage.Editor.Scripts.Elements.SingleElements.Decorator.Heading.Toggle;
using ACEPackage.Runtime.Scripts.SettingsCustom.Groups;
using ACEPackage.Runtime.Scripts.SettingsGlobal;
using JetBrains.Annotations;
using UnityEngine;

namespace ACEPackage.Editor.Scripts.Elements.GroupElements.HeadingGroup.Toggle
{
    public class ToggleGroup : HeadingGroup
    {
        private ToggleGroupLayout _toggleGroupLayout;
        private ToggleGroupDraw _toggleGroupDraw;
        
        protected override HeadingGroupLayout HeadingGroupLayout => _toggleGroupLayout;
        protected override HeadingGroupDraw HeadingGroupDraw => _toggleGroupDraw;
        
        public override FrameSettings HeadingGroupFrameSettings => AceTheme.GetToggleGroupFrameSettingsForLevel( ElementLevel );

        public ToggleGroup( 
            [CanBeNull] string toggleVarName, 
            GUIContent guiContent, 
            GroupCustomSettings groupCustomSettings,
            bool hideOnDisable = false, 
            params Element[] newElements 
            ) 
            : base( GUIContent.none, toggleVarName, new ToggleHeading( guiContent ), groupCustomSettings, newElements )
        {
            HideOnDisable = hideOnDisable;
        }
        
        
        protected override void InitializeLayout() => _toggleGroupLayout = new ToggleGroupLayout( this );

        protected override void InitializeDraw() => _toggleGroupDraw = new ToggleGroupDraw( this );
    }
}
