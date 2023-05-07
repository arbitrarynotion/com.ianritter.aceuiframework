using Packages.com.ianritter.aceuiframework.Runtime.Scripts.EventHandlers;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.RuntimeElementBuilding;
using Packages.com.ianritter.unityscriptingtools.Runtime.Services.CustomLogger;
using UnityEngine;

namespace Packages.com.ianritter.aceuiframework.Runtime.Scripts.AceRuntimeRoots
{
    /// <summary>
    ///     Inherit from this class to utilize the custom editor tool for monobehaviours.
    /// </summary>
    public abstract class AceMonobehaviourRoot : MonoBehaviour
    {
        [HideInInspector]
        public AceEventHandler aceEventHandler;
        // protected CustomLogger logger;
        
        /// <summary>
        ///     Provides a list of Elements which tells the editor how to draw the inspector.
        /// </summary>
        // public abstract Element[] GetElementList();

        public abstract string GetTargetName();
        
        public abstract ElementInfo[] GetElementInfoList();
        
        /// <summary>
        ///     Called when the target script detects a change in a value that manages its UI state.
        ///     The editor will respond by calling requesting a new layout via the target script's GetElementList method.
        /// </summary>
        public delegate void TargetUIStateChanged();
        public event TargetUIStateChanged OnTargetUIStateChanged;

        /// <summary>
        ///     Call this when the target script detects a change in a value that manages its UI state.
        ///     This is typically called in the target script's OnValidate method.
        /// </summary>
        protected void TargetUIStateChangedNotify()
        {
            OnTargetUIStateChanged?.Invoke();
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
        
        public delegate void DataUpdateRequired();
        public event DataUpdateRequired OnDataUpdateRequired;
        
        public void DataUpdateRequiredNotify() => OnDataUpdateRequired?.Invoke();
    }
}
