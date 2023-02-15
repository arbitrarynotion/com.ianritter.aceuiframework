namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.SingleElements.Button.Basic
{
    public class BasicButtonElementLayout : ButtonElementLayout
    {
        private readonly ButtonElement _buttonElement;
        protected override SingleElement SingleElement => _buttonElement;
        
        public BasicButtonElementLayout( ButtonElement buttonElement ) : base( buttonElement )
        {
            _buttonElement = buttonElement;
        }
    }
}
