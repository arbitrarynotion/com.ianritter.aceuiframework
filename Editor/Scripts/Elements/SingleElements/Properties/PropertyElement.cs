using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.ElementConditions;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsCustom.SingleElements;
using UnityEditor;
using UnityEngine;

namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.SingleElements.Properties
{
    public abstract class PropertyElement : SingleElement
    {
        /// <summary>
        ///     This string should match the variable name of the target data exactly as it is used to look up that
        ///     variable during initialization.
        /// </summary>
        protected string VarName { get; }

        /// <summary>
        ///     The propertyBeingChecked this element is built for.
        /// </summary>
        public SerializedProperty Property { get; private set; }
        
        /// <summary>
        ///     Action to take when propertyBeingChecked is changed in the UI.
        /// </summary>
        public Action ChangeCallBack { get; }
        
        public bool IsBool { get; private set; } = false;
        public bool HasSlider { get; private set; } = false;
        
        public abstract PropertyElementLayout PropertyElementLayout { get; }
        protected abstract PropertyElementDraw PropertyElementDraw { get; }

        public override SingleElementLayout SingleElementLayout => PropertyElementLayout;
        public override SingleElementDraw SingleElementDraw => PropertyElementDraw;

        
        protected PropertyElement( 
            string varName, 
            GUIContent guiContent, 
            SingleCustomSettings singleCustomSettings,
            Action changeCallBack,
            bool hideOnDisable, 
            ElementCondition[] conditions ) 
            : base( guiContent, singleCustomSettings, hideOnDisable, conditions )
        {
            VarName = varName;
            ChangeCallBack = changeCallBack;
        }


        protected override void InitializeElement( SerializedObject targetScriptableObject )
        {
            Property = targetScriptableObject.FindProperty( VarName );

            if (Property == null)
            {
                // Not stopping here so the rest of the UI can draw. This property will be highlighted to show it's missing its data.
                Debug.LogWarning( $"PE|IE: {GetName()}: Error! Failed to find property for \"{VarName}\"!" );
                return;
            }

            if (Property.propertyType == SerializedPropertyType.Boolean)
                IsBool = true;

            CheckIfPropertyHasSlider();
        }

        private void CheckIfPropertyHasSlider()
        {
            FieldInfo field = Property.serializedObject.targetObject.GetType().GetField( Property.propertyPath );
            if ( field == null ) 
                return;
            
            object[] customAttributes = field.GetCustomAttributes( typeof( PropertyAttribute ), true );
            if ( customAttributes.Length <= 0 ) 
                return;
            
            List<PropertyAttribute> attributes = new List<PropertyAttribute>( ( IEnumerable<PropertyAttribute> ) customAttributes );

            HasSlider = attributes.OfType<RangeAttribute>().Any();
        }

        public override bool ElementIsValid() => PropertyIsValid( Property );

        public static bool PropertyIsValid( SerializedProperty propertyBeingChecked ) => propertyBeingChecked != null;
        
        /// <summary>
        ///     For filtering out data types that don't can't be neatly split to an additional line.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public virtual bool PropertyTypeShouldUseCustomWideMode()
        {
            return Property.propertyType switch
            {
                SerializedPropertyType.Integer => true,
                SerializedPropertyType.Float => true,
                SerializedPropertyType.String => true,
                SerializedPropertyType.Character => true,
                SerializedPropertyType.Vector2 => true,
                SerializedPropertyType.Vector2Int => true,
                SerializedPropertyType.Vector3 => true,
                SerializedPropertyType.Vector3Int => true,
                SerializedPropertyType.Rect => true,
                SerializedPropertyType.RectInt => true,
                SerializedPropertyType.Enum => true,
                SerializedPropertyType.LayerMask => true,
                SerializedPropertyType.Color => true,
                SerializedPropertyType.Gradient => true,
                SerializedPropertyType.AnimationCurve => true,
                SerializedPropertyType.ObjectReference => true,
                
                SerializedPropertyType.Boolean => false,
                SerializedPropertyType.Vector4 => false,
                SerializedPropertyType.Quaternion => false,
                SerializedPropertyType.Bounds => false,
                SerializedPropertyType.BoundsInt => false,
                SerializedPropertyType.Generic => false,
                SerializedPropertyType.ArraySize => false,
                SerializedPropertyType.ExposedReference => false,
                SerializedPropertyType.FixedBufferSize => false,
                SerializedPropertyType.ManagedReference => false,
                
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        /// <summary>
        ///     For filtering data types that have a wide mode implementation by default. This is used to cancel custom
        ///     wide mode vertical spacing when the default wide mode turns off so the vertical spacing isn't double added.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public virtual bool PropertyTypeUsesDefaultWideMode()
        {
            switch (Property.propertyType)
            {
                case SerializedPropertyType.Generic:
                    break;
                case SerializedPropertyType.Integer:
                    break;
                case SerializedPropertyType.Boolean:
                    break;
                case SerializedPropertyType.Float:
                    break;
                case SerializedPropertyType.String:
                    break;
                case SerializedPropertyType.Color:
                    break;
                case SerializedPropertyType.ObjectReference:
                    break;
                case SerializedPropertyType.LayerMask:
                    break;
                case SerializedPropertyType.Enum:
                    break;
                case SerializedPropertyType.Vector2:
                    return true;
                case SerializedPropertyType.Vector3:
                    return true;
                case SerializedPropertyType.Vector4:
                    return true;
                case SerializedPropertyType.Rect:
                    return true;
                case SerializedPropertyType.ArraySize:
                    break;
                case SerializedPropertyType.Character:
                    break;
                case SerializedPropertyType.AnimationCurve:
                    break;
                case SerializedPropertyType.Bounds:
                    return true;
                case SerializedPropertyType.Gradient:
                    break;
                case SerializedPropertyType.Quaternion:
                    return true;
                case SerializedPropertyType.ExposedReference:
                    break;
                case SerializedPropertyType.FixedBufferSize:
                    break;
                case SerializedPropertyType.Vector2Int:
                    return true;
                case SerializedPropertyType.Vector3Int:
                    return true;
                case SerializedPropertyType.RectInt:
                    return true;
                case SerializedPropertyType.BoundsInt:
                    return true;
                case SerializedPropertyType.ManagedReference:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return false;
        }
    }
}