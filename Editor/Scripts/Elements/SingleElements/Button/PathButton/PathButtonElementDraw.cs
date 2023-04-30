using System.IO;
using Packages.com.ianritter.unityscriptingtools.Runtime.Services.CustomLogger;
using UnityEditor;
using UnityEngine;
using static Packages.com.ianritter.unityscriptingtools.Runtime.Services.TextFormatting.TextFormat;

namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.SingleElements.Button.PathButton
{
    public class PathButtonElementDraw : ButtonElementDraw
    {
        private readonly PathButtonElement _pathButtonElement;
        protected override SingleElement SingleElement => _pathButtonElement;

        private readonly CustomLogger _logger;
        
        
        public PathButtonElementDraw( PathButtonElement pathButtonElement )
        {
            _pathButtonElement = pathButtonElement;
            _logger = _pathButtonElement.Logger;
        }
        
        
        protected override void DrawElementContents()
        {
            using (new EditorGUI.DisabledScope( _pathButtonElement.CustomSettings.ForceDisable))
            {
                if ( !GUI.Button( _pathButtonElement.Layout.GetDrawRect(), _pathButtonElement.GUIContent ) ) return;
                
                PathButtonPressed();

                if ( _pathButtonElement.ButtonCallBack == null ) return;
                
                _pathButtonElement.ButtonCallBack?.Invoke();
            }
        }
        
        private void PathButtonPressed()
        {
            _logger.LogStart( true );
            _logger.Log( "User Themes Path button pressed." );
            _pathButtonElement.PathProperty.stringValue = EditorUtility.OpenFolderPanel( "User Themes Path", "", "" );

            string path = _pathButtonElement.PathProperty.stringValue;
            
            _logger.Log( $"User Themes Path Selected: {GetColoredStringGreen( path )}" );
            _logger.LogIndentStart( $"Application.dataPath: {GetColoredStringRed( Application.dataPath )}" );

            if ( path.StartsWith( Application.dataPath ) )
                _pathButtonElement.PathProperty.stringValue = $"Assets{path.Substring( Application.dataPath.Length )}";
            
            _logger.LogIndentStart( $"Final Path: {GetColoredStringGreen( _pathButtonElement.PathProperty.stringValue )}" );

            ProcessDirectory( _pathButtonElement.PathProperty.stringValue );
            
            _logger.LogEnd();
        }

        private void ProcessDirectory( string path )
        {
            _logger.LogStart();
            _logger.Log( $"Searching directory: {GetColoredStringGrey( path )}" );


            string[] files = Directory.GetFiles( path );
            // logger.LogIndentStart( $"Found {GetColoredStringYellow( files.Length.ToString() )} files at that location:" );

            foreach ( string file in files )
            {
                if ( file.EndsWith( ".meta" ) ) continue;
                // if ( file.EndsWith( ".asset" ) )
                // {
                //     logger.Log( GetColoredStringGreen( file ) );
                //     continue;
                // }
                _logger.Log( GetColoredStringYellow( file ) );
            }
            
            string[] subdirectories = Directory.GetDirectories( path );
            foreach ( string subdirectory in subdirectories )
            {
                ProcessDirectory( subdirectory );
            }
            
            _logger.LogEnd();
        }
    }
}