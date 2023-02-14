using UnityEditor;

namespace ACEPackage.Editor.Scripts.Elements.SingleElements.Decorator.Heading
{
    public abstract class HeadingElementLayout : SingleElementLayout
    {
        private readonly HeadingElement _headingElement;
        protected override SingleElement SingleElement => _headingElement;
        
        public override float GetCollapsedHeight() => _headingElement.HeadingElementLayout.GetElementHeight();


        protected HeadingElementLayout( HeadingElement headingElement ) : base( headingElement )
        {
            _headingElement = headingElement;
        }
        
        
        public override float GetElementHeight( bool updating = false ) => 
            EditorGUIUtility.singleLineHeight + _headingElement.HeadingElementFrameSettings.boxHeight;

        public override bool ShouldApplyGlobalLeftPadding() => false;
        public override bool ShouldApplyGlobalRightPadding() => false;
        public override bool ShouldApplyGlobalTopPadding() => false;
        public override bool ShouldApplyGlobalBottomPadding() => false;

        public override bool ShouldApplyFramePadding() => ShouldShowFrame();
        public override bool ShouldShowFrame() => Element.FrameSettings.applyFraming;
    }
}
