namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.SingleElements.Properties.Basic
{
    public class BasicPropertyDraw : PropertyElementDraw
    {
        private readonly BasicProperty _basicProperty;
        protected override PropertyElement PropertyElement => _basicProperty;
        
        public BasicPropertyDraw( BasicProperty basicProperty )
        {
            _basicProperty = basicProperty;
        }
    }
}
