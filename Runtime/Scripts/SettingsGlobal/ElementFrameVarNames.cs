namespace Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsGlobal
{
    // To bridge the serialization boundary, class variable names are stored in strings that include
    // the path information that will allow them to be retrieved from their respective serialized properties.
    // AceMonobehaviourRoot -> AceMo
    public abstract class ElementFrameVarNames
    {
        public readonly string ShowFrame;
        public readonly string IncludeOutline;
        public readonly string IncludeBackground;
        public readonly string FrameType;
        public readonly string FrameOutlineThickness;
        public readonly string FramePadding;

        // public readonly string FrameOutlineColorIndex;
        // public readonly string BackgroundColorIndex;
        public readonly string FrameOutlineColorName;
        public readonly string BackgroundColorName;
    
        public ElementFrameVarNames( string sourceSettingsVarName, int index )
        {
            string arrayLookupString = sourceSettingsVarName + ".Array.data[" + ( index.ToString() ) + "].";

            ShowFrame = arrayLookupString + nameof( FrameSettings.applyFraming );
            IncludeOutline = arrayLookupString + nameof( FrameSettings.includeOutline );
            IncludeBackground = arrayLookupString + nameof( FrameSettings.includeBackground );
            FrameType = arrayLookupString + nameof( FrameSettings.frameType );
            FrameOutlineThickness = arrayLookupString + nameof( FrameSettings.frameOutlineThickness );
            FramePadding = arrayLookupString + nameof( FrameSettings.frameAutoPadding );
            
            // FrameOutlineColorIndex = arrayLookupString + nameof( FrameSettings.frameOutlineColorIndex );
            // BackgroundColorIndex = arrayLookupString + nameof( FrameSettings.backgroundColorIndex );
            
            FrameOutlineColorName = arrayLookupString + nameof( FrameSettings.frameOutlineColorName );
            BackgroundColorName = arrayLookupString + nameof( FrameSettings.backgroundColorName );
        }
    }
}
