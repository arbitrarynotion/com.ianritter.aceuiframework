using Packages.com.ianritter.aceuiframework.Editor.Scripts.EditorWindows;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsGlobal.Colors;
using Packages.com.ianritter.unityscriptingtools.Editor.PopupWindows.CustomColorPicker;
using UnityEditor;
using UnityEngine;
using static Packages.com.ianritter.unityscriptingtools.Runtime.Services.UIGraphics.UIRectGraphics;
using static Packages.com.ianritter.unityscriptingtools.Runtime.Services.CustomColors.PresetColors;
using static Packages.com.ianritter.unityscriptingtools.Runtime.Services.TextFormatting.TextFormat;

namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(CustomColorEntry))]
    public class CustomColorEntryDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight( SerializedProperty property, GUIContent label ) => 
            EditorGUIUtility.singleLineHeight + 2f;
        
        
        public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
        {
            // Disable all field when toggle is off.
            SerializedProperty toggleProperty = property.FindPropertyRelative( "toggle" );
            
            Texture lockedTextureAsset = toggleProperty.boolValue 
                ? AceThemeEditorWindow.LockedIcon
                : AceThemeEditorWindow.UnlockedIcon;
        
            label = GUIContent.none;
        
            EditorGUI.BeginProperty( position, label, property );
            {
                const float lockIconWidth = 18f;
                const float divider = 2f;
                const float verticalPadding = 2f;
                float labelWidth = EditorGUIUtility.labelWidth;
                const float boolWidth = 16f;
                const float colorFieldWidth = 60f;
                // const float previousNameLabelWidth = 100f;
                float colorPickerWidth = ColorPickerHandler.GetColorPickerButtonWidth();
        
                EditorGUIUtility.labelWidth = 0.01f;
                
                // Icon rect
                var iconRect = new Rect( position );
                iconRect.width = lockIconWidth;
                EditorGUI.LabelField( iconRect, new GUIContent( lockedTextureAsset ) );
                
                position.xMin += lockIconWidth;
                
                // Data field
                var dataRect = new Rect( position );
                dataRect.yMax -= verticalPadding;
        
                // if ( label != GUIContent.none )
                // {
                //     var labelRect = new Rect( position )
                //     {
                //         height = EditorGUIUtility.singleLineHeight + verticalPadding,
                //         width = labelWidth
                //     };
                //     // DrawRectOutline( labelRect, Orange.color );
                //     EditorGUI.LabelField( labelRect, label );
                //     
                //     dataRect.xMin += labelWidth + divider;
                // }
        
                // Bool position
                var boolRect = new Rect( dataRect )
                {
                    width = boolWidth
                };
                // DrawRectOutline( boolRect, Orange.color );
                dataRect.xMin += boolWidth + divider;
        
                // Color Picker position
                var colorPickerRect = new Rect( dataRect );
                colorPickerRect.xMin += dataRect.width - colorPickerWidth;
                // DrawRectOutline( colorPickerRect, Yellow.color );
                dataRect.xMax -= colorPickerWidth + divider;
                
                // Color field position
                var colorFieldRect = new Rect( dataRect );
                colorFieldRect.xMin += dataRect.width - colorFieldWidth;
                // DrawRectOutline( colorFieldRect, YellowGreen.color );
                dataRect.xMax -= colorFieldWidth;
        
                // var previousNameRect = new Rect( dataRect );
                // previousNameRect.xMin += dataRect.width - previousNameLabelWidth;
                // // DrawRectOutline( previousNameRect, YellowGreen.color );
                // dataRect.xMax -= previousNameLabelWidth + divider;
                
                // Text field position
                var textFieldRect = new Rect( dataRect );
                // DrawRectOutline( textFieldRect, Green1.color );
                
                EditorGUI.PropertyField( boolRect, toggleProperty, GUIContent.none );

                // When the entry is locked, fields are visible but can't be modified.
                using ( new EditorGUI.DisabledScope( toggleProperty.boolValue ) )
                {
                    // EditorGUI.PropertyField( symbolRect, property.FindPropertyRelative( "customColor" ), GUIContent.none );
                    SerializedProperty colorProperty = property.FindPropertyRelative( "color" );
                    SerializedProperty nameProperty = property.FindPropertyRelative( "name" );
                    SerializedProperty previousNameProperty = property.FindPropertyRelative( "previousName" );
                    string currentName = nameProperty.stringValue;
                    EditorGUI.BeginChangeCheck();
                    EditorGUI.PropertyField( textFieldRect, nameProperty );
                    if ( EditorGUI.EndChangeCheck() )
                    {
                        previousNameProperty.stringValue = currentName;
                        // Debug.Log( $"CustomColorEntryPropertyDrawer: Color's name was changed from {GetColoredStringYellow(currentName)} to {GetColoredStringGreenYellow(nameProperty.stringValue)}" );
                        property.FindPropertyRelative( "wasUpdated" ).boolValue = true;
                    }
                    // EditorGUI.LabelField( previousNameRect, previousNameProperty.stringValue );
                    
                    EditorGUI.PropertyField( colorFieldRect, colorProperty );
                    ColorPickerHandler.SetWindowPosition( new Vector2( position.x, position.y ) );
                    ColorPickerHandler.DrawColorPickerPropertyButton( colorPickerRect, colorProperty );
        
                    EditorGUIUtility.labelWidth = labelWidth;
                }
            }
            EditorGUI.EndProperty();
        }

    }
}
