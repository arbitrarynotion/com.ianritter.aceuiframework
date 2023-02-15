namespace ACEPackage.Editor.Scripts.Elements.GroupElements.CompositeGroup
{
    public class CompositeGroupDraw : GroupElementDraw
    {
        protected override GroupElement GroupElement => _compositeGroup;
        private readonly CompositeGroup _compositeGroup;


        public CompositeGroupDraw( CompositeGroup groupElement )
        {
            _compositeGroup = groupElement;
        }


        // Hijacked from Single Element Layout.
        // Todo: Work out a way to have static group and single element share settings without duplicating code.
        protected override bool ShouldShowLayoutTools() => Element.HasParent() && !Element.ParentIsCompositeGroup();
    }
}