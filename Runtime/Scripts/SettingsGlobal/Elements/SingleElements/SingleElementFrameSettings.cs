using System;

namespace ACEPackage.Runtime.Scripts.SettingsGlobal.Elements.SingleElements
{
    [Serializable]
    public class SingleElementFrameSettings : FrameSettings
    {
        /// <summary>
        ///     Don't draw frames for elements that have their own line.
        /// </summary>
        public bool skipSingleLineFrames = true;
    }
}
