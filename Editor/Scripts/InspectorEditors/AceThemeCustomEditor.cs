using Packages.com.ianritter.aceuiframework.Editor.Scripts.ACECore;
using UnityEditor;
using static Packages.com.ianritter.aceuiframework.Runtime.Scripts.AceEditorConstants;

namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.InspectorEditors
{
    /// <summary>
    ///     This editor applies to the ACE theme when it's drawn in the inspector.
    /// </summary>
    [CustomEditor(typeof(AceTheme))]
    public class AceThemeCustomEditor : UnityEditor.Editor
    {
        private AceTheme _theme;
        public bool drawDefaultInspector = true;

        private void OnEnable() => _theme = (AceTheme) target;

        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox( ThemeCustomEditorHelpInfoText, 
                MessageType.Info );

            drawDefaultInspector = EditorGUILayout.Toggle( "Show Settings Anyways...", drawDefaultInspector );

            if ( !drawDefaultInspector )
                return;

            if ( DrawDefaultInspector() )
                _theme.DataUpdatedNotify();
        }
    }
}
