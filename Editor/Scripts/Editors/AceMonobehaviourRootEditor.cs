using System.Linq;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.ACECore;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.ElementConditions;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.AceRoots;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.RuntimeElementBuilding;
using UnityEditor;
using UnityEngine;
using static Packages.com.ianritter.aceuiframework.Editor.Scripts.ElementBuilding.ElementInfoConverter;
using static Packages.com.ianritter.aceuiframework.Editor.Scripts.EditorGraphics.EditorMeasurementLineGraphics;
using static Packages.com.ianritter.aceuiframework.Runtime.Scripts.DefaultInspectorDrawing;
using static Packages.com.ianritter.aceuiframework.Editor.Scripts.ACECore.ThemeLoader;
using static Packages.com.ianritter.aceuiframework.Runtime.Scripts.AceEditorConstants;
using static Packages.com.ianritter.aceuiframework.Editor.Scripts.ACECore.AceDelegates;

namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.Editors
{
    [CustomEditor( typeof( AceMonobehaviourRoot ), true )]
    public class AceMonobehaviourRootEditor : UnityEditor.Editor
    {
        private SerializedObject _targetSerializedObject;
        private AceMonobehaviourRoot _targetScript;

        private Element[] _inspectorElements;
        private AceThemeManagerDatabase _themeManagerDatabase;

        private AceTheme _theme;

        public string GetTargetName() => _targetScript.name;

        /// <summary>
        ///     If you override this method, be sure to first call base.OnEnable to ensure set up is still done.
        /// </summary>
        private void OnEnable()
        {
            _themeManagerDatabase = LoadScriptableObject<AceThemeManagerDatabase>( ThemeManagerDatabaseCoreName );
            _themeManagerDatabase.OnThemeAssignmentChanged += OnTargetsThemeAssignmentUpdated;
            
            _targetSerializedObject = serializedObject;
            _targetScript = (AceMonobehaviourRoot) target;
            
            InitializeTargetsTheme();

            OnEnableFirst();
            
            SceneView.duringSceneGui += DuringSceneGUI;
            Undo.undoRedoPerformed += OnUndoRedo;
            
            _targetScript.OnTargetUIStateChanged += OnTargetUiStateUpdated;
            
            GetInspectorElementsListFromTarget();
            
            InitializeProperties();
            SetPropertyConditions();
            
            OnEnableLast();
        }

        private void InitializeTargetsTheme() => UpdateTargetsTheme();

        private void UpdateTargetsTheme()
        {
            AceTheme newTheme = _themeManagerDatabase.GetThemeForScript( _targetScript.GetType().ToString().Split( '.' ).Last() );
            Debug.Log( $"AMRE|UTT: {_targetScript.name}'s theme was changed to {(newTheme != null ? newTheme.name : "null")}." );
            
            // Initialize theme on first run.
            if ( _theme == null )
            {
                Debug.Log( $"AMRE|UTT: {_targetScript.name}'s theme was null." );
                _theme = newTheme;
                _theme.OnDataUpdated += OnThemeUpdated;
                _theme.OnUIStateUpdated += OnThemeUpdated;
                Debug.Log( $"AMRE|UTT:     {_targetScript.name} has subscribed to changes in {_theme.name}." );
                _theme.PrintMyUIStateUpdatedNotifySubscribers();
            }
            else
            {
                AceTheme previousTheme = _theme;
                _theme = newTheme;
                
                _theme.OnDataUpdated += OnThemeUpdated;
                _theme.OnUIStateUpdated += OnThemeUpdated;
                
                previousTheme.OnDataUpdated -= OnThemeUpdated;
                previousTheme.OnUIStateUpdated -= OnThemeUpdated;
                Debug.Log( $"AMRE|UTT:     {_targetScript.name} has unsubscribed from {previousTheme.name}, and subscribed to changes in {_theme.name}." );
            }
        }

        private void OnTargetsThemeAssignmentUpdated()
        {
            UpdateTargetsTheme();
            OnTargetUiStateUpdated();
        }

        private void GetInspectorElementsListFromTarget()
        {
            // _inspectorElements = _targetScript.GetElementList();

            
            ElementInfo[] elementInfos = _targetScript.GetElementInfoList();
            _inspectorElements = new Element[ elementInfos.Length ];
            for (int i = 0; i < elementInfos.Length; i++)
            {
                _inspectorElements[i] = ConvertElement( elementInfos[i] );
            }

            // Initialize the inspector elements.
            foreach (Element element in _inspectorElements)
            {
                element.Initialize( _targetSerializedObject, _theme, true );
            }
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
        protected virtual void OnEnableFirst(){}
        
        /// <summary>
        ///     Call in OnEnable after everything else.
        /// </summary>
        protected virtual void OnEnableLast(){}
        
        /// <summary>
        ///     If you override this method, be sure to call base.OnDisable to ensure cleanup is still done.
        /// </summary>
        private void OnDisable()
        {
            SceneView.duringSceneGui -= DuringSceneGUI;
            if (_theme != null)
                _theme.OnDataUpdated -= OnThemeUpdated;
            
            _targetScript.OnTargetUIStateChanged -= OnTargetUiStateUpdated;
            _themeManagerDatabase.OnThemeAssignmentChanged -= OnTargetsThemeAssignmentUpdated;
            
            OnDisableInclusions();
        }
        
        /// <summary>
        ///     Use this method if you need to add something to OnDisable.
        /// </summary>
        protected virtual void OnDisableInclusions(){}

        /// <summary>
        ///     Call all initializers here (InitializeSection, InitializedMinMaxSlider, etc.), then use
        ///     AddInspectorElements to add all sections. This Method is called first in OnEnable.
        /// </summary>
        protected virtual void InitializeProperties(){}

        /// <summary>
        ///     Call ApplyPropertyConditionToSet to add a bool propertyInfo to a set of propertyInfo objects.
        ///     This method is called in OnEnable after InitializeProperties.
        /// </summary>
        protected virtual void SetPropertyConditions(){}
    
        /// <summary>
        ///     If you need to add things to this method, override OnInspectorGUIPreDraw or OnInspectorGUIPostDraw.
        ///     Do not override this method directly unless you want to fully take control over how the inspector
        ///     will be drawn.
        /// </summary>
        public override void OnInspectorGUI()
        {
            DrawCustomInspector();
        }

        private void DrawCustomInspector()
        {
            const float guideLinesBrightness = 0.4f;
            const float graphLinesBrightness = 0.25f;
            if (_theme.GetGlobalSettings().showGridLines)
                DrawVerticalGridLines( guideLinesBrightness, graphLinesBrightness );
            if (_theme.GetGlobalSettings().showMeasurementLines)
                DrawMeasurementLines( 20, 100, new Color( graphLinesBrightness, graphLinesBrightness, graphLinesBrightness) );

            if (DrawInspectorElements()) 
                OnPropertyChangesDetected();
        }

        private bool DrawInspectorElements()
        {
            _targetSerializedObject.Update();
            DrawScriptField( _targetSerializedObject );
            foreach (Element element in _inspectorElements)
            {
                element.DrawElement( true );
            }

            return _targetSerializedObject.ApplyModifiedProperties();
        }
        
        /// <summary>
        ///     Called in OnInspectorGUI before inspector elements are drawn.
        /// </summary>
        protected virtual void OnInspectorGUIPreDraw(){}
        
        /// <summary>
        ///     Called in OnInspectorGUI after inspector elements are drawn.
        /// </summary>
        protected virtual void OnInspectorGUIPostDraw(){}

        /// <summary>
        ///     This will be called when SerializedObject.ApplyModifiedProperties().
        /// </summary>
        protected virtual void OnPropertyChangesDetected(){}

        /// <summary>
        ///     This method is subscribed to SceneView.duringSceneGui in OnEnable, and unsubscribed in OnDisable.
        ///     If you don't need it, just leave it blank. It's here so you don't have to override OnEnable/OnDisable to use it.
        /// </summary>
        protected virtual void DuringSceneGUI( SceneView sceneView ){}

        private void OnUndoRedo()
        {
            OnUndoRedoPrePropUpdate();
            Repaint(); // Try repaint instead. May accomplish the same task but without the null value problem.
            OnUndoRedoPostPropUpdate();
        }

        /// <summary>
        ///     This method is called before calling ApplyModifiedProperties when an undo/redo event is triggered.
        /// </summary>
        protected virtual void OnUndoRedoPrePropUpdate(){}
        
        /// <summary>
        ///     This method is called after calling ApplyModifiedProperties when an undo/redo event is triggered.
        /// </summary>
        protected virtual void OnUndoRedoPostPropUpdate(){}

        /// <summary>
        ///     For adding a single bool condition to a set of inspector elements, used to determine if the property should be active.
        ///     For non-bool property conditions use AddPropertyConditions.
        /// </summary>
        protected void AddBoolConditionToSet( ElementCondition elementCondition, params Element[] inspectorElements )
        {
            foreach (Element element in inspectorElements)
            {
                element.AddElementConditions( elementCondition );
            }
        }
    }
}
