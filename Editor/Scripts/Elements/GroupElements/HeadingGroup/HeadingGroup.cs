using JetBrains.Annotations;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.SingleElements.Decorator.Heading;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsCustom.Groups;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsGlobal;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsGlobal.Elements.Groups.HeadingGroups;
using UnityEditor;
using UnityEngine;

namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.GroupElements.HeadingGroup
{
    public abstract class HeadingGroup : GroupElement
    {
        public HeadingGroupSettings HeadingGroupSettings => AceTheme.GetHeadingGroupSettingsForLevel( ElementLevel );
        public ChildAreaGroup.ChildAreaGroup ChildAreaGroup { get; private set; }

        public abstract FrameSettings HeadingGroupFrameSettings { get; }
        public HeadingElement HeadingElement { get; }

        public override GroupElementLayout GroupElementLayout => HeadingGroupLayout;
        public override ElementDraw Draw => HeadingGroupDraw;

        public override Settings Settings => HeadingGroupSettings;
        public override FrameSettings GetElementFrameSettings() => HeadingGroupFrameSettings;
        
        public override bool IsVisible
        {
            // As a heading group is always visible, the visibility queries are redirected to its child area.
            get => ChildAreaGroup.IsVisible;
            set => ChildAreaGroup.IsVisible = value;
        }
        
        public bool HasProperty { get; }
        private readonly string _headingPropertyVarName;
        public SerializedProperty HeadingProperty { get; private set; }


        protected HeadingGroup( 
            GUIContent guiContent, 
            [CanBeNull] string headingPropertyVarName, 
            HeadingElement headingElement, 
            GroupCustomSettings groupCustomSettings,
            Element[] newElements ) : 
            base( guiContent, groupCustomSettings )
        {
            HeadingElement = headingElement;
            
            if ( headingPropertyVarName != null )
            {
                HasProperty = true;
                _headingPropertyVarName = headingPropertyVarName;
            }
            _headingPropertyVarName = headingPropertyVarName;
            
            BuildElementSet( headingElement, newElements );
        }
        

        /// <summary>
        ///     Returns true if the heading group is collapsed. When collapsed, only the heading of the group is drawn.
        /// </summary>
        public bool ShouldHideChildren() => !IsVisible || !IsEnabled && HideOnDisable;
        
        public override int GetElementLevel() => IsRootElement() ? 0 : ParentElement.GetElementLevel() + 1;

        private void BuildElementSet( HeadingElement headingElement, Element[] newElements )
        {
            ChildAreaGroup = new ChildAreaGroup.ChildAreaGroup( true, newElements, 
                new GroupCustomSettings { NumberOfColumns = GroupCustomSettings.NumberOfColumns } );
            Element[] elements =
            {
                headingElement,
                ChildAreaGroup
            };
            InstantiateGroupData( elements );
        }
        
        protected abstract HeadingGroupLayout HeadingGroupLayout { get; }
        protected abstract HeadingGroupDraw HeadingGroupDraw { get; }
        
        protected override void InitializeElement( SerializedObject targetScriptableObject )
        {
            InitializeElementsList( targetScriptableObject, AceTheme, GroupCustomSettings );
            InitializeLocalProperties( targetScriptableObject );
        }
        
        private void InitializeLocalProperties( SerializedObject targetScriptableObject )
        {
            if (!HasProperty)
                return;
            
            HeadingProperty = targetScriptableObject.FindProperty( _headingPropertyVarName );

            if (HeadingProperty == null)
                Debug.LogWarning( $"TG|ILP: {GetName()}: Error! Failed to find heading property for {_headingPropertyVarName}!" );
        }

        /// <summary>
        ///     Ensure that the class member aligns with the initial state of the property it's aligned with.
        /// </summary>
        protected abstract void ApplyPropertyState();
    }
}