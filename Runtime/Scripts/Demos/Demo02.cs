using System;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.AceRuntimeRoots;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.RuntimeElementBuilding;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsCustom.Groups;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsCustom.SingleElements;
using static Packages.com.ianritter.aceuiframework.Runtime.Scripts.RuntimeElementBuilding.RuntimeElementBuilder;

using UnityEngine;
using Object = UnityEngine.Object;

namespace Packages.com.ianritter.aceuiframework.Runtime.Scripts.Demos
{
    [ExecuteInEditMode]
    public class Demo02 : AceMonobehaviourRoot
    {
        // Following is an exhaustive list of each potential inspector element types.
        // I had to skip a few since they don't work correctly but I left them listed so it was clear what was accounted for.
        // int
        public int intField = 5;
        [Range(0, 25)]
        public int intSliderField = 5;
        // float
        public float floatField = 4.0f;
        [Range(0f, 25f)]
        public float floatSliderField = 4.0f;
        // bool
        public bool boolField;
        // string
        public string stringField;
        // color
        public Color colorField;
        // object
        public Object objectField;
        // layer mask
        public LayerMask layerMaskField;
        // enum
        public ACEInspectorExample.InspectorLayouts enumsField;
        // vector2
        public Vector2 vector2Field;
        // vector3
        public Vector3 vector3Field;
        // vector4
        public Vector4 vector4Field;
        // Quaternion
        public Quaternion quaternion;
        // rect
        public Rect rectField;
        // array size
        // character
        public Char characterField;
        // animation curve
        public AnimationCurve animationCurveField;
        // bounds
        public Bounds boundsField;
        // gradient
        public Gradient gradient;
        // fixed buffer size
        // vector2 int
        // vector3 int
        // rect int
        // bounds int
        
        public int[] intArray;
        
        [Range( 0f, 0.3f )] public float closeFade1 = 0.012f;
        [Range( 0.6f, 1.0f )] public float farFade1 = 0.5f;
        
        [Range( 0f, 0.3f )] public float closeFade2 = 0.012f;
        [Range( 0.6f, 1.0f )] public float farFade2 = 0.5f;

        // private ACEInspectorExample.InspectorLayouts _enumsChangeCheck = ACEInspectorExample.InspectorLayouts.BasicExample1;
        
        private readonly ElementInfo _dividerElement = GetDividerElement();
        private readonly ElementInfo _layoutDropdownElement = GetElement( nameof( enumsField ), "Inspector Layouts", string.Empty );

        // public override Element[] GetElementList() => new Element[]
        //     { new LabelElement( new GUIContent( $"Test with int: {testInt.ToString()}." ) ) };
        public override string GetTargetName() => nameof(this.GetType);

        public override ElementInfo[] GetElementInfoList()
        {
            return new ElementInfo[] {
                _dividerElement,
                _layoutDropdownElement,
                _dividerElement,
                
                GetMinMaxSliderElement( "Short MM GetName", "MinMax tooltip!", 
                    nameof(closeFade2), nameof(farFade2), 0, 1, new SingleCustomSettings() ),
                GetElement( nameof(closeFade1), "Close Fade 1", string.Empty ), 
                GetGroupWithFoldoutHeading( null, "Section 1", "Head 1 tooltip!",
                    new GroupCustomSettings()
                    {
                        NumberOfColumns = 2,
                        IndentLevelIncrease = 0,
                        TopPadding = 20f
                    },
                    GetElement( nameof(boundsField), "Bounds", string.Empty),
                    GetMinMaxSliderElement( "Long MinMax Slider GetName", "MinMax tooltip!", 
                        nameof(closeFade1), nameof(farFade1), 0, 1 )
                )
            };
        }
    }
}