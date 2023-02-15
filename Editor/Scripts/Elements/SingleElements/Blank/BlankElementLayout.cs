namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.SingleElements.Blank
{
    public class BlankElementLayout : SingleElementLayout
    {
        private readonly BlankElement _blankElement;
        protected override SingleElement SingleElement => _blankElement;

        
        public BlankElementLayout( BlankElement blankElement ) : base( blankElement )
        {
            _blankElement = blankElement;
        }
    }
}
