using System;
using JetBrains.Annotations;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.Enums;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsCustom;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsCustom.Groups;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsCustom.SingleElements;
using UnityEngine;

// using ACEPackage.Scripts.EditorBound.ElementConditions;
// using ACEPackage.Scripts.EditorBound.Elements;
// using ACEPackage.Scripts.EditorBound.Elements.GroupElements.BasicGroup;
// using ACEPackage.Scripts.EditorBound.Elements.GroupElements.CompositeGroup;
// using ACEPackage.Scripts.EditorBound.Elements.GroupElements.HeadingGroup.FoldOut;
// using ACEPackage.Scripts.EditorBound.Elements.GroupElements.HeadingGroup.Labeled;
// using ACEPackage.Scripts.EditorBound.Elements.SingleElements.Button;
// using ACEPackage.Scripts.EditorBound.Elements.SingleElements.Button.Tab;
// using ACEPackage.Scripts.EditorBound.Elements.SingleElements.Decorator.DividingLine;
// using ACEPackage.Scripts.EditorBound.Elements.SingleElements.Decorator.Label;
// using ACEPackage.Scripts.EditorBound.Elements.SingleElements.Properties.MinMaxSlider;

namespace Packages.com.ianritter.aceuiframework.Runtime.Scripts.RuntimeElementBuilding
{
    public class ElementConditionInfo
    {
        public string PropVarName;
        public readonly ConditionOperator ConditionOperator;
        public float ConditionValue;

        public ElementConditionInfo( string propVarName, ConditionOperator conditionOperator, int conditionValue )
        {
            PropVarName = propVarName;
            ConditionOperator = conditionOperator;
            ConditionValue = conditionValue;
        }
    }
    
    
    // Collection of all info required to create an element facade.
    public abstract class ElementInfo
    {
        public ElementType ElementType { get; }
        public abstract CustomSettings CustomSettings { get; }
        public bool HideOnDisable { get; protected set; } = false;
        public ElementConditionInfo[] ElementConditionInfos { get; protected set; } = new ElementConditionInfo[] {};
        
        public GUIContent GUIContent { get; }
        public ElementInfo( ElementType elementType, GUIContent guiContent )
        {
            ElementType = elementType;
            GUIContent = guiContent;
        }
    }

    public abstract class SingleElementInfo : ElementInfo
    {
        public SingleCustomSettings SingleCustomSettings { get; }
        public override CustomSettings CustomSettings => SingleCustomSettings;

        public SingleElementInfo( ElementType elementType, GUIContent guiContent, SingleCustomSettings customSettings ) 
            : base( elementType, guiContent )
        {
            SingleCustomSettings = customSettings;
        }

    }

    public class LabelInfo : SingleElementInfo
    {
        public LabelInfo( GUIContent guiContent, SingleCustomSettings customSettings ) 
            : base( ElementType.SingleDecoratorLabel, guiContent, customSettings )
        {
        }
    }
    
    public class DividerInfo : SingleElementInfo
    {
        public DividerInfo() 
            : base( ElementType.SingleDecoratorDividingLine, GUIContent.none, new SingleCustomSettings() )
        {
        }
    }

    public abstract class SingleInteractiveInfo : SingleElementInfo
    {
        public Action Callback;

        public SingleInteractiveInfo( 
            ElementType elementType, 
            GUIContent guiContent, 
            SingleCustomSettings customSettings,
            Action callback ) 
            : base( elementType, guiContent, customSettings )
        {
            Callback = callback;
        }
    }

    public class BasicPropertyInfo : SingleInteractiveInfo
    {
        public string VarName { get; }

        // public BasicPropertyInfo( 
        //     ElementType elementType, 
        //     string varName, 
        //     SingleCustomSettings customSettings,
        //     Action callback ) 
        //     : base( elementType, GUIContent.none, customSettings, callback )
        // {
        //     VarName = varName;
        // }
        
        public BasicPropertyInfo( 
            string varName, 
            GUIContent guiContent, 
            SingleCustomSettings customSettings,
            Action callback ) 
            : base( ElementType.SinglePropertyBasic, guiContent, customSettings, callback )
        {
            VarName = varName;
        }

