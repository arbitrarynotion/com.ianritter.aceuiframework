
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
    }
}
