using Packages.com.ianritter.aceuiframework.Runtime.Scripts.Enums;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsGlobal;
using UnityEngine;

namespace Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsCustom
{
    public abstract class CustomSettings
    {
        /// <summary>
        ///     This is a modification to the global setting.
        ///     The column width priority level of all elements that share an inspector entry line are summed and the
        ///     amount of column width assigned to this element will be its column width priority divided by that sum. Thus,
        ///     increasing this amount will result in more width being assigned to this element.
        /// </summary>
        public float ColumnWidthPriority { get; set; } = 0f;

        /// <summary>
        ///     A property element is composed of a label and a field. This is width assigned to the field part of this element.
        /// </summary>
        public float ConstantWidth { get; set; } = 0;

        /// <summary>
        ///     If true, element will be disabled no matter what.
        /// </summary>
        public bool ForceDisable { get; set; } = false; 

        /// <summary>
        ///     Use when this element is part of a section that allows multiple elements per line. If this is true, this
        ///     element will have its own line regardless of how many other elements are in the section.
        /// </summary>
        public bool ForceSingleLine { get; set; } = false;
        
        /// <summary>
        ///     While elements are indented automatically to be one indent level deeper than their parent section, if they
        ///     have one, this allows their indent level to be further increased.
        /// </summary>
        public int IndentLevelIncrease { get; set; } = 0;

        /// <summary>
        ///     Set the horizontal position of a label.
        /// </summary>
        public Alignment LabelAlignment { get; set; } = Alignment.Left;

        /// <summary>
        /// Overrides Single Element global setting. Set the width allocated to the label when checking if element should move field to second line.
        /// </summary>
        public float LabelMinWidth { get; set; } = -1;
        
        /// <summary>
        /// Overrides Property Specific global setting. Sets the width of padding added at the end of a label.
        /// </summary>
        public float LabelEndPadding { get; set; } = -1;

        /// <summary>
        /// When true, label will print in bold.
        /// </summary>
        public bool BoldLabel { get; set; } = false;


        /// <summary>
        ///     If true, element will center in the vertical space available on its line. This will have no effect if
        ///     it is on it's own line since it will already fill the available space.
        /// </summary>
        public bool CenterInFullHeightOfLine { get; set; } 
        
        /// <summary>
        ///     If true, element's frame will draw to the full height available. This is only effective if 
        /// </summary>
        public bool FullHeightFrame { get; set; } 
        
        public bool UseIndentedDefaultLabelWidth { get; set; } 
        
        private bool _overrideGlobalFrameSettings = false;
        
        /// <summary>
        ///     Don't draw a frame regardless of the global settings.
        /// </summary>
        public bool BlockFrame() => _overrideGlobalFrameSettings && !CustomFrameSettings.applyFraming;
        
        /// <summary>
        ///     Disregard global settings and only use custom settings provided.
        /// </summary>
        public bool OverrideFrame() => _overrideGlobalFrameSettings && CustomFrameSettings.applyFraming;
        
        private FrameSettings _customFrameSettings;
        public FrameSettings CustomFrameSettings
        {
            get => _overrideGlobalFrameSettings ? _customFrameSettings : null;

            set
            {
                _overrideGlobalFrameSettings = true;
                _customFrameSettings = value;
            }
        }
        
        /// <summary>
        ///     Space added to the top of an element's position.
        /// </summary>
        [Range(0, 10)]
        public float TopPadding = 0f;
        
        /// <summary>
        ///     Space added to the left of an element's position.
        /// </summary>
        [Range(0, 10)] 
        public float LeftPadding = 0f;
        
        /// <summary>
        ///     Space added to the right of an element's position.
        /// </summary>
        [Range(0, 10)] 
        public float RightPadding = 0f;
        
        /// <summary>
        ///     Space added to the bottom of an element's position.
        /// </summary>
        [Range(0, 10)]
        public float BottomPadding = 0f;
    }
}