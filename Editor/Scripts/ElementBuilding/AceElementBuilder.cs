using System;
using ACEPackage.Editor.Scripts.ElementConditions;
using ACEPackage.Editor.Scripts.Elements;
using ACEPackage.Editor.Scripts.Elements.GroupElements.BasicGroup;
using ACEPackage.Editor.Scripts.Elements.GroupElements.CompositeGroup;
using ACEPackage.Editor.Scripts.Elements.GroupElements.HeadingGroup.FoldOut;
using ACEPackage.Editor.Scripts.Elements.GroupElements.HeadingGroup.Labeled;
using ACEPackage.Editor.Scripts.Elements.GroupElements.HeadingGroup.Toggle;
using ACEPackage.Editor.Scripts.Elements.SingleElements.Button;
using ACEPackage.Editor.Scripts.Elements.SingleElements.Button.Tab;
using ACEPackage.Editor.Scripts.Elements.SingleElements.Decorator.DividingLine;
using ACEPackage.Editor.Scripts.Elements.SingleElements.Decorator.Label;
using ACEPackage.Editor.Scripts.Elements.SingleElements.Properties.Basic;
using ACEPackage.Editor.Scripts.Elements.SingleElements.Properties.MinMaxSlider;
using ACEPackage.Runtime.Scripts.SettingsCustom;
using ACEPackage.Runtime.Scripts.SettingsCustom.Groups;
using ACEPackage.Runtime.Scripts.SettingsCustom.SingleElements;
using JetBrains.Annotations;
using UnityEngine;

namespace ACEPackage.Editor.Scripts.ElementBuilding
{
    public static class AceElementBuilder
    {
#region SingleElement
        /// <summary>
        ///     Get basic single element.
        /// </summary>
        public static Element GetElement( 
            string varName, 
            string title, 
            string tooltip = "" )
        {
            return new BasicProperty( varName, new GUIContent( new GUIContent( title, tooltip ) ), new SingleCustomSettings(), null );
        }
        
        /// <summary>
        ///     Get basic single element.
        /// </summary>
        public static Element GetElement( 
            string varName, 
            [CanBeNull] SingleCustomSettings settings )
        {
            return new BasicProperty( varName, GUIContent.none, settings, null );
        }

        /// <summary>
        ///     Get basic single element using both settings and conditions.
        /// </summary>
        public static Element GetElement( 
            string varName, 
            string title, 
            string tooltip, 
            [CanBeNull] SingleCustomSettings settings, 
            bool hideOnDisable = false, 
            params ElementCondition[] conditions )
        {
            return new BasicProperty( varName, new GUIContent( new GUIContent( title, tooltip ) ), settings ?? new SingleCustomSettings(), null, hideOnDisable, conditions );
        }
        
#endregion


#region LabelElements
        
        /// <summary>
        ///     Get label element.
        /// </summary>
        public static Element GetLabelElement( 
            string title )
        {
            return new LabelElement( new GUIContent( title ) );
        }
        
        /// <summary>
        ///     Get label element.
        /// </summary>
        public static Element GetLabelElement( 
            string title, string tooltip )
        {
            return new LabelElement( new GUIContent( new GUIContent( title, tooltip ) ) );
        }
        
        /// <summary>
        ///     Get label element.
        /// </summary>
        public static Element GetLabelElement( 
            string title, 
            [CanBeNull] SingleCustomSettings settings )
        {
            return new LabelElement( new GUIContent( title ), false, settings );
        }
        
        /// <summary>
        ///     Get label element.
        /// </summary>
        public static Element GetLabelElement( 
            string title, string tooltip, 
            [CanBeNull] SingleCustomSettings settings )
        {
            return new LabelElement( new GUIContent( new GUIContent( title, tooltip ) ), false, settings );
        }
        
#endregion


#region MinMaxSliderElements
        
        // /// <summary>
        // ///     Get min/max element.
        // /// </summary>
        // public static Element GetMinMaxSliderElement(
        //     string title, string tooltip,
        //     string minVarName, string maxVarName, float minLimit, float maxLimit )
        // {
        //     return new MinMaxSliderElement( 
        //         new GUIContent( new GUIContent( title, tooltip ) ), 
        //         minVarName, 
        //         maxVarName, 
        //         minLimit, 
        //         maxLimit,
        //         new SingleCustomSettings(), 
        //         null 
        //     );
        // }
        
        /// <summary>
        ///     Get min/max element.
        /// </summary>
        public static Element GetMinMaxSliderElement(
            string title, 
            string tooltip,
            string minVarName, 
            string maxVarName, 
            float minLimit, 
            float maxLimit,
            SingleCustomSettings settings = null,
            Action changeCallBack = null, 
            
            bool hideOnDisable = false, 
            params ElementCondition[] conditions )
        {
            return new MinMaxSliderElement( 
                new GUIContent( new GUIContent( title, tooltip ) ), 
                minVarName, 
                maxVarName, 
                minLimit, 
                maxLimit,
                settings, 
                changeCallBack,
                hideOnDisable,
                conditions
            );
        }
        
#endregion


#region DecoratorElements
        
        /// <summary>
        ///     Get divider element.
        /// </summary>
        public static Element GetDividerElement()
        {
            return new DividingLineElement();
        }
        
#endregion

        

        
#region GroupWithFoldoutHeading
        
        /// <summary>
        ///     Get a foldout group using a toggle, groups settings, and a variable list of elements.
        /// </summary>
        public static Element GetGroupWithFoldoutHeading( 
            [CanBeNull] string toggleVarName, 
            string title, 
            string tooltip, 
            [CanBeNull] GroupCustomSettings customSettings, 
            params Element[] elements )
        {
            return GetGroupWithFoldoutHeading(
                toggleVarName, title, tooltip, customSettings, elements, new Element[]{} );
        }
        
        /// <summary>
        ///     Get a foldout group using a toggle, groups settings, a list of elements, and a variable list of elements.
        /// </summary>
        public static Element GetGroupWithFoldoutHeading( 
            [CanBeNull] string toggleVarName, 
            string title, 
            string tooltip, 
            [CanBeNull] GroupCustomSettings customSettings, 
            Element[] elementList,
            params Element[] elementsSingles )
        {
            Element[] elements = new Element[elementList.Length + elementsSingles.Length];
            int index = 0;
            foreach ( Element element in elementList )
            {
                elements[index++] = element;
            }

            foreach ( Element element in elementsSingles )
            {
                elements[index++] = element;
            }
            
            return new FoldoutGroup( toggleVarName, new GUIContent( new GUIContent( title, tooltip ) ), customSettings, elements );
        }
        
