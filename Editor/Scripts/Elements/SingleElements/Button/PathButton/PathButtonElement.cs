using System;
using UnityEditor;
using UnityEngine;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.ElementConditions;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.SingleElements.Button.Basic;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsCustom.SingleElements;
using Packages.com.ianritter.unityscriptingtools.Runtime.Services.CustomLogger;
using static Packages.com.ianritter.unityscriptingtools.Editor.AssetLoader;
using static Packages.com.ianritter.aceuiframework.Runtime.Scripts.AceEditorConstants;

namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.SingleElements.Button.PathButton
{
    public class PathButtonElement : BasicButtonElement
    {
        private ButtonElementLayout _buttonElementLayout;
        private ButtonElementDraw _buttonElementDraw;

        public override ButtonElementLayout ButtonElementLayout => _buttonElementLayout;
        public override ButtonElementDraw ButtonElementDraw => _buttonElementDraw;
        
        private string PathVarName { get; }
        public SerializedProperty PathProperty { get; private set; }


        public CustomLogger Logger;
        
        
        public PathButtonElement( 
            string pathVarName, 
            GUIContent guiContent, 
            bool focused, 
            SingleCustomSettings singleCustomSettings,
            Action buttonCallBack = null, 
            bool hideOnDisable = false, 
            params ElementCondition[] conditions 
        ) : 
            base( guiContent, focused, singleCustomSettings, buttonCallBack, hideOnDisable, conditions )
        {
            PathVarName = pathVarName;
        }


        protected override void InitializeElement( SerializedObject targetScriptableObject )
        {
            Logger = GetAssetByName<CustomLogger>( ElementLoggerName );
            
            PathProperty = targetScriptableObject.FindProperty( PathVarName );

            if ( PathProperty != null ) return;
            
            // Not stopping here so the rest of the UI can draw. This property will be highlighted to show it's missing its data.
            Debug.LogWarning( $"PE|IE: {GetName()}: Error! Failed to find property for \"{PathVarName}\"!" );
        }
        
        protected override void InitializeLayout() => _buttonElementLayout = new PathButtonElementLayout( this );
        protected override void InitializeDraw() => _buttonElementDraw = new PathButtonElementDraw( this );
    }
}
