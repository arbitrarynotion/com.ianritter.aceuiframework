using System;
using UnityEngine;

namespace Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsGlobal.Colors
{
    [Serializable]
    public class CustomColor
    {
        public string name;
        public Color color;

        public CustomColor( string name, Color color )
        {
            this.name = name;
            this.color = color;
        }
    }
}