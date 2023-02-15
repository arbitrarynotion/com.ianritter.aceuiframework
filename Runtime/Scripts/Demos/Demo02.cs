using ACEPackage.Runtime.Scripts.AceRoots;
using ACEPackage.Runtime.Scripts.RuntimeElementBuilding;
using UnityEngine;

namespace ACEPackage.Runtime.Scripts.Demos
{
    [ExecuteInEditMode]
    public class Demo02 : AceMonobehaviourRoot
    {
        public int testInt = 5;

        // public override Element[] GetElementList() => new Element[]
        //     { new LabelElement( new GUIContent( $"Test with int: {testInt.ToString()}." ) ) };

        public override ElementInfo[] GetElementInfoList() => new ElementInfo[] {};
    }
}