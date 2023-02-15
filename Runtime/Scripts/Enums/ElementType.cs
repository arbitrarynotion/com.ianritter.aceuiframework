namespace Packages.com.ianritter.aceuiframework.Runtime.Scripts.Enums
{
    public enum ElementType
    {
        None, // Temporary to catch unassigned element info calls.
        
        // Single
        SinglePropertyBasic,
        SinglePropertyMinMaxSlider,
        SinglePropertyPopup,
        SingleBlank,
        SingleButtonBasic,
        SingleButtonTab,
        SingleDecoratorDividingLine,
        SingleDecoratorLabel,
        
        // Group
        GroupBasic,
        GroupComposite,
        GroupHeadingFoldout,
        GroupHeadingToggle,
        GroupHeadingLabeled,
    }
}
