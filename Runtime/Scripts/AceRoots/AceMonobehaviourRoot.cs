using ACEPackage.Runtime.Scripts.RuntimeElementBuilding;
using UnityEngine;

namespace ACEPackage.Runtime.Scripts.AceRoots
{
    /// <summary>
    ///     Inherit from this class to utilize the custom editor tool for monobehaviours.
    /// </summary>
    public abstract class AceMonobehaviourRoot : MonoBehaviour
    {
        // public AceTheme Theme { get; set; }

        /// <summary>
        ///     Provides a list of Elements which tells the editor how to draw the inspector.
        /// </summary>
        // public abstract Element[] GetElementList();
        
        
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
