namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.SingleElements.Button.PathButton
{
    public class PathButtonElementLayout : ButtonElementLayout
    {
        private readonly ButtonElement _buttonElement;
        protected override SingleElement SingleElement => _buttonElement;
        
        
        public PathButtonElementLayout( ButtonElement buttonElement ) : base( buttonElement )
        {
            _buttonElement = buttonElement;
        }
    }
}
