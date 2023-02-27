using Packages.com.ianritter.aceuiframework.Runtime.Scripts.AceRoots;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.RuntimeElementBuilding;
using UnityEngine;

namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.Editors
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
