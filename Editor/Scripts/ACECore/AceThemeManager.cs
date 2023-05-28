using System.Collections.Generic;
using System.Linq;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.AceEditorRoots;
using UnityEditor;
using UnityEngine;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.SingleElements.Button.PathButton;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.SingleElements.Decorator.Label;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsCustom.Groups;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsCustom.SingleElements;
using Packages.com.ianritter.unityscriptingtools.Runtime.Enums;
using static Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.ElementBuilding.AceElementBuilder;
using static Packages.com.ianritter.aceuiframework.Runtime.Scripts.AceEditorConstants;
using static Packages.com.ianritter.unityscriptingtools.Runtime.Services.TextFormatting.TextFormat;

namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.ACECore
{
    [CreateAssetMenu( menuName = ThemeManagerAssetMenuName )]
    public class AceThemeManager : AceScriptableObjectEditorRoot
    {
        [SerializeField] [HideInInspector]
        private int selectedScriptIndex = 0;
        [SerializeField] [HideInInspector]
        private int selectedThemeIndex = 0;
        
        [SerializeField]
        private List<ScriptThemeInfo> scriptThemeInfoList = new List<ScriptThemeInfo>();

        private TmListHandler _tmListHandler;
        private TmButtonHandler _tmButtonHandler;

        [SerializeField]
        private string userThemesPath;
        
        public bool themeSettingsEditsOwnTheme = false;
        [SerializeField] private bool debugFoldoutToggle = false;

        protected override string GetLoggerName() => ThemeManagerLoggerName;

        public override Element[] GetElementList() => new [] { GetScriptAndThemeDropdown() };

        public delegate void ThemeAssignmentChanged();
        public event ThemeAssignmentChanged OnThemeAssignmentChanged;
        private void ThemeAssignmentChangedNotify()
        {
            OnThemeAssignmentChanged?.Invoke();
            Logger.LogStart();
            
            Logger.LogIndentStart( $"{GetColoredStringOrange( NicifyVariableName( name ) )}'s subscribers (all open Inspector window targets):" );
            PrintSubscribersForEvent( OnThemeAssignmentChanged, nameof( OnThemeAssignmentChanged ) );
            Logger.DecrementMethodIndent();
            Logger.LogEnd();
            
        }
        
        // This delegate set exist only to detect when Unity has recompiled.
        public delegate void ScriptsReloaded();
        public static event ScriptsReloaded OnScriptsReloaded;
        [UnityEditor.Callbacks.DidReloadScripts]
        private static void OnScriptsReloadedNotify()
        {
            // Debug.Log( "ATMD|OSR: script recompile detected." );
            OnScriptsReloaded?.Invoke();
        }

        protected override void OnEnableLast()
        {
            Logger.LogStart();
            InitializeHandlers();

            // Rebuild the scripts list any time files are changed in the project window.
            EditorApplication.projectChanged += OnProjectChanged;
            OnScriptsReloaded += OnScriptsReloadedUpdate;

            RefreshScriptThemeInfoList();
            Logger.LogEnd();
        }

        private void InitializeHandlers()
        {
            if ( _tmListHandler == null )
                _tmListHandler = new TmListHandler( this, Logger );
            if ( _tmButtonHandler == null )
                _tmButtonHandler = new TmButtonHandler( _tmListHandler, this, Logger );
        }

        private void OnDisable()
        {
            EditorApplication.projectChanged -= OnProjectChanged;
            OnScriptsReloaded -= OnScriptsReloadedUpdate;
        }

        private void OnProjectChanged()
        {
            Logger.Log( "Project Changed event called." );
            RefreshScriptThemeInfoList();
        }

        private void OnScriptsReloadedUpdate()
        {
            // _logger.Log( $"{GetColoredStringOrange( "***" )} Scripts Reloaded event called." );
            Logger.LogEvent();
            RefreshScriptThemeInfoList();
        }

        public AceTheme GetThemeForIndex( int index ) => _tmListHandler.GetThemeForIndex( index );

        public void RefreshScriptThemeInfoList() => _tmListHandler.GetScriptThemeInfoList( scriptThemeInfoList );
        
        public bool IsSaved( MonoScript script ) => scriptThemeInfoList.Any( scriptInfo => scriptInfo.script == script );


        // Used to provide the theme for a given script.
        public AceTheme GetThemeForScript( string scriptName ) => 
            scriptThemeInfoList.Where( scriptThemeInfo => scriptThemeInfo.script.name == scriptName )
                .Select( scriptThemeInfo => scriptThemeInfo.theme ).FirstOrDefault();

        public ScriptThemeInfo GetSelectedScript()
        {
            Logger.Log( $"Selection script index is {selectedScriptIndex.ToString()}" );

            if ( selectedScriptIndex > ( scriptThemeInfoList.Count - 1 ) )
                selectedScriptIndex = 0;

            ScriptThemeInfo selectionScriptThemeInfo = scriptThemeInfoList[selectedScriptIndex];

            if ( selectionScriptThemeInfo == null )
            {
                selectedScriptIndex = 0;
                selectionScriptThemeInfo = scriptThemeInfoList[selectedScriptIndex];
            }
            
            return selectionScriptThemeInfo ?? null;
        }
        
        public string[] GetThemeOptions() => _tmListHandler.GetThemeOptions();

        public void ResetSelectedScript() => selectedScriptIndex = 0;

        private string[] GetScriptOptions()
        {
            string[] scriptOptions = new string[scriptThemeInfoList.Count];
            for (int i = 0; i < scriptThemeInfoList.Count; i++)
            {
                scriptOptions[i] = $"({i.ToString()}) {scriptThemeInfoList[i].script.name}";
            }

            return scriptOptions;
        }
        
        private Element GetScriptAndThemeDropdown()
        {
            InitializeHandlers();
            
            List<AceTheme> themeList = _tmListHandler.GetThemesList();
            if ( themeList == null || themeList.Count == 0 )
            {
                Logger.Log( "Theme list is null or empty!", CustomLogType.Error );
                return new LabelElement( new GUIContent( "Failed to get Themes.") );
            }
            
            if ( EditorApplication.isCompiling )
                return GetLabelElement( "Recompiling..." );

            // Build list of script options
            Element scriptPopup = scriptThemeInfoList.Count > 0 
                ? GetPopupElement( nameof( selectedScriptIndex ), GUIContent.none, GetScriptOptions(), new SingleCustomSettings(), ScriptDropdownUpdated ) 
                : GetLabelElement( "No ACE scripts found" );
            Element themePopup = _tmListHandler.GetThemesList().Count > 0 
                ? GetPopupElement( nameof( selectedThemeIndex ), GUIContent.none, GetThemeOptions(), new SingleCustomSettings(), ThemeDropdownUpdated ) 
                : GetLabelElement( "No ACE themes found" );

            return GetCompositeGroup( 
                null,
                GetGroupWithLabelHeading(
                    "Script Theme Assignment", string.Empty,
                    new GroupCustomSettings() { NumberOfColumns = 2 },
                    
                    new Element[]
                    {
                        scriptPopup,
                        themePopup,
                        GetDividerElement( 6f, 2f ),

                        new PathButtonElement( 
                            nameof( userThemesPath ),
                            new GUIContent( "User Themes Path"), 
                            true,
                            new SingleCustomSettings() {ForceSingleLine = true},
                            UserThemesPathButtonPressed
                        ), 
                        
                        GetLabelElement( userThemesPath ),
                        
                        GetDividerElement( 6f, 2f ),
                    },

                    _tmButtonHandler.GetButtons( scriptThemeInfoList.Count )
                ),
            
                // Todo: Menu for creating, duplicating, and deleting themes.
                
                GetGroupWithFoldoutHeading( nameof( debugFoldoutToggle ), "Debug", string.Empty, null,
                    GetElement( nameof( themeSettingsEditsOwnTheme ), "Theme Self-Edit" )
                )
            );
        }

        private void UserThemesPathButtonPressed()
        {
            DataUpdateRequiredNotify();
            RefreshScriptThemeInfoList();
            UIStateUpdatedNotify();
        }

        private void ThemeDropdownUpdated()
        {
            DataUpdateRequiredNotify();
            UpdateSelectedScriptTheme();
            UIStateUpdatedNotify();
        }

        private void ScriptDropdownUpdated()
        {
            DataUpdateRequiredNotify();
            selectedThemeIndex = GetSelectedScriptsThemeIndex();
            UIStateUpdatedNotify();
        }

        private int GetSelectedScriptsThemeIndex()
        {
            return _tmListHandler.GetThemesList().IndexOf( GetSelectedScript().theme );
        }

        private void UpdateSelectedScriptTheme()
        {
            // Important: call only after triggering OnDataUpdateRequired event or the index values
            // will not have been updated.
            ScriptThemeInfo selectedScript = GetSelectedScript();
            AceTheme previousTheme = selectedScript.theme;
            selectedScript.theme = _tmListHandler.GetThemesList()[selectedThemeIndex];
            Logger.Log( $"{GetColoredStringYellow( selectedScript.script.name )}'s theme was changed from " +
                        $"{GetColoredStringYellow( ( previousTheme == null ) ? "DELETED" : previousTheme.name )} " +
                        $"to {GetColoredStringGreen( selectedScript.theme.name )}." );

            ThemeAssignmentChangedNotify();
        }


        public string GetUserThemesPath() => userThemesPath;

        // public void ResetSelected()
        // {
        //     selectedScriptIndex = 0;
        //     selectedThemeIndex = 0;
        //     // _existingScripts.Clear();
        //     // _existingThemes.Clear();
        // }


#region PrintHelpers

        public void PrintScriptThemeInfoList()
        {
            Logger.LogStart();
            Logger.LogIndentStart( "Script theme assignments:", true );

            foreach ( ScriptThemeInfo scriptThemeInfo in scriptThemeInfoList )
            {
                Logger.LogOneTimeIndent(
                    $"{GetColoredStringYellow( NicifyVariableName( scriptThemeInfo.script.name ) )} " +
                    $": {GetColoredStringGreen( NicifyVariableName( scriptThemeInfo.theme.name ) )}" );
            }

            Logger.LogEnd();
        }

        public void PrintScriptOptions( string[] scriptOptions )
        {
            Logger.Log( $"{scriptOptions.Length.ToString()} script options found:" );

            foreach ( string script in scriptOptions )
            {
                Logger.Log( $"{GetColoredStringYellow( NicifyVariableName( script ) )}" );
            }
        }

        public void PrintThemeOptions( string[] themeOptions )
        {
            Logger.Log( $"{GetColoredStringYellow( themeOptions.Length.ToString() )} theme options found:" );
            foreach ( string theme in themeOptions )
            {
                Logger.Log( $"{GetColoredStringYellow( NicifyVariableName( theme ) )}" );
            }
        }

#endregion


    }
}
