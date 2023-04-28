using System;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts;
using UnityEditor;
using UnityEngine;

namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.SingleElements.Properties.MinMaxSlider
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

            if ( _minMaxSliderElement.IsFloatType )
            {
                DrawMinMaxFloatField( _minMaxSliderElement.MinPropertyElement, leftFieldRect, 
                    _minMaxSliderElement.MinLimit, 
                    _minMaxSliderElement.MaxPropertyElement.Property.floatValue );
            }
            else
            {
                DrawMinMaxIntField( _minMaxSliderElement.MinPropertyElement, leftFieldRect, 
                    (int) _minMaxSliderElement.MinLimit, 
                    _minMaxSliderElement.MaxPropertyElement.Property.intValue );
            }
        }

        private void DrawRightFloatField( Rect fieldRect )
        {
            // Max float field goes in third and final part.
            var rightFieldRect = new Rect( fieldRect )
            {
                width = _minMaxSliderElement.PropertiesSettings.minMaxSliderFloatFieldWidth
            };
            rightFieldRect.x += fieldRect.width - _minMaxSliderElement.PropertiesSettings.minMaxSliderFloatFieldWidth;

            if ( _minMaxSliderElement.IsFloatType )
            {
                DrawMinMaxFloatField( _minMaxSliderElement.MaxPropertyElement, rightFieldRect, 
                    _minMaxSliderElement.MinPropertyElement.Property.floatValue, 
                    _minMaxSliderElement.MaxLimit );
            }
            else
            {
                DrawMinMaxIntField( _minMaxSliderElement.MaxPropertyElement, rightFieldRect, 
                    _minMaxSliderElement.MinPropertyElement.Property.intValue, 
                    (int) _minMaxSliderElement.MaxLimit );
            }
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
            
            int decimalPlace = _minMaxSliderElement.IsFloatType 
                ? _minMaxSliderElement.PropertiesSettings.minMaxDecimalPlace 
                : 0;
            
            // float minValue = minProperty.floatValue.RoundToDecimalPlace( decimalPlace );
            // float maxValue = maxProperty.floatValue.RoundToDecimalPlace( decimalPlace );
            
            float minValue = GetPropertyValueAsFloat( minProperty ).RoundToDecimalPlace( decimalPlace );
            float maxValue = GetPropertyValueAsFloat( maxProperty ).RoundToDecimalPlace( decimalPlace );

            EditorGUI.BeginChangeCheck();
            EditorGUI.MinMaxSlider( minMaxSliderRect, GUIContent.none, ref minValue, ref maxValue, minLimit, maxLimit );
            if (!EditorGUI.EndChangeCheck())
                return;
            
            // minProperty.floatValue = minValue.RoundToDecimalPlace( decimalPlace );
            // maxProperty.floatValue = maxValue.RoundToDecimalPlace( decimalPlace );
            
            SetFloatValue( minProperty, minValue.RoundToDecimalPlace( decimalPlace ) );
            SetFloatValue( maxProperty, maxValue.RoundToDecimalPlace( decimalPlace ) );
        }
    
        /// <summary>
        ///     Draws a float field in the provided rect. 
        /// </summary>
        private void DrawMinMaxFloatField( PropertyElement propertyElement, Rect floatRect, float clampLower, float clampUpper )
        {
            // float value = propertyElement.Property.floatValue.RoundToDecimalPlace( _minMaxSliderElement.PropertiesSettings.minMaxDecimalPlace );
            float value = GetPropertyValueAsFloat( propertyElement.Property ).RoundToDecimalPlace( _minMaxSliderElement.PropertiesSettings.minMaxDecimalPlace );
        
            EditorGUI.BeginChangeCheck();
            value = EditorGUI.FloatField( floatRect, value );

            if (!EditorGUI.EndChangeCheck())
                return;

            value = Mathf.Clamp( value.RoundToDecimalPlace( _minMaxSliderElement.PropertiesSettings.minMaxDecimalPlace ), clampLower, clampUpper );

            // propertyElement.Property.floatValue = value;
            SetFloatValue( propertyElement.Property, value );
        }
        
        /// <summary>
        ///     Draws a float field in the provided rect. 
        /// </summary>
        private void DrawMinMaxIntField( PropertyElement propertyElement, Rect floatRect, int clampLower, int clampUpper )
        {
            int value = propertyElement.Property.intValue;
        
            EditorGUI.BeginChangeCheck();
            value = EditorGUI.IntField( floatRect, value );

            if (!EditorGUI.EndChangeCheck())
                return;

            value = Mathf.Clamp( value, clampLower, clampUpper );

            propertyElement.Property.intValue = value;
        }

        private float GetPropertyValueAsFloat( SerializedProperty property )
        {
            return property.propertyType switch
            {
                SerializedPropertyType.Float => property.floatValue,
                SerializedPropertyType.Integer => property.intValue,
                _ => throw new NotSupportedException(
                    $"Min/Max slider does not support value type: {property.propertyType.ToString()}" )
            };
        }

        private void SetFloatValue( SerializedProperty property, float value )
        {
            switch (property.propertyType)
            {
                case SerializedPropertyType.Float:
                    property.floatValue = value;
                    break;
                case SerializedPropertyType.Integer:
                    property.intValue = (int) value;
                    break;
                default:
                    throw new NotSupportedException(
                        $"Min/Max slider does not support value type: {property.propertyType.ToString()}" );
            }
        }
    }
}
