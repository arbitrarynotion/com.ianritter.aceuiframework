using System;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.AceRuntimeRoots;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.RuntimeElementBuilding;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsCustom.Groups;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsCustom.SingleElements;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsGlobal.Elements.SingleElements;
using UnityEngine;
using Object = UnityEngine.Object;
using static Packages.com.ianritter.aceuiframework.Runtime.Scripts.RuntimeElementBuilding.RuntimeElementBuilder;

namespace Packages.com.ianritter.aceuiframework.Runtime.Scripts.Demos
{
    public class ACEInspectorExample : AceMonobehaviourRoot
    {
        public enum InspectorLayouts
        {
            BasicExample1,
            BasicExample2,
            BasicExample3,
            BasicExample4,
            GroupTypesExample,
            PropertyTypesExample,
            NestedBasicGroupsExample,
            CompositeGroupExample,
            ComplexExample
        }
        
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
        public InspectorLayouts enumsField;
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

        public int minMaxIntMinLimit = 0;
        public int minMaxIntMaxLimit = 10;
        public int minMaxLowerInt = 0;
        public int minMaxUpperInt = 5;

        private InspectorLayouts _enumsChangeCheck = InspectorLayouts.BasicExample1;
        
        private readonly ElementInfo _dividerElement = GetDividerElement();
        private readonly ElementInfo _layoutDropdownElement = GetElement( nameof( enumsField ), "Inspector Layouts", string.Empty );
        
        
        // Editor reference.
        public SingleElementSettings singleElementSettings = new SingleElementSettings();
        
        
        public override string GetTargetName() => name;

        public override ElementInfo[] GetElementInfoList()
        {
            return enumsField switch
            {
                InspectorLayouts.BasicExample1 => BasicExample1(),
                InspectorLayouts.BasicExample2 => BasicExample2(),
                InspectorLayouts.BasicExample3 => BasicExample3(),
                InspectorLayouts.BasicExample4 => BasicExample4(),
                InspectorLayouts.GroupTypesExample => GroupTypesExample(),
                InspectorLayouts.PropertyTypesExample => PropertyTypesExample(),
                InspectorLayouts.NestedBasicGroupsExample => NestedBasicGroupsExample(),
                InspectorLayouts.CompositeGroupExample => CompositeGroupExample(),
                InspectorLayouts.ComplexExample => ComplexExample(),
                _ => BasicExample1()
            };
        }

        private void OnValidate()
        {
            if (_enumsChangeCheck == enumsField)
                return;

            TargetUIStateChangedNotify();
            _enumsChangeCheck = enumsField;
        }

        private ElementInfo[] BasicExample1()
        {
            return new ElementInfo[] { 
                _dividerElement,
                _layoutDropdownElement,
                _dividerElement,
                
                GetGroupWithFoldoutHeading( null, "Section 1", "Section 1 tooltip!",
                    new GroupCustomSettings()
                    {
                        NumberOfColumns = 2,
                        IndentLevelIncrease = 0
                    },
                
                    GetElement( nameof(intField), "Int", string.Empty ), 
                
                    GetElement( nameof(characterField), "Char", string.Empty),
                
                    GetGroupWithFoldoutHeading( null, "Section 2", "Section 2 tooltip!",
                        new GroupCustomSettings()
                        {
                            NumberOfColumns = 2,
                            ColumnWidthPriority = 0f
                        },
                        GetElement( nameof(floatField), "Float", string.Empty ), 
                        GetElement( nameof(stringField), "String", string.Empty )
                    ),
                    
                    GetElement( nameof(animationCurveField), "Animation Curve", string.Empty)
                )
            };
        }

        private ElementInfo[] BasicExample2()
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

