using UnityEditor;
using UnityEngine;

namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.GroupElements.CompositeGroup
{
    public class CompositeGroupLayout : GroupElementLayout
    {
        public override float ChildIndentAmount { get; } = 0f;
        
        protected override GroupElement GroupElement => _compositeGroup;
        protected override float HeightWithChildren { get; set; }
        protected override float ColumnWidthPriorityAdjustment => 0;
        
        private readonly CompositeGroup _compositeGroup;


        public CompositeGroupLayout( CompositeGroup compositeGroup ) : base( compositeGroup )
        {
            _compositeGroup = compositeGroup;
        }

        
        protected override float GetSingleLineChildLabelWidth() =>
            EditorGUIUtility.labelWidth - GetTotalLeftIndentAmount();

        protected override void SetHeightWithChildren( float heightOfGroup ) => HeightWithChildren = heightOfGroup;

        // Hijacked from Single Element Layout.
        // Todo: Work out a way to have static group and single element share settings without duplicating code.
        public override bool ShouldApplyGlobalLeftPadding() => ShouldApplyPaddingToAnySide();
        public override bool ShouldApplyGlobalRightPadding() => ShouldApplyPaddingToAnySide();
        public override bool ShouldApplyGlobalTopPadding() => ShouldApplyPaddingToAnySide();
        public override bool ShouldApplyGlobalBottomPadding() => ShouldApplyPaddingToAnySide();

        public override bool ShouldApplyFramePadding() => ShouldApplyPaddingToAnySide();

        public override bool ShouldShowFrame()
        {
            if ( _compositeGroup.IsRootElement() )
                return false;

            if ( _compositeGroup.CustomSettings.BlockFrame() )
                return false;

            if ( !_compositeGroup.ParentElement.GroupCustomSettings.ChildSingleElementsHaveFrames )
                return false;

            if ( _compositeGroup.HasOwnLine() && _compositeGroup.SingleElementFrameSettings.skipSingleLineFrames )
                return false;

            return _compositeGroup.FrameSettings.applyFraming;
        }

        public override Rect GetFrameRect()
        {
            if ( _compositeGroup.UseSingleElementSettings ) return GetPositionRectWithGlobalAndCustomPadding();

            var customPosRect = new Rect( GetPositionRect() );
            customPosRect = ApplyFullHeightFrameAdjustment( customPosRect );
            return customPosRect;
        }
        
        private bool ShouldApplyPaddingToAnySide() =>
            ShouldShowFrame() && _compositeGroup.UseSingleElementSettings && _compositeGroup.HasParent();
    }
}