using System;
using UnityEngine;

namespace ACEPackage.Runtime.Scripts.SettingsGlobal.Elements.Groups
{
    [Serializable]
    public abstract class GroupSettings : Settings
    {
        protected GroupSettings( Color layoutToolsPosRectColor, Color layoutToolsDrawRectColor ) 
            : base( layoutToolsPosRectColor, layoutToolsDrawRectColor )
        {
        }
    }
}
