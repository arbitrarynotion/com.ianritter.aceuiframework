namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.GroupElements.HeadingGroup
{
    public abstract class HeadingGroupDraw : GroupElementDraw
    {
        protected override GroupElement GroupElement => HeadingGroup;
        private HeadingGroup HeadingGroup { get; }


        protected HeadingGroupDraw( HeadingGroup headingGroup )
        {
            HeadingGroup = headingGroup;
        }


        protected override void DrawElementContents()
        {
            HeadingGroup.HeadingElement.DrawElement( false );

            if ( HeadingGroup.ShouldHideChildren() )
                return;

            HeadingGroup.ChildAreaGroup.DrawElement( false );
        }

        protected override bool ShouldShowLayoutTools() => true;
    }
}