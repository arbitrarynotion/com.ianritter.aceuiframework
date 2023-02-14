namespace ACEPackage.Editor.Scripts.Elements.GroupElements.BasicGroup
{
    public class BasicGroupDraw : GroupElementDraw
    {
        protected override GroupElement GroupElement => _basicGroup;
        private readonly BasicGroup _basicGroup;

        public BasicGroupDraw( BasicGroup groupElement )
        {
            _basicGroup = groupElement;
        }

        protected override bool ShouldShowLayoutTools() => true;
    }
}
