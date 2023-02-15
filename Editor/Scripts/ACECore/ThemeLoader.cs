using System;
using System.Collections.Generic;
using ACEPackage.Editor.Scripts.AceRoots;
using ACEPackage.Runtime.Scripts.AceRoots;
using UnityEditor;
using UnityEngine;
using static ACEPackage.Runtime.Scripts.AceEditorConstants;

namespace ACEPackage.Editor.Scripts.ACECore
{
    public static class ThemeLoader
    {
        public static T LoadScriptableObject<T>( string settingsAssetName )
        {
            string[] guids = AssetDatabase.FindAssets( settingsAssetName, null );
            
            if (guids.Length <= 0)
                return (T) (object) null;

            string path = AssetDatabase.GUIDToAssetPath( guids[0] );
            var settings = (T) (object) AssetDatabase.LoadAssetAtPath( path, typeof(T) );
            return settings;
        }

        // Todo: Make generic version that loads all scriptable objects of the specified type.
        public static AceTheme[] GetAllThemes()
        {
            // Debug.Log( "TL|GAT: Getting all themes..." );
            UnityEngine.Object[] returnArray = Resources.LoadAll(ThemesResourceFolderName, typeof(AceTheme));
            // Debug.Log( $"TL|GAT:     {returnArray.Length.ToString()} themes are being returned." );

            return Array.ConvertAll( returnArray, item => (AceTheme) item );
        }

        // Todo: Make generic version that loads all scripts of a list of script types passed as a 'params' parameter.
        public static IEnumerable<MonoScript> GetAllAceUsers()
        {
            // Debug.Log( "TL|GAT: Getting all user scripts..." );
            string[] guids = AssetDatabase.FindAssets( "t:MonoScript", new[] {UsersSearchFolderName} );
            
            List<MonoScript> aceScripts = new List<MonoScript>();
            
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath( guid );
                var currentObject = (MonoScript)AssetDatabase.LoadAssetAtPath( path, typeof(MonoScript) );
                Type scriptType = currentObject.GetClass();
                if ( currentObject.name.Equals( ThemeCoreName ) || currentObject.name.Equals( ThemeManagerDatabaseCoreName ) )
                    continue;
                
                if ( scriptType != null && scriptType.IsSubclassOf( typeof( AceMonobehaviourRoot ) ) )
                    aceScripts.Add( currentObject );
                
                if ( scriptType != null && scriptType.IsSubclassOf( typeof( AceScriptableObjectRoot ) ) )
                    aceScripts.Add( currentObject );
            }
            
            // Debug.Log( $"TL|GAT:     {aceScripts.Count.ToString()} scripts found." );

            return aceScripts.ToArray();
        }
    }
}