        public BasicPropertyInfo( 
            string varName, 
            GUIContent guiContent, 
            SingleCustomSettings customSettings,
            Action callback,
            bool hideOnDisable, 
            ElementConditionInfo[] elementConditionInfos ) 
            : base( ElementType.SinglePropertyBasic, guiContent, customSettings, callback )
        {
            VarName = varName;
            HideOnDisable = hideOnDisable;
            ElementConditionInfos = elementConditionInfos;
        }
    }
    
    public class MinMaxSliderInfo : SingleInteractiveInfo
    {
        public string MinVarName { get; }
        public string MaxVarName { get; }
        public float MinLimit { get; }
        public float MaxLimit { get; }

        public MinMaxSliderInfo( 
            GUIContent guiContent, 
            SingleCustomSettings customSettings, 
            string minVarName, 
            string maxVarName, 
            float minLimit, 
            float maxLimit ) 
            : base( ElementType.SinglePropertyMinMaxSlider, guiContent, customSettings, null )
        {
            MinVarName = minVarName;
            MaxVarName = maxVarName;
            MinLimit = minLimit;
            MaxLimit = maxLimit;
        }
    }
    
    
    
    public abstract class GroupElementInfo : ElementInfo
    {
        public GroupCustomSettings GroupCustomSettings { get; set; }
        public override CustomSettings CustomSettings => GroupCustomSettings;
        public ElementInfo[] ElementInfos { get; }

        public GroupElementInfo( 
            ElementType elementType,
            GUIContent guiContent, 
            GroupCustomSettings customSettings,
            ElementInfo[] elementInfos ) 
            : base( elementType, guiContent )
        {
            GroupCustomSettings = customSettings;
            ElementInfos = elementInfos;
        }
    }

    public class FoldoutGroupInfo : GroupElementInfo
    {
        public string VarName { get; }
        public FoldoutGroupInfo(
            string varName,
            GUIContent guiContent, 
            GroupCustomSettings customSettings,
            ElementInfo[] elementInfos ) 
            : base( ElementType.GroupHeadingFoldout, guiContent, customSettings, elementInfos )
        {
            VarName = varName;
        }
    }
    
    public class ToggleGroupInfo : GroupElementInfo
    {
        public string VarName { get; }
        public ToggleGroupInfo(
            string varName,
            GUIContent guiContent, 
            GroupCustomSettings customSettings,
            bool hideOnDisable,
            ElementInfo[] elementInfos ) 
            : base( ElementType.GroupHeadingToggle, guiContent, customSettings, elementInfos )
        {
            VarName = varName;
            HideOnDisable = hideOnDisable;
        }
    }
    
    public class LabeledGroupInfo : GroupElementInfo
    {
        public LabeledGroupInfo(
            GUIContent guiContent, 
            GroupCustomSettings customSettings,
            ElementInfo[] elementInfos ) 
            : base( ElementType.GroupHeadingLabeled, guiContent, customSettings, elementInfos )
        {
        }
    }
    
    public class BasicGroupInfo : GroupElementInfo
    {
        public BasicGroupInfo(
            GroupCustomSettings customSettings,
            ElementInfo[] elementInfos ) 
            : base( ElementType.GroupBasic, GUIContent.none, customSettings, elementInfos )
        {
        }
    }
    
    public class CompositeGroupInfo : GroupElementInfo
    {
        public CompositeGroupInfo(
            GroupCustomSettings customSettings,
            ElementInfo[] elementInfos ) 
            : base( ElementType.GroupComposite, GUIContent.none, customSettings, elementInfos )
        {
        }
    }
    
    
    
    
    
    
    
    public static class RuntimeElementBuilder
    {
#region SingleElement
        
        
        /// <summary>
        ///     Get basic single element.
        /// </summary>
        public static ElementInfo GetElement( 
            string varName, 
            string title, 
            string tooltip = "" )
        {
            return new BasicPropertyInfo( 
                varName, 
                new GUIContent( title, tooltip), 
                new SingleCustomSettings(), 
                null
                );
        }

        /// <summary>
        ///     Get basic single element.
        /// </summary>
        public static ElementInfo GetElement( 
            string varName, 
            [CanBeNull] SingleCustomSettings settings,
            Action callback = null )
        {
            return new BasicPropertyInfo( varName, GUIContent.none, settings, callback );
        }

