using System;
using UnityEngine;

namespace Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsGlobal.PropertySpecific
{
    [Serializable]
    public class PropertySpecificSettings
    {
        /// <summary>
        /// Wide boxes show the required width of an element in yellow. When that width drops below the usable width, the outline box turns yellow. Used to verify when certain elements should be moving their field the the next line.
        /// </summary>
        public bool showWideModeBoxes = false;
        
        // Labels
            
        /// <summary>
        /// Minimum width of the field element of a property. Used to move field to second line when there's not enough space on a single line.
        /// </summary>
        [Range( 25, 300 )] 
        public float propertyChildLabelWidth = 100f;
            
        /// <summary>
        /// Applied in Element.DrawElement(Rect) and labelWidth In InspectorElement. Needs to be in both places
        /// to offset both the placement and the width calculation.
        /// </summary>
        [Range(0, 10)] 
        public float propertyLabelEndPadding = 0f;

            
        // Fields minimum widths

        /// <summary>
        ///     This includes both ints and floats.
        /// </summary>
        [Range( 40, 200 )] 
        public float propertyNumberFieldsMinWidth = 60f;
            
        /// <summary>
        ///     This includes both ints and floats.
        /// </summary>
        [Range( 40, 200 )] 
        public float propertySlidersMinWidth = 60f;
            
        /// <summary>
        ///     This includes any property with a text field like strings and chars.
        /// </summary>
        [Range( 40, 200 )] 
        public float propertyStringFieldsMinWidth = 60f;
            
        [Range( 40, 200 )] 
        public float propertyBoolsMinWidth = 60f;
            
        [Range( 40, 200 )] 
        public float propertyColorsMinWidth = 60f;
            
        [Range( 40, 200 )] 
        public float propertyAnimationCurvesMinWidth = 60f;
            
        /// <summary>
        ///     Dropdowns include any popups as well as fields that include a popup like layer masks.
        /// </summary>
        [Range( 40, 200 )] 
        public float propertyDropDownsMinWidth = 60f;
            
        /// <summary>
        ///     Sets are things like vectors, rects, and bounds where their field consists of multiple entries.
        /// </summary>
        [Range( 40, 200 )] 
        public float propertySetsMinWidth = 60f;
        
        [Range( 40, 200 )] 
        public float propertyGenericMinWidth = 60f;


        // Min/max Slider
            
        /// <summary>
        ///     Rounding for the float values.
        /// </summary>
        [Range( 1, 5 )] 
        public int minMaxDecimalPlace = 2;

        /// <summary>
        ///     The minimum width of the min/max slider including its float fields. Used to move the slider and its fields to a second line when there is not enough space for it and its label on a single line.
        /// </summary>
        [Range( 40, 200 )] 
        public float minMaxSliderMinWidth = 200f;

        /// <summary>
        ///     Hard float field width for min/max slider. The slider will scale with window width but the float fields won't.
        /// </summary>
        [Range( 35, 60 )] 
        public float minMaxSliderFloatFieldWidth = 35f;

        /// <summary>
        ///     Spacing between the min/max slider and its float fields. Left and right are both always equal.
        /// </summary>
        [Range( 2f, 10f )] 
        public float minMaxSliderSeparation = 4f;
        
        
    }
}