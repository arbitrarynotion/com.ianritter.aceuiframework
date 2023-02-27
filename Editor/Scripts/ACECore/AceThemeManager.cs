using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.AceRoots;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.Services;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsCustom.Groups;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsCustom.SingleElements;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsGlobal.Colors;
using static Packages.com.ianritter.aceuiframework.Runtime.Scripts.AceEditorConstants;
using static Packages.com.ianritter.aceuiframework.Runtime.Scripts.Services.TextFormat;

using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements;
using static Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.ElementBuilding.AceElementBuilder;

namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.ACECore
{
    [CreateAssetMenu( menuName = ThemeManagerAssetMenuName )]
    public class AceThemeManager : AceScriptableObjectRoot
    {
        [SerializeField] [HideInInspector]
        private int selectedScriptIndex = 0;
        [SerializeField] [HideInInspector]
        private int selectedThemeIndex = 0;
        
        [SerializeField]
        private List<ScriptThemeInfo> scriptThemeInfoList = new List<ScriptThemeInfo>();

        private TMListHandler _tmListHandler;
        private TMButtonHandler _tmButtonHandler;

        private ColorPickerHandler _colorPickerHandler;
        
        
        
        public override Element[] GetElementList() => new [] { GetScriptAndThemeDropdown() };

        public delegate void ThemeAssignmentChanged();
        public event ThemeAssignmentChanged OnThemeAssignmentChanged;
        private void ThemeAssignmentChangedNotify()
        {
            OnThemeAssignmentChanged?.Invoke();
            logger.LogStart( MethodBase.GetCurrentMethod() );
            
            logger.Log( $"{name}'s subscribers:", true );
            PrintSubscribersForEvent( OnThemeAssignmentChanged, nameof( OnThemeAssignmentChanged ) );
            logger.LogEnd( MethodBase.GetCurrentMethod() );
            
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

        protected void OnEnable()
        {
            logger.Log( "On enable called..." );
            _tmListHandler = new TMListHandler( this, logger );
            _tmButtonHandler = new TMButtonHandler( _tmListHandler, this, logger );
            var customColorPickerLogger = ObjectLoader.LoadScriptableObject<CustomLogger>( "ColorPickerLogger" );
            _colorPickerHandler = new ColorPickerHandler( 
                customColorPickerLogger, 
                new Rect( new Vector2( 0, 0 ), new Vector2( 0, 0) ), 
                new Vector2(350, 400), 
                5 );
            
            if ( _colorPickerHandler == null )
            {
                logger.LogError( "Color Picker is null!" );
            }

            // Rebuild the scripts list any time files are changed in the project window.
            EditorApplication.projectChanged += OnProjectChanged;
            OnScriptsReloaded += OnScriptsReloadedUpdate;
            _colorPickerHandler.OnColorSelected += OnColorSelection;
            
            
            RefreshScriptThemeInfoList();
        }

        private void OnDisable()
        {
            EditorApplication.projectChanged -= OnProjectChanged;
            OnScriptsReloaded -= OnScriptsReloadedUpdate;
            _colorPickerHandler.OnColorSelected -= OnColorSelection;
        }

        private void OnColorSelection( CustomColor color )
        {
            logger.Log( $"Color picker returned color: {GetColoredString( color.name, color.GetHex() )}" );
            _colorPickerHandler.Close();
        }

        private void OnProjectChanged()
        {
            logger.Log( "Project Changed event called." );
            RefreshScriptThemeInfoList();
        }

        

        private void OnScriptsReloadedUpdate()
        {
            // _logger.Log( $"{GetColoredStringOrange( "***" )} Scripts Reloaded event called." );
            logger.LogEvent( MethodBase.GetCurrentMethod() );
            RefreshScriptThemeInfoList();
        }

        public AceTheme GetThemeForIndex( int index )
        {
            return _tmListHandler.GetThemeForIndex( index );
        }

        public void RefreshScriptThemeInfoList() => 
            _tmListHandler.GetScriptThemeInfoList( scriptThemeInfoList );
        
        public bool IsSaved( MonoScript script ) => scriptThemeInfoList.Any( scriptInfo => scriptInfo.script == script );


        // Used to provide the theme for a given script.
        public AceTheme GetThemeForScript( string scriptName ) => 
            scriptThemeInfoList.Where( scriptThemeInfo => scriptThemeInfo.script.name == scriptName )
                .Select( scriptThemeInfo => scriptThemeInfo.theme ).FirstOrDefault();

        public ScriptThemeInfo GetSelectedScript()
        {
            logger.Log( $"Selection script index is {selectedScriptIndex.ToString()}" );
            return scriptThemeInfoList[selectedScriptIndex] ?? null;
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
            if ( EditorApplication.isCompiling )
                return GetLabelElement( "Recompiling..." );

            // Build list of script options
            Element scriptPopup = scriptThemeInfoList.Count > 0 
                ? GetPopupElement( nameof( selectedScriptIndex ), GUIContent.none, GetScriptOptions(), new SingleCustomSettings(), ScriptDropdownUpdated ) 
                : GetLabelElement( "No ACE scripts found" );
            Element themePopup = _tmListHandler.GetThemesList().Count > 0 
                ? GetPopupElement( nameof( selectedThemeIndex ), GUIContent.none, GetThemeOptions(), new SingleCustomSettings(), ThemeDropdownUpdated ) 
                : GetLabelElement( "No ACE themes found" );

            return GetGroup( 
                null,
                GetGroupWithLabelHeading(
                    "Script Theme Assignment", string.Empty,
                    new GroupCustomSettings() { NumberOfColumns = 2 },
                    new Element[]
                    {
                        scriptPopup,
                        themePopup,
                        GetDividerElement( 6f, 2f ),
                        GetBasicButton( new GUIContent( "Show Preset Colors"), true, new SingleCustomSettings(), _colorPickerHandler.ColorPickerButtonPressed ), 
                    },
                    _tmButtonHandler.GetButtons( scriptThemeInfoList.Count )
                )
            
                // Todo: Menu for creating, duplicating, and deleting themes.
                
            );
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
            logger.Log( $"{GetColoredStringYellow( selectedScript.script.name )}'s theme was changed from " +
                        $"{GetColoredStringYellow( previousTheme.name )} " +
                        $"to {GetColoredStringGreen( selectedScript.theme.name )}." );

            ThemeAssignmentChangedNotify();
        }
        
        
        

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
            logger.LogStart( MethodBase.GetCurrentMethod() );
            logger.Log( "Script theme assignments:", true );

            foreach ( ScriptThemeInfo scriptThemeInfo in scriptThemeInfoList )
            {
                logger.Log(
                    $"{GetColoredStringYellow( logger.ApplyNameFormatting( scriptThemeInfo.script.name ) )} " +
                    $": {GetColoredStringGreen( logger.ApplyNameFormatting( scriptThemeInfo.theme.name ) )}",
                    true, true );
            }

            logger.LogEnd( MethodBase.GetCurrentMethod() );
        }

        public void PrintScriptOptions( string[] scriptOptions )
        {
            logger.Log( $"ACETMD|PSO:     {scriptOptions.Length.ToString()} script options found:" );

            foreach ( string script in scriptOptions )
            {
                logger.Log( $"{GetColoredStringYellow( logger.ApplyNameFormatting( script ) )}" );
            }
        }

        public void PrintThemeOptions( string[] themeOptions )
        {
            logger.Log( $"{GetColoredStringYellow( themeOptions.Length.ToString() )} theme options found:" );
            foreach ( string theme in themeOptions )
            {
                logger.Log( $"{GetColoredStringYellow( logger.ApplyNameFormatting( theme ) )}" );
            }
        }

#endregion


    }
}
