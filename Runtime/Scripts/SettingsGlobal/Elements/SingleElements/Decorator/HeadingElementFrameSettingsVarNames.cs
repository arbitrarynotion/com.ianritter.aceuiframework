using static Packages.com.ianritter.aceuiframework.Runtime.Scripts.AceEditorConstants;

namespace Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsGlobal.Elements.SingleElements.Decorator
{
    public class HeadingElementFrameSettingsVarNames : ElementFrameVarNames
    {
        public readonly string TextHorizontalOffset;
        public readonly string BoxHeight;
        public readonly string EnabledTextColorIndex;
        public readonly string DisabledTextColorIndex;
        public readonly string BackgroundInactiveColorIndex;

        public HeadingElementFrameSettingsVarNames( int index ) 
            : base( GetHeadingElementFrameSettingsListVarName, index )
        {
            string arrayLookupString = GetHeadingElementFrameSettingsListVarName + ".Array.data[" + ( index.ToString() ) + "].";
            
            TextHorizontalOffset = arrayLookupString + nameof( HeadingElementFrameSettings.textHorizontalOffset );
            BoxHeight = arrayLookupString + nameof( HeadingElementFrameSettings.boxHeight );
            EnabledTextColorIndex = arrayLookupString + nameof( HeadingElementFrameSettings.enabledTextColorIndex );
            DisabledTextColorIndex = arrayLookupString + nameof( HeadingElementFrameSettings.disabledTextColorIndex );
            BackgroundInactiveColorIndex = arrayLookupString + nameof( HeadingElementFrameSettings.backgroundInactiveColorIndex );
        }
    }
}
