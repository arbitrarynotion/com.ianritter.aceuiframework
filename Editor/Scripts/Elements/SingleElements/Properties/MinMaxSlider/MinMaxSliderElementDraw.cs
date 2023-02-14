using ACEPackage.Runtime.Scripts;
using UnityEditor;
using UnityEngine;

namespace ACEPackage.Editor.Scripts.Elements.SingleElements.Properties.MinMaxSlider
{
    public class MinMaxSliderElementDraw : PropertyElementDraw
    {
        private readonly MinMaxSliderElement _minMaxSliderElement;
        protected override PropertyElement PropertyElement => _minMaxSliderElement;


        public MinMaxSliderElementDraw( MinMaxSliderElement minMaxSliderElement )
        {
            _minMaxSliderElement = minMaxSliderElement;
        }
        

        protected override void DrawPropertyFieldWithOutLabel( Rect fieldRect ) => DrawMinMaxSliderWithFloatFields( fieldRect );

        protected override void DrawPropertyFieldWithLabel( Rect drawRect )
        {
            var labelDrawRect = new Rect( drawRect )
            {
                width = EditorGUIUtility.labelWidth
            };
            labelDrawRect.width -= Element.PropertySettings.propertyLabelEndPadding;
            DrawLabelField( labelDrawRect );
            
            var fieldRect = new Rect( drawRect );
            fieldRect.xMin += EditorGUIUtility.labelWidth;
            DrawMinMaxSliderWithFloatFields( fieldRect );
        }

        private void DrawMinMaxSliderWithFloatFields( Rect fieldRect )
        {
            fieldRect.xMin += 2f;
            DrawLeftFloatField( fieldRect );
            DrawMinMaxSlider( fieldRect, 
                _minMaxSliderElement.MinPropertyElement.Property,
                _minMaxSliderElement.MaxPropertyElement.Property, 
                _minMaxSliderElement.MinLimit,
                _minMaxSliderElement.MaxLimit
            );
            DrawRightFloatField( fieldRect );
        }

        private void DrawLeftFloatField( Rect fieldRect )
        {
            // Min float field goes in first part.
            var leftFieldRect = new Rect( fieldRect )
            {
                width = _minMaxSliderElement.PropertiesSettings.minMaxSliderFloatFieldWidth
            };

            DrawMinMaxFloatField( _minMaxSliderElement.MinPropertyElement, leftFieldRect, 
                _minMaxSliderElement.MinLimit, 
                _minMaxSliderElement.MaxPropertyElement.Property.floatValue );
        }

        private void DrawRightFloatField( Rect fieldRect )
        {
            // Max float field goes in third and final part.
            var rightFieldRect = new Rect( fieldRect )
            {
                width = _minMaxSliderElement.PropertiesSettings.minMaxSliderFloatFieldWidth
            };
            rightFieldRect.x += fieldRect.width - _minMaxSliderElement.PropertiesSettings.minMaxSliderFloatFieldWidth;

            DrawMinMaxFloatField( _minMaxSliderElement.MaxPropertyElement, rightFieldRect, 
                _minMaxSliderElement.MinPropertyElement.Property.floatValue, _minMaxSliderElement.MaxLimit );
        }

        /// <summary>
        ///     Draws a min/max slider in the provided rect.
        /// </summary>
        private void DrawMinMaxSlider( Rect fieldRect, SerializedProperty minProperty, SerializedProperty maxProperty, float minLimit, float maxLimit )
        {
            // Min/max slider goes in second part.
            // Adjust rect to include buffer space between the two float fields.
            var minMaxSliderRect = new Rect( fieldRect );
            minMaxSliderRect.xMin += ( _minMaxSliderElement.PropertiesSettings.minMaxSliderFloatFieldWidth + _minMaxSliderElement.PropertiesSettings.minMaxSliderSeparation );
            minMaxSliderRect.xMax -= ( _minMaxSliderElement.PropertiesSettings.minMaxSliderFloatFieldWidth + _minMaxSliderElement.PropertiesSettings.minMaxSliderSeparation );
            
            float minValue = minProperty.floatValue.RoundToDecimalPlace( _minMaxSliderElement.PropertiesSettings.minMaxDecimalPlace );
            float maxValue = maxProperty.floatValue.RoundToDecimalPlace( _minMaxSliderElement.PropertiesSettings.minMaxDecimalPlace );

            EditorGUI.BeginChangeCheck();
            EditorGUI.MinMaxSlider( minMaxSliderRect, GUIContent.none, ref minValue, ref maxValue, minLimit, maxLimit );
            if (!EditorGUI.EndChangeCheck())
                return;
            
            minProperty.floatValue = minValue.RoundToDecimalPlace( _minMaxSliderElement.PropertiesSettings.minMaxDecimalPlace );
            maxProperty.floatValue = maxValue.RoundToDecimalPlace( _minMaxSliderElement.PropertiesSettings.minMaxDecimalPlace );
        }
    
        /// <summary>
        ///     Draws a float field in the provided rect. 
        /// </summary>
        private void DrawMinMaxFloatField( PropertyElement propertyElement, Rect floatRect, float clampLower, float clampUpper )
        {
            float value = propertyElement.Property.floatValue.RoundToDecimalPlace( _minMaxSliderElement.PropertiesSettings.minMaxDecimalPlace );
        
            EditorGUI.BeginChangeCheck();
            value = EditorGUI.FloatField( floatRect, value );

            if (!EditorGUI.EndChangeCheck())
                return;

            value = Mathf.Clamp( value.RoundToDecimalPlace( _minMaxSliderElement.PropertiesSettings.minMaxDecimalPlace ), clampLower, clampUpper );

            propertyElement.Property.floatValue = value;
        }
    }
}
