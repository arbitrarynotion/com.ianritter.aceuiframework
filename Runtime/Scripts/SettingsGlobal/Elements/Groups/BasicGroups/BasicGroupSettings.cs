using System;
using UnityEngine;

namespace Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsGlobal.Elements.Groups.BasicGroups
{
    [Serializable]
    public class BasicGroupSettings : GroupSettings
    {
        public BasicGroupSettings() 
            : base( 
                new Color( 0f, 0.2f, 0.4f ), 
                new Color( 0.2f, 0.5f, 0.7f ) 
            )
        {
        }
    }
}