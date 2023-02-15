namespace Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsCustom.Groups
{
    public class GroupCustomSettings : CustomSettings
    {
        /// <summary>
        ///     The maximum number of columns allows on a single line of this section.
        /// </summary>
        public int NumberOfColumns { get; set; } = 1;

        public bool ChildSingleElementsHaveFrames { get; set; } = true;
        
        public bool ChildGroupElementsHaveFrames { get; set; } = true;
        
        public bool ChildSectionElementsHaveFrames { get; set; } = true;
        
        public bool ChildElementsSkipSingleLineFrames { get; set; } = true;
        
        public virtual bool IndentChildren { get; set; } = true;
    }
}
