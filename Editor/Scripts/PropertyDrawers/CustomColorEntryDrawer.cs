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
            // Disable all field when locked is off.
            SerializedProperty toggleProperty = property.FindPropertyRelative( "locked" );
            
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
                // const float boolWidth = 16f;
                const float colorFieldWidth = 60f;
                // const float previousNameLabelWidth = 100f;
                const float userCountLabelWidth = 30f;
                float colorPickerWidth = ColorPickerHandler.GetColorPickerButtonWidth();
        
                EditorGUIUtility.labelWidth = 0.01f;
                
                // Icon rect
                var iconRect = new Rect( position );
                iconRect.width = lockIconWidth;
                iconRect.yMax -= 2f;
                // EditorGUI.LabelField( iconRect, new GUIContent( lockedTextureAsset ) );
                var style = new GUIStyle( EditorStyles.label ) {};
                if (GUI.skin.customStyles.Length > 0)
                {
                    GUI.skin.customStyles[0].onHover.textColor = Color.yellow;
                }
                if ( GUI.Button( iconRect, new GUIContent( lockedTextureAsset ), style ) )
                {
                    toggleProperty.boolValue = !toggleProperty.boolValue;
                }
                
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
        
                // // Bool position
                // var boolRect = new Rect( dataRect )
                // {
                //     width = boolWidth
                // };
                // // DrawRectOutline( boolRect, Orange.color );
                // dataRect.xMin += boolWidth + divider;
        
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
        
                // Use to show the previous name in the editor.
                // var previousNameRect = new Rect( dataRect );
                // previousNameRect.xMin += dataRect.width - previousNameLabelWidth;
                // // DrawRectOutline( previousNameRect, YellowGreen.color );
                // dataRect.xMax -= previousNameLabelWidth + divider;
                
                // Use to display the number of users in the editor.
                var userCountRect = new Rect( dataRect );
                userCountRect.xMin += dataRect.width - userCountLabelWidth;
                // DrawRectOutline( previousNameRect, YellowGreen.color );
                dataRect.xMax -= userCountLabelWidth + divider;
                
                // Text field position
                var textFieldRect = new Rect( dataRect );
                // DrawRectOutline( textFieldRect, Green1.color );
                
                // EditorGUI.PropertyField( boolRect, toggleProperty, GUIContent.none );

                SerializedProperty userCountProperty = property.FindPropertyRelative( "userCount" );
                string userCount = userCountProperty.intValue == 0 ? GetColoredStringGray( "0" ) : userCountProperty.intValue.ToString();
                EditorGUI.LabelField( userCountRect, new GUIContent( userCount, property.FindPropertyRelative( "userList" ).stringValue ), 
                    new GUIStyle( EditorStyles.label ) {alignment = TextAnchor.MiddleCenter, richText = true} );
                // EditorGUI.LabelField( userCountRect, new GUIContent( userCount, "Total users of this color.\nSecond line." ), 
                //     new GUIStyle( EditorStyles.label ) {alignment = TextAnchor.MiddleCenter, richText = true} );
                
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
