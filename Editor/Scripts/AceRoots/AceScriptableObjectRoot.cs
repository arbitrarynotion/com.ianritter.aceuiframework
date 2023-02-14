using ACEPackage.Editor.Scripts.ACECore;
using ACEPackage.Editor.Scripts.Elements;
using UnityEngine;

namespace ACEPackage.Editor.Scripts.AceRoots
{
    /// <summary>
    ///     Inherit from this class to utilize the custom editor tool for scriptable objects.
    /// </summary>
    public abstract class AceScriptableObjectRoot : ScriptableObject
    {
        public AceTheme Theme { get; set; }
        
        /// <summary>
        ///     Provides a list of InspectorElements which tells the editor how to draw the inspector.
        /// </summary>
        public abstract Element[] GetElementList();
        
        
        public delegate void DataUpdated();
        /// <summary>
        ///     This event is invoked when a data change occurs that justifies a repaint (element values have changed).
        /// </summary>
        public event DataUpdated OnDataUpdated;

        /// <summary>
        ///     Used to tell the inspector to update when the global settings have changed.
        /// </summary>
        public void DataUpdatedNotify() => OnDataUpdated?.Invoke();
        
        
        public delegate void UIStateChanged();
        /// <summary>
        ///     Called when the target script detects a change that justifies rebuilding its inspector. The editor will
        ///     call this element's GetElementInfoList in response.
        /// </summary>
        public event UIStateChanged OnUIStateChanged;

        /// <summary>
        ///     Call this when the target script detects a change that justifies rebuilding its inspector. This will result
        ///     in the editor calling the target script's GetElementList to get the updated element list.
        /// </summary>
        public void UIStateChangedNotify() => OnUIStateChanged?.Invoke();
        
        
        public delegate void DataUpdateRequired();
        public event DataUpdateRequired OnDataUpdateRequired;
        
        public void DataUpdateRequiredNotify() => OnDataUpdateRequired?.Invoke();
    }
}
