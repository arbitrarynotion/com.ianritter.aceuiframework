using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.GroupElements.HeadingGroup;
using UnityEngine;

namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.SingleElements.Decorator.Heading.Foldout
{
    public class FoldoutHeading : HeadingElement
    {
        private FoldoutHeadingLayout _foldoutHeadingLayout;
        private FoldoutHeadingDraw _foldoutHeadingDraw;
        
        public override HeadingElementLayout HeadingElementLayout => _foldoutHeadingLayout;
        public override HeadingElementDraw HeadingElementDraw => _foldoutHeadingDraw;

        
        public FoldoutHeading( GUIContent guiContent ) 
            : base( guiContent )
        {
        }
        

        protected override void InitializeLayout() => _foldoutHeadingLayout = new FoldoutHeadingLayout( this );
        protected override void InitializeDraw() => _foldoutHeadingDraw = new FoldoutHeadingDraw( this );
    }
}
