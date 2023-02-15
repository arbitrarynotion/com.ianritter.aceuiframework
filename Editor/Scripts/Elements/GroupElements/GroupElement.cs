using ACEPackage.Editor.Scripts.ACECore;
using ACEPackage.Runtime.Scripts.SettingsCustom;
using ACEPackage.Runtime.Scripts.SettingsCustom.Groups;
using UnityEditor;
using UnityEngine;

namespace ACEPackage.Editor.Scripts.Elements.GroupElements
{
    public abstract class GroupElement : Element
    {
        private GroupElementList GroupElementList { get; set; }

        public GroupCustomSettings GroupCustomSettings { get; }
        public abstract GroupElementLayout GroupElementLayout { get; }

        public override CustomSettings CustomSettings => GroupCustomSettings;
        public override ElementLayout Layout => GroupElementLayout;


        protected GroupElement( GUIContent guiContent, GroupCustomSettings groupCustomSettings ) :
            base( guiContent )
        {
            GroupCustomSettings = groupCustomSettings ?? new GroupCustomSettings();
        }
        
        
        protected void InstantiateGroupData( Element[] newElements )
        {
            GroupElementList = new GroupElementList( this );
            GroupElementList.AddElements( newElements );
        }
        
        
        // These are all required to avoid making group element list public. 

        public void InitializeElementsList( SerializedObject targetSerializedObject,
            AceTheme newAceTheme,
            GroupCustomSettings groupCustomSettings )
        {
            GroupElementList.InitializeElementsList( targetSerializedObject, newAceTheme, groupCustomSettings );
        }

        public int GetNumberOfLines() => GroupElementList.GetNumberOfLines();
        public int GetNumberOfElements() => GroupElementList.GetNumberOfElements();
        public float GetHeightOfLine( int line ) => GroupElementList.GetHeightOfLine( line );
        public int GetNumberOfElementsOnLine( int line ) => GroupElementList.GetNumberOfElementOnLine( line );
        public Element GetElement( int line, int column ) => GroupElementList.GetElement( line, column );
        public float GetWidthPriorityTotalForLine( int line ) => GroupElementList.GetWidthPriorityTotalForLine( line );
        public float GetConstantWidthTotalForLine( int line ) => GroupElementList.GetConstantWidthTotalForLine( line );
        public void SetHeightForLine( int line, float heightOfTallestElement ) => GroupElementList.SetHeightForLine( line, heightOfTallestElement );
    }
}
