using Packages.com.ianritter.aceuiframework.Runtime.Scripts.AceRoots;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.RuntimeElementBuilding;
using UnityEngine;

namespace Packages.com.ianritter.aceuiframework.Runtime.Scripts.Demos
{
    [ExecuteInEditMode]
    public class Demo03 : AceMonobehaviourRoot
    {
        public int testInt = 5;

        // public override Element[] GetElementList() => new Element[]
        //     { new LabelElement( new GUIContent( $"Test with int: {testInt.ToString()}." ) ) };
        public override string GetTargetName() => nameof(this.GetType);

        public override ElementInfo[] GetElementInfoList() => new ElementInfo[] {};
    }
}