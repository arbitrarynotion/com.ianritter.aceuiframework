using System;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.ElementConditions;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.GroupElements.BasicGroup;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.GroupElements.CompositeGroup;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.GroupElements.HeadingGroup.FoldOut;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.GroupElements.HeadingGroup.Labeled;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.GroupElements.HeadingGroup.Toggle;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.SingleElements.Decorator.DividingLine;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.SingleElements.Decorator.Label;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.SingleElements.Properties.Basic;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.SingleElements.Properties.CustomColor;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.SingleElements.Properties.MinMaxSlider;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.SingleElements.Properties.Popup;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.Enums;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.RuntimeElementBuilding;
using UnityEngine;

namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.ElementBuilding
{
    /// <summary>
    ///     Convert ElementInfo into Element for the Ace UI Framework.
    /// </summary>
    public static class ElementInfoConverter
    {
        // Receive element info and return the instantiated element.
        private static Element ConvertElement( ElementInfo elementInfo )
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
                    var popupPropertyInfo = (PopupPropertyInfo) elementInfo;
                    return new PopupElement( 
                        popupPropertyInfo.VarName, 
                        popupPropertyInfo.GUIContent, 
                        popupPropertyInfo.Options,
                        popupPropertyInfo.SingleCustomSettings, 
                        popupPropertyInfo.Callback, 
                        popupPropertyInfo.HideOnDisable, 
                        ConvertElementConditions( popupPropertyInfo.ElementConditionInfos )
                    );
                    
                case ElementType.SingleCustomColor:
                    var customColorInfo = (CustomColorInfo) elementInfo;
                    return new ColorPickerElement( 
                        customColorInfo.VarName, 
                        customColorInfo.GUIContent, 
                        customColorInfo.SingleCustomSettings, 
                        customColorInfo.Callback, 
                        customColorInfo.HideOnDisable, 
                        ConvertElementConditions( customColorInfo.ElementConditionInfos )
                    );
                
                case ElementType.SingleBlank:
                    break;
                
                case ElementType.SingleButtonBasic:
                    break;
                
                case ElementType.SingleButtonTab:
                    break;
                
                case ElementType.SingleDecoratorDividingLine:
                    var dividingLineInfo = (DividerInfo) elementInfo;

                    if ( !dividingLineInfo.UseCustomColor ) return new DividingLineElement();
                    
                    if ( dividingLineInfo.SettingsAreLive )
                    {
                        return new DividingLineElement(
                            dividingLineInfo.Color,
                            dividingLineInfo.BoxHeight,
                            dividingLineInfo.DividerThickness,
                            dividingLineInfo.LeftTrimPercentPropertyVarName,
                            dividingLineInfo.RightTrimPercentPropertyVarName,
                            dividingLineInfo.SingleCustomSettings
                        );
                    }

                    return new DividingLineElement(
                        dividingLineInfo.Color,
                        dividingLineInfo.BoxHeight,
                        dividingLineInfo.DividerThickness,
                        dividingLineInfo.LeftTrimPercent,
                        dividingLineInfo.RightTrimPercent,
                        dividingLineInfo.SingleCustomSettings
                    );

                case ElementType.SingleDecoratorLabel:
                    var labelInfo = (LabelInfo) elementInfo;
                    return new LabelElement( labelInfo.GUIContent, labelInfo.SingleCustomSettings );

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
                        foldoutGroupInfo.ChangeCallBack,
                        ConvertElementInfoList( foldoutGroupInfo.ElementInfos ) 
                    );
                
                
                case ElementType.GroupHeadingToggle:
                    var toggleGroupInfo = (ToggleGroupInfo) elementInfo;
                    return new ToggleGroup( 
                        toggleGroupInfo.VarName, 
                        toggleGroupInfo.GUIContent, 
                        toggleGroupInfo.GroupCustomSettings, 
                        toggleGroupInfo.HideOnDisable,
                        toggleGroupInfo.ChangeCallBack,
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


        public static Element[] ConvertElementInfoList( ElementInfo[] elementInfos )
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
