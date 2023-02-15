using JetBrains.Annotations;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.SingleElements.Decorator.Heading.Toggle;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsCustom.Groups;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsGlobal;
using UnityEngine;

namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.GroupElements.HeadingGroup.Toggle
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


        protected override void ApplyPropertyState() => IsEnabled = HeadingProperty.boolValue;

        protected override void InitializeLayout() => _toggleGroupLayout = new ToggleGroupLayout( this );

        protected override void InitializeDraw() => _toggleGroupDraw = new ToggleGroupDraw( this );
    }
}
