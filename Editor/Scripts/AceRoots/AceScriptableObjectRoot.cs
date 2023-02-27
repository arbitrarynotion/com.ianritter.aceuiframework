using System;
using System.Collections.Generic;
using System.Reflection;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.RuntimeElementBuilding;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.Services;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsGlobal.Colors;
using UnityEditor;
using static Packages.com.ianritter.aceuiframework.Runtime.Scripts.Services.TextFormat;
using UnityEngine;
using UnityEngine.XR;

namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.AceRoots
{
    /// <summary>
    ///     Inherit from this class to utilize the custom editor tool for scriptable objects.
    /// </summary>
    public abstract class AceScriptableObjectRoot : ScriptableObject
    {
        // public AceEventHandler aceEventHandler;
        public CustomLogger logger;


        public CustomLogger GetLogger => logger;

        
        // public void OnEnable()
        // {
        //     Debug.Log( "AceScriptableObjectRoot OnEnable called." );
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
        ///     in the editor calling the target script's GetElementList to get the updated element list.
        /// </summary>
        public void UIStateUpdatedNotify()
        {
            OnUIStateUpdated?.Invoke();
            // PrintMyUIStateUpdatedNotifySubscribers();
        }
        
        public delegate void DataUpdateRequired();
        public event DataUpdateRequired OnDataUpdateRequired;
        
        public void DataUpdateRequiredNotify() => OnDataUpdateRequired?.Invoke();

        public void PrintMySubscribers()
        {
            logger.LogStart( MethodBase.GetCurrentMethod(), true );
            logger.Log( $"{GetColoredStringOrange( logger.ApplyNameFormatting( name ) )}'s Events and their subscribers:", true );
            PrintSubscribersForEvent( OnDataUpdated, nameof( OnDataUpdated ) );
            PrintSubscribersForEvent( OnUIStateUpdated, nameof( OnUIStateUpdated ) );
            logger.LogEnd( MethodBase.GetCurrentMethod(), true );
        }

        protected void PrintSubscribersForEvent( Delegate myEvent, string eventName )
        {
            logger.Log( $"{GetColoredStringGreen( logger.ApplyNameFormatting( eventName ) )} subscribers:" );
            
            if ( myEvent == null || myEvent.GetInvocationList().Length == 0 )
            {
                logger.Log( "None", true, true );
                return;
            }

            foreach ( Delegate @delegate in myEvent.GetInvocationList() )
            {
                // Todo: At least for now, all editors will be monobehaviour roots. I'll need to update this if I include the scriptable object root.
                // var monobehaviourRoot = (AceMonobehaviourRootEditor) @delegate.Target;
                logger.Log( $"• {GetColoredStringYellow( logger.ApplyNameFormatting( @delegate.Target.GetType().Name ) )}", 
                    true, true );
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