        private ElementInfo[] BasicExample3()
        {
            return new ElementInfo[]
            {
                _dividerElement,
                _layoutDropdownElement,
                _dividerElement,
                GetElement( nameof(stringField), "String", string.Empty ), 
                GetElement( nameof(animationCurveField), "Animation Curve", string.Empty ),  
                GetGroupWithFoldoutHeading( null, "Heading 1", "Head 1 tooltip!", null,
                    GetElement( nameof(boolField), "Bool", string.Empty ), 
                    GetElement( nameof(intField), "Int", "Int tooltip!" ) 
                ), 
                GetGroupWithFoldoutHeading( null, "Heading 2", "Head 2 tooltip!", null,
                    GetLabelElement( "Label 1", "Label 1 tooltip!" ), 
                    GetElement( nameof(floatField), "Float, indented by 1", string.Empty,
                        new SingleCustomSettings()
                        {
                            IndentLevelIncrease = 1
                        }
                    ), 
                    GetElement( nameof(stringField), "String, indented by 2", string.Empty,
                        new SingleCustomSettings()
                        {
                            IndentLevelIncrease = 2
                        }
                    ), 
                    GetGroupWithFoldoutHeading( null, "Heading 3", "Head 3 tooltip!", null,
                        GetElement( nameof(objectField), "Object", string.Empty ),
                        GetElement( nameof(intSliderField), "Int Slider", string.Empty )
                    ),
                    GetElement( nameof(characterField), "Char", string.Empty )
                )
            };
        }

        private ElementInfo[] BasicExample4()
        {
            return new ElementInfo[]
            {
                _dividerElement,
                _layoutDropdownElement,
                _dividerElement,
                GetElement( nameof(stringField), "String", string.Empty ), 
                GetGroupWithFoldoutHeading( null, "Heading 1", "Head 1 tooltip!",
                    new GroupCustomSettings()
                    {
                        NumberOfColumns = 2
                    },
                    GetElement( nameof(boolField), "Bool", string.Empty ) 
                ),
                GetGroup(
                    new GroupCustomSettings()
                    {
                        NumberOfColumns = 2
                    },
                    GetElement( nameof(vector3Field), "Vector3", string.Empty ),
                    GetGroup( null,
                        GetLabelElement( "Label 4", "Label 4 tooltip!" ), 
                        GetElement( nameof(colorField), "Color", string.Empty,
                            new SingleCustomSettings()
                            {
                                IndentLevelIncrease = 1
                            }
                        ) 
                    ), 
                    GetElement( nameof(intArray), "Integer Array", string.Empty,
                        new SingleCustomSettings()
                        {
                            IndentLevelIncrease = 1
                        }
                    )
                )
            };
        }

        private ElementInfo[] GroupTypesExample()
        {
            return new ElementInfo[]
            {
                _dividerElement,
                _layoutDropdownElement,
                _dividerElement,
                GetElement( nameof(stringField), "String", string.Empty ), 
                GetGroupWithFoldoutHeading( null, "Heading 1", "Head 1 tooltip!", null,
                    
                    GetGroup( null,
                        GetElement( nameof(vector3Field), "Vector3", string.Empty ), 
                        GetElement( nameof(intArray), "Integer Array", string.Empty) 
                    ), 
                    
                    GetGroup( null,
                        GetLabelElement( "Label 4", "Label 4 tooltip!" ), 
                        GetElement( nameof(colorField), "Color", string.Empty) 
                    ), 
                    
                    GetElement( nameof(boolField), "Bool", string.Empty ),  
                    GetElement( nameof(intField), "Int", "Int tooltip!" ) 
                ),
                GetElement( nameof(animationCurveField), "Animation Curve", string.Empty )
            };
        }

        private ElementInfo[] PropertyTypesExample()
        {
            return new ElementInfo[]
            {
                _dividerElement,
                _layoutDropdownElement,
                _dividerElement,
                GetGroupWithFoldoutHeading( null, "Property Field Width Examples", "Head 1 tooltip!",
                    new GroupCustomSettings()
                    {
                        NumberOfColumns = 2
                    },
                    GetElement( nameof(intField), "Int", string.Empty ),  
                    GetElement( nameof(intSliderField), "Int Slider", "Int tooltip!" ),
                    GetElement( nameof(floatField), "Float", "IntSlider tooltip!" ),
                    GetElement( nameof(floatSliderField), "Float Slider", "Float tooltip!" ),
                    GetElement( nameof(boolField), "Bool", "FloatSlider tooltip!" ),
                    GetElement( nameof(stringField), "String", "Bool tooltip!" ),
                    GetElement( nameof(colorField), "Color", "String!" ),
                    GetElement( nameof(objectField), "Object", "Color tooltip!" ),
                    GetElement( nameof(layerMaskField), "Layer Mask", "Object tooltip!" ),
                    GetElement( nameof(enumsField), "Enums", "LayerMask tooltip!" ),
                    GetElement( nameof(vector2Field), "Vector2", "Enums tooltip!" ),
                    GetElement( nameof(vector3Field), "Vector3", "Vector2 tooltip!" ),
                    GetElement( nameof(vector4Field), "Vector4", "Vector3 tooltip!" ),
                    GetElement( nameof(quaternion), "Quaternion", "Vector4 tooltip!" ),
                    GetElement( nameof(rectField), "Rect", "Quaternion tooltip!" ),
                    GetElement( nameof(characterField), "Character", "Rect tooltip!" ),
                    GetElement( nameof(animationCurveField), "Animation Curve", "Animation tooltip!" ),
                    GetElement( nameof(boundsField), "Bounds", "Character tooltip!" ),
                    GetElement( nameof(intArray), "Int Array", "Animation tooltip!" ),
                    GetMinMaxSliderElement( "MinMax Slider", "MinMax tooltip!", 
                        nameof(closeFade1), nameof(farFade1), 0, 1 ),
                    GetElement( nameof(gradient), "Gradient", "Bounds tooltip!" )
                ),
            };
        }

