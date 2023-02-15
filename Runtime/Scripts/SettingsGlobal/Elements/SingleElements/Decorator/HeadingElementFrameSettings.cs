using System;
using UnityEngine;

namespace ACEPackage.Runtime.Scripts.SettingsGlobal.Elements.SingleElements.Decorator
{
    [Serializable]
    public class HeadingElementFrameSettings : FrameSettings
    {
        /// <summary>
        ///     Shifts the element to the right.
        /// </summary>
        [Range( 0, 30 )]
        public float textHorizontalOffset = 15f;
        
        /// <summary>
        ///     Expands the element vertically with the heading vertically centered.
        /// </summary>
        [Range( 0, 15 )] 
        public float boxHeight = 0;
        
        /// <summary>
        ///     The color of the heading text when the heading is active.
        /// </summary>
        public int enabledTextColorIndex = 0;
        
        /// <summary>
        ///     The color of the heading text when the heading is inactive.
        /// </summary>
        public int disabledTextColorIndex = 1;
    }
}
