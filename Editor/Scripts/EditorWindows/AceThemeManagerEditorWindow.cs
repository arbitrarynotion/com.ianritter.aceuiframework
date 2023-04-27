using Packages.com.ianritter.aceuiframework.Editor.Scripts.AceEditorRoots;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements;
using UnityEditor;
using UnityEngine;
using static Packages.com.ianritter.aceuiframework.Runtime.Scripts.AceEditorConstants;
using static Packages.com.ianritter.unityscriptingtools.Editor.AssetLoader;

namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.EditorWindows
{
    /// <summary>
    ///     The is the editor window for the Ace Theme Manager.
    /// </summary>
    public class AceThemeManagerEditorWindow : AceEditorWindow
    {
        [MenuItem( ThemeManagerSettingsWindowMenuItemName )]
        public static void OpenThemeManagerSettings() => GetWindow<AceThemeManagerEditorWindow>();
        
        public override string GetTargetName() => GetTarget().name;

        protected override Vector2 GetEditorWindowMinSize() => new Vector2( 375, 80);

        protected override string GetEditorWindowThemeName() => EditorWindowThemeName;

        protected override string GetTitle() => ThemeManagerWindowTitle;
        protected override string GetTooltip() => string.Empty;
        
        protected override AceScriptableObjectEditorRoot GetTarget() => 
            LoadScriptableObject<AceScriptableObjectEditorRoot>( ThemeManagerCoreName );

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
