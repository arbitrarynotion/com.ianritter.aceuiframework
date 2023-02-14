using static ACEPackage.Runtime.Scripts.AceEditorConstants;

namespace ACEPackage.Runtime.Scripts.SettingsGlobal.Elements.SingleElements.Decorator
{
    public class HeadingElementFrameSettingsVarNames : ElementFrameVarNames
    {
        public readonly string TextHorizontalOffset;
        public readonly string BoxHeight;
        public readonly string EnabledTextColorIndex;
        public readonly string DisabledTextColorIndex;

        public HeadingElementFrameSettingsVarNames( int index ) 
            : base( GetHeadingElementFrameSettingsListVarName, index )
        {
            string arrayLookupString = GetHeadingElementFrameSettingsListVarName + ".Array.data[" + ( index.ToString() ) + "].";
            
            TextHorizontalOffset = arrayLookupString + nameof( HeadingElementFrameSettings.textHorizontalOffset );
            BoxHeight = arrayLookupString + nameof( HeadingElementFrameSettings.boxHeight );
            EnabledTextColorIndex = arrayLookupString + nameof( HeadingElementFrameSettings.enabledTextColorIndex );
            DisabledTextColorIndex = arrayLookupString + nameof( HeadingElementFrameSettings.disabledTextColorIndex );
        }
    }
}
