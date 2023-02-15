namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.SingleElements.Properties.Basic
{
    public class BasicPropertyLayout : PropertyElementLayout
    {
        private readonly BasicProperty _basicProperty;
        protected override PropertyElement PropertyElement => _basicProperty;
        
        public BasicPropertyLayout( BasicProperty basicProperty ) : base( basicProperty )
        {
            _basicProperty = basicProperty;
        }
    }
}