        /// <summary>
        ///     Get a foldout group using a toggle, groups settings, a single element, and a variable list of elements.
        /// </summary>
        public static Element GetGroupWithFoldoutHeading( 
            [CanBeNull] string toggleVarName, 
            string title, 
            string tooltip, 
            [CanBeNull] GroupCustomSettings customSettings, 
            Element singleElement,
            params Element[] elementList )
        {
            Element[] elements = new Element[elementList.Length + 1];
            int index = 0;
            elements[index++] = singleElement;
            foreach ( Element element in elementList )
            {
                elements[index++] = element;
            }

            return new FoldoutGroup( toggleVarName, new GUIContent( new GUIContent( title, tooltip ) ), customSettings, elements );
        }
        
        /// <summary>
        ///     Get a foldout group using a toggle, groups settings, a list of elements, and a single element.
        /// </summary>
        public static Element GetGroupWithFoldoutHeading( 
            [CanBeNull] string toggleVarName, 
            string title, 
            string tooltip, 
            [CanBeNull] GroupCustomSettings customSettings, 
            Element[] elementList,
            Element singleElement )
        {
            Element[] elements = new Element[elementList.Length + 1];
            
            int index = 0;
            foreach ( Element element in elementList )
            {
                elements[index++] = element;
            }
            
            elements[index] = singleElement;

            return new FoldoutGroup( toggleVarName, new GUIContent( title, tooltip ), customSettings, elements );
        }

#endregion


#region GroupWithToggleHeading
        
        /// <summary>
        ///     Get a foldout group using a toggle, groups settings, and a variable list of elements.
        /// </summary>
        public static Element GetGroupWithToggleHeading( 
            [CanBeNull] string toggleVarName, 
            string title, 
            string tooltip, 
            [CanBeNull] GroupCustomSettings customSettings, 
            bool hideOnDisable,
            params Element[] elements )
        {
            return new ToggleGroup( toggleVarName, new GUIContent( title, tooltip ), customSettings, hideOnDisable, elements );
        }

#endregion


#region GroupWithLabelHeading

        /// <summary>
        ///     Get a foldout group using a toggle, groups settings, and a variable list of elements.
        /// </summary>
        public static Element GetGroupWithLabelHeading( 
            string title, 
            string tooltip, 
            [CanBeNull] GroupCustomSettings customSettings, 
            params Element[] elements )
        {
            return new LabeledGroup( new GUIContent( title, tooltip ), customSettings, elements );
        }

#endregion


#region GroupsWithoutHeadings

#region BasicGroups
        
        /// <summary>
        ///     Get a group using groups settings, and a variable list of elements.
        /// </summary>
        public static Element GetGroup(
            [CanBeNull] GroupCustomSettings customSettings, 
            params Element[] elements )
        {
            return new BasicGroup( customSettings, elements );
        }

#endregion


#region CompositeGroups

        /// <summary>
        ///  Get a group using groups settings, and a variable list of elements.
        /// This group will be placed on the same settings level as it's parent.
        /// </summary>
        public static Element GetCompositeGroup(
            [CanBeNull] GroupCustomSettings customSettings, 
            params Element[] elements )
        {
            return new CompositeGroup( customSettings, elements );
        }

#endregion
        
#endregion

        
#region TabbedSections

        public static Element GetTabbedOptionsSection( params ( string name, string tooltip, bool focused, Action callback, Element section )[] buttonInfoList )
        {
            ButtonElement[] newButtons = new ButtonElement[buttonInfoList.Length];
            Element[] options = new Element[buttonInfoList.Length];
            for (int i = 0; i < newButtons.Length; i++)
            {
                (string name, string tooltip, bool focused, Action callback, Element section) currentInfo = buttonInfoList[i];
                newButtons[i] = new TabButtonElement( new GUIContent( currentInfo.name, currentInfo.tooltip ), currentInfo.focused, new SingleCustomSettings(), currentInfo.callback );
                options[i] = currentInfo.section;
            }

            return GetTabbedSection( newButtons, options );
        }

        private static Element GetTabbedSection( ButtonElement[] buttons, Element[] optionGroups )
        {
            // The length of the buttons array and the options array must be the same.
            if (buttons.Length != optionGroups.Length)
            {
                Debug.LogWarning( "HGSS|GTS: Error! The number of buttons must be the same as the number of options in a tabbed section!" );
                return GetLabelElement( "Array length mis-match between Button & Options!" );
            }

            // Determine which button is currently active, this is the index of the options array that should shown.
            int activeIndex = 0;
            for (int i = 0; i < buttons.Length; i++)
            {
                if (buttons[i].Focused)
                {
                    activeIndex = i;
                    break;
                }
            }

            Element buttonsGroup = GetGroup( new GroupCustomSettings()
                {
                    ForceSingleLine = true, 
                    NumberOfColumns = buttons.Length,
                    // CustomFrameSettings = new CustomFrameSettings()
                    // {
                    //     frameType = ElementFrameType.BottomOnly,
                    // }
                    // HeadingBoxOutlineThickness = 5f
                }, 
                buttons
            );
            
            // One element for the buttons group and one for the selected option group.
            Element[] selectSectionFinishedList = new Element[2];
            selectSectionFinishedList[0] = buttonsGroup;
            selectSectionFinishedList[1] = optionGroups[activeIndex];


            // Return the group with the buttons and active options section.
            return GetGroup( new GroupCustomSettings()
                {
                    ForceSingleLine = true, 
                    NumberOfColumns = buttons.Length, 
                    CustomFrameSettings = new CustomFrameSettings() {applyFraming = false}
                },
                selectSectionFinishedList
            );
        }
        
