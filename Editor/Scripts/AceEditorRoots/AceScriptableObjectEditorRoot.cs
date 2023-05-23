using System;
using UnityEngine;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.EditorWindows;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.InspectorEditors;
using Packages.com.ianritter.unityscriptingtools.Runtime.Services.CustomLogger;
using static Packages.com.ianritter.unityscriptingtools.Runtime.Services.TextFormatting.TextFormat;
using static Packages.com.ianritter.unityscriptingtools.Runtime.Services.CustomColors.PresetColors;
using static Packages.com.ianritter.unityscriptingtools.Editor.AssetLoader;
using static Packages.com.ianritter.aceuiframework.Runtime.Scripts.AceEditorConstants;

namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.AceEditorRoots
{
    /// <summary>
    ///     Inherit from this class to utilize the custom editor tool for scriptable objects.
    /// </summary>
    public abstract class AceScriptableObjectEditorRoot : ScriptableObject
    {
        // public AceEventHandler aceEventHandler;
        protected CustomLogger logger;
        
        public CustomLogger GetLogger => logger;

        
        // public void OnEnable()
        // {
        //     Debug.Log( "AceScriptableObjectEditorRoot OnEnable called." );
        //     // Logger = LoadScriptableObject<CustomLogger>( ThemeLoggerName, LoggersSearchFolderName );
        //     // Debug.Log( ( Logger != null ) ? "Logger successfully assigned." : "Could not find Logger!" );
        //
        //     // aceEventHandler = new AceEventHandler( name, Logger );
        //     // Debug.Log( ( aceEventHandler != null ) ? "AceEventHandler successfully instantiated." : "Could not instantiate AceEventHandler!" );
        //
        //     OnEnableLast();
        // }
        //
        // protected virtual void OnEnableLast() {}

        /// <summary>
        ///     Provides a list of InspectorElements which tells the editor how to draw the inspector.
        /// </summary>
        public abstract Element[] GetElementList();
        
        // public abstract ElementInfo[] GetElementInfoList();

        private void OnEnable()
        {
            logger = GetAssetByName<CustomLogger>( GetLoggerName(), LoggersSearchFolderName );
            if ( logger == null )
            {
                logger = GetAssetByName<CustomLogger>( DefaultSoLoggerName, LoggersSearchFolderName );
                Debug.LogError( $"Failed to load {GetLoggerName()}! Loading Default theme." );
            }
            // string result = logger == null ? $"{GetColoredStringMaroon( "failed" )}" : $"{GetColoredStringGreenYellow( "succeeded" )}";
            // Debug.LogWarning( $"Loading SO's theme: {result}." );
            OnEnableFirst();
            OnEnableLast();
        }

        protected abstract string GetLoggerName();

        /// <summary>
        ///     Called in OnEnable before anything else.
        /// </summary>
        protected virtual void OnEnableFirst()
        {
        }

        /// <summary>
        ///     Call in OnEnable after everything else.
        /// </summary>
        protected virtual void OnEnableLast()
        {
        }



#region EventHandling
        
        public delegate void DataUpdated();
        /// <summary>
        ///     This event is invoked when a data change occurs that justifies a repaint (element values have changed).
        /// </summary>
        public event DataUpdated OnDataUpdated;

        /// <summary>
        ///     Used to tell the inspector to update when the global settings have changed.
        /// </summary>
        public void DataUpdatedNotify() => OnDataUpdated?.Invoke();
        
        public delegate void UIStateUpdated();
        /// <summary>
        ///     Called when the target script detects a change that justifies rebuilding its inspector. The editor will
        ///     call this element's GetElementInfoList in response.
        /// </summary>
        public event UIStateUpdated OnUIStateUpdated;

        /// <summary>
        ///     Call this when the target script detects a change that justifies rebuilding its inspector. This will result
        ///     in the editor calling ApplyModifiedProperties, then calling the target script's GetElementList to get the updated
        ///     element list, then calling Repaint.
        /// </summary>
        public void UIStateUpdatedNotify()
        {
            OnUIStateUpdated?.Invoke();
            // PrintMyUIStateUpdatedNotifySubscribers();
        }
        
        /// <summary>
        /// Use when ApplyModifiedProperties needs to be called without triggering a complete rebuild of the settings window.
        /// </summary>
        public delegate void DataUpdateRequired();
        public event DataUpdateRequired OnDataUpdateRequired;
        
        public void DataUpdateRequiredNotify() => OnDataUpdateRequired?.Invoke();

        public void PrintMySubscribers()
        {
            logger.LogStart( true );
            logger.LogIndentStart( $"{GetColoredStringOrange( NicifyVariableName( name ) )}'s Events and their subscribers:" );
            PrintSubscribersForEvent( OnDataUpdated, nameof( OnDataUpdated ) );
            PrintSubscribersForEvent( OnUIStateUpdated, nameof( OnUIStateUpdated ) );
            logger.LogEnd();
        }

        protected void PrintSubscribersForEvent( Delegate myEvent, string eventName )
        {
            logger.Log( $"• {GetColoredStringGreen( NicifyVariableName( eventName ) )} subscribers:" );
            
            if ( myEvent == null || myEvent.GetInvocationList().Length == 0 )
            {
                logger.LogOneTimeIndent( "None" );
                return;
            }

            foreach ( Delegate currentDelegate in myEvent.GetInvocationList() )
            {
                // Todo: At least for now, all editors will be monobehaviour roots. I'll need to update this if I include the scriptable object root.
                // var monobehaviourRoot = (AceMonobehaviourRoot) currentDelegate.Target;
                // logger.Log( $"• {GetColoredStringYellow( logger.ApplyNameFormatting( currentDelegate.Target.GetType().Name ) )}", true, true );
                // Type type = currentDelegate.GetType();
                
                if ( currentDelegate.Target.GetType() == typeof( AceThemeEditorWindow ) )
                {
                    var target = (AceThemeEditorWindow) currentDelegate.Target;
                    logger.LogOneTimeIndent( $"• {GetColoredStringYellow( NicifyVariableName( currentDelegate.Target.GetType().Name ) )}: " +
                                             $"{GetColoredString( target.GetTargetName(), Yellow.color )}" );
                }
                // else if ( currentDelegate.Target.GetType() == typeof( AceMonobehaviourRoot ) )
                // {
                //     var target = (AceMonobehaviourRoot) currentDelegate.Target;
                //     logger.Log( $"• {GetColoredStringYellow( logger.ApplyNameFormatting( target.GetTargetName() ) )}", true, true );
                // }
                else if ( currentDelegate.Target.GetType() == typeof( AceMonobehaviourRootEditor ) )
                {
                    var target = (AceMonobehaviourRootEditor) currentDelegate.Target;
                    logger.LogOneTimeIndent( $"• {GetColoredStringYellow( NicifyVariableName( currentDelegate.Target.GetType().Name ) )}: " +
                                $"{GetColoredString( target.target.name, GreenYellow.color )}" );
                }
                else
                {
                    logger.LogIndentStart( $"{GetColoredStringRed( "Couldn't identify the target type" )}:", true );
                    logger.LogIndentStart( $"delegate.GetType() = {GetColoredStringOrange( currentDelegate.GetType().ToString() )}", true );
                    logger.Log( $"delegate.Target.GetType() = {GetColoredStringYellow( currentDelegate.Target.GetType().ToString() )}" );
                    logger.DecrementMethodIndent( 2 );
                }
            }
            
            // // Testing color selection
            // logger.Log( "All Color Fields:" );
            // logger.IncrementMethodTabLevel();
            // IEnumerable<FieldInfo> colorFields = PresetColors.GetAllFields( typeof( PresetColors ) );
            // foreach ( FieldInfo fieldInfo in colorFields )
            // {
            //     logger.Log( $"{fieldInfo.Name}" );
            //     logger.Log( $"{((CustomColor)fieldInfo.GetValue( null )).name.ToString()}" );
            // }
        }

        // private string CleanUpEventName( string eventName )
        // {
        //     // Take this:  (Packages.com.ianritter.aceuiframework.Editor.Scripts.Editors.AceThemeEditorWindow)
        //     // and return this: Ace Theme Editor Window
        //     
        // }

#endregion
    }
}
