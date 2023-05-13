namespace Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsCustom.SingleElements.PropertyCustomSettings
{
    public class PropertyCustomSettings : SingleCustomSettings
    {
        /// <summary>
        ///     Specify the minimum width of this element's field which is used to determine when to add a second line for
        ///     applicable element types. Note that this is an override for the CISettings field min width settings.
        /// </summary>
        public bool ConvertToLiveLabel { get; set; } = false;
    }
}
