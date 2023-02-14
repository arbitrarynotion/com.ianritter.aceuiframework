namespace ACEPackage.Editor.Scripts.Elements.SingleElements.Decorator.Heading.Foldout
{
    public class FoldoutHeadingLayout : HeadingElementLayout
    {
        FoldoutHeading _foldoutHeading;
        
        public FoldoutHeadingLayout( FoldoutHeading foldoutHeading ) 
            : base( foldoutHeading )
        {
            _foldoutHeading = foldoutHeading;
        }
    }
}
