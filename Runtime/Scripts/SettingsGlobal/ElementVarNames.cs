namespace Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsGlobal
{
    public class ElementVarNames
    {
        public readonly string TopPadding;
        public readonly string LeftPadding;
        public readonly string RightPadding;
        public readonly string BottomPadding;

        public readonly string ShowLayoutVisualizations;
        public readonly string HideElements;
        public readonly string LayoutVisualizationsFrameType;
        
        public readonly string ShowPosRect;
        public readonly string PosRectColor;
        
        public readonly string ShowFrameRect;
        public readonly string FrameRectColor;
        
        public readonly string ShowDrawRect;
        public readonly string DrawRectColor;
    
        public ElementVarNames( string sourceSettingsVarName, int index )
        {
            string arrayLookupString = sourceSettingsVarName + ".Array.data[" + ( index.ToString() ) + "].";

            TopPadding = arrayLookupString + nameof( Settings.topPadding );
            LeftPadding = arrayLookupString + nameof( Settings.leftPadding );
            RightPadding = arrayLookupString + nameof( Settings.rightPadding );
            BottomPadding = arrayLookupString + nameof( Settings.bottomPadding );
        
            LayoutVisualizationsFrameType = arrayLookupString + nameof( Settings.layoutVisualizationsFrameType );
            ShowLayoutVisualizations = arrayLookupString + nameof( Settings.showLayoutVisualizations );
            HideElements = arrayLookupString + nameof( Settings.hideElements );
            
            ShowPosRect = arrayLookupString + nameof( Settings.showPosRect );
            PosRectColor = arrayLookupString + nameof( Settings.layoutToolsPosRectColor );
            
            ShowFrameRect = arrayLookupString + nameof( Settings.showFrameRect );
            FrameRectColor = arrayLookupString + nameof( Settings.frameRectColor );
            
            ShowDrawRect = arrayLookupString + nameof( Settings.showDrawRect );
            DrawRectColor = arrayLookupString + nameof( Settings.layoutToolsDrawRectColor );
        }
    }
}
