namespace ACEPackage.Editor.Scripts.Elements.SingleElements.Decorator.DividingLine
{
    public class DividingLineElementLayout : SingleElementLayout
    {
        private readonly DividingLineElement _dividingLineElement;
        protected override SingleElement SingleElement => _dividingLineElement;
        
        
        public DividingLineElementLayout( DividingLineElement dividingLineElement ) : base( dividingLineElement )
        {
            _dividingLineElement = dividingLineElement;
        }


        // This ensures that no padding is added to the single element.
        public override float GetElementHeight( bool updating = false ) => _dividingLineElement.BoxHeight;
    }
}
