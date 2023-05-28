using System;
using System.Linq;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.ACECore;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.AceEditorRoots;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.AceRuntimeRoots;
using Packages.com.ianritter.unityscriptingtools.Editor;
using Packages.com.ianritter.unityscriptingtools.Runtime.Services.CustomLogger;
using UnityEditor;
using UnityEngine;
using static Packages.com.ianritter.aceuiframework.Editor.Scripts.Graphics.EditorMeasurementLineGraphics;
using static Packages.com.ianritter.aceuiframework.Runtime.Scripts.AceEditorConstants;
using static Packages.com.ianritter.unityscriptingtools.Editor.AssetLoader;
using static Packages.com.ianritter.unityscriptingtools.Runtime.Services.TextFormatting.TextFormat;

namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.EditorWindows
{
    /// <summary>
    ///     Draws the ACE theme settings window. Note, as this can be a source of confusion: the settings
    ///     being edited are from the target theme (loaded from the themeManager) and the settings being
    ///     used to draw the settings window are from the editor window theme.
    /// </summary>
    public class AceThemeEditorWindow : AceEditorWindow
    {
        [MenuItem( ThemeSettingsWindowMenuItemName )]
        public static void OpenCustomEditorToolSettings() => GetWindow<AceThemeEditorWindow>();

        // Icons
        public static Texture LockedIcon;
        public static Texture UnlockedIcon;
        
        public int selectedThemeIndex;
        
        private Vector2 _scrollPosition;
        private AceThemeManager _themeManager;
        
        protected override Vector2 GetEditorWindowMinSize() => new Vector2( 375, 300);

        protected override string GetEditorWindowThemeName() => EditorWindowThemeName;

        protected override string GetTitle() => ThemeSettingsWindowTitle;
        protected override string GetTooltip() => string.Empty;
        

        protected override void OnEnableFirst()
        {
            if ( _themeManager == null )
            {
                _themeManager = GetAssetsByType<AceThemeManager>().FirstOrDefault();
                if ( _themeManager == null )
                    throw new NullReferenceException( "Unable to load theme manager.");
            }
            
            if ( LockedIcon == null )
                LockedIcon = GetAssetByName<Texture>( "d_TrackLockButtonEnabled" );
            if ( UnlockedIcon == null )
                UnlockedIcon = GetAssetByName<Texture>( "d_TrackLockButtonDisabled" );
        }
        
        
        public override string GetTargetName() => GetTarget().name;
        /// <summary>
        ///     Returns the theme to be edited. If themeSettingsEditsOwnTheme is true, the theme settings window will load its own theme.
        /// </summary>
        protected override AceScriptableObjectEditorRoot GetTarget() => _themeManager.themeSettingsEditsOwnTheme ? AceTheme : _themeManager.GetThemeForIndex( selectedThemeIndex );
        
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
            
            // Show name of currently edited theme. Good for catching asset loading errors.
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
        
        private void ThemeDropdownUpdated() => PerformTargetSwap();

        private void DrawDebugVisuals()
        {
            const float guideLinesBrightness = 0.4f;
            const float graphLinesBrightness = 0.25f;
            if (AceTheme.GetGlobalSettings().showGridLines)
                DrawVerticalGridLines( guideLinesBrightness, graphLinesBrightness, true );
            
            if (AceTheme.GetGlobalSettings().showMeasurementLines)
                DrawMeasurementLines( 20, 100, new Color( graphLinesBrightness, graphLinesBrightness, graphLinesBrightness) );
        }
    }
}