        /// <summary>
        ///     Get basic single element using both settings and conditions.
        /// </summary>
        public static ElementInfo GetElement( 
            string varName, 
            string title, 
            string tooltip, 
            [CanBeNull] SingleCustomSettings settings,
            Action callback = null, 
            bool hideOnDisable = false, 
            params ElementConditionInfo[] conditions )
        {
            return new BasicPropertyInfo( 
                varName, 
                new GUIContent( title, tooltip), 
                settings ?? new SingleCustomSettings(),
                callback,
                hideOnDisable, 
                conditions );
        }
        
#endregion


#region LabelElements
        
        /// <summary>
        ///     Get label element.
        /// </summary>
        public static ElementInfo GetLabelElement( string title )
        {
            return new LabelInfo( new GUIContent( title ), new SingleCustomSettings() );
        }
        
        /// <summary>
        ///     Get label element.
        /// </summary>
        public static ElementInfo GetLabelElement( string title, string tooltip )
        {
            return new LabelInfo( new GUIContent( title, tooltip), new SingleCustomSettings() );
        }

        /// <summary>
        ///     Get label element.
        /// </summary>
        public static ElementInfo GetLabelElement( string title, SingleCustomSettings settings )
        {
            return new LabelInfo( new GUIContent( title ), settings );
        }
        
        /// <summary>
        ///     Get label element.
        /// </summary>
        public static ElementInfo GetLabelElement( string title, string tooltip, SingleCustomSettings settings )
        {
            return new LabelInfo( new GUIContent( title, tooltip), settings );
        }
        
#endregion


#region MinMaxSliderElements
        
        /// <summary>
        ///     Get min/max element.
        /// </summary>
        public static ElementInfo GetMinMaxSliderElement(
            string title, string tooltip,
            string minVarName, string maxVarName, float minLimit, float maxLimit )
        {
            return new MinMaxSliderInfo( 
                new GUIContent( title, tooltip), 
                new SingleCustomSettings(), 
                minVarName, 
                maxVarName, 
                minLimit, 
                maxLimit );
        }
        
        /// <summary>
        ///     Get min/max element.
        /// </summary>
        public static ElementInfo GetMinMaxSliderElement(
            string title, string tooltip,
            string minVarName, string maxVarName, float minLimit, float maxLimit,
            SingleCustomSettings settings )
        {
            return new MinMaxSliderInfo( 
                new GUIContent( title, tooltip), 
                settings, 
                minVarName, 
                maxVarName, 
                minLimit, 
                maxLimit );
        }
        
#endregion


#region DecoratorElements
        
        /// <summary>
        ///     Get divider element.
        /// </summary>
        public static ElementInfo GetDividerElement()
        {
            return new DividerInfo();
        }
        
#endregion

        

        
#region GroupWithFoldoutHeading
        
        /// <summary>
        ///     Get a foldout group using a toggle, groups settings, and a variable list of elements.
        /// </summary>
        public static ElementInfo GetGroupWithFoldoutHeading( 
            [CanBeNull] string toggleVarName, 
            string title, 
            string tooltip, 
            [CanBeNull] GroupCustomSettings customSettings, 
            params ElementInfo[] elements )
        {
            return GetGroupWithFoldoutHeading(
                toggleVarName, title, tooltip, customSettings, elements, new ElementInfo[]{} );
        }
        
        /// <summary>
        ///     Get a foldout group using a toggle, groups settings, a list of elements, and a variable list of elements.
        /// </summary>
        public static ElementInfo GetGroupWithFoldoutHeading( 
            [CanBeNull] string toggleVarName, 
            string title, 
            string tooltip, 
            [CanBeNull] GroupCustomSettings customSettings, 
            ElementInfo[] elementList,
            params ElementInfo[] elementsSingles )
        {
            ElementInfo[] elements = new ElementInfo[elementList.Length + elementsSingles.Length];
            int index = 0;
            foreach ( ElementInfo element in elementList )
            {
                elements[index++] = element;
            }

            foreach ( ElementInfo element in elementsSingles )
            {
                elements[index++] = element;
            }
            
            return new FoldoutGroupInfo( toggleVarName, new GUIContent( title, tooltip ), customSettings, elements );
        }
        
