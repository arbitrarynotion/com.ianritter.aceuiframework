using System;
using UnityEngine;

namespace Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsGlobal.Elements.Groups.ChildAreaGroups
{
    [Serializable]
    public class ChildAreaGroupSettings : GroupSettings
    {
        public ChildAreaGroupSettings() 
            : base( 
                new Color( 0.5f, 0.3f, 0.3f ), 
                new Color( 0.5f, 0.5f, 0.7f )
            )
        {
        }
    }
}
