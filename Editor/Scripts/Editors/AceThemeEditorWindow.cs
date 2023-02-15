using System.Collections.Generic;
using System.Linq;
using ACEPackage.Editor.Scripts.ACECore;
using ACEPackage.Editor.Scripts.AceRoots;
using UnityEditor;
using UnityEngine;
using static ACEPackage.Editor.Scripts.EditorGraphics.EditorMeasurementLineGraphics;
using static ACEPackage.Editor.Scripts.ACECore.ThemeLoader;
using static ACEPackage.Runtime.Scripts.AceEditorConstants;

namespace ACEPackage.Editor.Scripts.Editors
{
    /// <summary>
    ///     Draws the ACE theme settings window. Note that the data drawn comes from the target
    ///     theme while how it is drawn comes from the editor window theme.
    /// </summary>
    public class AceThemeEditorWindow : AceEditorWindow
    {
        [MenuItem( ThemeSettingsWindowMenuItemName )]
        public static void OpenCustomEditorToolSettings() => GetWindow<AceThemeEditorWindow>();
        
        private List<AceTheme> _existingThemes;

        private string[] _themeOptions;
        private int _selectedThemeIndex = 2;
        
        protected override Vector2 GetEditorWindowMinSize() => new Vector2( 375, 300);

        protected override string GetEditorWindowThemeName() => EditorWindowThemeName;

        protected override string GetTitle() => ThemeSettingsWindowTitle;
        protected override string GetTooltip() => string.Empty;
        
        private Vector2 _scrollPosition;


        protected override void OnEnableFirst()
        {
            InitializeLists();
        }

        private void InitializeLists()
        {
            _existingThemes = GetAllThemes().ToList();
            _themeOptions = GetThemeOptions();
        }
        
        private string[] GetThemeOptions()
        {
            string[] themeOptions = new string[_existingThemes.Count];
            for (int i = 0; i < _existingThemes.Count; i++)
            {
                themeOptions[i] = $"({i.ToString()}) {_existingThemes[i].name}";
            }
            return themeOptions;
        }
        
        private void ThemeDropdownUpdated() => PerformTargetSwap();

        protected override AceScriptableObjectRoot GetTarget()
        {
            Debug.Log( $"ACETEW|GT: Getting target theme..." );
            if ( _selectedThemeIndex > ( _existingThemes.Count - 1 ) )
            {
                Debug.Log( $"ACETEW|GT:     Theme index {_selectedThemeIndex.ToString()} exceeds existing lists length of {_existingThemes.Count.ToString()}." );

                InitializeLists();
            }
                
            return _existingThemes[_selectedThemeIndex];
        }

        /// <summary>
        ///     Draw the elements to the editor window. Returns true if the settings get changed.
        /// </summary>
        protected override void DrawElements()
        {
            DrawDebugVisuals();

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField( "Selected Theme:" );
            _selectedThemeIndex = EditorGUILayout.Popup( _selectedThemeIndex, _themeOptions );
            EditorGUILayout.EndHorizontal();
            if ( EditorGUI.EndChangeCheck() )
            {
                ThemeDropdownUpdated();
            }
            EditorGUILayout.LabelField( $" {_existingThemes[_selectedThemeIndex].name}" );
            
            // Draw the first three elements: divider, enums field, divider.
            for (int i = 0; i < 3; i++)
            {
                Elements[i].DrawElement( true );
            }
            
            // Then start scroll field and draw the rest of the elements.
            _scrollPosition = GUILayout.BeginScrollView( _scrollPosition, GUILayout.Width( position.width ),
                GUILayout.Height( position.height - 80 ) );
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
    }
}
