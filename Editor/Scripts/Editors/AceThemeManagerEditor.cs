using Packages.com.ianritter.aceuiframework.Editor.Scripts.ACECore;
using UnityEditor;

namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.Editors
{
    /// <summary>
    ///     This editor applies to the CET theme manager database when it's drawn in the inspector.
    /// </summary>
    [CustomEditor(typeof(AceThemeManagerDatabase))]
    public class AceThemeManagerEditor : UnityEditor.Editor
    {
        private AceThemeManagerDatabase _target;

        private void OnEnable() => _target = (AceThemeManagerDatabase) target;

        public override void OnInspectorGUI()
        {
            if (DrawDefaultInspector()) 
                _target.DataUpdatedNotify();
        }
    }
}
