
namespace Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsCustom.SingleElements
{
    /// <summary>
    ///     Element settings provide a way to control the positioning, padding, and framing of an element. Some of these
    ///     settings act as an override, while others act as a modification to the global settings (settings window).
    /// </summary>
    public class SingleCustomSettings : CustomSettings
    {
        /// <summary>
        ///     Specify the minimum width of this element's field which is used to determine when to add a second line for
        ///     applicable element types. Note that this is an override for the CISettings field min width settings.
        /// </summary>
        public float FieldMinWidth { get; set; } = 0;
        
        /// <summary>
        ///     When true, a property's value will be drawn as a label rather than a field. This will result in only the
        ///     field value being output - its title and tooltip will not be used. If you want to include a label, precede
        ///     this element with a label element and use custom settings to align things how you want.
        /// </summary>
        public bool ConvertFieldToLabel { get; set; } = false;
    }
}