        /// <summary>
        ///     Get a foldout group using a toggle, groups settings, a single element, and a variable list of elements.
        /// </summary>
        public static ElementInfo GetGroupWithFoldoutHeading( 
            [CanBeNull] string toggleVarName, 
            string title, 
            string tooltip, 
            [CanBeNull] GroupCustomSettings customSettings, 
            ElementInfo singleElement,
            params ElementInfo[] elementList )
        {
            ElementInfo[] elements = new ElementInfo[elementList.Length + 1];
            int index = 0;
            elements[index++] = singleElement;
            foreach ( ElementInfo element in elementList )
            {
                elements[index++] = element;
            }

            return new FoldoutGroupInfo( toggleVarName, new GUIContent( title, tooltip ), customSettings, elements );
        }
        
        /// <summary>
        ///     Get a foldout group using a toggle, groups settings, a list of elements, and a single element.
        /// </summary>
        public static ElementInfo GetGroupWithFoldoutHeading( 
            [CanBeNull] string toggleVarName, 
            string title, 
            string tooltip, 
            [CanBeNull] GroupCustomSettings customSettings, 
            ElementInfo[] elementList,
            ElementInfo singleElement )
        {
            ElementInfo[] elements = new ElementInfo[elementList.Length + 1];
            
            int index = 0;
            foreach ( ElementInfo element in elementList )
            {
                elements[index++] = element;
            }
            
            elements[index] = singleElement;

            return new FoldoutGroupInfo( toggleVarName, new GUIContent( title, tooltip ), customSettings, elements );
        }

#endregion


#region GroupWithToggleHeading
        
        /// <summary>
        ///     Get a foldout group using a toggle, groups settings, and a variable list of elements.
        /// </summary>
        public static ElementInfo GetGroupWithToggleHeading( 
            [CanBeNull] string toggleVarName, 
            string title, 
            string tooltip, 
            [CanBeNull] GroupCustomSettings customSettings, 
            bool hideOnDisable,
            params ElementInfo[] elements )
        {
            return new ToggleGroupInfo( toggleVarName, new GUIContent( title, tooltip ), customSettings, hideOnDisable, elements );
        }

#endregion


#region GroupWithLabelHeading

        /// <summary>
        ///     Get a foldout group using a toggle, groups settings, and a variable list of elements.
        /// </summary>
        public static ElementInfo GetGroupWithLabelHeading( 
            string title, 
            string tooltip, 
            [CanBeNull] GroupCustomSettings customSettings, 
            params ElementInfo[] elements )
        {
            return new LabeledGroupInfo( new GUIContent( title, tooltip ), customSettings, elements );
        }
        
#endregion


#region GroupsWithoutHeadings

#region BasicGroups
        
        /// <summary>
        ///     Get a group using groups settings, and a variable list of elements.
        /// </summary>
        public static ElementInfo GetGroup(
            [CanBeNull] GroupCustomSettings customSettings, 
            params ElementInfo[] elements )
        {
            return new BasicGroupInfo( customSettings, elements );
        }
        
#endregion


#region CompositeGroups

        /// <summary>
        ///  Get a group using groups settings, and a variable list of elements.
        /// This group will be placed on the same settings level as it's parent.
        /// </summary>
        public static ElementInfo GetCompositeGroup(
            [CanBeNull] GroupCustomSettings customSettings, 
            params ElementInfo[] elements )
        {
            return new CompositeGroupInfo( customSettings, elements );
        }

#endregion
        
#endregion

        
#region TabbedSections

