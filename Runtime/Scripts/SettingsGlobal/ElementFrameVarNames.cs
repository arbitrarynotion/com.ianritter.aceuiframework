namespace Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsGlobal
{
    public abstract class ElementFrameVarNames
    {
        public readonly string ShowFrame;
        public readonly string IncludeBackground;
        public readonly string FrameType;
        public readonly string FrameOutlineThickness;
        public readonly string FramePadding;

        public readonly string FrameOutlineColorIndex;
        public readonly string BackgroundColorIndex;
    
        public ElementFrameVarNames( string sourceSettingsVarName, int index )
        {
            string arrayLookupString = sourceSettingsVarName + ".Array.data[" + ( index.ToString() ) + "].";

            ShowFrame = arrayLookupString + nameof( FrameSettings.applyFraming );
            IncludeBackground = arrayLookupString + nameof( FrameSettings.includeBackground );
            FrameType = arrayLookupString + nameof( FrameSettings.frameType );
            FrameOutlineThickness = arrayLookupString + nameof( FrameSettings.frameOutlineThickness );
            FramePadding = arrayLookupString + nameof( FrameSettings.frameAutoPadding );
            
            FrameOutlineColorIndex = arrayLookupString + nameof( FrameSettings.frameOutlineColorIndex );
            BackgroundColorIndex = arrayLookupString + nameof( FrameSettings.backgroundColorIndex );
        }
    }
}
