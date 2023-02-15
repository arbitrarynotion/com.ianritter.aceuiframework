using static Packages.com.ianritter.aceuiframework.Runtime.Scripts.AceEditorConstants;

namespace Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsGlobal.Elements.SingleElements
{
    public class SingleElementFrameVarNames : ElementFrameVarNames
    {
        public readonly string FramesSkipSingleLine;

        public SingleElementFrameVarNames( int index ) 
            : base( GetSingleElementFrameSettingsListVarName, index )
        {
            string arrayLookupString = GetSingleElementFrameSettingsListVarName + ".Array.data[" + ( index.ToString() ) + "].";
            
            FramesSkipSingleLine = arrayLookupString + nameof( SingleElementFrameSettings.skipSingleLineFrames );
        }
    }
}
