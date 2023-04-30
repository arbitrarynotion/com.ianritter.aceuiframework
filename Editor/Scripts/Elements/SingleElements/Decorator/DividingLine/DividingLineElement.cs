using System;
using JetBrains.Annotations;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.ElementConditions;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsCustom;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsCustom.SingleElements;
using UnityEditor;
using UnityEngine;

namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.SingleElements.Decorator.DividingLine
{
    public class DividingLineElement : SingleElement
    {
        private DividingLineElementLayout _dividingLineElementLayout;
        private DividingLineElementDraw _dividingLineElementDraw;

        public override SingleElementLayout SingleElementLayout => _dividingLineElementLayout;
        public override SingleElementDraw SingleElementDraw => _dividingLineElementDraw;
        
        public readonly float DividerThickness;
        public readonly float BoxHeight;
        public readonly Color Color;
        public readonly bool UseCustomSettings;
        public readonly float LeftTrimPercent;
        public readonly float RightTrimPercent;
        public readonly bool SettingsAreLive;
        private readonly string _leftTrimPercentPropertyVarName;
        private readonly string _rightTrimPercentPropertyVarName;
        public SerializedProperty LeftTrimPercentProperty;
        public SerializedProperty RightTrimPercentProperty;
        

        // Todo: Add option to change divider color to settings window.
        /// <summary>
        /// An element which draws a divider as its inspector line entry. The color defaults to the global outline color
        /// settings.
        /// </summary>
        public DividingLineElement( 
            float boxHeight = 5f, 
            float dividerThickness = 1f,
            bool allowFrame = false) : 
            base( 
                GUIContent.none, 
                new SingleCustomSettings()
                {
                    ForceSingleLine = true,
                    CustomFrameSettings = new CustomFrameSettings() {applyFraming = allowFrame, includeBackground = allowFrame}
                }, 
                false, 
                new ElementCondition[] {} 
            )
        {
            BoxHeight = boxHeight;
            DividerThickness = dividerThickness;
            DividerThickness.AtMost( BoxHeight );
        }

        
        /// <summary>
        /// An element which draws a divider as its inspector line entry. The color defaults to the global outline color
        /// settings.
        /// </summary>
        public DividingLineElement( 
            Color color, 
            float boxHeight = 5f, 
            float dividerThickness = 1f,
            float leftTrimPercent = 0f,
            float rightTrimPercent = 0f,
            SingleCustomSettings customSettings = null
        ) : 
            base( 
                GUIContent.none, 
                customSettings, 
                false, 
                new ElementCondition[] {} 
            )
        {
            UseCustomSettings = true;
            DividerThickness = dividerThickness;
            BoxHeight = boxHeight;
            Color = color;
            LeftTrimPercent = leftTrimPercent;
            RightTrimPercent = rightTrimPercent;
            DividerThickness.AtMost( BoxHeight );
        }
        
        /// <summary>
        /// An element which draws a divider as its inspector line entry. The color defaults to the global outline color
        /// settings.
        /// </summary>
        public DividingLineElement( 
            Color color, 
            float boxHeight, 
            float dividerThickness,
            string leftTrimPercentPropertyVarName,
            string rightTrimPercentPropertyVarName,
            SingleCustomSettings customSettings
        ) : 
            base( 
                GUIContent.none, 
                customSettings, 
                false, 
                new ElementCondition[] {} 
            )
        {
            UseCustomSettings = true;
            DividerThickness = dividerThickness;
            BoxHeight = boxHeight;
            Color = color;
            SettingsAreLive = true;
            _leftTrimPercentPropertyVarName = leftTrimPercentPropertyVarName;
            _rightTrimPercentPropertyVarName = rightTrimPercentPropertyVarName;
            DividerThickness.AtMost( BoxHeight );
        }
        
        
        protected override void InitializeElement( SerializedObject targetScriptableObject )
        {
            SingleCustomSettings.ForceSingleLine = true;
            
            if ( !SettingsAreLive ) return;
            
            LeftTrimPercentProperty = targetScriptableObject.FindProperty( _leftTrimPercentPropertyVarName );
            RightTrimPercentProperty = targetScriptableObject.FindProperty( _rightTrimPercentPropertyVarName );
            
            if ( LeftTrimPercentProperty == null || RightTrimPercentProperty == null )
                throw new NullReferenceException( "Divider Element: Failed to find Left and/or Right Trim Percent properties.");

            if ( LeftTrimPercentProperty.propertyType != SerializedPropertyType.Float ||
                 RightTrimPercentProperty.propertyType != SerializedPropertyType.Float )
                throw new Exception( "Divider Element: trim properties must both be float types.");
        }

        protected override void InitializeLayout() => _dividingLineElementLayout = new DividingLineElementLayout( this );

        protected override void InitializeDraw() => _dividingLineElementDraw = new DividingLineElementDraw( this );
    }
}
