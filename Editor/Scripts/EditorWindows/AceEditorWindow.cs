using Packages.com.ianritter.aceuiframework.Editor.Scripts.ACECore;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.AceEditorRoots;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements;
using UnityEditor;
using UnityEngine;
using static Packages.com.ianritter.unityscriptingtools.Editor.AssetLoader;

namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.EditorWindows
{
    /// <summary>
    ///     This class holds all common behavior between Ace editor windows.
    ///     Extend AceScriptableObjectRoot
    /// </summary>
    public abstract class AceEditorWindow : EditorWindow
    {
        /// <summary>
        ///     Serialized object has to be handled manually in editor windows, while it's readily available
        ///     in an Editor script.
        /// </summary>
        public SerializedObject targetSerializedObject;

        // This is the theme used to draw the window.
        protected AceTheme AceTheme { get; private set; }
        
        public abstract string GetTargetName();

        // This theme is the data that the menu will be displaying, it has no effect on this window's layout.
        private AceScriptableObjectEditorRoot TargetScript
        {
            get
            {
                if ( _targetScript == null )
                    _targetScript = GetTarget();
                return _targetScript;
            }
            set => _targetScript = value;
        }
        private AceScriptableObjectEditorRoot _targetScript;


        protected Element[] Elements { get; private set; }
        
        protected virtual void OnEnableFirst() { }
        protected virtual void OnEnableLast() { }

        protected abstract string GetEditorWindowThemeName();

        protected abstract AceScriptableObjectEditorRoot GetTarget();
        
        protected abstract Vector2 GetEditorWindowMinSize();
        protected abstract string GetTitle();
        protected abstract string GetTooltip();

        private void OnEnable()
        {
            OnEnableFirst();
            
            LoadSettingsScriptableObjects();
            ApplyEditorWindowSettings();
            EstablishCallBacks();
            GetElementsListFromTarget();
            
            OnEnableLast();
        }

        // Todo: Get string for theme. Can't load target if type is specific.
        private void LoadSettingsScriptableObjects()
        {
            AceTheme = GetAssetByName<AceTheme>( GetEditorWindowThemeName() );
            if ( AceTheme == null )
                Debug.LogError( "Failed to load CET Editor Window theme!" );

            // The SO will be used to connect the elements to their actual data. The data is not available when
            // elements are initialized so it must be attached later during a manual element initialization phase.
            targetSerializedObject = new SerializedObject( TargetScript );
            
        }



        private void ApplyEditorWindowSettings()
        {
            minSize = GetEditorWindowMinSize();
            titleContent = new GUIContent( GetTitle(), GetTooltip() );
        }

        private void EstablishCallBacks()
        {
            // Window doesn't update correctly after redos/undos without this.
            Undo.undoRedoPerformed += OnUndoRedo;

            // When the target settings
            SubscribeToTargetEvents();

            // When changes are made to the editor window's theme, this will trigger a repaint so they
            // changes are shown in realtime.
            AceTheme.OnDataUpdated += OnEditorThemeUpdated;
        }

        private void OnDisable()
        {
            UnsubscribeFromTargetEvents();
            AceTheme.OnDataUpdated -= OnEditorThemeUpdated;
        }

        private void SubscribeToTargetEvents()
        {
            TargetScript.OnUIStateUpdated += OnTargetUiStateUpdated;
            TargetScript.OnDataUpdated += OnTargetDataUpdated;
            TargetScript.OnDataUpdateRequired += OnTargetDataUpdateRequired;
        }

        private void UnsubscribeFromTargetEvents()
        {
            TargetScript.OnUIStateUpdated -= OnTargetUiStateUpdated;
            TargetScript.OnDataUpdated -= OnTargetDataUpdated;
            TargetScript.OnDataUpdateRequired -= OnTargetDataUpdateRequired;
        }

        protected void PerformTargetSwap()
        {
            UnsubscribeFromTargetEvents();
            TargetScript = GetTarget();
            targetSerializedObject = new SerializedObject( TargetScript );
            SubscribeToTargetEvents();
            GetElementsListFromTarget();
            Repaint();
        }

        protected virtual void OnTargetDataUpdateRequired() => targetSerializedObject.ApplyModifiedProperties();

        /// <summary>
        ///     The target script has made a change to its element list so rebuild the list and repaint.
        /// </summary>
        protected virtual void OnTargetUiStateUpdated()
        {
            targetSerializedObject.ApplyModifiedProperties();
            GetElementsListFromTarget();
            Repaint();
        }

        /// <summary>
        ///     Target scriptable object was changed outside of this editor window. Repaint to reflect changes.
        /// </summary>
        protected virtual void OnTargetDataUpdated() => Repaint();

        /// <summary>
        ///     The theme used to draw this editor window was changed. Repaint to reflect changes.
        /// </summary>
        private void OnEditorThemeUpdated() => Repaint();

        private void GetElementsListFromTarget()
        {
            Elements = TargetScript.GetElementList();

            foreach ( Element element in Elements )
            {
                // Element initialization attaches each element to it's parent SO and the theme
                // tells it how to be drawn.
                element.Initialize( targetSerializedObject, AceTheme, false );
            }
        }

        private void OnUndoRedo() => Repaint();

        public void OnGUI()
        {
            targetSerializedObject.Update();

            DrawElements();

            if ( targetSerializedObject.ApplyModifiedProperties() )
                TargetScript.DataUpdatedNotify();
        }

        protected abstract void DrawElements();
    }
}

