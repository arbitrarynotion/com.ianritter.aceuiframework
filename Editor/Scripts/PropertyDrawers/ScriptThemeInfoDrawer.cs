using Packages.com.ianritter.aceuiframework.Editor.Scripts.ACECore;
using UnityEditor;
using UnityEngine;

namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(ScriptThemeInfo))]
    public class ScriptThemeInfoDrawer : PropertyDrawer
    {
        public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
        {
            EditorGUI.BeginProperty( position, label, property );
            {
                // Split the remaining space between the string and the color fields.
                var stringRect = new Rect( position);
                stringRect.height = EditorGUIUtility.singleLineHeight + 2f;
                float singleFieldWidth = ( position.width / 2f ) - 4f;
                stringRect.width = singleFieldWidth;
                var colorFieldRect = new Rect( position );
                colorFieldRect.height = EditorGUIUtility.singleLineHeight + 2f;
                colorFieldRect.xMin += singleFieldWidth + 4f;

                EditorGUI.LabelField( stringRect, property.FindPropertyRelative( "script" ).objectReferenceValue.name);
                float labelWidth = EditorGUIUtility.labelWidth;
                EditorGUIUtility.labelWidth = 0.01f;
                SerializedProperty theme = property.FindPropertyRelative( "theme" );
                EditorGUI.PropertyField( colorFieldRect, theme , new GUIContent( theme.objectReferenceValue.name ) );
                EditorGUIUtility.labelWidth = labelWidth;
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight( SerializedProperty property, GUIContent label ) => EditorGUIUtility.singleLineHeight + 2f;
    }
}