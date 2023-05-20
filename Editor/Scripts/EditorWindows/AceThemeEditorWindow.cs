using System;
using System.Linq;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.ACECore;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.AceEditorRoots;
using Packages.com.ianritter.unityscriptingtools.Editor;
using UnityEditor;
using UnityEngine;
using static Packages.com.ianritter.aceuiframework.Editor.Scripts.EditorGraphics.EditorMeasurementLineGraphics;
using static Packages.com.ianritter.aceuiframework.Runtime.Scripts.AceEditorConstants;
using static Packages.com.ianritter.unityscriptingtools.Editor.AssetLoader;

namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.EditorWindows
{
    /// <summary>
    ///     Draws the ACE theme settings window. Note that the data drawn comes from the target
    ///     theme while how it is drawn comes from the editor window theme.
    /// </summary>
    public class AceThemeEditorWindow : AceEditorWindow
    {
        // Icons
        public static Texture LockedIcon;
        public static Texture UnlockedIcon;
        [SerializeField] private static int colorIdBase = 0000;
        
        [MenuItem( ThemeSettingsWindowMenuItemName )]
        public static void OpenCustomEditorToolSettings() => GetWindow<AceThemeEditorWindow>();
        
        public int selectedThemeIndex;
        
        public override string GetTargetName() => GetTarget().name;

        protected override Vector2 GetEditorWindowMinSize() => new Vector2( 375, 300);

        protected override string GetEditorWindowThemeName() => EditorWindowThemeName;

        protected override string GetTitle() => ThemeSettingsWindowTitle;
        protected override string GetTooltip() => string.Empty;
        
        private Vector2 _scrollPosition;

        private AceThemeManager _themeManager;

        protected override void OnEnableFirst()
        {
            _themeManager = GetAssetsByType<AceThemeManager>().FirstOrDefault();
            if ( _themeManager == null )
                throw new NullReferenceException( "Unable to load theme manager.");
            
            LockedIcon = AssetLoader.GetAssetByName<Texture>( "d_TrackLockButtonEnabled" );
            UnlockedIcon = AssetLoader.GetAssetByName<Texture>( "d_TrackLockButtonDisabled" );
        }

        // private string[] GetThemeOptions()
        // {
        //     List<AceTheme> themes = _themeManager.GetThemeList();
        //     string[] themeOptions = new string[themes.Count];
        //     for (int i = 0; i < themes.Count; i++)
        //     {
        //         themeOptions[i] = $"({i.ToString()}) {themes[i].name}";
        //     }
        //     return themeOptions;
        // }
        
        private void ThemeDropdownUpdated() => PerformTargetSwap();

        protected override AceScriptableObjectEditorRoot GetTarget()
        {
            // Use this option to have the theme settings window modify the same theme that it's using.
            // AceTheme = _themeManager.GetThemeForIndex( selectedThemeIndex );
            return AceTheme;
            
            // This is the default behavior, keeping the system theme and target theme separate.
            // return _themeManager.GetThemeForIndex( selectedThemeIndex );

            // List<AceTheme> themes = _themeManager.GetThemeList();
            // Debug.Log( $"{name}: Getting target theme..." );
            // if ( selectedThemeIndex > ( themes.Count - 1 ) )
            //     Debug.LogWarning( $"{name}: theme index {selectedThemeIndex.ToString()} exceeds existing list's length of {themes.Count.ToString()}." );
            //
            // return themes[selectedThemeIndex];
        }

        /// <summary>
        ///     Draw the elements to the editor window. Returns true if the settings get changed.
        /// </summary>
        protected override void DrawElements()
        {
            DrawDebugVisuals();

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField( "Selected theme:" );
            selectedThemeIndex = EditorGUILayout.Popup( selectedThemeIndex, _themeManager.GetThemeOptions() );
            EditorGUILayout.EndHorizontal();
            if ( EditorGUI.EndChangeCheck() )
            {
                ThemeDropdownUpdated();
            }
            // EditorGUILayout.LabelField( $" {_themeManager.GetThemeForIndex(selectedThemeIndex).name}" );
            
            // Draw the first three elements: divider, enums field, divider.
            for (int i = 0; i < 3; i++)
            {
                Elements[i].DrawElement( true );
            }
            
            // Then start scroll field and draw the rest of the elements.
            _scrollPosition = GUILayout.BeginScrollView( _scrollPosition, GUILayout.Width( position.width ), GUILayout.Height( position.height - 80 ) );
            {
                for (int i = 3; i < Elements.Length; i++)
                {
                    Elements[i].DrawElement( true );
                }
            }
            GUILayout.EndScrollView();
        }
        
        private void DrawDebugVisuals()
        {
            const float guideLinesBrightness = 0.4f;
            const float graphLinesBrightness = 0.25f;
            if (AceTheme.GetGlobalSettings().showGridLines)
                DrawVerticalGridLines( guideLinesBrightness, graphLinesBrightness, true );
            
            if (AceTheme.GetGlobalSettings().showMeasurementLines)
                DrawMeasurementLines( 20, 100, new Color( graphLinesBrightness, graphLinesBrightness, graphLinesBrightness) );
        }

        public static int GetNextColorId()
        {
            return colorIdBase++;
        }
    }
}
