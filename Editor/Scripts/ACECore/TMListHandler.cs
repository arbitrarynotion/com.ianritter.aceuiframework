using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.AceRoots;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.AceRoots;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.Services;
using static Packages.com.ianritter.aceuiframework.Runtime.Scripts.Services.ObjectLoader;
using static Packages.com.ianritter.aceuiframework.Runtime.Scripts.AceEditorConstants;

namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.ACECore
{
    /// <summary>
    ///     Manages all operations on the script theme info list.
    /// </summary>
    public class TMListHandler
    {
        private List<MonoScript> ExistingScripts { get; set; }
        private List<AceTheme> ExistingThemes { get; set; }

        private readonly AceTheme _defaultTheme;
        private readonly AceThemeManager _themeManager;
        private readonly CustomLogger _logger;

        public TMListHandler( AceThemeManager themeManager, CustomLogger logger )
        {
            _themeManager = themeManager;
            _logger = logger;
            
            _defaultTheme = LoadScriptableObject<AceTheme>( DefaultThemeName );

            if ( _defaultTheme == null )
                throw new NullReferenceException( $"Unable to load theme \"{DefaultThemeName}\"" );
        }

        public List<AceTheme> GetThemesList( bool update = false )
        {
            // _logger.Log( "GetThemeList was called..." );
            if ( !update && ExistingThemes != null ) 
                return ExistingThemes;
            
            ExistingThemes = GetAssetsByType<AceTheme>( ThemesSearchFolderName );
            
            return ExistingThemes;
        }

        public AceTheme GetThemeForIndex( int index )
        {
            return GetThemesList()[Mathf.Min( index, ( ExistingThemes.Count - 1 ) )];
        }
        
        public string[] GetThemeOptions()
        {
            string[] themeOptions = new string[ExistingThemes.Count];
            for (int i = 0; i < ExistingThemes.Count; i++)
            {
                themeOptions[i] = $"({i.ToString()}) {ExistingThemes[i].name}";
            }
            return themeOptions;
        }

        public void GetScriptThemeInfoList( List<ScriptThemeInfo> scriptThemeInfoList )
        {
            _logger.LogStart( MethodBase.GetCurrentMethod(), true );

            // Update script and theme lists.
            ExistingScripts = GetAllAceUsers();
            PrintScripts();
            ExistingThemes = GetAssetsByType<AceTheme>( ThemesSearchFolderName );
            PrintThemes();
            
            // Update script theme info list to reflect current state of script and theme lists.
            PurgeDeletedScriptsFromDb( scriptThemeInfoList );
            AddNewScriptsToDb( scriptThemeInfoList );
            
            // Notify subscribers that the theme manager's state may have changed.
            _themeManager.UIStateUpdatedNotify();
            
            _logger.LogEnd( MethodBase.GetCurrentMethod(), true );
        }
        
        

        private void PurgeDeletedScriptsFromDb( List<ScriptThemeInfo> scriptThemeInfoList )
        {
            _logger.LogStart( MethodBase.GetCurrentMethod() );

            bool foundDeleted = false;

            // Are previously saved scripts missing?
            if ( scriptThemeInfoList.Count != 0 ) // previous scripts saved.
            {
                if ( ExistingScripts.Count == 0 ) // all scripts removed, just reset.
                {
                    scriptThemeInfoList.Clear();
                    _logger.Log( "    No scripts to search.", true );
                    _logger.LogEnd( MethodBase.GetCurrentMethod() );
                    return;
                }
                
                // Remove deleted scripts from database.
                // Note that using ".ToList()" allow safely modifying the list being iterated through.
                foreach ( ScriptThemeInfo scriptInfo in scriptThemeInfoList.ToList().Where( scriptInfo => !ExistingScripts.Contains( scriptInfo.script ) ) )
                {
                    foundDeleted = true;
                        
                    // If this script was selected, reset selected script index.
                    if ( scriptThemeInfoList.IndexOf( _themeManager.GetSelectedScript() ) == scriptThemeInfoList.IndexOf( scriptInfo ) )
                    {
                        _themeManager.ResetSelectedScript();
                    }
                    
                    // Todo: Can't report the name of the script removed because it doesn't exist at this point. Cache the name?
                    // _logger.Log( $"    {TextFormat.GetColoredStringRed(scriptInfo.script.name)} was deleted. Removing from database." );
                    scriptThemeInfoList.Remove( scriptInfo );
                }
            }

            if ( !foundDeleted )
                _logger.Log( $"    No scripts were deleted.", true );

            _logger.LogEnd( MethodBase.GetCurrentMethod() );
        }

        private void AddNewScriptsToDb( List<ScriptThemeInfo> scriptThemeInfoList )
        {
            _logger.LogStart( MethodBase.GetCurrentMethod() );

            // Are there new scripts that haven't yet been saved?
            bool foundNew = false;
            foreach ( MonoScript script in ExistingScripts.Where( script => !_themeManager.IsSaved( script ) ) )
            {
                foundNew = true;
                _logger.Log( $"{TextFormat.GetColoredStringBlue(script.name)} is new. Adding to database.", true, true );
                
                // Create new script theme info entry for this script.
                scriptThemeInfoList.Add( new ScriptThemeInfo( script.name, script, _defaultTheme ) );
            }
            
            if ( !foundNew )
                _logger.Log( "No new scripts were found.", true );

            _logger.LogEnd( MethodBase.GetCurrentMethod() );
        }


        private List<MonoScript> GetAllAceUsers()
        {
            _logger.LogStart( MethodBase.GetCurrentMethod(), true );
            List<MonoScript> monoScripts = GetAssetsByType<MonoScript>( UsersSearchFolderName, DemosSearchFolderName );
            // Debug.Log( $"    Checking {TextFormat.GetColoredStringYellow(monoScripts.Count.ToString())} assets..." );

            List<MonoScript> aceScripts = new List<MonoScript>();
            foreach ( MonoScript monoScript in monoScripts )
            {
                Type scriptType = monoScript.GetClass();

                // Debug.Log( $"        Scanning \"{TextFormat.GetColoredStringYellow(monoScript.name)}\"..." );

                if ( monoScript.name.Equals( ThemeCoreName ) || monoScript.name.Equals( ThemeManagerCoreName ) )
                    continue;

                if ( scriptType != null && scriptType.IsSubclassOf( typeof( AceMonobehaviourRoot ) ) )
                    aceScripts.Add( monoScript );

                if ( scriptType != null && scriptType.IsSubclassOf( typeof( AceScriptableObjectRoot ) ) )
                    aceScripts.Add( monoScript );
            }

            _logger.Log( $"{TextFormat.GetColoredStringYellow( aceScripts.Count.ToString() )} ACE scripts found." );
            _logger.LogEnd( MethodBase.GetCurrentMethod(), true );

            return aceScripts;
        }

        private void PrintScripts()
        {
            _logger.LogStart( MethodBase.GetCurrentMethod() );
            PrintList( _logger, ExistingScripts );
            _logger.LogEnd( MethodBase.GetCurrentMethod() );
        }

        private void PrintThemes()
        {
            _logger.LogStart( MethodBase.GetCurrentMethod() );
            PrintList( _logger, ExistingThemes );
            _logger.LogEnd( MethodBase.GetCurrentMethod() );
        }

        private static void PrintList<T>( CustomLogger logger, List<T> list ) where T : UnityEngine.Object
        {
            if ( list == null ) return;
            
            logger.Log( $"{TextFormat.GetColoredStringGreen(list.Count.ToString())} in list:" );

            foreach ( T theme in list )
            {
                logger.Log( $"{TextFormat.GetColoredStringYellow( logger.ApplyNameFormatting( theme.name ) )}", true,
                    true );
            }
        }
    }
}