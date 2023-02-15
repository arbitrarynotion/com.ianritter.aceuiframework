namespace ACEPackage.Editor.Scripts.Elements.SingleElements
{
    public abstract class SingleElementDraw : ElementDraw
    {
        protected abstract SingleElement SingleElement { get; }
        protected override Element Element => SingleElement;

        protected override bool ShouldShowLayoutTools() => Element.HasParent() && !Element.ParentIsCompositeGroup();
    }
}