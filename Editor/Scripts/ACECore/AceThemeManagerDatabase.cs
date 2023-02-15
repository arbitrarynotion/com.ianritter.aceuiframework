using System;
using System.Collections.Generic;
using System.Linq;
using ACEPackage.Editor.Scripts.AceRoots;
using ACEPackage.Editor.Scripts.Elements;
using ACEPackage.Editor.Scripts.Elements.SingleElements.Button.Basic;
using ACEPackage.Editor.Scripts.Elements.SingleElements.Decorator.DividingLine;
using ACEPackage.Editor.Scripts.Elements.SingleElements.Decorator.Label;
using ACEPackage.Editor.Scripts.Elements.SingleElements.Properties.Popup;
using ACEPackage.Runtime.Scripts.SettingsCustom.Groups;
using ACEPackage.Runtime.Scripts.SettingsCustom.SingleElements;
using UnityEditor;
using UnityEngine;
using static ACEPackage.Editor.Scripts.ElementBuilding.AceElementBuilder;
using static ACEPackage.Runtime.Scripts.AceEditorConstants;

namespace ACEPackage.Editor.Scripts.ACECore
{
    [CreateAssetMenu(menuName = ThemeDatabaseAssetMenuName)]
    public class AceThemeManagerDatabase : AceScriptableObjectRoot
    {
        public List<ScriptThemeInfo> scriptThemeInfoList = new List<ScriptThemeInfo>();

        private ScriptThemeInfo SelectedScript => scriptThemeInfoList[selectedScriptIndex] ?? null;
        
        public int selectedScriptIndex = -1;
        public int selectedThemeIndex = -1;

        private List<MonoScript> _existingScripts;
        private List<AceTheme> _existingThemes;
        private AceTheme _defaultTheme;
        
        public delegate void ThemeChanged();
        public event ThemeChanged OnThemeChanged;

        private void ThemeChangedNotify() => OnThemeChanged?.Invoke();
        
        private void OnEnable()
        {
            _defaultTheme = ThemeLoader.LoadScriptableObject<AceTheme>( DefaultThemeName );

            RefreshResourceLists();
            UpdateSavedScriptsList();
        }
        
        private void UpdateSavedScriptsList()
        {
            PurgeDeletedScriptsFromDb();
            AddNewScriptsToDb();
        }

        private void RefreshResourceLists()
        {
            _existingScripts = ThemeLoader.GetAllAceUsers().ToList();
            _existingThemes = ThemeLoader.GetAllThemes().ToList();
            
            // Debug.Log( "ACETMD|PERL:     Results:" );
            // PrintLocatedScripts();
            // PrintLocatedThemes();
            
            if ( _defaultTheme == null )
                throw new NullReferenceException( $"ACETMD|PERL: Error!! Unable to load theme \"{DefaultThemeName}\"" );
        }

        private void PurgeDeletedScriptsFromDb()
        {
            Debug.Log( "ACETMD|I: Checking for deleted scripts..." );
            
            bool foundDeleted = false;
            
            // Are previously saved scripts missing?
            if ( scriptThemeInfoList.Count != 0 ) // previous scripts saved.
            {
                if ( _existingScripts.Count == 0 ) // all scripts removed, just reset.
                {
                    scriptThemeInfoList.Clear();
                    Debug.Log( $"ACETMD|I:     No scripts to search." );
                    return;
                }
                
                // Remove deleted scripts from database.
                // Note that using ".ToList()" allow safely modifying the list being iterated through.
                foreach ( ScriptThemeInfo scriptInfo in scriptThemeInfoList.ToList().Where( scriptInfo => !_existingScripts.Contains( scriptInfo.script ) ) )
                {
                    foundDeleted = true;
                        
                    // If this script was selected, reset selected script index.
                    if ( selectedScriptIndex == scriptThemeInfoList.IndexOf(scriptInfo) )
                    {
                        selectedScriptIndex = ( scriptThemeInfoList.Count > 0 ) ? 0 : -1;
                    }
                        
                    Debug.Log( $"ACETMD|I:     {scriptInfo.script.name} was deleted. Removing from database." );
                    scriptThemeInfoList.Remove( scriptInfo );
                }
            }
            
            if (!foundDeleted)
                Debug.Log( $"ACETMD|I:     No scripts were deleted." );
        }
        
        private void AddNewScriptsToDb()
        {
            Debug.Log( "ACETMD|ASTD: Checking for new scripts..." );
            
            // Are there new scripts that haven't yet been saved?
            bool foundNew = false;
            foreach ( MonoScript script in _existingScripts.Where( script => !IsSaved( script ) ) )
            {
                foundNew = true;
                Debug.Log( $"ACETMD|ASTD:     {script.name} is new. Adding to database." );
                
                // Create new script theme info entry for this script.
                scriptThemeInfoList.Add( new ScriptThemeInfo() { script = script, theme = _defaultTheme } );
            }
            
            if (!foundNew)
                Debug.Log( $"ACETMD|ASTD:     No new scripts were found." );
        }
        
