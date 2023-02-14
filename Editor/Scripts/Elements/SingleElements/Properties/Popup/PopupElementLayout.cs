namespace ACEPackage.Editor.Scripts.Elements.SingleElements.Properties.Popup
{
    public class PopupElementLayout : PropertyElementLayout
    {
        private readonly PopupElement _popupElement;
        protected override PropertyElement PropertyElement => _popupElement;

        
        public PopupElementLayout( PopupElement popupElement ) : base( popupElement )
        {
            _popupElement = popupElement;
        }
    }
}
