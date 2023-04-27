using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsCustom.SingleElements;
using Packages.com.ianritter.unityscriptingtools.Runtime.Services.CustomLogger;
using static Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.ElementBuilding.AceElementBuilder;
using UnityEngine;

namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.ACECore
{
    public class TMButtonHandler
    {
        private readonly TMListHandler _tmListHandler;
        private readonly AceThemeManager _themeManager;
        private readonly CustomLogger _logger;

        public TMButtonHandler( TMListHandler tmListHandler, AceThemeManager themeManager, CustomLogger logger )
        {
            _tmListHandler = tmListHandler;
            _themeManager = themeManager;
            _logger = logger;
        }

        /// <summary>
        /// Get the Scan Scripts button and a button for each theme to report it's event subscribers for debugging.
        /// </summary>
        /// <param name="scriptInfoListSize">Used to check if themes list is empty.</param>
        /// <returns></returns>
        public Element[] GetButtons( int scriptInfoListSize )
        {
            // return new Element[] {new LabelElement( new GUIContent( "Buttons!" ) )};
            return ( scriptInfoListSize > 0 ) ?
                new Element[]
                {
                    
                    GetScanScriptsButton(),
                    // GetCurrentThemeSubscribersButton()
                    GetDividerElement( 6f, 2f ),
                    GetGroupWithLabelHeading( "Subscriber Reports", string.Empty, null, GetThemeSubscriberReportButtons().ToArray() )
                } 
                : new Element[]
                {
                    GetLabelElement( "No ACE scripts found." ), 
                    GetScanScriptsButton()
                };
        }
        
        private Element GetScanScriptsButton()
        {
            return GetBasicButton( 
                new GUIContent( "Scan For script Changes" ), 
                true, 
                new SingleCustomSettings() { ForceSingleLine = true },
                ScanForScriptChanges
            );
        }

        // Button callbacks go here.
        private void ScanForScriptChanges()
        {
            _logger.LogStart( true );
            _logger.Log( "ScriptThemeInfoList state before refresh:" );
            _themeManager.PrintScriptThemeInfoList();
            _themeManager.RefreshScriptThemeInfoList();
            _logger.Log( "ScriptThemeInfoList state after refresh:" );
            _themeManager.PrintScriptThemeInfoList();
            _logger.LogEnd();
        }
        
        
        // Get a button for each of the theme's
        private List<Element> GetThemeSubscriberReportButtons()
        {
            // Make a list of current subscriber buttons, one for each theme
            return _tmListHandler.GetThemesList().Select( GetThemeSubscribersButtonForTheme ).ToList();
        }
        
        private Element GetThemeSubscribersButtonForTheme( AceTheme theme )
        {
            return GetBasicButton( 
                new GUIContent( $"{theme.name}'s Subscribers" ), 
                true, 
                new SingleCustomSettings() { ForceSingleLine = true },
                theme.PrintMySubscribers
            );
        }
    }
}