        private void UpdateSelectedScriptTheme()
        {
            // Important: call only after triggering OnDataUpdateRequired event or the index values
            // will not have been updated.
            SelectedScript.theme = _existingThemes[selectedThemeIndex];
            ThemeChangedNotify();
        }

        public AceTheme GetThemeForScript( string scriptName )
        {
            MonoScript script = _existingScripts.FirstOrDefault( monoScript => scriptName == monoScript.name );
            
            // Find the script in the scriptInfoList then return that entry's theme.
            return scriptThemeInfoList.Where( scriptThemeInfo => scriptThemeInfo.script == script ).Select( scriptThemeInfo => scriptThemeInfo.theme ).FirstOrDefault();
        }

        private bool IsSaved( MonoScript script ) => scriptThemeInfoList.Any( scriptInfo => scriptInfo.script == script );
        
        public override Element[] GetElementList() => new [] { GetScriptAndThemeDropdown() };
        
        private Element GetScriptAndThemeDropdown()
        {
            // Refresh button.
            Element refreshButton = new BasicButtonElement( 
                new GUIContent( "Scan For Scripts", 
                    "Press this if you create or delete a script while this window is open." ), 
                true, 
                new SingleCustomSettings() { ForceSingleLine = true },
                RefreshButtonPressed
            );
            
            // Build list of script options
            Element scriptPopup = new PopupElement( nameof( selectedScriptIndex ), GUIContent.none, GetScriptOptions(), new SingleCustomSettings(), ScriptDropdownUpdated );
            Element themePopup = new PopupElement( nameof( selectedThemeIndex ), GUIContent.none, GetThemeOptions(), new SingleCustomSettings(), ThemeDropdownUpdated );
        
            
            return scriptThemeInfoList.Count > 0
                ? GetGroup
                ( 
                    null,
                    GetGroupWithLabelHeading
                    (
                        "Script Theme Assignment", string.Empty,
                        new GroupCustomSettings() { NumberOfColumns = 2 },
                        scriptPopup,
                        themePopup,
                        new DividingLineElement( 6f, 2f ),
                        refreshButton
                    )
                
                    // Todo: Menu for creating, duplicating, and deleting themes.
                )
                : GetGroupWithLabelHeading
                (
                    "Assign a Theme to Your Script", string.Empty,
                    new GroupCustomSettings { NumberOfColumns = 2 },
                    new LabelElement( new GUIContent( "Script" ) ),
                    new LabelElement( new GUIContent( "Theme" ) ),
                    new LabelElement( new GUIContent( "No ACE scripts found." ) ),
                    refreshButton
                );
        }
        
        private string[] GetScriptOptions()
        {
            string[] scriptOptions = new string[_existingScripts.Count];
            for (int i = 0; i < _existingScripts.Count; i++)
            {
                scriptOptions[i] = $"({i.ToString()}) {_existingScripts[i].name}";
            }

            return scriptOptions;
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

        private int GetSelectedScriptsThemeIndex() => _existingThemes.IndexOf( SelectedScript.theme );

        private void ScriptDropdownUpdated()
        {
            DataUpdateRequiredNotify();
            selectedThemeIndex = GetSelectedScriptsThemeIndex();
            UIStateChangedNotify();
        }
        
        private void ThemeDropdownUpdated()
        {
            DataUpdateRequiredNotify();
            UpdateSelectedScriptTheme();
            UIStateChangedNotify();
        }

        private void RefreshButtonPressed() => RefreshResourceLists();


#region PrintHelpers

        private void PrintLocatedScripts()
        {
            Debug.Log( $"ACETMD|PLS:     {_existingScripts.Count.ToString()} scripts found:" );
            foreach ( MonoScript script in _existingScripts )
            {
                Debug.Log( $"ACETMD|PLS:         {script.name}" );
            }
        }

        private void PrintLocatedThemes()
        {
            Debug.Log( $"ACETMD|PLS:     {_existingThemes.Count.ToString()} themes found:" );
            foreach ( AceTheme theme in _existingThemes )
            {
                Debug.Log( $"ACETMD|PLT:         {theme.name}" );
            }
        }
        
        private void PrintScriptOptions( string[] scriptOptions )
        {
            Debug.Log( $"ACETMD|PSO:     {scriptOptions.Length.ToString()} script options found:" );
            foreach ( string script in scriptOptions )
            {
                Debug.Log( $"ACETMD|PSO:         {script}" );
            }
        }
        
        private void PrintThemeOptions( string[] themeOptions )
        {
            Debug.Log( $"ACETMD|PSO:     {themeOptions.Length.ToString()} theme options found:" );
            foreach ( string theme in themeOptions )
            {
                Debug.Log( $"ACETMD|PSO:         {theme}" );
            }
        }

#endregion
    }
}
