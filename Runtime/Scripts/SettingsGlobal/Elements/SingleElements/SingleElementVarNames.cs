using static ACEPackage.Runtime.Scripts.AceEditorConstants;

namespace ACEPackage.Runtime.Scripts.SettingsGlobal.Elements.SingleElements
{
    public class SingleElementVarNames : ElementVarNames
    {
        public SingleElementVarNames( int index ) 
            : base( GetSingleElementSettingsListVarName, index )
        {
        }
    }
}
