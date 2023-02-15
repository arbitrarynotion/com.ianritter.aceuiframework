using Packages.com.ianritter.aceuiframework.Editor.Scripts.ElementConditions;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsCustom.SingleElements;
using UnityEditor;
using UnityEngine;

namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.SingleElements.Blank
{
    public class BlankElement : SingleElement
    {
        private BlankElementLayout _blankElementLayout;
        private BlankElementDraw _blankElementDraw;

        public override SingleElementLayout SingleElementLayout => _blankElementLayout;
        public override SingleElementDraw SingleElementDraw => _blankElementDraw;
        
        
        public BlankElement( SingleCustomSettings singleCustomSettings ) 
            : base( GUIContent.none, singleCustomSettings, false, new ElementCondition[] {} )
        {
        }
        

        protected override void InitializeElement( SerializedObject targetScriptableObject ) {}
        
        protected override void InitializeLayout() => _blankElementLayout = new BlankElementLayout( this );
        
        protected override void InitializeDraw() => _blankElementDraw = new BlankElementDraw( this );
    }
}
