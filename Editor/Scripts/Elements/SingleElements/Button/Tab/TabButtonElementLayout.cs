namespace ACEPackage.Editor.Scripts.Elements.SingleElements.Button.Tab
{
    public class TabButtonElementLayout : ButtonElementLayout
    {
        private readonly ButtonElement _buttonElement;
        protected override SingleElement SingleElement => _buttonElement;
        
        
        public TabButtonElementLayout( ButtonElement buttonElement ) : base( buttonElement )
        {
            _buttonElement = buttonElement;
        }
    }
}
