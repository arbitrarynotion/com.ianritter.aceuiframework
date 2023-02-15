using System;
using ACEPackage.Runtime.Scripts.Enums;
using UnityEngine;

namespace ACEPackage.Runtime.Scripts.SettingsGlobal
{
    [Serializable]
    public abstract class Settings
    {
        [Range(0, 10)]
        public float topPadding = 0f;
        [Range(0, 10)] 
        public float leftPadding = 0f;
        [Range(0, 10)] 
        public float rightPadding = 0f;
        [Range(0, 10)]
        public float bottomPadding = 0f;

        public bool showLayoutTools = false;
        public bool hideElements = false;
        
        public DebugFrameType layoutToolsFrameType = DebugFrameType.FullSolid;
        
        public bool showPosRect;
        public Color layoutToolsPosRectColor;
        
        public bool showFrameRect;
        public Color frameRectColor;
        
        public bool showDrawRect;
        public Color layoutToolsDrawRectColor;

        protected Settings( Color layoutToolsPosRectColor, Color layoutToolsDrawRectColor )
        {
            // Note that these are just the default colors. They can be changed in the settings window.
            this.layoutToolsPosRectColor = layoutToolsPosRectColor;
            this.layoutToolsDrawRectColor = layoutToolsDrawRectColor;
        }
    }
}