        // public static ElementInfo GetTabbedOptionsSection( params ( string name, string tooltip, bool focused, Action callback, ElementInfo section )[] buttonInfoList )
        // {
        //     ButtonElement[] newButtons = new ButtonElement[buttonInfoList.Length];
        //     ElementInfo[] options = new Element[buttonInfoList.Length];
        //     for (int i = 0; i < newButtons.Length; i++)
        //     {
        //         (string name, string tooltip, bool focused, Action callback, ElementInfo section) currentInfo = buttonInfoList[i];
        //         newButtons[i] = new TabButtonElement( currentInfo.name, currentInfo.tooltip, currentInfo.focused, currentInfo.callback );
        //         options[i] = currentInfo.section;
        //     }
        //
        //     return GetTabbedSection( newButtons, options );
        // }
        //
        // private static ElementInfo GetTabbedSection( ButtonElement[] buttons, ElementInfo[] optionGroups )
        // {
        //     // The length of the buttons array and the options array must be the same.
        //     if (buttons.Length != optionGroups.Length)
        //     {
        //         Debug.LogWarning( "HGSS|GTS: Error! The number of buttons must be the same as the number of options in a tabbed section!" );
        //         return GetLabelElement( "Array length mis-match between Button & Options!" );
        //     }
        //
        //     // Determine which button is currently active, this is the index of the options array that should shown.
        //     int activeIndex = 0;
        //     for (int i = 0; i < buttons.Length; i++)
        //     {
        //         if (buttons[i].Focused)
        //         {
        //             activeIndex = i;
        //             break;
        //         }
        //     }
        //
        //     ElementInfo buttonsGroup = GetGroup( new GroupCustomSettings()
        //         {
        //             ForceSingleLine = true, 
        //             NumberOfColumns = buttons.Length,
        //             // CustomFrameSettings = new CustomFrameSettings()
        //             // {
        //             //     frameType = ElementFrameType.BottomOnly,
        //             // }
        //             // HeadingBoxOutlineThickness = 5f
        //         }, 
        //         buttons
        //     );
        //     
        //     // One element for the buttons group and one for the selected option group.
        //     ElementInfo[] selectSectionFinishedList = new Element[2];
        //     selectSectionFinishedList[0] = buttonsGroup;
        //     selectSectionFinishedList[1] = optionGroups[activeIndex];
        //
        //
        //     // Return the group with the buttons and active options section.
        //     return GetGroup( new GroupCustomSettings()
        //         {
        //             ForceSingleLine = true, 
        //             NumberOfColumns = buttons.Length, 
        //             CustomFrameSettings = new CustomFrameSettings() {applyFraming = false}
        //         },
        //         selectSectionFinishedList
        //     );
        // }
        //
        // public static ElementInfo GetTabbedOptionsSectionDisabled( ElementInfo activeSection, params ( string name, string tooltip, bool focused, Action callback )[] buttonInfoList )
        // {
        //     TabButtonElement[] newButtons = new TabButtonElement[buttonInfoList.Length];
        //     for (int i = 0; i < newButtons.Length; i++)
        //     {
        //         (string name, string tooltip, bool focused, Action callback) currentInfo = buttonInfoList[i];
        //         newButtons[i] = new TabButtonElement( currentInfo.name, currentInfo.tooltip, currentInfo.focused, currentInfo.callback,
        //             new SingleCustomSettings()
        //             {
        //                 ForceDisable = true
        //             }
        //         );
        //     }
        //
        //     return GetTabbedSection( activeSection, newButtons );
        // }
        //
        // public static ElementInfo GetTabbedOptionsSection( ElementInfo activeSection, params ( string name, string tooltip, bool focused, Action callback )[] buttonInfoList )
        // {
        //     TabButtonElement[] newButtons = new TabButtonElement[buttonInfoList.Length];
        //     for (int i = 0; i < newButtons.Length; i++)
        //     {
        //         var currentInfo = buttonInfoList[i];
        //         newButtons[i] = new TabButtonElement( currentInfo.name, currentInfo.tooltip, currentInfo.focused, currentInfo.callback );
        //     }
        //
        //     return GetTabbedSection( activeSection, newButtons );
        // }
        //
        // private static ElementInfo GetTabbedSection( ElementInfo activeSection, TabButtonElement[] buttons )
        // {
        //     ElementInfo buttonsGroup = GetGroup( new GroupCustomSettings()
        //         {
        //             ForceSingleLine = true, 
        //             NumberOfColumns = buttons.Length,
        //             LeftPadding = 4f,
        //             RightPadding = 4f,
        //             BottomPadding = 2f
        //         }, 
        //         buttons
        //     );
        //
        //     // Return the group with the buttons and active options section.
        //     return GetGroup( new GroupCustomSettings()
        //         {
        //             ForceSingleLine = true, 
        //             NumberOfColumns = buttons.Length, 
        //             CustomFrameSettings = new CustomFrameSettings() {applyFraming = false}
        //         },
        //         buttonsGroup,
        //         activeSection
        //     );
        // }
        
#endregion
        
    }
}