        public static Element GetTabbedOptionsSectionDisabled( Element activeSection, params ( string name, string tooltip, bool focused, Action callback )[] buttonInfoList )
        {
            TabButtonElement[] newButtons = new TabButtonElement[buttonInfoList.Length];
            for (int i = 0; i < newButtons.Length; i++)
            {
                (string name, string tooltip, bool focused, Action callback) currentInfo = buttonInfoList[i];
                newButtons[i] = new TabButtonElement( new GUIContent( currentInfo.name, currentInfo.tooltip ), currentInfo.focused,
                    new SingleCustomSettings()
                    {
                        ForceDisable = true
                    }, 
                    currentInfo.callback
                );
            }

            return GetTabbedSection( activeSection, newButtons );
        }
        
        public static Element GetTabbedOptionsSection( Element activeSection, params ( string name, string tooltip, bool focused, Action callback )[] buttonInfoList )
        {
            TabButtonElement[] newButtons = new TabButtonElement[buttonInfoList.Length];
            for (int i = 0; i < newButtons.Length; i++)
            {
                var currentInfo = buttonInfoList[i];
                newButtons[i] = new TabButtonElement( new GUIContent( currentInfo.name, currentInfo.tooltip ), currentInfo.focused, new SingleCustomSettings(), currentInfo.callback );
            }

            return GetTabbedSection( activeSection, newButtons );
        }
        
