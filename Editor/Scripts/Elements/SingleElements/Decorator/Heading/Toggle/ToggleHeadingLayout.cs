namespace ACEPackage.Editor.Scripts.Elements.SingleElements.Decorator.Heading.Toggle
{
    public class ToggleHeadingLayout : HeadingElementLayout
    {
        private ToggleHeading _toggleHeading;
        
        
        public ToggleHeadingLayout( ToggleHeading toggleHeading ) 
            : base( toggleHeading )
        {
            _toggleHeading = toggleHeading;
        }
    }
}
