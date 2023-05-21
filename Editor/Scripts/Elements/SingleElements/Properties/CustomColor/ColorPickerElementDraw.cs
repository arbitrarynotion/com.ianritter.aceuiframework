using Packages.com.ianritter.unityscriptingtools.Editor.PopupWindows.CustomColorPicker;
using UnityEditor;
using UnityEngine;

namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.SingleElements.Properties.CustomColor
{
    public class ColorPickerElementDraw : PropertyElementDraw
    {
        private readonly ColorPickerElement _colorPickerElement;
        protected override PropertyElement PropertyElement => _colorPickerElement;

        
        public ColorPickerElementDraw( ColorPickerElement colorPickerElement )
        {
            _colorPickerElement = colorPickerElement;
        }
        
        
        protected override void DrawPropertyFieldWithOutLabel( Rect fieldRect ) => DrawBasicColorProperty( fieldRect, _colorPickerElement.Property );

        protected override void DrawPropertyFieldWithLabel( Rect drawRect )
        {
            // Label width has already been handled. Just need to draw the label with the EditorGUIUtility.LabelWidth
            // and draw the min/max slider with float fields in the remaining width of the draw rect.
            
            // Build label rect.
            // var labelRect = new Rect( drawRect )
            // {
            //     width = EditorGUIUtility.labelWidth
            // };
            //
            // DrawLabelField( labelRect );
            //
            // // Build field rect.
            // var fieldRect = new Rect( drawRect );
            // fieldRect.xMin += EditorGUIUtility.labelWidth;
            //
            // DrawBasicColorProperty( fieldRect, _colorPickerElement.Property );
            DrawBasicColorProperty( drawRect, _colorPickerElement.Property );
        }
        
        // private void DrawCustomColorElement( Rect drawRect )
        // {
        //     // EditorGUI.BeginChangeCheck();
        //     // _colorPickerElement.Property.intValue = EditorGUI.Popup( drawRect, _colorPickerElement.Property.intValue, _colorPickerElement.Options );
        //     // if (EditorGUI.EndChangeCheck())
        //     //     _colorPickerElement.ChangeCallBack?.Invoke();
        // }
        
        private void DrawBasicColorProperty( Rect positionRect, SerializedProperty property, GUIContent guiContent = null ) => 
            DrawPropertyWithColorPicker( positionRect, property, property, guiContent );

        private void DrawCustomColorProperty( Rect positionRect, SerializedProperty property, GUIContent guiContent = null )
        {
            SerializedProperty colorProperty = property.FindPropertyRelative( "color" );
            if ( colorProperty == null )
            {
                Debug.LogError( "Failed to find 'customColor' property or its 'color' property." );
                return;
            }
            DrawPropertyWithColorPicker( positionRect, property, colorProperty, guiContent );
        }
        
        private void DrawPropertyWithColorPicker( Rect controlRect, SerializedProperty fieldProperty, SerializedProperty targetProperty, GUIContent guiContent )
        {
            ColorPickerHandler.SetWindowPosition( controlRect.position );
            
            // Rect controlRect = EditorGUILayout.GetControlRect();

            // Exclude color picker button width from available width.
            float availableWidth = controlRect.width - ColorPickerHandler.GetColorPickerButtonWidth();

            var lineRect = new Rect( controlRect )
            {
                width = availableWidth
            };
            // DrawRectOutline( lineRect, Color.grey );

            // Draw Property label and field.
            var propertyFieldRect = new Rect( lineRect );
            // DrawRectOutline( propertyField, Color.magenta );
            if ( guiContent != null )
            {
                EditorGUI.PropertyField( propertyFieldRect, fieldProperty, guiContent );
            }
            else
            {
                EditorGUI.PropertyField( propertyFieldRect, fieldProperty );
            }

            // Set the color picker button rect to start at the end of the available space plus a spacer.
            // Then get the width from the color picker handler.
            var buttonRect = new Rect( controlRect );
            buttonRect.xMin += availableWidth;
            // buttonRect.width = ColorPickerHandler.GetColorPickerButtonWidth();
            // DrawRectOutline( buttonRect, Color.yellow );
            
            // Finally, pass the button rect and the color property to the color picker handler.
            // This can either be a direct color property via serializedObject.FindProperty or an indirect one via property.FindPropertyRelative.
            ColorPickerHandler.DrawColorPickerPropertyButton( buttonRect, targetProperty );
        }


    }
}
