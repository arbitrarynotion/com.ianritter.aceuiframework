using Packages.com.ianritter.aceuiframework.Runtime.Scripts.AceRuntimeRoots;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.RuntimeElementBuilding;

namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.InspectorEditors
{
    public class CustomLoggerEditor : AceMonobehaviourRoot
    {
        public override string GetTargetName() => nameof( this.GetType );

        public override ElementInfo[] GetElementInfoList()
        {
            return new ElementInfo[]
            {
                
            };
        }
    }
}
