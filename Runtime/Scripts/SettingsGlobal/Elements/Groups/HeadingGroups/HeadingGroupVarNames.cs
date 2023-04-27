namespace Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsGlobal.Elements.Groups.HeadingGroups
{
    public class HeadingGroupVarNames : ElementVarNames
    {
        public readonly string UseSeparateHeadingSettings;
        
        // public readonly string HideFoldoutGroupElements;
        // public readonly string HideToggleGroupElements;
        // public readonly string HideLabelGroupElements;
        
        public HeadingGroupVarNames( string listVarName, int index ) 
            : base( listVarName, index )
        {
            string arrayLookupString = listVarName + ".Array.data[" + ( index.ToString() ) + "].";

            UseSeparateHeadingSettings = arrayLookupString + nameof( HeadingGroupSettings.useSeparateHeadingSettings );
            
            // HideFoldoutGroupElements = arrayLookupString + nameof( HeadingGroupSettings.hideFoldoutGroupElements );
            // HideToggleGroupElements = arrayLookupString + nameof( HeadingGroupSettings.hideToggleGroupElements );
            // HideLabelGroupElements = arrayLookupString + nameof( HeadingGroupSettings.hideLabelGroupElements );
        }
    }
}