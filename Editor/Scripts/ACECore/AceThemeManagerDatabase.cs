using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.AceRoots;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.SingleElements.Button.Basic;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.SingleElements.Decorator.DividingLine;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.SingleElements.Decorator.Label;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.SingleElements.Properties.Popup;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsCustom.Groups;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsCustom.SingleElements;
using static Packages.com.ianritter.aceuiframework.Editor.Scripts.ElementBuilding.AceElementBuilder;
using static Packages.com.ianritter.aceuiframework.Editor.Scripts.ACECore.AceDelegates;
using static Packages.com.ianritter.aceuiframework.Runtime.Scripts.AceEditorConstants;

namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.ACECore
{
    [CreateAssetMenu(menuName = ThemeDatabaseAssetMenuName)]
    public class AceThemeManagerDatabase : AceScriptableObjectRoot
    {
        public int selectedScriptIndex = -1;
        public int selectedThemeIndex = -1;
        public List<ScriptThemeInfo> scriptThemeInfoList = new List<ScriptThemeInfo>();
        private ScriptThemeInfo SelectedScript => scriptThemeInfoList[selectedScriptIndex] ?? null;
        private List<MonoScript> _existingScripts;
        private List<AceTheme> _existingThemes;
        private AceTheme _defaultTheme;
        
        public delegate void ThemeAssignmentChanged();
        public event ThemeAssignmentChanged OnThemeAssignmentChanged;
        private void ThemeAssignmentChangedNotify()
        {
            OnThemeAssignmentChanged?.Invoke();
            PrintMySubscribers( GetType().Name, OnThemeAssignmentChanged, MethodBase.GetCurrentMethod().Name );
        }
        
        public AceTheme GetThemeForScript( string scriptName )
        {
            MonoScript script = _existingScripts.FirstOrDefault( monoScript => scriptName == monoScript.name );
            
            // Find the script in the scriptInfoList then return that entry's theme.
            return scriptThemeInfoList.Where( scriptThemeInfo => scriptThemeInfo.script == script ).Select( scriptThemeInfo => scriptThemeInfo.theme ).FirstOrDefault();
        }

        public override Element[] GetElementList() => new [] { GetScriptAndThemeDropdown() };

        
        private void OnEnable()
        {
            _defaultTheme = ThemeLoader.LoadScriptableObject<AceTheme>( DefaultThemeName );

            RefreshResourceLists();
            UpdateSavedScriptsList();

            // Rebuild the scripts list any time files are changed in the project window.
            EditorApplication.projectChanged += OnProjectChanged;
            OnScriptsReloaded += OnScriptsReloadedUpdate;
        }

        private void OnDisable()
        {
            EditorApplication.projectChanged -= OnProjectChanged;
            OnScriptsReloaded -= OnScriptsReloadedUpdate;
        }

        private void OnProjectChanged()
        {
            Debug.Log( "ACETMD|OPC: Project Changed event called." );
            UpdateSavedScriptsList();
        }
        
        private void OnScriptsReloadedUpdate()
        {
            Debug.Log( "ACETMD|OPC: Scripts Reloaded event called." );
            UpdateSavedScriptsList();
        }
        
        public delegate void ScriptsReloaded();

        public static event ScriptsReloaded OnScriptsReloaded;

        [UnityEditor.Callbacks.DidReloadScripts]
        private static void OnScriptsReloadedNotify()
        {
            Debug.Log( "ATMD|OSR: script recompile detected." );
            OnScriptsReloaded?.Invoke();
        }

        private void UpdateSavedScriptsList()
        {
            Debug.Log( "ACETMD|USSL: Rebuilding ACE user scripts list..." );
            PurgeDeletedScriptsFromDb();
            AddNewScriptsToDb();
            Debug.Log( "ACETMD|USSL:     Rebuild complete." );
            UIStateUpdatedNotify();
        }

        private void RefreshResourceLists()
        {
            _existingScripts = ThemeLoader.GetAllAceUsers().ToList();
            _existingThemes = ThemeLoader.GetAllThemes().ToList();
            
            Debug.Log( "ACETMD|PERL:     Results:" );
            PrintLocatedScripts();
            PrintLocatedThemes();
            
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
                        
                    Debug.Log( $"ACETMD|I:     {scriptInfo.Name} was deleted. Removing from database." );
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
                scriptThemeInfoList.Add( new ScriptThemeInfo( script.name, script, _defaultTheme ) );
            }
            
            if (!foundNew)
                Debug.Log( $"ACETMD|ASTD:     No new scripts were found." );
        }

        private bool IsSaved( MonoScript script ) => scriptThemeInfoList.Any( scriptInfo => scriptInfo.script == script );

        private Element GetScriptAndThemeDropdown()
        {
            if ( EditorApplication.isCompiling )
                return new LabelElement( new GUIContent( "Recompiling..." ) );
            
            if ( _existingScripts == null )
                return new LabelElement( new GUIContent( "Existing Scripts is being reset..." ) );
            
            // Refresh button.
            Element refreshButton = new BasicButtonElement( 
                new GUIContent( "Scan For script Changes" ), 
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
                        "script theme Assignment", string.Empty,
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
                    "Assign a theme to Your script", string.Empty,
                    new GroupCustomSettings { NumberOfColumns = 2 },
                    new LabelElement( new GUIContent( "script" ) ),
                    new LabelElement( new GUIContent( "theme" ) ),
                    new LabelElement( new GUIContent( "No ACE scripts found." ) ),
                    refreshButton
                );
        }

        private string[] GetScriptOptions()
        {
            if ( _existingScripts == null )
            {
                Debug.LogWarning( "ATMD|GSO: Aborting script options list as existing scripts list is null." );
                return new string[] {};
            }
            
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

        private void ScriptDropdownUpdated()
        {
            DataUpdateRequiredNotify();
            selectedThemeIndex = GetSelectedScriptsThemeIndex();
            UIStateUpdatedNotify();
        }

        private int GetSelectedScriptsThemeIndex() => _existingThemes.IndexOf( SelectedScript.theme );

        private void ThemeDropdownUpdated()
        {
            DataUpdateRequiredNotify();
            UpdateSelectedScriptTheme();
            UIStateUpdatedNotify();
        }

        private void UpdateSelectedScriptTheme()
        {
            // Important: call only after triggering OnDataUpdateRequired event or the index values
            // will not have been updated.
            string currentThemeName = SelectedScript.theme.name;
            SelectedScript.theme = _existingThemes[selectedThemeIndex];
            Debug.Log( $"ATMD|USST: {SelectedScript.Name}'s theme was changed from {currentThemeName} to {SelectedScript.theme.name}." );
            ThemeAssignmentChangedNotify();
        }

        private void RefreshButtonPressed() => UpdateSavedScriptsList();


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
