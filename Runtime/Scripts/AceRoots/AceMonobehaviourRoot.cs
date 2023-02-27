using Packages.com.ianritter.aceuiframework.Runtime.Scripts.EventHandlers;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.RuntimeElementBuilding;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.Services;
using UnityEngine;

namespace Packages.com.ianritter.aceuiframework.Runtime.Scripts.AceRoots
{
    /// <summary>
    ///     Inherit from this class to utilize the custom editor tool for monobehaviours.
    /// </summary>
    public abstract class AceMonobehaviourRoot : MonoBehaviour
    {
        [HideInInspector]
        public AceEventHandler aceEventHandler;
        protected CustomLogger logger;
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
    }
}
