namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.SingleElements.Properties.CustomColor
{
    public class ColorPickerElementLayout : PropertyElementLayout
    {
        private readonly ColorPickerElement _popupPickerElement;
        protected override PropertyElement PropertyElement => _popupPickerElement;

        
        public ColorPickerElementLayout( ColorPickerElement colorPickerElement ) : base( colorPickerElement )
        {
            _popupPickerElement = colorPickerElement;
        }
    }
}
