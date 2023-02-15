using Packages.com.ianritter.aceuiframework.Editor.Scripts.AceRoots;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements;
using UnityEditor;
using UnityEngine;
using static Packages.com.ianritter.aceuiframework.Editor.Scripts.ACECore.ThemeLoader;
using static Packages.com.ianritter.aceuiframework.Runtime.Scripts.AceEditorConstants;


namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.Editors
{
    public class AceThemeManagerEditorWindow : AceEditorWindow
    {
        [MenuItem( ThemeManagerSettingsWindowMenuItemName )]
        public static void OpenThemeManagerSettings() => GetWindow<AceThemeManagerEditorWindow>();

        protected override Vector2 GetEditorWindowMinSize() => new Vector2( 375, 80);

        protected override string GetEditorWindowThemeName() => EditorWindowThemeName;

        protected override string GetTitle() => ThemeManagerSettingsWindowTitle;
        protected override string GetTooltip() => string.Empty;
        
        protected override AceScriptableObjectRoot GetTarget() => 
            LoadScriptableObject<AceScriptableObjectRoot>( ThemeManagerDatabaseCoreName );

        /// <summary>
        ///     Draw the elements to the editor window. Returns true if the settings get changed.
        /// </summary>
        protected override void DrawElements()
        {
            foreach ( Element element in Elements )
            {
                element.DrawElement( true );
            }
        }
    }
}
