using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsGlobal;
using UnityEditor;
using UnityEngine;

namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.SingleElements.Decorator.Heading
{
    public abstract class HeadingElementDraw : SingleElementDraw
    {
        protected readonly HeadingElement HeadingElement;
        protected override SingleElement SingleElement => HeadingElement;
        
        protected const float DefaultLeftPadding = 2f;

        
        protected HeadingElementDraw( HeadingElement headingElement )
        {
            HeadingElement = headingElement;
        }

        
        protected override bool ShouldShowLayoutTools() => true;
        
        protected override Settings LayoutToolsSettingsSource => Element.ParentElement.Settings;
        
        protected GUIStyle GetHeadingStyle( GUIStyle baseGuiStyle )
        {
            Color enabledColor = Element.AceTheme.GetColorForColorName( HeadingElement.HeadingElementFrameSettings.enabledTextColorName );
            // Color enabledColor = Element.AceTheme.GetColorForIndex( _headingElement.HeadingElementFrameSettings.enabledTextColorIndex );
            
            return new GUIStyle( baseGuiStyle )
            {
                fontStyle = FontStyle.Bold, 
                
                normal = {textColor = Element.AceTheme.GetColorForColorName( HeadingElement.HeadingElementFrameSettings.disabledTextColorName )}, 
                // normal = {textColor = Element.AceTheme.GetColorForIndex( _headingElement.HeadingElementFrameSettings.disabledTextColorIndex )}, 
                onNormal = {textColor = enabledColor}
            };
        }
        
        protected GUIStyle GetHeadingLabelStyle( bool enabled )
        {
            Color textColor = enabled
                ? Element.AceTheme.GetColorForColorName( HeadingElement.HeadingElementFrameSettings.enabledTextColorName )
                : Element.AceTheme.GetColorForColorName( HeadingElement.HeadingElementFrameSettings.disabledTextColorName );
            
            // Color textColor = enabled
            //     ? Element.AceTheme.GetColorForIndex( _headingElement.HeadingElementFrameSettings.enabledTextColorIndex )
            //     : Element.AceTheme.GetColorForIndex( _headingElement.HeadingElementFrameSettings.disabledTextColorIndex );
            
            return new GUIStyle( EditorStyles.label )
            {
                fontStyle = FontStyle.Bold, 
                
                normal = {textColor = textColor}
            };
        }
        
        protected override string GetBackgroundColorName()
        {
            return HeadingIsEnabled()
                ? HeadingElement.HeadingElementFrameSettings.backgroundActiveColorName
                : HeadingElement.HeadingElementFrameSettings.backgroundInactiveColorName;
        }

        // protected override int GetBackgroundColorIndex()
        // {
        //     return HeadingIsEnabled()
        //         ? _headingElement.HeadingElementFrameSettings.backgroundColorIndex
        //         : _headingElement.HeadingElementFrameSettings.backgroundInactiveColorIndex;
        // }

        protected abstract bool HeadingIsEnabled();
    }
}