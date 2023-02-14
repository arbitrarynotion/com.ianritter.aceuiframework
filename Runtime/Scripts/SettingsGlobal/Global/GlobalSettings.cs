using System;
using UnityEngine;

namespace ACEPackage.Runtime.Scripts.SettingsGlobal.Global
{
    [Serializable]
    public class GlobalSettings
    {
        /// <summary>
        /// Adjusts the amount of indent applied to section elements.
        /// </summary>
        [Range(0, 45)] // [Tooltip("Adjusts the amount of indent applied to section elements.")]
        public float leftIndentUnitAmount = 15f;
        
        /// <summary>
        /// Separator between an element and the next vertical element.
        /// </summary>
        [Range(0, 20 )]
        public float elementVerticalPadding = 2f;
        
        /// <summary>
        /// Separator between an element and the next horizontal element on the same line. Not applied to first element on line.
        /// </summary>
        [Range(0, 10 )]
        public float columnGap = 0f;
        
        
        // - Width Priorities
        /// <summary>
        /// Assigns more column width to sections.
        /// </summary>
        [Range( 0, 1 )] 
        public float sectionWidthPriorityAdjustment = 0f;
        
        /// <summary>
        /// Assigns more column width to sections without headings.
        /// </summary>
        [Range( 0, 1 )] 
        public float groupWidthPriorityAdjustment = 0f;
        
        /// <summary>
        /// Assigns more column width to property elements.
        /// </summary>
        [Range( 0, 1 )] 
        public float propertyWidthPriorityAdjustment = 0f;
        
        /// <summary>
        /// Assigns more column width to min/max slider elements.
        /// </summary>
        [Range( 0, 1 )] 
        public float minMaxWidthPriorityAdjustment = 0f;
        
        private const float OutlineBrightness = 0.32f;

        // Default colors
        /// <summary>
        /// The color used for drawing lines around the heading box, the heading frame, and the element frame.
        /// </summary>
        public Color frameOutlineColor = new Color( OutlineBrightness, OutlineBrightness, OutlineBrightness );
        
        /// <summary>
        /// The color used the heading text when it is expanded/enabled.
        /// </summary>
        public Color headingTextEnabledColor = new Color( 1f, 1f, 1f );
        
        
        // Debug options
        /// <summary>
        /// Turn off the feature that ensures position rects don't exceed the width of the window. Useful for diagnosing
        /// column alignment issues which would otherwise have their source masked by this feature.
        /// </summary>
        public bool widthTruncating = true;
        
        /// <summary>
        /// Use grid lines to align elements to set indent levels.
        /// </summary>
        public bool showGridLines = false;
    
        /// <summary>
        /// Vertical lines starting from the default indent and extending out in intervals of 100 pts. Useful for verifying the length of drawn elements.
        /// </summary>
        public bool showMeasurementLines = false;
    }
}
