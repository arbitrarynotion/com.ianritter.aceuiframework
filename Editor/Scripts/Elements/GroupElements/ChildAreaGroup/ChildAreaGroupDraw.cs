namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.GroupElements.ChildAreaGroup
{
    public class ChildAreaGroupDraw : GroupElementDraw
    {
        protected override GroupElement GroupElement => _childAreaGroup;
        private readonly ChildAreaGroup _childAreaGroup;
        
        
        public ChildAreaGroupDraw( ChildAreaGroup groupElement )
        {
            _childAreaGroup = groupElement;
        }

        // ShowLayoutTools defaults to true so this makes the child area's layout tools visibility dependent on it's parent.
        protected override bool ShouldShowLayoutTools() => _childAreaGroup.ParentElement.Settings.showLayoutTools;
    }
}
