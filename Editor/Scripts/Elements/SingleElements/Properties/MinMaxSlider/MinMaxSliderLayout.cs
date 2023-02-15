namespace ACEPackage.Editor.Scripts.Elements.SingleElements.Properties.MinMaxSlider
{
    public class MinMaxSliderLayout : PropertyElementLayout
    {
        private readonly MinMaxSliderElement _minMaxSliderElement;
        protected override PropertyElement PropertyElement => _minMaxSliderElement;
        
        protected override float ColumnWidthPriorityAdjustment => _minMaxSliderElement.GlobalSettings.minMaxWidthPriorityAdjustment;
        protected override float GetFieldMinWidth() => _minMaxSliderElement.PropertiesSettings.minMaxSliderMinWidth;
        
        
        public MinMaxSliderLayout( MinMaxSliderElement minMaxSliderElement ) : base( minMaxSliderElement )
        {
            _minMaxSliderElement = minMaxSliderElement;
        }
    }
}
