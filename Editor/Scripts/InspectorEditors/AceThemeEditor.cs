using Packages.com.ianritter.aceuiframework.Editor.Scripts.ACECore;
using UnityEditor;

namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.InspectorEditors
{
    /// <summary>
    ///     This editor applies to the ACE theme when it's drawn in the inspector.
    /// </summary>
    [CustomEditor(typeof(AceTheme))]
    public class AceThemeEditor : UnityEditor.Editor
    {
        private AceTheme _theme;
        public bool drawDefaultInspector = true;

        private void OnEnable() => _theme = (AceTheme) target;

        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox( "For the sake of your sanity, use the settings window at \"Tools / Custom Editor Tool (CET) Settings\".", 
                MessageType.Info );

            drawDefaultInspector = EditorGUILayout.Toggle( "Show Settings Anyways...", drawDefaultInspector );

            if ( !drawDefaultInspector )
                return;

            if ( DrawDefaultInspector() )
                _theme.DataUpdatedNotify();
        }
    }
}
