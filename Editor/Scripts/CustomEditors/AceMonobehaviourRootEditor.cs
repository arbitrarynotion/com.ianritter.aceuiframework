using System.Linq;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.ACECore;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.ElementConditions;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.AceRuntimeRoots;
using Packages.com.ianritter.unityscriptingtools.Editor;
using Packages.com.ianritter.unityscriptingtools.Runtime.Enums;
using Packages.com.ianritter.unityscriptingtools.Runtime.Services.CustomLogger;
using UnityEditor;
using UnityEngine;
using static Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.ElementBuilding.ElementInfoConverter;
using static Packages.com.ianritter.aceuiframework.Editor.Scripts.DefaultInspectorDrawing;
using static Packages.com.ianritter.aceuiframework.Runtime.Scripts.AceEditorConstants;
using static Packages.com.ianritter.unityscriptingtools.Editor.AssetLoader;
using static Packages.com.ianritter.unityscriptingtools.Runtime.Services.TextFormatting.TextFormat;
using static Packages.com.ianritter.aceuiframework.Editor.Scripts.Graphics.EditorMeasurementLineGraphics;

namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.CustomEditors
{
    [CustomEditor( typeof( AceMonobehaviourRoot ), true )]
    public class AceMonobehaviourRootEditor : UnityEditor.Editor
    {
        private AceMonobehaviourRoot _targetScript;

        private Element[] _inspectorElements;
        private AceThemeManager _themeManager;

        private AceTheme _theme;

        private CustomLogger _aceLogger;
        private SerializedProperty _userLoggerProperty;

        public string GetTargetName() => _targetScript.GetType().ToString();

        private void Awake()
        {
            Initialize();
            
            // // Debug.Log( $"Property's type is: {loggerProperty.propertyType.ToString()}" );
            // string loggerName = _targetScript.GetLoggerName();
            // // Debug.Log( $"AMRE|Awake: loading logger: {GetColoredStringYellow(loggerName)}" );
            // loggerProperty.objectReferenceValue = GetAssetByName<CustomLogger>( loggerName );
            // if ( loggerProperty.objectReferenceValue == null )
            // {
            //     loggerProperty.objectReferenceValue = GetAssetByName<CustomLogger>( DefaultMbRootLoggerName, LoggersSearchFolderName );
            //     _aceLogger.Log( $"Loading of logger '{GetColoredStringYellow(loggerName)}' failed! " +
            //                     $"Loading default logger {GetColoredStringDeepPink(DefaultMbRootLoggerName)}.", CustomLogType.Warning );
            //     // Debug.LogError( $"Failed to load {loggerName}! Loading Default theme." );
            // }
            // // else
            // // {
            // //     Debug.Log( $"    loading of logger {GetColoredStringYellow(loggerName)} succeeded!." );
            // // }
            //
            // serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        ///     If you override this method, be sure to first call base.OnEnable to ensure set up is still done.
        /// </summary>
        private void OnEnable()
        {
            Initialize();

            _themeManager.OnThemeAssignmentChanged += OnTargetsThemeAssignmentUpdated;
            
            InitializeTargetsTheme();

            OnEnableFirst();

            SceneView.duringSceneGui += DuringSceneGUI;
            Undo.undoRedoPerformed += OnUndoRedo;

            _targetScript.OnTargetUIStateChanged += OnTargetUiStateUpdated;
            _targetScript.OnDataUpdateRequired += OnTargetDataUpdateRequired;

            GetInspectorElementsListFromTarget();

            InitializeProperties();
            SetPropertyConditions();

            OnEnableLast();
            
            _aceLogger.LogEnd();
        }

        /// <summary>
        ///     If you override this method, be sure to call base.OnDisable to ensure cleanup is still done.
        /// </summary>
        private void OnDisable()
        {
            SceneView.duringSceneGui -= DuringSceneGUI;
            if ( _theme != null )
            {
                _theme.OnDataUpdated -= OnThemeUpdated;
                _theme.OnUIStateUpdated -= OnThemeUpdated;
            }

            _targetScript.OnTargetUIStateChanged -= OnTargetUiStateUpdated;
            _targetScript.OnDataUpdateRequired -= OnTargetDataUpdateRequired;
            _themeManager.OnThemeAssignmentChanged -= OnTargetsThemeAssignmentUpdated;

            OnDisableInclusions();
        }

        private void InitializeTargetsTheme()
        {
            _aceLogger.LogStart();

            // If the theme is not null when OnEnable is called, this is likely the result of a compilation. If so, the subscriptions
            // would be lost so we'll re-up them.
            if ( _theme != null )
            {
                _theme.OnDataUpdated -= OnThemeUpdated;
                _theme.OnDataUpdated += OnThemeUpdated;
                _theme.OnUIStateUpdated -= OnThemeUpdated;
                _theme.OnUIStateUpdated += OnThemeUpdated;
                _aceLogger.LogEnd( $"Theme {_theme.name} was not null. Re-established subscriptions." );
                return;
            }

            _theme = _themeManager.GetThemeForScript( _targetScript.GetType().ToString().Split( '.' ).Last() );
            _theme.OnDataUpdated += OnThemeUpdated;
            _theme.OnUIStateUpdated += OnThemeUpdated;

            _aceLogger.LogEnd( $"Theme was null. Retrieve theme {GetColoredStringOrange(_theme.name)} and subscribed to its events." );
        }
        
        /// <summary>
        ///     Run in both Awake and OnEnable to ensure assets are loaded. On start up and during a game start, Awake runs, then OnEnable.
        ///     But after compilation, only OnEnable runs which will cause errors if assets aren't checked for values and loaded in each case.
        /// </summary>
        private void Initialize()
        {
            if ( _aceLogger == null )
            {
                _aceLogger = GetAssetByName<CustomLogger>( MonobehaviourRootEditorLoggerName, LoggersSearchFolderName );
                if ( _aceLogger == null ) Debug.LogError( $"Loading of {MonobehaviourRootEditorLoggerName} {GetColoredStringFireBrick( "failed" )}!" );
                // Debug.Log( $"Loading of {MonobehaviourRootEditorLoggerName}: {( ( _aceLogger == null ) ? $"{GetColoredStringIndianRed( "failed" )}" : $"{GetColoredStringGreen( "successful" )}" )}" );
                // Debug.LogWarning( $"AMRE|OE: Loading of {MonobehaviourRootEditorLoggerName}: {( _aceLogger == null ? "failed" : "successful" )}" );
            }
            
            if ( _targetScript == null )
                _targetScript = (AceMonobehaviourRoot) target;
            
            _aceLogger.LogStart( true, $"{_targetScript.name} is initializing (happens in Awake and OnEnable)." );

            if ( _userLoggerProperty == null )
            {
                _aceLogger.Log( $"Loading user logger: {_targetScript.GetLoggerName()}." );
                _userLoggerProperty = serializedObject.FindProperty( "logger" );
                LoadAssetToPropertyByNameWithDefault( _userLoggerProperty, _targetScript.GetLoggerName(), DefaultMbRootLoggerName, LoggersSearchFolderName );
                _aceLogger.LogObjectAssignmentResult( "User logger (user defined or default)", _userLoggerProperty.objectReferenceValue );
            }

            if ( _themeManager == null )
            {
                _aceLogger.Log( $"Loading themeManager: {ThemeManagerCoreName}." );
                _themeManager = GetAssetByName<AceThemeManager>( ThemeManagerCoreName, SystemCoreSearchFolderName );
                _aceLogger.LogObjectAssignmentResult( ThemeManagerCoreName, _themeManager );
            }
        }

        private void UpdateTargetsTheme()
        {
            _aceLogger.LogStart();
            _aceLogger.LogIndentStart( $"Updating {_targetScript.name}'s theme." );

            AceTheme newTheme = _themeManager.GetThemeForScript( _targetScript.GetType().ToString().Split( '.' ).Last() );
            _aceLogger.LogIndentStart( $"{_targetScript.name}'s theme was changed to {( newTheme != null ? newTheme.name : "null" )}." );

            if ( _theme != newTheme )
            {
                string previousThemeName = "null";
                if ( _theme != null )
                {
                    AceTheme previousTheme = _theme;
                    previousTheme.OnDataUpdated -= OnThemeUpdated;
                    previousTheme.OnUIStateUpdated -= OnThemeUpdated;
                    previousThemeName = previousTheme.name;
                }

                _theme = newTheme;
                _theme.OnDataUpdated += OnThemeUpdated;
                _theme.OnUIStateUpdated += OnThemeUpdated;

                _aceLogger.Log( $"{_targetScript.name} has unsubscribed from {previousThemeName}, and subscribed to changes in {_theme.name}." );
            }
            else
                _aceLogger.Log( $"No change in {_targetScript.name}'s theme detected." );

            _aceLogger.LogEnd();
        }

        private void GetInspectorElementsListFromTarget()
        {
            _aceLogger.LogStart();
            
            _inspectorElements = ConvertElementInfoList( _targetScript.GetElementInfoList() );

            // Initialize the inspector elements.
            foreach ( Element element in _inspectorElements )
            {
                element.Initialize( serializedObject, _theme, true );
            }

            _aceLogger.LogEnd();
        }

        /// <summary>
        ///     If you need to add things to this method, override OnInspectorGUIPreDraw or OnInspectorGUIPostDraw.
        ///     Do not override this method directly unless you want to fully take control over how the inspector
        ///     will be drawn.
        /// </summary>
        public override void OnInspectorGUI()
        {
            // _aceLogger.Log( $"Event.current: {( Event.current == null ? "null" : Event.current.type.ToString() )}" );

            OnInspectorGUIPreDraw();
            DrawCustomInspector();
            OnInspectorGUIPostDraw();
        }

        private void DrawCustomInspector()
        {
            const float guideLinesBrightness = 0.4f;
            const float graphLinesBrightness = 0.25f;
            
            if ( _theme.GetGlobalSettings().showGridLines )
                DrawVerticalGridLines( guideLinesBrightness, graphLinesBrightness );
            
            if ( _theme.GetGlobalSettings().showMeasurementLines )
                DrawMeasurementLines( 20, 100, new Color( graphLinesBrightness, graphLinesBrightness, graphLinesBrightness ) );

            if ( DrawInspectorElements() )
                OnPropertyChangesDetected();
        }

        private bool DrawInspectorElements()
        {
            serializedObject.Update();
            
            if ( !_theme.GetGlobalSettings().hideScriptField )
                DrawScriptField( serializedObject );
            
            foreach ( Element element in _inspectorElements )
            {
                element.DrawElement( true );
            }

            return serializedObject.ApplyModifiedProperties();
        }

#region Event Callbacks

        protected virtual void OnTargetDataUpdateRequired() => serializedObject.ApplyModifiedProperties();

        private void OnTargetsThemeAssignmentUpdated()
        {
            UpdateTargetsTheme();
            OnTargetUiStateUpdated();
        }

        private void OnTargetUiStateUpdated()
        {
            GetInspectorElementsListFromTarget();
            Repaint();
        }

        private void OnThemeUpdated() => Repaint();

        /// <summary>
        ///     Called in OnEnable before anything else.
        /// </summary>
        protected virtual void OnEnableFirst() {}

        /// <summary>
        ///     Call in OnEnable after everything else.
        /// </summary>
        protected virtual void OnEnableLast() {}

        /// <summary>
        ///     Use this method if you need to add something to OnDisable.
        /// </summary>
        protected virtual void OnDisableInclusions() {}

        /// <summary>
        ///     Call all initializers here (InitializeSection, InitializedMinMaxSlider, etc.), then use
        ///     AddInspectorElements to add all sections. This Method is called first in OnEnable.
        /// </summary>
        protected virtual void InitializeProperties() {}

        /// <summary>
        ///     Call ApplyPropertyConditionToSet to add a bool propertyInfo to a set of propertyInfo objects.
        ///     This method is called in OnEnable after InitializeProperties.
        /// </summary>
        protected virtual void SetPropertyConditions() {}

        /// <summary>
        ///     Called in OnInspectorGUI before inspector elements are drawn.
        /// </summary>
        protected virtual void OnInspectorGUIPreDraw() {}

        /// <summary>
        ///     Called in OnInspectorGUI after inspector elements are drawn.
        /// </summary>
        protected virtual void OnInspectorGUIPostDraw() {}

        /// <summary>
        ///     This will be called when SerializedObject.ApplyModifiedProperties().
        /// </summary>
        protected virtual void OnPropertyChangesDetected() {}

        /// <summary>
        ///     This method is subscribed to SceneView.duringSceneGui in OnEnable, and unsubscribed in OnDisable.
        ///     If you don't need it, just leave it blank. It's here so you don't have to override OnEnable/OnDisable to use it.
        /// </summary>
        protected virtual void DuringSceneGUI( SceneView sceneView ) {}

        private void OnUndoRedo()
        {
            OnUndoRedoPrePropUpdate();
            Repaint();
            OnUndoRedoPostPropUpdate();
        }

        /// <summary>
        ///     This method is called before calling ApplyModifiedProperties when an undo/redo event is triggered.
        /// </summary>
        protected virtual void OnUndoRedoPrePropUpdate() {}

        /// <summary>
        ///     This method is called after calling ApplyModifiedProperties when an undo/redo event is triggered.
        /// </summary>
        protected virtual void OnUndoRedoPostPropUpdate() {}
        
#endregion


        /// <summary>
        ///     For adding a single bool condition to a set of inspector elements, used to determine if the property should be active.
        ///     For non-bool property conditions use AddPropertyConditions.
        /// </summary>
        protected void AddBoolConditionToSet( ElementCondition elementCondition, params Element[] inspectorElements )
        {
            foreach ( Element element in inspectorElements )
            {
                element.AddElementConditions( elementCondition );
            }
        }
    }
}