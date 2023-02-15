using static Packages.com.ianritter.aceuiframework.Runtime.Scripts.AceEditorConstants;

namespace Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsGlobal.Elements.SingleElements
{
    public class SingleElementVarNames : ElementVarNames
    {
        public SingleElementVarNames( int index ) 
            : base( GetSingleElementSettingsListVarName, index )
        {
        }
    }
}