        private ElementInfo[] NestedBasicGroupsExample()
        {
            return new ElementInfo[]
            {
                _dividerElement,
                _layoutDropdownElement,
                _dividerElement,
                GetElement( nameof(stringField), "String", string.Empty ), 
                GetGroup(
                    new GroupCustomSettings()
                    {
                        NumberOfColumns = 2
                    },
                    GetElement( nameof(vector3Field), "Vector3", string.Empty ),
                    GetGroup( null,
                        GetLabelElement( "Label 4", "Label 4 tooltip!" ), 
                        GetElement( nameof(colorField), "Color", string.Empty,
                            new SingleCustomSettings()
                            {
                                IndentLevelIncrease = 1
                            }
                        ) 
                    ),
                    GetGroup(
                        new GroupCustomSettings()
                        {
                            NumberOfColumns = 2
                        },
                        GetGroup( null,
                            GetElement( nameof(colorField), "Color", string.Empty ) 
                        ),
                        GetGroup( null,
                            GetElement( nameof(animationCurveField), "Animation Curve", string.Empty) 
                        )
                    ),
                    GetElement( nameof(intArray), "Integer Array", string.Empty)
                )
            };
        }

        // This example is dependent on editor code. Need to rebuild with code in runtime assembly.
        private ElementInfo[] CompositeGroupExample()
        {
            // const string relativePathNameSingleElementSettings = nameof( singleElementSettings );

            return new ElementInfo[]{
                _dividerElement,
                _layoutDropdownElement,
                _dividerElement,
                
                GetElement( nameof(vector3Field), "Vector3", string.Empty ),
                
                // GetGroupWithToggleHeading( 
                //     null,
                //     "Layout Position and Draw Rects",
                //     "Show the outlines of the position boxes used to place each element. " +
                //     "Useful in determining exactly what settings are doing when it's not clear.",
                //     new GroupCustomSettings() { NumberOfColumns = 2 },
                //     false,
                //     
                //     GetElement( nameof(animationCurveField), "Animation Curve", string.Empty,
                //         new SingleCustomSettings() {CenterInFullHeightOfLine = true, FullHeightFrame = false}
                //     ),
                //     
                //     SettingsSection.GetCompositeLayoutDrawGroup( "Group 1", false,
                //         relativePathNameSingleElementSettings + "." + nameof( singleElementSettings.layoutVisualizationsFrameType ),
                //         relativePathNameSingleElementSettings + "." + nameof( singleElementSettings.showPosRect ),
                //         relativePathNameSingleElementSettings + "." + nameof( singleElementSettings.layoutToolsPosRectColor ),
                //         relativePathNameSingleElementSettings + "." + nameof( singleElementSettings.showFrameRect ),
                //         relativePathNameSingleElementSettings + "." + nameof( singleElementSettings.frameRectColor ),
                //         relativePathNameSingleElementSettings + "." + nameof( singleElementSettings.showDrawRect ),
                //         relativePathNameSingleElementSettings + "." + nameof( singleElementSettings.layoutToolsDrawRectColor ), false
                //     ),
                //     SettingsSection.GetCompositeLayoutDrawGroup( "Group 2", false,
                //         relativePathNameSingleElementSettings + "." + nameof( singleElementSettings.layoutVisualizationsFrameType ),
                //         relativePathNameSingleElementSettings + "." + nameof( singleElementSettings.showPosRect ),
                //         relativePathNameSingleElementSettings + "." + nameof( singleElementSettings.layoutToolsPosRectColor ),
                //         relativePathNameSingleElementSettings + "." + nameof( singleElementSettings.showFrameRect ),
                //         relativePathNameSingleElementSettings + "." + nameof( singleElementSettings.frameRectColor ),
                //         relativePathNameSingleElementSettings + "." + nameof( singleElementSettings.showDrawRect ),
                //         relativePathNameSingleElementSettings + "." + nameof( singleElementSettings.layoutToolsDrawRectColor ), false
                //     ),
                //     SettingsSection.GetCompositeLayoutDrawGroup( "Group 3", false,
                //         relativePathNameSingleElementSettings + "." + nameof( singleElementSettings.layoutVisualizationsFrameType ),
                //         relativePathNameSingleElementSettings + "." + nameof( singleElementSettings.showPosRect ),
                //         relativePathNameSingleElementSettings + "." + nameof( singleElementSettings.layoutToolsPosRectColor ),
                //         relativePathNameSingleElementSettings + "." + nameof( singleElementSettings.showFrameRect ),
                //         relativePathNameSingleElementSettings + "." + nameof( singleElementSettings.frameRectColor ),
                //         relativePathNameSingleElementSettings + "." + nameof( singleElementSettings.showDrawRect ),
                //         relativePathNameSingleElementSettings + "." + nameof( singleElementSettings.layoutToolsDrawRectColor ), false
                //     ),
                //     SettingsSection.GetCompositeLayoutDrawGroup( "Group 4", true,
                //         relativePathNameSingleElementSettings + "." + nameof( singleElementSettings.layoutVisualizationsFrameType ),
                //         relativePathNameSingleElementSettings + "." + nameof( singleElementSettings.showPosRect ),
                //         relativePathNameSingleElementSettings + "." + nameof( singleElementSettings.layoutToolsPosRectColor ),
                //         relativePathNameSingleElementSettings + "." + nameof( singleElementSettings.showFrameRect ),
                //         relativePathNameSingleElementSettings + "." + nameof( singleElementSettings.frameRectColor ),
                //         relativePathNameSingleElementSettings + "." + nameof( singleElementSettings.showDrawRect ),
                //         relativePathNameSingleElementSettings + "." + nameof( singleElementSettings.layoutToolsDrawRectColor )
                //     )
                //     
                // )
            };
        }

