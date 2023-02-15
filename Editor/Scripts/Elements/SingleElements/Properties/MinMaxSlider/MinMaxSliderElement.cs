using System;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.ElementConditions;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.SingleElements.Properties.Basic;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsCustom.SingleElements;
using UnityEditor;
using UnityEngine;

namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.SingleElements.Properties.MinMaxSlider
{
    public class MinMaxSliderElement : PropertyElement
    {
        private MinMaxSliderLayout _minMaxSliderLayout;
        private MinMaxSliderElementDraw _minMaxSliderDraw;
        
        public override PropertyElementLayout PropertyElementLayout => _minMaxSliderLayout;
        protected override PropertyElementDraw PropertyElementDraw => _minMaxSliderDraw;
        
        public PropertyElement MinPropertyElement { get; private set; }
        public PropertyElement MaxPropertyElement { get; private set; }
        private readonly string _minVarName;
        private readonly string _maxVarName;
        public readonly float MinLimit;
        public readonly float MaxLimit;
        
        
        public MinMaxSliderElement( 
            GUIContent guiContent,
            string minVarName, 
            string maxVarName, 
            float minLimit, 
            float maxLimit, 
            SingleCustomSettings singleCustomSettings,
            Action changeCallBack, 
            
            bool hideOnDisable = false, 
            params ElementCondition[] conditions ) :
            base( "", guiContent, singleCustomSettings, changeCallBack, hideOnDisable, conditions )
        {
            _minVarName = minVarName;
            _maxVarName = maxVarName;
            MinLimit = minLimit;
            MaxLimit = maxLimit;
        }
        
        
        protected override void InitializeElement( SerializedObject targetScriptableObject )
        {
            MinPropertyElement = new BasicProperty( _minVarName, new GUIContent( ObjectNames.NicifyVariableName( _minVarName ) ), new SingleCustomSettings(), null );
            MaxPropertyElement = new BasicProperty( _maxVarName, new GUIContent( ObjectNames.NicifyVariableName( _maxVarName ) ), new SingleCustomSettings(), null );
            MinPropertyElement.Initialize( targetScriptableObject, AceTheme, DrawnInInspector );
            MaxPropertyElement.Initialize( targetScriptableObject, AceTheme, DrawnInInspector );
        }

        protected override void InitializeLayout() => _minMaxSliderLayout = new MinMaxSliderLayout( this );
        protected override void InitializeDraw() => _minMaxSliderDraw = new MinMaxSliderElementDraw( this );

        public override bool ElementIsValid() => 
            PropertyIsValid( MinPropertyElement.Property ) && PropertyIsValid( MaxPropertyElement.Property );
        
        public override bool PropertyTypeShouldUseCustomWideMode() => true;
        public override bool PropertyTypeUsesDefaultWideMode() => false;
    }
}