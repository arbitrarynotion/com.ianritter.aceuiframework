using System;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.GroupElements.HeadingGroup.FoldOut;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.Enums;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsGlobal;
using UnityEditor;
using UnityEngine;
using static Packages.com.ianritter.unityscriptingtools.Runtime.Services.UIGraphics.UIRectGraphics;

namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements
{
    public abstract class ElementDraw
    {
#region Protected Virtual Class Data

        protected virtual Settings LayoutToolsSettingsSource => Element.Settings;

#endregion
        
#region Protected Abstract Class Data

        protected abstract Element Element { get; }

#endregion
        
        
        

#region Public Methods

        /// <summary>
        ///     Draw element in the editor. This includes the element's optional frame and outline, layout tools, and data - in that order.
        /// </summary>
        public void DrawElement()
        {
            DrawElementFrameBackground();
            DrawElementFrameOutline();
            DrawLayoutToolsPosAndDrawRects();
            DrawElementContents();
        }

#endregion
        

#region Protected Methods

        /// <summary>
        ///     Draws the guicontents to a label using alignment specified in the element's custom settings.
        /// </summary>
        protected void DrawAlignedLabelField( GUIStyle style = null )
        {
            DrawAlignedLabelField( Element.Layout.GetDrawRect(), style );
        }

        /// <summary>
        ///     Draws the guicontents to a label using alignment specified in the element's custom settings.
        /// </summary>
        protected void DrawAlignedLabelField( Rect drawRect, GUIStyle style = null )
        {
            style = ProcessGuiStyle( style );
            EditorGUI.LabelField( Element.Layout.GetAlignedLabelDrawRect( drawRect ), Element.GUIContent, style );
        }

        private GUIStyle ProcessGuiStyle( GUIStyle style )
        {
            if ( style == null )
                return Element.CustomSettings.BoldLabel ? EditorStyles.boldLabel : EditorStyles.label;

            if ( Element.CustomSettings.BoldLabel )
                style.fontStyle = FontStyle.Bold;
            
            return style;
        }

        /// <summary>
        ///     Draws the guicontents to a label using the default editor label style.
        /// </summary>
        protected void DrawLabelField( Rect drawRect )
        {
            DrawAlignedLabelField( drawRect );
            
            // Defaulting to aligned label rect.
            // EditorGUI.LabelField( drawRect, Element.GUIContent, EditorStyles.label );
        }

        /// <summary>
        ///     Draws the guicontents to a label using the specified style.
        /// </summary>
        protected void DrawLabelField( Rect drawRect, GUIStyle style )
        {
            DrawAlignedLabelField( drawRect, style );
            // Defaulting to aligned label rect.

            // if ( style == null )
            //     style = EditorStyles.label;
            //
            // EditorGUI.LabelField( drawRect, Element.GUIContent, style );
        }
        
#endregion
        
        
#region Protected Abstract Methods

        /// <summary>
        ///     Draw the elements editor UI contents.
        /// </summary>
        protected abstract void DrawElementContents();
        
        /// <summary>
        ///     Provide logic to determine of layout tools should be shown for this element.
        /// </summary>
        protected abstract bool ShouldShowLayoutTools();


#endregion
        
#region Private Methods

        private void DrawElementFrameBackground()
        {
            if ( !Element.Layout.ShouldShowFrame() || !Element.FrameSettings.includeBackground ) return;
            
            DrawSolidRect( Element.Layout.GetFrameRect(), Element.AceTheme.GetColorForIndex( Element.FrameSettings.backgroundColorIndex ) );
        }

        private void DrawElementFrameOutline()
        {
            if (!Element.Layout.ShouldShowFrame()) return;

            DrawRect( 
                Element.Layout.GetFrameRect(), 
                Element.FrameSettings.frameType, 
                Element.AceTheme.GetColorForIndex( Element.FrameSettings.frameOutlineColorIndex ),
                Element.AceTheme.GetColorForIndex( Element.FrameSettings.backgroundColorIndex ),
                Element.FrameSettings.frameOutlineThickness,
                false );

            if ( Element.GetType() != typeof( FoldoutGroup ) ) return;
        }
        
        private void DrawLayoutToolsPosAndDrawRects()
        {
            Settings settings = LayoutToolsSettingsSource;
            if ( !settings.showLayoutVisualizations || !ShouldShowLayoutTools() ) return;
            
            if ( settings.showPosRect ) DrawDebugElementFrame( settings.layoutVisualizationsFrameType, Element.Layout.GetPositionRect(), settings.layoutToolsPosRectColor );
            if ( settings.showFrameRect ) DrawDebugElementFrame( settings.layoutVisualizationsFrameType, Element.Layout.GetFrameRect(), settings.frameRectColor );
            if ( settings.showDrawRect ) DrawDebugElementFrame( settings.layoutVisualizationsFrameType, Element.Layout.GetDrawRect(), settings.layoutToolsDrawRectColor );
        }

        private void DrawDebugElementFrame( DebugFrameType frameType, Rect frameRect, Color color )
        {
            switch (frameType)
            {
                case DebugFrameType.FullSolid:
                    DrawSolidRect( frameRect, color );
                    break;
                case DebugFrameType.FullOutline:
                    DrawRectOutline( frameRect, color );
                    break;
                default:
                    throw new ArgumentOutOfRangeException( nameof( frameType ), frameType, "Type not handled. Was the enum updated with new values?" );            
            }
        }

#endregion
    }
}
