using System;
using System.Linq;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.ACECore;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.ElementConditions;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.GroupElements;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.GroupElements.CompositeGroup;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsCustom;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsGlobal;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsGlobal.Colors;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsGlobal.Global;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsGlobal.PropertySpecific;
using UnityEditor;
using UnityEngine;

namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements
{
    public abstract class Element
    {

        // This is the minimum info required to define an element.
#region Element Info

        /// <summary>
        ///     Elements title and tooltip.
        /// </summary>
        public GUIContent GUIContent { get; private set; }
        
        public abstract CustomSettings CustomSettings { get; }


#endregion
        
        
        

#region Public Class Data

        // Data
        
        /// <summary>
        ///     The hierarchical depth of the element. This is used to determine what global settings to apply.
        /// </summary>
        public int ElementLevel { get; set; }
        
        /// <summary>
        ///     When true, element is drawn with default inspector margins included in its position.
        /// </summary>
        public bool DrawnInInspector { get; private set; }
        
        /// <summary>
        ///     The section this element is a part of.
        /// </summary>
        public GroupElement ParentElement { get; set; }
        
        
        // theme reference.
        /// <summary>
        ///     The settings used to modify the layout and look of the elements. This is a reference shared by
        ///     all elements and is modified via the Custom Editor Tool settings window.
        /// </summary>
        public AceTheme AceTheme { get; private set; }
        
        /// <summary>
        ///     Settings that control the element's frame. Uses custom frame settings if specified, otherwise uses
        ///     the element's type and level appropriate global settings.
        /// </summary>
        public FrameSettings FrameSettings => CustomSettings.OverrideFrame()
            ? CustomSettings.CustomFrameSettings
            : GetElementFrameSettings();
        
        // Global settings references.
        public GlobalSettings GlobalSettings => AceTheme.GetGlobalSettings();
        
        public PropertySpecificSettings PropertySettings => AceTheme.GetPropertySpecificSettings();
        
        
        
        
        // Reference properties.

        /// <summary>
        ///     Used to grey-out this element if it's toggled off by either a locked section above it or by a bool element
        ///     it's dependent on via an element condition.
        /// </summary>
        public bool IsEnabled
        {
            get
            {
                // Case 1: This element has been permanently set to false upon instantiation.
                if ( CustomSettings.ForceDisable )
                    return false;
                
                // Case 2: Element has been manually disabled.
                if ( !_isEnabled )
                    return false;

                // Case 3: An element condition this element is dependent on evaluates to false.
                if ( ElementConditions != null )
                {
                    if ( !ElementConditions.Aggregate( true, ( current, disabledCheck ) => current & disabledCheck.Evaluate() ) )
                        return false;
                }
                
                // Case 4: There are no direct reasons to disable but element has parent so mimic the parent.
                if ( HasParent() )
                    return ParentElement.IsEnabled;

                return true;
            }
            set => _isEnabled = value;
        }
        private bool _isEnabled = true;

#endregion


#region Public Abstract Class Data

        // Child element data references.
        public abstract Settings Settings { get; }
        public abstract FrameSettings GetElementFrameSettings();

        public abstract ElementLayout Layout { get; }
        public abstract ElementDraw Draw { get; }

        public abstract bool IsVisible { get; set; }

#endregion
        
        
#region Protected Class Data

        protected bool HideOnDisable { get; set; } = false;

#endregion
        
        
#region Private Class Data

        /// <summary>
        ///     Element conditions used to determine if this element is visible and/or enabled.
        /// </summary>
        private ElementCondition[] ElementConditions { get; set; }

#endregion
        
        
#region Constructors

        protected Element( GUIContent guiContent )
        {
            AssignGUIContent( guiContent );
        }

        protected Element( GUIContent guiContent, bool hideOnDisable, ElementCondition[] conditions )
        {
            AssignGUIContent( guiContent );
            HideOnDisable = hideOnDisable;
            ElementConditions = conditions;
        }

#endregion


#region Public Methods

        /// <summary>
        ///     The display name of the element.
        /// </summary>
        public string GetName() => GUIContent.text;
        
        /// <summary>
        ///     Have element initialize all of its necessary data, catching any errors encountered in the process.
        /// </summary>
        public void Initialize( SerializedObject targetScriptableObject, AceTheme newAceTheme, bool drawnInInspector )
        {
            DrawnInInspector = drawnInInspector;
            AceTheme = newAceTheme;

            if ( CustomSettings == null )
                throw new NullReferenceException( $"E|I: {GetName()} failed to initialize its CustomSettings!" );

            // FrameSettings.FrameOutlineColorIndex = AceTheme.GetIndexForColorName( FrameSettings.frameOutlineColorName );
            // FrameSettings.BackgroundColorIndex = AceTheme.GetIndexForColorName( FrameSettings.backgroundActiveColorName );

            InitializeElement( targetScriptableObject );

            InitializeElementConditions( targetScriptableObject );

            InitializeLayout();
            if ( Layout == null ) throw new NullReferenceException( $"E|I: {GetName()} failed to initialize its layout!" );

            InitializeDraw();
            if ( Draw == null ) throw new NullReferenceException( $"E|I: {GetName()} failed to initialize its draw!" );
        }

#region LayoutEvent
        
        // public void DoElementLayout()
        // {
        //     DoLayout();
        // }

        // protected abstract void DoLayout();
        
#endregion


#region RepaintEvent
        
        /// <summary>
        ///     Draw the element in the editor.
        /// </summary>
        /// <param name="updateRequired">
        ///     Set to true only when called on root element. This is typically when called from an editor
        ///     window or inspector editor script.
        /// </param>
        public void DrawElement( bool updateRequired )
        {
            // Layout Event
            if ( IsRootElement() ) Layout.AssignNewPositionRect( updateRequired );

            // Repaint Event
            // if ( Event.current.type != EventType.Repaint ) return;
            Draw.DrawElement();
        }
        
#endregion


        /// <summary>
        ///     Add a property condition to the set of property conditions that will be used to determine if this property should
        ///     be active.
        ///     For bool conditions, just use AddBoolConditionToSet.
        /// </summary>
        public void AddElementConditions( params ElementCondition[] newPropertyConditions ) =>
            ElementConditions = ( ElementConditions == null )
                ? newPropertyConditions
                : ElementConditions.Concat( newPropertyConditions ).ToArray();
        
        /// <summary>
        ///     True when this element is part of a section.
        /// </summary>
        public bool HasParent() => ParentElement != null;

        /// <summary>
        ///     True when this element is part of a section that is expanded.
        /// </summary>
        public bool HeadingIsExpanded() => ParentElement?.IsVisible ?? true;

        /// <summary>
        ///     True if this element does not share the inspector line entry with any other elements. This includes elements
        ///     in it's parent's neighboring columns. This is used to determine if the element's field should align with the
        ///     inspector's default label/field dividing line.
        /// </summary>
        public bool HasOwnLine() => IsRootElement() || ParentElement.HasOwnLine() && Layout.NumberOfNeighbors == 0;

        public bool IsRootElement() => ParentElement == null;

        public bool ParentIsCompositeGroup() => HasParent() && ParentElement.GetType() == typeof( CompositeGroup );
        
        /// <summary>
        ///     True only if non-empty text was provided for this element's GetName.
        /// </summary>
        public bool HasLabel() => GUIContent != GUIContent.none;

#endregion
        

#region Public Virtual Methods

        public virtual int GetElementLevel() => IsRootElement() ? 0 : ParentElement.GetElementLevel();
        
#endregion

        
#region Protected Abstract Methods

        /// <summary>
        ///     The ElementLayout implementation should be instantiated here.
        /// </summary>
        protected abstract void InitializeLayout();

        /// <summary>
        ///     The ElementDraw implementation should be instantiated here.
        /// </summary>
        protected abstract void InitializeDraw();

#endregion
        
        
#region Protected Virtual Methods

        /// <summary>
        ///     Use when an element has a property that needs to be initialized or otherwise has some setup it needs to do.
        /// </summary>
        protected virtual void InitializeElement( SerializedObject targetScriptableObject )
        {
        }

        private void InitializeElementConditions( SerializedObject targetScriptableObject )
        {
            if ( ElementConditions == null ) return;

            foreach ( ElementCondition elementCondition in ElementConditions )
            {
                elementCondition.Initialize( targetScriptableObject );
            }
        }

#endregion


#region Private Methods

        private void AssignGUIContent( GUIContent guiContent )
        {
            GUIContent = guiContent;
            if ( GUIContent.text == string.Empty )
                GUIContent = GUIContent.none;
        }

        private void SubscribeForColorChanges()
        {
            // For each color, subscribe to the NameChanged event.
            
            
        }

        private void OnNameChanged( string nameOfColorThatChanged )
        {
            
        }

#endregion
        
        
        
        
        
        
        
        
        
        
        
        
#region Public Class Data
        

#endregion
        
        
#region Public Abstract Class Data



#endregion
        
        
#region Protected Class Data


        
#endregion
        
        
#region Private Class Data



#endregion




        
#region Constructors



#endregion
        
        
        
        
        
        
#region Public Methods



#endregion
        
        
        
#region Public Abstract Methods
        
        
        
#endregion

        
#region Public Virtual Methods


        
#endregion

        
        
        

#region Protected Methods
        
        
        
#endregion
        
        
#region Protected Abstract Methods



#endregion
        
        
#region Protected Virtual Methods

        

#endregion
        
        
#region Private Methods



#endregion
        
        
        // Child class formatting
#region Public Class Data
        

#endregion
        
#region Public Abstract Class Data



#endregion
        
#region Public Override Class Data



#endregion
        
#region Protected Class Data


        
#endregion
        
        
#region Private Class Data



#endregion
        
        
        
#region Constructors



#endregion
        
        
        
#region Public Methods



#endregion
        
#region Public Abstract Methods
        
        
        
#endregion   
        
#region Public Override Methods
        
        
        
#endregion

#region Public Virtual Methods


        
#endregion

        
        
#region Protected Methods
        
        
        
#endregion
     
#region Protected Abstract Methods



#endregion
        
#region Protected Override Methods



#endregion

#region Protected Virtual Methods

        

#endregion
        
        

#region Private Methods



#endregion
    }
}