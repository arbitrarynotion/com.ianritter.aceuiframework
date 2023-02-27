using System;
using System.Reflection;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.Services;
using UnityEngine;

namespace Packages.com.ianritter.aceuiframework.Runtime.Scripts.EventHandlers
{
    [Serializable]
    public class AceEventHandler
    {
        private CustomLogger _logger;
        private readonly string _owner;

        public AceEventHandler( string owner, CustomLogger logger )
        {
            Debug.Log( "AceEventHandler constructor called." );
            _owner = owner;
            _logger = logger;
            if ( logger == null )
            {
                Debug.LogWarning( "    Failed to set logger!" );
            }
        }


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
            _logger.LogStart( MethodBase.GetCurrentMethod(), true );
            _logger.Log( $"{_owner}'s Events and their subscribers:" );
            PrintSubscribersForEvent( OnDataUpdated );
            PrintSubscribersForEvent( OnUIStateUpdated );
            _logger.LogEnd( MethodBase.GetCurrentMethod(), true );
        }

        private void PrintSubscribersForEvent( Delegate myEvent )
        {
            _logger.Log( $"{TextFormat.GetColoredStringGreen(nameof(UIStateUpdated))} subscribers:" );
            
            if ( myEvent == null || myEvent.GetInvocationList().Length == 0 )
            {
                _logger.Log( "    No subscribers found." );
                return;
            }

            foreach ( Delegate @delegate in myEvent.GetInvocationList() )
            {
                // Todo: At least for now, all editors will be monobehaviour roots. I'll need to update this if I include the scriptable object root.
                // var monobehaviourRoot = (AceMonobehaviourRootEditor) @delegate.Target;
                _logger.Log( $"    {TextFormat.GetColoredStringYellow(@delegate.Target.ToString())}" );
            }
        }
        
    }
}