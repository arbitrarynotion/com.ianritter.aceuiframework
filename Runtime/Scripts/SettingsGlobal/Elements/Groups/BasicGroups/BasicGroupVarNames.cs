using static ACEPackage.Runtime.Scripts.AceEditorConstants;

namespace ACEPackage.Runtime.Scripts.SettingsGlobal.Elements.Groups.BasicGroups
{
    public class BasicGroupVarNames : ElementVarNames
    {
        public BasicGroupVarNames( int index ) 
            : base( GetBasicGroupSettingsListVarName, index )
        {
        }
    }
}