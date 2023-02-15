using System;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.Enums;
using UnityEngine;

namespace Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsGlobal
{
    [Serializable]
    public class FrameSettings
    {
        public bool applyFraming = true;
        public bool includeBackground = false;
        public ElementFrameType frameType = ElementFrameType.LeftAndBottomOnly;
        [Range( 1, 4 )]
        public int frameOutlineThickness = 1;
        [Range(0, 5)]
        public float frameAutoPadding = 0f;

        public int frameOutlineColorIndex = 0;
        public int backgroundColorIndex = 1;
    }
}
