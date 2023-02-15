using UnityEditor;

namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.GroupElements.ChildAreaGroup
{
    /// <summary>
    ///     This is group is used to encapsulate the child elements of a group element. 
    ///     It is only used for this context and should not be used as a stand-alone group.
    /// </summary>
    public class ChildAreaGroupLayout : GroupElementLayout
    {
        public override float ChildIndentAmount => Indent ? 1 : 0;
        
        protected override GroupElement GroupElement => ChildAreaGroup;
        protected override float HeightWithChildren { get; set; }
        protected override float ColumnWidthPriorityAdjustment => ChildAreaGroup.GlobalSettings.groupWidthPriorityAdjustment; 
        
        private ChildAreaGroup ChildAreaGroup { get; }
        private bool Indent { get; }
        
        
        public ChildAreaGroupLayout( ChildAreaGroup childAreaGroup, bool indent ) : base ( childAreaGroup )
        {
            ChildAreaGroup = childAreaGroup;
            Indent = indent;
        }
        

        public override float GetLeftEdgeTypeBasedAdjustment() =>
            ChildIndentAmount * ChildAreaGroup.GlobalSettings.leftIndentUnitAmount;
        
        public override bool ShouldShowFrame() => false;

        public override bool ShouldApplyFramePadding() => true;
        
        protected override float GetSingleLineChildLabelWidth() =>
            EditorGUIUtility.labelWidth - ( GetTotalLeftIndentAmount() + Element.Layout.PaddingHandler.GetLeftEdgePadding() );

        protected override void SetHeightWithChildren( float heightOfGroup ) => HeightWithChildren = heightOfGroup;
    }
}