        private static Element GetTabbedSection( Element activeSection, TabButtonElement[] buttons )
        {
            Element buttonsGroup = GetGroup( new GroupCustomSettings()
                {
                    ForceSingleLine = true, 
                    NumberOfColumns = buttons.Length,
                    LeftPadding = 4f,
                    RightPadding = 4f,
                    BottomPadding = 2f
                }, 
                buttons
            );

            // Return the group with the buttons and active options section.
            return GetGroup( new GroupCustomSettings()
                {
                    ForceSingleLine = true, 
                    NumberOfColumns = buttons.Length, 
                    CustomFrameSettings = new CustomFrameSettings() {applyFraming = false}
                },
                buttonsGroup,
                activeSection
            );
        }
        
#endregion
        
        
        
        
        
        
        
//         #region GroupWithFoldoutHeading
//         
//         /// <summary>
//         ///     Get a foldout group using a toggle, groups settings, and a variable list of elements.
//         /// </summary>
//         public static Element GetGroupWithFoldoutHeading( 
//             [CanBeNull] string toggleVarName, 
//             string title, 
//             string tooltip, 
//             [CanBeNull] GroupCustomSettings customSettings, 
//             params Element[] elements )
//         {
//             return GetGroupWithFoldoutHeading(
//                 toggleVarName, title, tooltip, customSettings, elements, new Element[]{} );
//         }
//         
//         /// <summary>
//         ///     Get a foldout group using a toggle, groups settings, a list of elements, and a variable list of elements.
//         /// </summary>
//         public static Element GetGroupWithFoldoutHeading( 
//             [CanBeNull] string toggleVarName, 
//             string title, 
//             string tooltip, 
//             [CanBeNull] GroupCustomSettings customSettings, 
//             Element[] elementList,
//             params Element[] elementsSingles )
//         {
//             Element[] elements = new Element[elementList.Length + elementsSingles.Length];
//             int index = 0;
//             foreach ( Element element in elementList )
//             {
//                 elements[index++] = element;
//             }
//
//             foreach ( Element element in elementsSingles )
//             {
//                 elements[index++] = element;
//             }
//             
//             return new FoldoutGroup( toggleVarName, new GUIContent( new GUIContent( title, tooltip ) ), customSettings, elements );
//         }
//         
//         /// <summary>
//         ///     Get a foldout group using a toggle, groups settings, a single element, and a variable list of elements.
//         /// </summary>
//         public static Element GetGroupWithFoldoutHeading( 
//             [CanBeNull] string toggleVarName, 
//             string title, 
//             string tooltip, 
//             [CanBeNull] GroupCustomSettings customSettings, 
//             Element singleElement,
//             params Element[] elementList )
//         {
//             Element[] elements = new Element[elementList.Length + 1];
//             int index = 0;
//             elements[index++] = singleElement;
//             foreach ( Element element in elementList )
//             {
//                 elements[index++] = element;
//             }
//
//             return new FoldoutGroup( toggleVarName, new GUIContent( new GUIContent( title, tooltip ) ), customSettings, elements );
//         }
//         
//         /// <summary>
//         ///     Get a foldout group using a toggle, groups settings, a list of elements, and a single element.
//         /// </summary>
//         public static Element GetGroupWithFoldoutHeading( 
//             [CanBeNull] string toggleVarName, 
//             string title, 
//             string tooltip, 
//             [CanBeNull] GroupCustomSettings customSettings, 
//             Element[] elementList,
//             Element singleElement )
//         {
//             Element[] elements = new Element[elementList.Length + 1];
//             
//             int index = 0;
//             foreach ( Element element in elementList )
//             {
//                 elements[index++] = element;
//             }
//             
//             elements[index] = singleElement;
//
//             return new FoldoutGroup( toggleVarName, new GUIContent( title, tooltip ), customSettings, elements );
//         }
//         
//         // /// <summary>
//         // ///     Get a foldout group using a toggle, group settings, and a variable list of tuples.
//         // /// </summary>
//         // public static Element GetGroupWithFoldoutHeading
//         //     ( 
//         //         [CanBeNull] string toggleVarName, 
//         //         string title, 
//         //         string tooltip, 
//         //         [CanBeNull] GroupCustomSettings customSettings,
//         //         params 
//         //             (
//         //                 string varName, 
//         //                 string title, 
//         //                 string tooltip
//         //             )
//         //             [] elementTuples 
//         //     )
//         // {
//         //     return new FoldoutGroup( toggleVarName, new GUIContent( title, tooltip ), customSettings, ListOperations.GetElementListFromTuples( elementTuples ));
//         // }
//
//         // /// <summary>
//         // ///     Get a foldout group using a toggle, group settings and variable list of tuples with settings.
//         // /// </summary>
//         // public static Element GetGroupWithFoldoutHeading
//         // ( 
//         //     [CanBeNull] string toggleVarName, 
//         //     string title, 
//         //     string tooltip, 
//         //     [CanBeNull] GroupCustomSettings customSettings,
//         //     params 
//         //         (
//         //         string varName, 
//         //         string title, 
//         //         string tooltip, 
//         //         SingleCustomSettings settings 
//         //         )
//         //         [] elementTuples 
//         // )
//         // {
//         //     return new FoldoutGroup( toggleVarName, new GUIContent( title, tooltip ), customSettings, ListOperations.GetElementListFromTuples( elementTuples ) );
//         // }
//         //
//         // /// <summary>
//         // ///     Get a foldout group using a toggle, group settings and variable list of tuples with conditions.
//         // /// </summary>
//         // public static Element GetGroupWithFoldoutHeading
//         //     ( 
//         //         [CanBeNull] string toggleVarName, 
//         //         string title, 
//         //         string tooltip, 
//         //         [CanBeNull] GroupCustomSettings customSettings, 
//         //         params 
//         //             (
//         //                 string varName, 
//         //                 string title, 
//         //                 string tooltip, 
//         //                 ElementCondition[] filters 
//         //             )
//         //             [] elementTuples 
//         //     )
//         // {
//         //     return new FoldoutGroup( toggleVarName, new GUIContent( title, tooltip ), customSettings, ListOperations.GetElementListFromTuples( elementTuples ) );
//         // }
//         //
//         // /// <summary>
//         // ///     Get a foldout group using a toggle, group settings and variable list of tuples with both settings and conditions.
//         // /// </summary>
//         // public static Element GetGroupWithFoldoutHeading
//         //     ( 
//         //         [CanBeNull] string toggleVarName, 
//         //         string title, 
//         //         string tooltip, 
//         //         [CanBeNull] GroupCustomSettings customSettings,
//         //         params 
//         //             (
//         //                 string varName, 
//         //                 string title, 
//         //                 string tooltip, 
//         //                 SingleCustomSettings settings,
//         //                 ElementCondition[] filters
//         //             )
//         //             [] elementTuples 
//         //     )
//         // {
//         //     return new FoldoutGroup( toggleVarName, new GUIContent( title, tooltip ), customSettings, ListOperations.GetElementListFromTuples( elementTuples ) );
//         // }
//         //
//         // /// <summary>
//         // ///     Get a foldout group using group settings, list of tuples and a variable list of elements.
//         // /// </summary>
//         // public static Element GetGroupWithFoldoutHeading
//         // ( 
//         //     [CanBeNull] string toggleVarName, 
//         //     string title, 
//         //     string tooltip, 
//         //     [CanBeNull] GroupCustomSettings customSettings, 
//         //     (
//         //         string varName, 
//         //         string title, 
//         //         string tooltip
//         //     )
//         //     [] elementTuples, 
//         //     params Element[] elements )    
//         // {
//         //     return new FoldoutGroup( toggleVarName, new GUIContent( title, tooltip ), customSettings, ListOperations.GetCombinedElementAndTuplesList( elementTuples, elements ) );
//         // }
//         //
//         // /// <summary>
//         // ///     Get a foldout group using group settings, list of tuples with settings, and a variable list of elements.
//         // /// </summary>
//         // public static Element GetGroupWithFoldoutHeading
//         // ( 
//         //     [CanBeNull] string toggleVarName, 
//         //     string title, 
//         //     string tooltip, 
//         //     [CanBeNull] GroupCustomSettings customSettings,
//         //         (
//         //         string varName, 
//         //         string title, 
//         //         string tooltip, 
//         //         SingleCustomSettings settings 
//         //         )
//         //         [] elementTuples, 
//         //     params Element[] elements 
//         // )
//         // {
//         //     return new FoldoutGroup( toggleVarName, new GUIContent( title, tooltip ), customSettings, ListOperations.GetCombinedElementAndTuplesList( elementTuples, elements ) );
//         // }
//         //
//         // /// <summary>
//         // ///     Get a foldout group using group settings, list of tuples with conditions, and a variable list of elements.
//         // /// </summary>
//         // public static Element GetGroupWithFoldoutHeading
//         // ( 
//         //     [CanBeNull] string toggleVarName, 
//         //     string title, 
//         //     string tooltip, 
//         //     [CanBeNull] GroupCustomSettings customSettings, 
//         //     (
//         //         string varName, 
//         //         string title, 
//         //         string tooltip, 
//         //         ElementCondition[] filters 
//         //         )
//         //         [] elementTuples, 
//         //     params Element[] elements )    
//         // {
//         //     return new FoldoutGroup( toggleVarName, new GUIContent( title, tooltip ), customSettings, ListOperations.GetCombinedElementAndTuplesList( elementTuples, elements ) );
//         // }
//         //
//         // /// <summary>
//         // ///     Get a foldout group using group settings, list of tuples with both settings and conditions, and a variable list of elements.
//         // /// </summary>
//         // public static Element GetGroupWithFoldoutHeading
//         // ( 
//         //     [CanBeNull] string toggleVarName, 
//         //     string title, 
//         //     string tooltip, 
//         //     [CanBeNull] GroupCustomSettings customSettings,
//         //         (
//         //         string varName, 
//         //         string title, 
//         //         string tooltip, 
//         //         SingleCustomSettings settings,
//         //         ElementCondition[] filters
//         //         )
//         //         [] elementTuples, 
//         //     params Element[] elements 
//         // )
//         // {
//         //     return new FoldoutGroup( toggleVarName, new GUIContent( title, tooltip ), customSettings, ListOperations.GetCombinedElementAndTuplesList( elementTuples, elements ) );
//         // }
//
// #endregion
//
//
// #region GroupWithToggleHeading
//         
//         /// <summary>
//         ///     Get a foldout group using a toggle, groups settings, and a variable list of elements.
//         /// </summary>
//         public static Element GetGroupWithToggleHeading( 
//             [CanBeNull] string toggleVarName, 
//             string title, 
//             string tooltip, 
//             [CanBeNull] GroupCustomSettings customSettings, 
//             bool hideOnDisable,
//             params Element[] elements )
//         {
//             return new ToggleGroup( toggleVarName, new GUIContent( title, tooltip ), customSettings, hideOnDisable, elements );
//         }
//         
//
//         // /// <summary>
//         // ///     Get a foldout group using a toggle, group settings, and a variable list of tuples.
//         // /// </summary>
//         // public static Element GetGroupWithToggleHeading
//         //     ( 
//         //         [CanBeNull] string toggleVarName, 
//         //         string title, 
//         //         string tooltip, 
//         //         [CanBeNull] GroupCustomSettings customSettings,
//         //         bool hideOnDisable,
//         //         params 
//         //             (
//         //                 string varName, 
//         //                 string title, 
//         //                 string tooltip
//         //             )
//         //             [] elementTuples 
//         //     )
//         // {
//         //     return new ToggleGroup( toggleVarName, new GUIContent( title, tooltip ), customSettings, hideOnDisable, ListOperations.GetElementListFromTuples( elementTuples ));
//         // }
//         //
//         // /// <summary>
//         // ///     Get a foldout group using a toggle, group settings and variable list of tuples with settings.
//         // /// </summary>
//         // public static Element GetGroupWithToggleHeading
//         // ( 
//         //     [CanBeNull] string toggleVarName, 
//         //     string title, 
//         //     string tooltip, 
//         //     [CanBeNull] GroupCustomSettings customSettings,
//         //     bool hideOnDisable,
//         //     params 
//         //         (
//         //         string varName, 
//         //         string title, 
//         //         string tooltip, 
//         //         SingleCustomSettings settings 
//         //         )
//         //         [] elementTuples 
//         // )
//         // {
//         //     return new ToggleGroup( toggleVarName, new GUIContent( title, tooltip ), customSettings, hideOnDisable, ListOperations.GetElementListFromTuples( elementTuples ) );
//         // }
//         //
//         // /// <summary>
//         // ///     Get a foldout group using a toggle, group settings and variable list of tuples with conditions.
//         // /// </summary>
//         // public static Element GetGroupWithToggleHeading
//         //     ( 
//         //         [CanBeNull] string toggleVarName, 
//         //         string title, 
//         //         string tooltip, 
//         //         [CanBeNull] GroupCustomSettings customSettings, 
//         //         bool hideOnDisable,
//         //         params 
//         //             (
//         //                 string varName, 
//         //                 string title, 
//         //                 string tooltip, 
//         //                 ElementCondition[] filters 
//         //             )
//         //             [] elementTuples 
//         //     )
//         // {
//         //     return new ToggleGroup( toggleVarName, new GUIContent( title, tooltip ), customSettings, hideOnDisable, ListOperations.GetElementListFromTuples( elementTuples ) );
//         // }
//         //
//         // /// <summary>
//         // ///     Get a foldout group using a toggle, group settings and variable list of tuples with both settings and conditions.
//         // /// </summary>
//         // public static Element GetGroupWithToggleHeading
//         //     ( 
//         //         [CanBeNull] string toggleVarName, 
//         //         string title, 
//         //         string tooltip, 
//         //         [CanBeNull] GroupCustomSettings customSettings,
//         //         bool hideOnDisable,
//         //         params 
//         //             (
//         //                 string varName, 
//         //                 string title, 
//         //                 string tooltip, 
//         //                 SingleCustomSettings settings,
//         //                 ElementCondition[] filters
//         //             )
//         //             [] elementTuples 
//         //     )
//         // {
//         //     return new ToggleGroup( toggleVarName, new GUIContent( title, tooltip ), customSettings, hideOnDisable, ListOperations.GetElementListFromTuples( elementTuples ) );
//         // }
//         //
//         //
//         // /// <summary>
//         // ///     Get a foldout group using group settings, list of tuples and a variable list of elements.
//         // /// </summary>
//         // public static Element GetGroupWithToggleHeading
//         // ( 
//         //     [CanBeNull] string toggleVarName, 
//         //     string title, 
//         //     string tooltip, 
//         //     [CanBeNull] GroupCustomSettings customSettings, 
//         //     bool hideOnDisable,
//         //     (
//         //         string varName, 
//         //         string title, 
//         //         string tooltip
//         //     )
//         //     [] elementTuples, 
//         //     params Element[] elements )    
//         // {
//         //     return new ToggleGroup( toggleVarName, new GUIContent( title, tooltip ), customSettings, hideOnDisable, ListOperations.GetCombinedElementAndTuplesList( elementTuples, elements ) );
//         // }
//         //
//         // /// <summary>
//         // ///     Get a foldout group using group settings, list of tuples with settings, and a variable list of elements.
//         // /// </summary>
//         // public static Element GetGroupWithToggleHeading
//         // ( 
//         //     [CanBeNull] string toggleVarName, 
//         //     string title, 
//         //     string tooltip, 
//         //     [CanBeNull] GroupCustomSettings customSettings,
//         //     bool hideOnDisable,
//         //         (
//         //         string varName, 
//         //         string title, 
//         //         string tooltip, 
//         //         SingleCustomSettings settings 
//         //         )
//         //         [] elementTuples, 
//         //     params Element[] elements 
//         // )
//         // {
//         //     return new ToggleGroup( toggleVarName, new GUIContent( title, tooltip ), customSettings, hideOnDisable, ListOperations.GetCombinedElementAndTuplesList( elementTuples, elements ) );
//         // }
//         //
//         // /// <summary>
//         // ///     Get a foldout group using group settings, list of tuples with conditions, and a variable list of elements.
//         // /// </summary>
//         // public static Element GetGroupWithToggleHeading
//         // ( 
//         //     [CanBeNull] string toggleVarName, 
//         //     string title, 
//         //     string tooltip, 
//         //     [CanBeNull] GroupCustomSettings customSettings, 
//         //     bool hideOnDisable,
//         //     (
//         //         string varName, 
//         //         string title, 
//         //         string tooltip, 
//         //         ElementCondition[] filters 
//         //         )
//         //         [] elementTuples, 
//         //     params Element[] elements )    
//         // {
//         //     return new ToggleGroup( toggleVarName, new GUIContent( title, tooltip ), customSettings, hideOnDisable, ListOperations.GetCombinedElementAndTuplesList( elementTuples, elements ) );
//         // }
//         //
//         // /// <summary>
//         // ///     Get a foldout group using group settings, list of tuples with both settings and conditions, and a variable list of elements.
//         // /// </summary>
//         // public static Element GetGroupWithToggleHeading
//         // ( 
//         //     [CanBeNull] string toggleVarName, 
//         //     string title, 
//         //     string tooltip, 
//         //     [CanBeNull] GroupCustomSettings customSettings,
//         //     bool hideOnDisable,
//         //         (
//         //         string varName, 
//         //         string title, 
//         //         string tooltip, 
//         //         SingleCustomSettings settings,
//         //         ElementCondition[] filters
//         //         )
//         //         [] elementTuples, 
//         //     params Element[] elements 
//         // )
//         // {
//         //     return new ToggleGroup( toggleVarName, new GUIContent( title, tooltip ), customSettings, hideOnDisable, ListOperations.GetCombinedElementAndTuplesList( elementTuples, elements ) );
//         // }
//
// #endregion
//
//
// #region GroupWithLabelHeading
//
//         /// <summary>
//         ///     Get a foldout group using a toggle, groups settings, and a variable list of elements.
//         /// </summary>
//         public static Element GetGroupWithLabelHeading( 
//             string title, 
//             string tooltip, 
//             [CanBeNull] GroupCustomSettings customSettings, 
//             params Element[] elements )
//         {
//             return new LabeledGroup( new GUIContent( title, tooltip ), customSettings, elements );
//         }
//         
//
//         /// <summary>
//         ///     Get a foldout group using a toggle, group settings, and a variable list of tuples.
//         /// </summary>
//         public static Element GetGroupWithLabelHeading
//             ( 
//                 string title, 
//                 string tooltip, 
//                 [CanBeNull] GroupCustomSettings customSettings,
//                 params 
//                     (
//                         string varName, 
//                         string title, 
//                         string tooltip
//                     )
//                     [] elementTuples 
//             )
//         {
//             return new LabeledGroup( new GUIContent( title, tooltip ), customSettings, ListOperations.GetElementListFromTuples( elementTuples ));
//         }
//         
//         /// <summary>
//         ///     Get a foldout group using a toggle, group settings, and a variable list of tuples.
//         /// </summary>
//         public static Element GetGroupWithLabelHeading
//         ( 
//             string title, 
//             string tooltip, 
//             [CanBeNull] GroupCustomSettings customSettings,
//             Action callback,
//             params 
//                 (
//                 string varName, 
//                 string title, 
//                 string tooltip
//                 )
//                 [] elementTuples 
//         )
//         {
//             return new LabeledGroup( new GUIContent( title, tooltip ), customSettings, ListOperations.GetElementListFromTuples( elementTuples, callback ));
//         }
//
//         /// <summary>
//         ///     Get a foldout group using a toggle, group settings and variable list of tuples with settings.
//         /// </summary>
//         public static Element GetGroupWithLabelHeading
//         ( 
//             string title, 
//             string tooltip, 
//             [CanBeNull] GroupCustomSettings customSettings,
//             params 
//                 (
//                 string varName, 
//                 string title, 
//                 string tooltip, 
//                 SingleCustomSettings settings 
//                 )
//                 [] elementTuples 
//         )
//         {
//             return new LabeledGroup( new GUIContent( title, tooltip ), customSettings, ListOperations.GetElementListFromTuples( elementTuples ) );
//         }
//         
//         /// <summary>
//         ///     Get a foldout group using a toggle, group settings and variable list of tuples with conditions.
//         /// </summary>
//         public static Element GetGroupWithLabelHeading
//             ( 
//                 string title, 
//                 string tooltip, 
//                 [CanBeNull] GroupCustomSettings customSettings, 
//                 params 
//                     (
//                         string varName, 
//                         string title, 
//                         string tooltip, 
//                         ElementCondition[] filters 
//                     )
//                     [] elementTuples 
//             )
//         {
//             return new LabeledGroup( new GUIContent( title, tooltip ), customSettings, ListOperations.GetElementListFromTuples( elementTuples ) );
//         }
//         
//         /// <summary>
//         ///     Get a foldout group using a toggle, group settings and variable list of tuples with both settings and conditions.
//         /// </summary>
//         public static Element GetGroupWithLabelHeading
//             ( 
//                 string title, 
//                 string tooltip, 
//                 [CanBeNull] GroupCustomSettings customSettings,
//                 params 
//                     (
//                         string varName, 
//                         string title, 
//                         string tooltip, 
//                         SingleCustomSettings settings,
//                         ElementCondition[] filters
//                     )
//                     [] elementTuples 
//             )
//         {
//             return new LabeledGroup( new GUIContent( title, tooltip ), customSettings, ListOperations.GetElementListFromTuples( elementTuples ) );
//         }
//         
//         /// <summary>
//         ///     Get a foldout group using group settings, list of tuples and a variable list of elements.
//         /// </summary>
//         public static Element GetGroupWithLabelHeading
//         ( 
//             string title, 
//             string tooltip, 
//             [CanBeNull] GroupCustomSettings customSettings, 
//             (
//                 string varName, 
//                 string title, 
//                 string tooltip
//             )
//             [] elementTuples, 
//             params Element[] elements )    
//         {
//             return new LabeledGroup( new GUIContent( title, tooltip ), customSettings, ListOperations.GetCombinedElementAndTuplesList( elementTuples, elements ) );
//         }
//
//         /// <summary>
//         ///     Get a foldout group using group settings, list of tuples with settings, and a variable list of elements.
//         /// </summary>
//         public static Element GetGroupWithLabelHeading
//         ( 
//             string title, 
//             string tooltip, 
//             [CanBeNull] GroupCustomSettings customSettings,
//                 (
//                 string varName, 
//                 string title, 
//                 string tooltip, 
//                 SingleCustomSettings settings 
//                 )
//                 [] elementTuples, 
//             params Element[] elements 
//         )
//         {
//             return new LabeledGroup( new GUIContent( title, tooltip ), customSettings, ListOperations.GetCombinedElementAndTuplesList( elementTuples, elements ) );
//         }
//         
//         /// <summary>
//         ///     Get a foldout group using group settings, list of tuples with conditions, and a variable list of elements.
//         /// </summary>
//         public static Element GetGroupWithLabelHeading
//         ( 
//             string title, 
//             string tooltip, 
//             [CanBeNull] GroupCustomSettings customSettings, 
//             (
//                 string varName, 
//                 string title, 
//                 string tooltip, 
//                 ElementCondition[] filters 
//                 )
//                 [] elementTuples, 
//             params Element[] elements )    
//         {
//             return new LabeledGroup( new GUIContent( title, tooltip ), customSettings, ListOperations.GetCombinedElementAndTuplesList( elementTuples, elements ) );
//         }
//         
//         /// <summary>
//         ///     Get a foldout group using group settings, list of tuples with both settings and conditions, and a variable list of elements.
//         /// </summary>
//         public static Element GetGroupWithLabelHeading
//         ( 
//             string title, 
//             string tooltip, 
//             [CanBeNull] GroupCustomSettings customSettings,
//                 (
//                 string varName, 
//                 string title, 
//                 string tooltip, 
//                 SingleCustomSettings settings,
//                 ElementCondition[] filters
//                 )
//                 [] elementTuples, 
//             params Element[] elements 
//         )
//         {
//             return new LabeledGroup( new GUIContent( title, tooltip ), customSettings, ListOperations.GetCombinedElementAndTuplesList( elementTuples, elements ) );
//         }
//         
// #endregion
//
//
// #region GroupsWithoutHeadings
//
// #region BasicGroups
//         
//         /// <summary>
//         ///     Get a group using groups settings, and a variable list of elements.
//         /// </summary>
//         public static Element GetGroup(
//             [CanBeNull] GroupCustomSettings customSettings, 
//             params Element[] elements )
//         {
//             return new BasicGroup( customSettings, elements );
//         }
//         
//         /// <summary>
//         ///     Get a group using group settings, and a variable list of tuples.
//         /// </summary>
//         public static Element GetGroup
//             (
//                 [CanBeNull] GroupCustomSettings customSettings,
//                 params 
//                     (
//                         string varName, 
//                         string title, 
//                         string tooltip
//                     )
//                     [] elementTuples 
//             )
//         {
//             return new BasicGroup( customSettings, ListOperations.GetElementListFromTuples( elementTuples ) );
//         }
//
//         /// <summary>
//         ///     Get a group using group settings and variable list of tuples with settings.
//         /// </summary>
//         public static Element GetGroup
//             (
//                 [CanBeNull] GroupCustomSettings customSettings,
//                 params 
//                     (
//                     string varName, 
//                     string title, 
//                     string tooltip, 
//                     SingleCustomSettings settings 
//                     )
//                     [] elementTuples 
//             )
//         {
//             return new BasicGroup( customSettings, ListOperations.GetElementListFromTuples( elementTuples ) );
//         }
//         
//         /// <summary>
//         ///     Get a group using group settings and variable list of tuples with conditions.
//         /// </summary>
//         public static Element GetGroup
//             (
//                 [CanBeNull] GroupCustomSettings customSettings, 
//                 params 
//                     (
//                         string varName, 
//                         string title, 
//                         string tooltip, 
//                         ElementCondition[] filters 
//                     )
//                     [] elementTuples 
//             )
//         {
//             return new BasicGroup( customSettings, ListOperations.GetElementListFromTuples( elementTuples ) );
//         }
//         
//         /// <summary>
//         ///     Get a group using group settings and variable list of tuples with both settings and conditions.
//         /// </summary>
//         public static Element GetGroup
//             (
//                 [CanBeNull] GroupCustomSettings customSettings,
//                 params 
//                     (
//                         string varName, 
//                         string title, 
//                         string tooltip, 
//                         SingleCustomSettings settings,
//                         ElementCondition[] filters
//                     )
//                     [] elementTuples 
//             )
//         {
//             return new BasicGroup( customSettings, ListOperations.GetElementListFromTuples( elementTuples ) );
//         }
//         
//         /// <summary>
//         ///     Get a group using group settings, list of tuples and a variable list of elements.
//         /// </summary>
//         public static Element GetGroup
//         (
//             [CanBeNull] GroupCustomSettings customSettings, 
//             (
//                 string varName, 
//                 string title, 
//                 string tooltip
//             )
//             [] elementTuples, 
//             params Element[] elements )    
//         {
//             return new BasicGroup( customSettings, ListOperations.GetCombinedElementAndTuplesList( elementTuples, elements ) );
//         }
//
//         /// <summary>
//         ///     Get a group using group settings, list of tuples with settings, and a variable list of elements.
//         /// </summary>
//         public static Element GetGroup
//         (
//             [CanBeNull] GroupCustomSettings customSettings,
//                 (
//                 string varName, 
//                 string title, 
//                 string tooltip, 
//                 SingleCustomSettings settings 
//                 )
//                 [] elementTuples, 
//             params Element[] elements 
//         )
//         {
//             return new BasicGroup( customSettings, ListOperations.GetCombinedElementAndTuplesList( elementTuples, elements ) );
//         }
//         
//         /// <summary>
//         ///     Get a group using group settings, list of tuples with conditions, and a variable list of elements.
//         /// </summary>
//         public static Element GetGroup
//         (
//             [CanBeNull] GroupCustomSettings customSettings, 
//             (
//                 string varName, 
//                 string title, 
//                 string tooltip, 
//                 ElementCondition[] filters 
//                 )
//                 [] elementTuples, 
//             params Element[] elements )    
//         {
//             return new BasicGroup( customSettings, ListOperations.GetCombinedElementAndTuplesList( elementTuples, elements ) );
//         }
//         
//         /// <summary>
//         ///     Get a group using group settings, list of tuples with both settings and conditions, and a variable list of elements.
//         /// </summary>
//         public static Element GetGroup
//         (
//             [CanBeNull] GroupCustomSettings customSettings,
//                 (
//                 string varName, 
//                 string title, 
//                 string tooltip, 
//                 SingleCustomSettings settings,
//                 ElementCondition[] filters
//                 )
//                 [] elementTuples, 
//             params Element[] elements 
//         )
//         {
//             return new BasicGroup( customSettings, ListOperations.GetCombinedElementAndTuplesList( elementTuples, elements ) );
//         }
//         
// #endregion
//
//
// #region CompositeGroups
//
//         /// <summary>
//         ///  Get a group using groups settings, and a variable list of elements.
//         /// This group will be placed on the same settings level as it's parent.
//         /// </summary>
//         public static Element GetCompositeGroup(
//             [CanBeNull] GroupCustomSettings customSettings, 
//             params Element[] elements )
//         {
//             return new CompositeGroup( customSettings, elements );
//         }
//         
//         /// <summary>
//         ///     Get a group using group settings, and a variable list of tuples.
//         /// This group will be placed on the same settings level as it's parent.
//         /// </summary>
//         public static Element GetCompositeGroup
//             (
//                 [CanBeNull] GroupCustomSettings customSettings,
//                 params 
//                     (
//                         string varName, 
//                         string title, 
//                         string tooltip
//                     )
//                     [] elementTuples 
//             )
//         {
//             return new CompositeGroup( customSettings, ListOperations.GetElementListFromTuples( elementTuples ) );
//         }
//         
//         /// <summary>
//         ///     Get a group using group settings, and a variable list of tuples.
//         /// This group will be placed on the same settings level as it's parent.
//         /// </summary>
//         public static Element GetCompositeGroup
//         (
//             [CanBeNull] GroupCustomSettings customSettings,
//             params 
//                 (
//                 string varName, 
//                 GUIContent guiContent
//                 )
//                 [] elementTuples 
//         )
//         {
//             return new CompositeGroup( customSettings, ListOperations.GetElementListFromTuples( elementTuples ) );
//         }
//
//         /// <summary>
//         ///     Get a group using group settings and variable list of tuples with settings.
//         /// This group will be placed on the same settings level as it's parent.
//         /// </summary>
//         public static Element GetCompositeGroup
//             (
//                 [CanBeNull] GroupCustomSettings customSettings,
//                 params 
//                     (
//                     string varName, 
//                     string title, 
//                     string tooltip, 
//                     SingleCustomSettings settings 
//                     )
//                     [] elementTuples 
//             )
//         {
//             return new CompositeGroup( customSettings, ListOperations.GetElementListFromTuples( elementTuples ) );
//         }
//         
//         /// <summary>
//         ///     Get a group using group settings and variable list of tuples with settings.
//         /// This group will be placed on the same settings level as it's parent.
//         /// </summary>
//         public static Element GetCompositeGroup
//         (
//             [CanBeNull] GroupCustomSettings customSettings,
//             params 
//                 (
//                 string varName, 
//                 GUIContent guiContent,
//                 SingleCustomSettings settings 
//                 )
//                 [] elementTuples 
//         )
//         {
//             return new CompositeGroup( customSettings, ListOperations.GetElementListFromTuples( elementTuples ) );
//         }
//         
//         /// <summary>
//         ///     Get a group using group settings and variable list of tuples with conditions.
//         /// This group will be placed on the same settings level as it's parent.
//         /// </summary>
//         public static Element GetCompositeGroup
//             (
//                 [CanBeNull] GroupCustomSettings customSettings, 
//                 params 
//                     (
//                         string varName, 
//                         string title, 
//                         string tooltip, 
//                         ElementCondition[] filters 
//                     )
//                     [] elementTuples 
//             )
//         {
//             return new CompositeGroup( customSettings, ListOperations.GetElementListFromTuples( elementTuples ) );
//         }
//         
//         /// <summary>
//         ///     Get a group using group settings and variable list of tuples with both settings and conditions.
//         /// This group will be placed on the same settings level as it's parent.
//         /// </summary>
//         public static Element GetCompositeGroup
//             (
//                 [CanBeNull] GroupCustomSettings customSettings,
//                 params 
//                     (
//                         string varName, 
//                         string title, 
//                         string tooltip, 
//                         SingleCustomSettings settings,
//                         ElementCondition[] filters
//                     )
//                     [] elementTuples 
//             )
//         {
//             return new CompositeGroup( customSettings, ListOperations.GetElementListFromTuples( elementTuples ) );
//         }
//
//         /// <summary>
//         ///     Get a group using group settings, list of tuples and a variable list of elements.
//         /// This group will be placed on the same settings level as it's parent.
//         /// </summary>
//         public static Element GetCompositeGroup
//         (
//             [CanBeNull] GroupCustomSettings customSettings, 
//             (
//                 string varName, 
//                 string title, 
//                 string tooltip
//             )
//             [] elementTuples, 
//             params Element[] elements )    
//         {
//             return new CompositeGroup( customSettings, ListOperations.GetCombinedElementAndTuplesList( elementTuples, elements ) );
//         }
//
//         /// <summary>
//         ///     Get a group using group settings, list of tuples with settings, and a variable list of elements.
//         /// This group will be placed on the same settings level as it's parent.
//         /// </summary>
//         public static Element GetCompositeGroup
//         (
//             [CanBeNull] GroupCustomSettings customSettings,
//                 (
//                 string varName, 
//                 string title, 
//                 string tooltip, 
//                 SingleCustomSettings settings 
//                 )
//                 [] elementTuples, 
//             params Element[] elements 
//         )
//         {
//             return new CompositeGroup( customSettings, ListOperations.GetCombinedElementAndTuplesList( elementTuples, elements ) );
//         }
//         
//         /// <summary>
//         ///     Get a group using group settings, list of tuples with conditions, and a variable list of elements.
//         /// This group will be placed on the same settings level as it's parent.
//         /// </summary>
//         public static Element GetCompositeGroup
//         (
//             [CanBeNull] GroupCustomSettings customSettings, 
//             (
//                 string varName, 
//                 string title, 
//                 string tooltip, 
//                 ElementCondition[] filters 
//                 )
//                 [] elementTuples, 
//             params Element[] elements )    
//         {
//             return new CompositeGroup( customSettings, ListOperations.GetCombinedElementAndTuplesList( elementTuples, elements ) );
//         }
//         
//         /// <summary>
//         ///     Get a group using group settings, list of tuples with both settings and conditions, and a variable list of elements.
//         /// This group will be placed on the same settings level as it's parent.
//         /// </summary>
//         public static Element GetCompositeGroup
//         (
//             [CanBeNull] GroupCustomSettings customSettings,
//                 (
//                 string varName, 
//                 string title, 
//                 string tooltip, 
//                 SingleCustomSettings settings,
//                 ElementCondition[] filters
//                 )
//                 [] elementTuples, 
//             params Element[] elements 
//         )
//         {
//             return new CompositeGroup( customSettings, ListOperations.GetCombinedElementAndTuplesList( elementTuples, elements ) );
//         }
//
// #endregion
//         
// #endregion
    }
}
