using Packages.com.ianritter.aceuiframework.Editor.Scripts.EditorWindows;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.SettingsSections.Colors;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsGlobal.Colors;
using Packages.com.ianritter.unityscriptingtools.Editor;
using Packages.com.ianritter.unityscriptingtools.Editor.PopupWindows.CustomColorPicker;
using UnityEditor;
using UnityEngine;

namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(CustomColorEntry))]
    public class CustomColorEntryDrawer : PropertyDrawer
    {
        public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
        {
            // Load lock icon.
            // Unlocked: d_TrackLockButtonDisabled.png
            // Locked: d_TrackLockButtonEnabled.png
            // var lockedTextureAsset = AssetLoader.GetAssetByName<Texture>( "d_TrackLockButtonDisabled" );

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

                EditorGUIUtility.labelWidth = 0.01f;
                
                // Icon rect
                var iconRect = new Rect( position );
                iconRect.width = lockIconWidth;
                EditorGUI.LabelField( iconRect, new GUIContent( lockedTextureAsset ) );
                
                position.xMin += lockIconWidth;
                
                // Data field
                var dataRect = new Rect( position );

                if ( label != GUIContent.none )
                {
                    var labelRect = new Rect( position )
                    {
                        height = EditorGUIUtility.singleLineHeight + verticalPadding,
                        width = labelWidth
                    };
                    // DrawRectOutline( labelRect, Orange.color );
                    EditorGUI.LabelField( labelRect, label );
                    
                    dataRect.xMin += labelWidth + divider;
                }

                // Bool
                var boolRect = new Rect( dataRect )
                {
                    width = boolWidth
                };
                // DrawRectOutline( boolRect, Blue.color );
                EditorGUI.PropertyField( boolRect, toggleProperty, GUIContent.none );

                dataRect.xMin += boolRect.width + divider;

                // Symbol
                var symbolRect = new Rect( dataRect )
                {
                    width = ( dataRect.width - ColorPickerHandler.GetColorPickerButtonWidth() - divider )
                };
                // DrawRectOutline( symbolRect, Yellow.color );

                dataRect.xMin += symbolRect.width + divider;

                // Add color picker at the end.
                var colorPickerRect = new Rect( dataRect );
                
                
                // // Symbol
                // var customColorRect = new Rect( dataRect )
                // {
                //     width = ( dataRect.width - ColorPickerHandler.GetColorPickerButtonWidth() - divider )
                //     // width = ( dataRect.width - ColorPickerHandler.GetColorPickerButtonWidth() - divider - idWidth - divider )
                // };
                // DrawRectOutline( customColorRect, Yellow.color );
                //
                //
                // dataRect.xMin += customColorRect.width + divider;
                //
                // var idRect = new Rect( dataRect )
                // {
                //     width = idWidth
                // };
                // SerializedProperty idProperty = property.FindPropertyRelative( "colorId" );
                // EditorGUI.LabelField( idRect, $"ID:{idProperty.intValue.ToString("0000")}" );
                //
                // dataRect.xMin += idWidth + divider;
                //
                // // Add color picker at the end.
                // var colorPickerRect = new Rect( dataRect );
                
                using ( new EditorGUI.DisabledScope( toggleProperty.boolValue ) )
                {
                    EditorGUI.PropertyField( symbolRect, property.FindPropertyRelative( "customColor" ), GUIContent.none );
                    ColorPickerHandler.SetWindowPosition( new Vector2( position.x, position.y ) );
                    ColorPickerHandler.DrawColorPickerPropertyButton( colorPickerRect, property.FindPropertyRelative( "customColor" ).FindPropertyRelative( "color" ) );

                    EditorGUIUtility.labelWidth = labelWidth;
                }
            }
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight( SerializedProperty property, GUIContent label ) => 
            EditorGUIUtility.singleLineHeight + 2f;

    }
}