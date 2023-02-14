using UnityEngine;

namespace ACEPackage.Editor.Scripts.Elements.GroupElements
{
    public abstract class GroupElementDraw : ElementDraw
    {
        protected abstract GroupElement GroupElement { get; }
        protected override Element Element => GroupElement;


        protected override void DrawElementContents()
        {
            int numberOfLines = GroupElement.GetNumberOfLines();
            for (int line = 0; line < numberOfLines; line++)
            {
                for (int column = 0; column < GroupElement.GetNumberOfElementsOnLine( line ); column++)
                {
                    Element element = GroupElement.GetElement( line, column );
                    if ( element == null )
                    {
                        Debug.LogWarning( $"GED|DES: {GroupElement.GetName()} has a null child!." );
                        continue;
                    }

                    if ( !element.HeadingIsExpanded() )
                        continue;

                    element.DrawElement( false );
                }
            }
        }
    }
}