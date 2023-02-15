using System;
using UnityEngine;

namespace Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsGlobal.Elements.SingleElements
{
    [Serializable]
    public class SingleElementSettings : Settings
    {
        public SingleElementSettings() 
            : base( 
                new Color( 0.1f, 0.4f, 0.2f ), 
                new Color( 0.4f, 0.5f, 0.1f ) 
            )
        {
        }
    }
}