namespace ACEPackage.Editor.Scripts.Elements.SingleElements.Decorator.Label
{
    public class LabelElementLayout : SingleElementLayout
    {
        private readonly LabelElement _labelElement;
        protected override SingleElement SingleElement => _labelElement;

        
        public LabelElementLayout( LabelElement labelElement ) : base( labelElement )
        {
            _labelElement = labelElement;
        }
    }
}
