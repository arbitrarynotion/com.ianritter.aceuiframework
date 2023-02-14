using System;
using ACEPackage.Editor.Scripts.ElementConditions;
using ACEPackage.Editor.Scripts.Elements;
using ACEPackage.Editor.Scripts.Elements.GroupElements.BasicGroup;
using ACEPackage.Editor.Scripts.Elements.GroupElements.CompositeGroup;
using ACEPackage.Editor.Scripts.Elements.GroupElements.HeadingGroup.FoldOut;
using ACEPackage.Editor.Scripts.Elements.GroupElements.HeadingGroup.Labeled;
using ACEPackage.Editor.Scripts.Elements.GroupElements.HeadingGroup.Toggle;
using ACEPackage.Editor.Scripts.Elements.SingleElements.Decorator.DividingLine;
using ACEPackage.Editor.Scripts.Elements.SingleElements.Decorator.Label;
using ACEPackage.Editor.Scripts.Elements.SingleElements.Properties.Basic;
using ACEPackage.Editor.Scripts.Elements.SingleElements.Properties.MinMaxSlider;
using ACEPackage.Runtime.Scripts.Enums;
using ACEPackage.Runtime.Scripts.RuntimeElementBuilding;

namespace ACEPackage.Editor.Scripts.ElementBuilding
{
    public static class ElementInfoConverter
    {
        // Receive element info and return the instantiated element.
        public static Element ConvertElement( ElementInfo elementInfo )
        {
            switch ( elementInfo.ElementType )
            {
                case ElementType.SinglePropertyBasic:
                    var propertyInfo = (BasicPropertyInfo) elementInfo;
                    return new BasicProperty( 
                        propertyInfo.VarName, 
                        propertyInfo.GUIContent, 
                        propertyInfo.SingleCustomSettings, 
                        propertyInfo.Callback, 
                        propertyInfo.HideOnDisable, 
                        ConvertElementConditions( propertyInfo.ElementConditionInfos )
                    );

                case ElementType.SinglePropertyMinMaxSlider:
                    var minMaxSliderInfo = (MinMaxSliderInfo) elementInfo;
                    return new MinMaxSliderElement( 
                        minMaxSliderInfo.GUIContent, 
                        minMaxSliderInfo.MinVarName, 
                        minMaxSliderInfo.MaxVarName, 
                        minMaxSliderInfo.MinLimit, 
                        minMaxSliderInfo.MaxLimit,
                        minMaxSliderInfo.SingleCustomSettings, 
                        minMaxSliderInfo.Callback,
                        minMaxSliderInfo.HideOnDisable,
                        ConvertElementConditions( minMaxSliderInfo.ElementConditionInfos )
                    );
                
                case ElementType.SinglePropertyPopup:
                    break;
                
                case ElementType.SingleBlank:
                    break;
                
                case ElementType.SingleButtonBasic:
                    break;
                
                case ElementType.SingleButtonTab:
                    break;
                
                case ElementType.SingleDecoratorDividingLine:
                    return new DividingLineElement();

                
                case ElementType.SingleDecoratorLabel:
                    var singleElementInfo = (SingleElementInfo) elementInfo;
                    return new LabelElement( singleElementInfo.GUIContent );

                case ElementType.GroupBasic:
                    var basicGroupInfo = (BasicGroupInfo) elementInfo;
                    return new BasicGroup( 
                        basicGroupInfo.GroupCustomSettings, 
                        ConvertElementInfoList( basicGroupInfo.ElementInfos ) );
                
                
                case ElementType.GroupComposite:
                    var compositeGroupInfo = (CompositeGroupInfo) elementInfo;
                    return new CompositeGroup( 
                        compositeGroupInfo.GroupCustomSettings, 
                        ConvertElementInfoList( compositeGroupInfo.ElementInfos ) );
                
                
                case ElementType.GroupHeadingFoldout:
                    var foldoutGroupInfo = (FoldoutGroupInfo) elementInfo;
                    return new FoldoutGroup( 
                        foldoutGroupInfo.VarName, 
                        foldoutGroupInfo.GUIContent, 
                        foldoutGroupInfo.GroupCustomSettings, 
                        ConvertElementInfoList( foldoutGroupInfo.ElementInfos ) 
                    );
                
                
                case ElementType.GroupHeadingToggle:
                    var toggleGroupInfo = (ToggleGroupInfo) elementInfo;
                    return new ToggleGroup( 
                        toggleGroupInfo.VarName, 
                        toggleGroupInfo.GUIContent, 
                        toggleGroupInfo.GroupCustomSettings, 
                        toggleGroupInfo.HideOnDisable,
                        ConvertElementInfoList( toggleGroupInfo.ElementInfos ) 
                    );
                
                
                case ElementType.GroupHeadingLabeled:
                    var labeledGroupInfo = (LabeledGroupInfo) elementInfo;
                    return new LabeledGroup( 
                        labeledGroupInfo.GUIContent, 
                        labeledGroupInfo.GroupCustomSettings, 
                        ConvertElementInfoList( labeledGroupInfo.ElementInfos ) 
                    );
                

                default:
                    throw new ArgumentOutOfRangeException( elementInfo.ToString(), $"Element type not supported!" );
            }
            
            return null;
        }


        private static Element[] ConvertElementInfoList( ElementInfo[] elementInfos )
        {
            Element[] elements = new Element[ elementInfos.Length ];
            for (int i = 0; i < elementInfos.Length; i++)
            {
                elements[i] = ConvertElement( elementInfos[i] );
            }

            return elements;
        }

        private static ElementCondition[] ConvertElementConditions( ElementConditionInfo[] elementConditionInfos )
        {
            ElementCondition[] conditions = new ElementCondition[ elementConditionInfos.Length ];
            for (int i = 0; i < elementConditionInfos.Length; i++)
            {
                ElementConditionInfo currentConditionInfo = elementConditionInfos[i];
                conditions[i] = new ElementCondition( currentConditionInfo.PropVarName, currentConditionInfo.ConditionOperator, currentConditionInfo.ConditionValue );
            }

            return conditions;
        }
        
        
    }
}
