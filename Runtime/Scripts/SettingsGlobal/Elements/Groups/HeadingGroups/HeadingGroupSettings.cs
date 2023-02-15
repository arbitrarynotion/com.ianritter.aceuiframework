using System;
using UnityEngine;

namespace Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsGlobal.Elements.Groups.HeadingGroups
{
    [Serializable]
    public class HeadingGroupSettings : GroupSettings
    {
        public bool useSeparateHeadingSettings = false;
        public bool hideFoldoutGroupElements;
        public bool hideToggleGroupElements;
        public bool hideLabelGroupElements;

        public HeadingGroupSettings() 
            : base( 
                new Color( 0.3f, 0.1f, 0.1f ), 
                new Color( 0.2f, 0.2f, 0.4f ) 
            )
        {
        }
    }
}