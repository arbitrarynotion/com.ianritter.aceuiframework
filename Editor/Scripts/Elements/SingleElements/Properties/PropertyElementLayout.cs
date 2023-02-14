using System;
using UnityEditor;
using UnityEngine;

namespace ACEPackage.Editor.Scripts.Elements.SingleElements.Properties
{
    public abstract class PropertyElementLayout : SingleElementLayout
    {
        protected abstract PropertyElement PropertyElement { get; }
        protected override SingleElement SingleElement => PropertyElement;

        protected override float ColumnWidthPriorityAdjustment =>
            PropertyElement.GlobalSettings.propertyWidthPriorityAdjustment;


        protected PropertyElementLayout( Element element ) : base( element )
        {
        }


        // Arrays headings are shifted to the left by one indent by default. This undoes that shift.
        public override float GetLeftEdgeTypeBasedAdjustment() =>
            PropertyElement.Property != null
            && ( PropertyElement.Property.isArray &&
                 PropertyElement.Property.propertyType == SerializedPropertyType.Generic
                 || PropertyElement.Property.propertyType == SerializedPropertyType.Quaternion
                 || PropertyElement.Property.propertyType == SerializedPropertyType.Vector4 )
                ? 15
                : 0;

        protected override float GetFieldMinWidth()
        {
            switch (PropertyElement.Property.propertyType)
            {
                case SerializedPropertyType.Integer:
                case SerializedPropertyType.Float:
                    return PropertyElement.HasSlider
                        ? PropertyElement.PropertiesSettings.propertySlidersMinWidth
                        : PropertyElement.PropertiesSettings.propertyNumberFieldsMinWidth;

                case SerializedPropertyType.Boolean:
                    return PropertyElement.PropertiesSettings.propertyBoolsMinWidth;

                case SerializedPropertyType.String:
                case SerializedPropertyType.Character:
                    return PropertyElement.PropertiesSettings.propertyStringFieldsMinWidth;


                case SerializedPropertyType.Color:
                    return PropertyElement.PropertiesSettings.propertyColorsMinWidth;


                case SerializedPropertyType.AnimationCurve:
                case SerializedPropertyType.ObjectReference:
                case SerializedPropertyType.Gradient:
                    return PropertyElement.PropertiesSettings.propertyAnimationCurvesMinWidth;


                case SerializedPropertyType.LayerMask:
                case SerializedPropertyType.Enum:
                    return PropertyElement.PropertiesSettings.propertyDropDownsMinWidth;

                case SerializedPropertyType.Vector2:
                case SerializedPropertyType.Vector3:
                case SerializedPropertyType.Vector4:
                case SerializedPropertyType.Rect:
                case SerializedPropertyType.ArraySize:
                case SerializedPropertyType.Bounds:
                case SerializedPropertyType.Quaternion:
                case SerializedPropertyType.Vector2Int:
                case SerializedPropertyType.Vector3Int:
                case SerializedPropertyType.RectInt:
                case SerializedPropertyType.BoundsInt:
                    return PropertyElement.PropertiesSettings.propertySetsMinWidth;


                case SerializedPropertyType.ExposedReference:
                case SerializedPropertyType.FixedBufferSize:
                case SerializedPropertyType.ManagedReference:
                case SerializedPropertyType.Generic:
                    return PropertyElement.PropertiesSettings.propertyGenericMinWidth;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override float GetElementHeight( bool updating = false )
        {
            float height = GetSinglePropertyHeight( PropertyElement.Property, PropertyElement.GUIContent );
            if ( !PropertyElement.HasLabel() )
                return height;

            if ( !ShouldApplyVariableHeight || HasRoom
                                            || !EditorGUIUtility.wideMode &&
                                            PropertyElement.PropertyTypeUsesDefaultWideMode() )
                return height;

            height += EditorGUIUtility.singleLineHeight + 2f;

            return height;
        }

        public bool ShouldApplyVariableHeight => PropertyElement.HasLabel()
                                                 && PropertyElement.HasParent()
                                                 && PropertyElement.PropertyTypeShouldUseCustomWideMode();

        /// <summary>
        ///     Copied from internal source code.
        /// </summary>
        private static float GetSinglePropertyHeight( SerializedProperty property, GUIContent label ) =>
            property == null ? 18f : EditorGUI.GetPropertyHeight( property, label, property.isExpanded );
    }
}