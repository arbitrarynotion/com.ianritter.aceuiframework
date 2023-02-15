using UnityEditor;

namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.GroupElements.BasicGroup
{
    public class BasicGroupLayout : GroupElementLayout
    {
        public override float ChildIndentAmount { get; } = 0f;
        
        protected override GroupElement GroupElement => BasicGroup;
        protected override float HeightWithChildren { get; set; }

        protected override float ColumnWidthPriorityAdjustment =>
            BasicGroup.GlobalSettings.groupWidthPriorityAdjustment;
        
        private BasicGroup BasicGroup { get; }



        public BasicGroupLayout( BasicGroup basicGroup ) : base( basicGroup )
        {
            BasicGroup = basicGroup;
        }

        

        public override bool ShouldApplyFramePadding() => ShouldShowFrame();

        public override bool ShouldShowFrame()
        {
            if ( BasicGroup.CustomSettings.BlockFrame() )
                return false;

            if ( BasicGroup.HasParent() && !BasicGroup.ParentElement.GroupCustomSettings.ChildGroupElementsHaveFrames )
                return false;

            return BasicGroup.FrameSettings.applyFraming;
        }
        
        protected override float GetSingleLineChildLabelWidth() =>
            EditorGUIUtility.labelWidth - ( GetTotalLeftIndentAmount() + PaddingHandler.GetLeftEdgePadding() );

        protected override void SetHeightWithChildren( float heightOfGroup ) => HeightWithChildren = heightOfGroup;
    }
}