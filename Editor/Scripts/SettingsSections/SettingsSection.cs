using System;
using System.Collections.Generic;
using System.Linq;
using Packages.com.ianritter.unityscriptingtools.Runtime.Enums;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.ACECore;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.ElementConditions;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.Enums;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsCustom;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsCustom.Groups;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsCustom.SingleElements;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsGlobal;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsGlobal.Elements.SingleElements.Decorator;
using UnityEngine;
using static Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.ElementBuilding.AceElementBuilder;
using static Packages.com.ianritter.aceuiframework.Editor.Scripts.ACECore.AceDelegates;

namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.SettingsSections
{
    [Serializable]
    public abstract class SettingsSection
    {
        /// <summary>
        ///     This event is invoked when a data change occurs that justifies a repaint (element values have changed).
        /// </summary>
        public event DataUpdated OnDataUpdated;

        /// <summary>
        ///     This event is invoked when a data change occurs that requires a full UI rebuild (element layout was changed).
        /// </summary>
        public event UIStateUpdated OnUIStateUpdated;

        protected readonly CustomFrameSettings NoFrame = new CustomFrameSettings() { applyFraming = false };

        protected readonly ElementCondition[] NoFilter = new ElementCondition[0];

        protected AceTheme AceTheme;

        protected string MyRelativeVarName;


        public abstract Element GetSection();
        
        public void ClearSubscriptions() => AceTheme.OnColorsUpdated -= OnColorsChanged;


        /// <summary>
        ///     Used to notify when data that affects the UI data has occured, requiring a redraw.
        /// </summary>
        protected void DataUpdatedNotify()
        {
            // Debug.Log( "Setting section's Data Update Notify was called." );
            OnDataUpdated?.Invoke();
        }

        /// <summary>
        ///     Used to notify when data that affects the UI layout has occured, requiring a full UI rebuild.
        /// </summary>
        protected void UIStateUpdatedNotify() => OnUIStateUpdated?.Invoke();

        protected void EstablishSubscriptions() => AceTheme.OnColorsUpdated += OnColorsChanged;

        protected abstract string GetRelativePathVarName( string varName );


        protected ( string varName, string title, string tooltip )[] GetPositionSection(
            string topVarName,
            string leftVarName,
            string rightVarName,
            string bottomVarName )
        {
            return new[]
            {
                ( topVarName, "Top", string.Empty ),
                ( leftVarName, "Left", string.Empty ),
                ( rightVarName, "Right", string.Empty ),
                ( bottomVarName, "Bottom", string.Empty )
            };
        }

        protected Element GetPositionSection( string title, string tooltip,
            ElementVarNames varNames, bool skipTop = false )
        {
            return skipTop
                ? GetGroupWithLabelHeading(
                    title, tooltip, null,
                    GetElement( varNames.LeftPadding, "Left", tooltip ),
                    GetElement( varNames.RightPadding, "Right", tooltip ),
                    GetElement( varNames.BottomPadding, "Bottom", tooltip ) )
                : GetGroupWithLabelHeading(
                    title, tooltip, null,
                    GetElement( varNames.TopPadding, "Top", tooltip ),
                    GetElement( varNames.LeftPadding, "Left", tooltip ),
                    GetElement( varNames.RightPadding, "Right", tooltip ),
                    GetElement( varNames.BottomPadding, "Bottom", tooltip ) );
        }

        protected Element GetFramesSection( string title, string tooltip, 
            FrameSettings frameSettings, ElementFrameVarNames frameVarNames, 
            bool skipAutoBorder = false,
            params Element[] prependStyleElements )
        {
            List<Element> subSections = new List<Element>()
            {
                GetFrameStyleSection( frameVarNames, skipAutoBorder, prependStyleElements ),
                GetFramesColorsSection( frameSettings, frameVarNames )
            };

            return GetGroupWithToggleHeading( 
                frameVarNames.ShowFrame, title, tooltip,
                null,
                true,
                subSections.ToArray() );
        }

        // protected Element GetTextColorsSection( HeadingElementFrameSettings frameSettings, HeadingElementFrameSettingsVarNames frameVarNames )
        // {
        //     return GetGroupWithLabelHeading( "Text Colors", string.Empty, null,
        //         AceTheme.GetColorSelectionElement( "Enabled", string.Empty,
        //             frameSettings.enabledTextColorIndex,
        //             frameVarNames.EnabledTextColorIndex, OnColorSelectionChanged,
        //             GetMustHaveOutlineFilter( frameVarNames.FrameType ) ),
        //         AceTheme.GetColorSelectionElement( "Disabled", string.Empty,
        //             frameSettings.disabledTextColorIndex,
        //             frameVarNames.DisabledTextColorIndex, OnColorSelectionChanged )
        //     );
        // }

        protected Element GetLayoutVisualizationSection( params Element[] layoutVisualizationSubsections )
        {
            return GetCompositeGroup(
                null,
                layoutVisualizationSubsections
            );
        }

        protected static Element GetLayoutVisualizationSubsection( string title, ElementVarNames varNames, bool indent = false )
        {
            return GetCompositeLayoutDrawGroup( title, indent,
                varNames.LayoutVisualizationsFrameType,
                varNames.ShowPosRect,
                varNames.PosRectColor,
                varNames.ShowFrameRect,
                varNames.FrameRectColor,
                varNames.ShowDrawRect,
                varNames.DrawRectColor
            );
        }

        private Element GetFrameStyleSection( ElementFrameVarNames frameVarNames, bool skipAutoBorder,
            params Element[] prependStyleElements )
        {
            List<Element> returnList = prependStyleElements.ToList();

            returnList.AddRange( GetFrameStyleTypeAndThicknessSection( frameVarNames, skipAutoBorder ) );
            
            return GetGroupWithLabelHeading( "Style", string.Empty, null, returnList.ToArray() );
        }

        private List<Element> GetFrameStyleTypeAndThicknessSection( ElementFrameVarNames frameVarNames, bool skipAutoBorder )
        {
            List<Element> returnList = new List<Element>()
            {
                GetElement( frameVarNames.IncludeBackground, "Background", string.Empty ), 
                GetElement( frameVarNames.FrameType, "Type", string.Empty )
            };
            
            if ( !skipAutoBorder )
                returnList.Add( GetElement( frameVarNames.FramePadding, "Frame Padding", string.Empty, new SingleCustomSettings() { IndentLevelIncrease = 1 }, 
                    false, GetMustHaveOutlineFilter( frameVarNames.FrameType ) ) );
            
            returnList.Add( GetElement( frameVarNames.FrameOutlineThickness, "Line Thickness", string.Empty, new SingleCustomSettings() { IndentLevelIncrease = 1 }, 
                false, GetMustHaveOutlineFilter( frameVarNames.FrameType ) ) );

            return returnList;
        }

        private Element GetFramesColorsSection( FrameSettings frameSettings, ElementFrameVarNames frameVarNames )
        {
            return GetGroupWithLabelHeading( "Colors", string.Empty, null,
                AceTheme.GetColorSelectionElement( "Outline", string.Empty,
                    frameSettings.frameOutlineColorIndex,
                    frameVarNames.FrameOutlineColorIndex, OnColorSelectionChanged,
                    GetMustHaveOutlineFilter( frameVarNames.FrameType ) ),
                AceTheme.GetColorSelectionElement( "Background", string.Empty,
                    frameSettings.backgroundColorIndex,
                    frameVarNames.BackgroundColorIndex, OnColorSelectionChanged,
                    GetMustHaveBackgroundFilter( frameVarNames.IncludeBackground ) )
            );
        }

        protected void OnColorSelectionChanged() => DataUpdatedNotify();

        private void OnColorsChanged() => DataUpdatedNotify();

        private static Element GetCompositeLayoutDrawGroup( string title, bool indent,
            string frameTypeVarName, 
            string posRectToggleVarName, string posRectColorVarName, 
            string frameRectToggleVarName, string frameRectColorVarName, 
            string drawRectToggleVarName, string drawRectColorVarName, bool hasOwnLine = true )
        {
            int indentAmount = indent ? 1 : 0;
            
            var rootGroupCustomSettings = new GroupCustomSettings()
            {
                NumberOfColumns = 2,
                IndentLevelIncrease = indentAmount,
            };

            var labelAndDropDownElementSettings = new SingleCustomSettings()
            {
                UseIndentedDefaultLabelWidth = true
            };
            
            var labeledToggleAndColorGroupSettings = new GroupCustomSettings()
            {
                NumberOfColumns = 2,
                CustomFrameSettings = new CustomFrameSettings()
                {
                    applyFraming = true,
                    includeBackground = true,
                    frameType = ElementFrameType.FullOutline,
                    frameOutlineThickness = 1,
                    frameAutoPadding = 3f,
                    frameOutlineColorIndex = 0,
                    backgroundColorIndex = 1
                }
            };
            
            var labelCustomSettings = new SingleCustomSettings();
            
            var toggleAndColorCustomSettings = new SingleCustomSettings()
            {
                ColumnWidthPriority = 0.95f
            };

            if ( !hasOwnLine )
            {
                labelCustomSettings.ConstantWidth = 60f;
                toggleAndColorCustomSettings.FieldMinWidth = 60f;
                labeledToggleAndColorGroupSettings.ColumnWidthPriority = 0.95f;
            }
            else
            {
                toggleAndColorCustomSettings.ColumnWidthPriority = 0.95f;
            }
            
            return GetCompositeGroup(
                rootGroupCustomSettings,

                // Title and frame type popup.
                GetCompositeGroup( 
                    new GroupCustomSettings()
                    {
                        UseIndentedDefaultLabelWidth = hasOwnLine,
                        CenterInFullHeightOfLine = true,
                        FullHeightFrame = true,
                        CustomFrameSettings = new CustomFrameSettings()
                        {
                            applyFraming = true,
                            includeBackground = false,
                            frameType = ElementFrameType.CornersLeftBottomOnly,
                            // frameOutlineThickness = 1,
                            frameAutoPadding = 4f,
                            frameOutlineColorIndex = 0,
                            backgroundColorIndex = 1
                        },
                        LeftPadding = 3f,
                        RightPadding = 3f
                    },
                    GetLabelElement( title, labelAndDropDownElementSettings ),
                    GetElement( frameTypeVarName, labelAndDropDownElementSettings )
                ),
                
                // Position and draw colors with toggles.
                GetCompositeGroup( labeledToggleAndColorGroupSettings,
                    GetElement( posRectToggleVarName, "Pos", string.Empty, labelCustomSettings ),
                    GetElement( posRectColorVarName, toggleAndColorCustomSettings ),
                    
                    GetElement( frameRectToggleVarName, "Frame", string.Empty, labelCustomSettings ),
                    GetElement( frameRectColorVarName, toggleAndColorCustomSettings ),

                    GetElement( drawRectToggleVarName, "Draw", string.Empty, labelCustomSettings ),
                    GetElement( drawRectColorVarName, toggleAndColorCustomSettings )
                )
            );
        }

        // Conditions: Returns true if the condition passes.
        // NotEqualTo is like saying "all but these are allowed".
        protected ElementCondition[] GetMustHaveOutlineFilter( string frameTypeVarName )
        {
            return new[]
            {
                new ElementCondition( frameTypeVarName, ConditionOperator.NotEqualTo, (int) ElementFrameType.None )
            };
        }

        protected ElementCondition[] GetMustHaveBackgroundFilter( string includeBackgroundVarName )
        {
            return new[]
            {
                new ElementCondition( includeBackgroundVarName ),
            };
        }
    }
}