        private ElementInfo[] ComplexExample()
        {
            return new ElementInfo[] {
                _dividerElement,
                _layoutDropdownElement,
                _dividerElement,
                
                GetElement( nameof(intField), "Int Standalone", "Int tooltip!" ), 
                GetElement( nameof(objectField), "Object Standalone", string.Empty ), 
                
                // Root Element level
                GetGroupWithFoldoutHeading( null, "Foldout Group, indented by 1", "Head 1 tooltip!",
                    new GroupCustomSettings()
                    {
                        NumberOfColumns = 2,
                        IndentLevelIncrease = 1
                    },
                    GetElement( nameof(boundsField), "Bounds", string.Empty ), 
                    GetElement( nameof(rectField), "Rect", string.Empty ), 
                    
                    GetDividerElement( 10f, 10f, Color.yellow, 0.25f, 0f ),
                    GetDividerElement( 10f, 10f, Color.red, 0f, 0.25f ),
                    GetDividerElement( 10f, 10f, Color.green, 0.25f, 0.25f ),
                    
                    // Element level 1
                    GetGroupWithLabelHeading( "Labeled Group, indented by 1", "Head 2 tooltip!",
                        new GroupCustomSettings()
                        {
                            NumberOfColumns = 2,
                            IndentLevelIncrease = 1
                        },
                        GetLabelElement( "Label w constant width.", "Label 2 tooltip!",
                            new SingleCustomSettings()
                            {
                                ConstantWidth = 160
                            }
                        ), 
                        GetLabelElement( "Label.", "Label 3 tooltip!" ), 
                        GetElement( nameof(floatSliderField), "Int Slider Standalone", string.Empty ),
                        GetMinMaxSliderElement( "MinMax Slider", "MinMax tooltip!", 
                            nameof(closeFade1), nameof(farFade1), 0, 1 ),
                        GetMinMaxSliderElement( "MinMax Slider on its own line", "MinMax tooltip!", 
                            nameof(closeFade2), nameof(farFade2), 0, 1,
                            new SingleCustomSettings()
                            {
                                ForceSingleLine = true,
                                TopPadding = 4f
                            }
                        ),
                        GetElement( nameof( closeFade2 ), GUIContent.none, new SingleCustomSettings() { ForceDisable = true } ),
                        GetElement( nameof( farFade2 ), GUIContent.none, new SingleCustomSettings() { ForceDisable = true } ),
                        
                        GetMinMaxSliderElement( "MinMax Slider (Int) on its own line", "MinMax tooltip!", 
                            nameof(minMaxLowerInt), nameof(minMaxUpperInt), minMaxIntMinLimit, minMaxIntMaxLimit,
                            new SingleCustomSettings()
                            {
                                ForceSingleLine = true,
                                TopPadding = 4f
                            }
                        ),

                        GetElement( nameof( minMaxLowerInt ), GUIContent.none, new SingleCustomSettings() { ForceDisable = true } ),
                        GetElement( nameof( minMaxUpperInt ), GUIContent.none, new SingleCustomSettings() { ForceDisable = true } ),
                        
                        // Element level 2
                        GetGroupWithToggleHeading( null, "Toggle Group", "Head 3 tooltip!", null, false,
                            GetElement( nameof(floatField), "Float", string.Empty ), 
                            GetElement( nameof(stringField), "String", string.Empty ), 
                            
                            // Element level 3
                            GetGroupWithFoldoutHeading( null, "Foldout Group, indented by 2", "Head 4 tooltip!",
                                new GroupCustomSettings()
                                {
                                    IndentLevelIncrease = 2
                                },
                                GetElement( nameof(floatSliderField), "Float Slider", string.Empty )
                            )
                        ), 
                        
                        GetGroup(
                            null,
                            GetLabelElement( "Label in Basic Group.", "Label 1 tooltip!" )
                        )
                    )
                ),
                
                GetElement( nameof(intSliderField), "Int Slider Standalone", string.Empty ), 
                
                // Root Element level
                GetGroupWithFoldoutHeading( null, "Section 5", "Head 5 tooltip!", null,
                    GetElement( nameof(boolField), "Bool", string.Empty ), 
                    GetElement( nameof(enumsField), "Enums", string.Empty ), 
                    
                    // Element level 1
                    GetGroup( null,
                        GetElement( nameof(vector3Field), "Vector3 in Basic Group", string.Empty ), 
                        GetElement( nameof(intArray), "Integer Array in Basic Group", "I'm always indented by one to compensate for my left-indented label." ) 
                    ), 
                    
                    GetGroupWithFoldoutHeading( null, "Section 6", "Head 6 tooltip!", null,
                        GetElement( nameof(layerMaskField), "Layer Mask", string.Empty ), 
                        GetElement( nameof(vector2Field), "Vector2, indented by 1", string.Empty,
                            new SingleCustomSettings()
                            {
                                IndentLevelIncrease = 1
                            } 
                        ), 
                        
                        // Element level 2
                        GetGroupWithFoldoutHeading( null, "Foldout Group", "Head 7 tooltip!",
                            new GroupCustomSettings()
                            {
                                NumberOfColumns = 2
                            },
                            GetElement( nameof(animationCurveField), "Animation Curve", string.Empty ), 
                            GetElement( nameof(characterField), "Char", string.Empty ), 
                
                            // Element level 3
                            GetGroup(
                                new GroupCustomSettings()
                                {
                                    IndentLevelIncrease = 0
                                },
                                GetLabelElement( "Label in Basic Group", "Label 4 tooltip!" ), 
                                GetElement( nameof(colorField), "Color in Basic Group", string.Empty,
                                    new SingleCustomSettings()
                                    {
                                        IndentLevelIncrease = 0
                                    }
                                )
                            ),
                
                            GetElement( nameof(vector3Field), "Vector 3", string.Empty,
                                new SingleCustomSettings() {CenterInFullHeightOfLine = true, FullHeightFrame = true}
                            )
                        )
                    ) 
                ) 
            };
        }
    }
}
