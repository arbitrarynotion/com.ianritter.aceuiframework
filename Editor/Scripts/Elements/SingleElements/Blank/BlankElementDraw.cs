namespace ACEPackage.Editor.Scripts.Elements.SingleElements.Blank
{
    public class BlankElementDraw : SingleElementDraw
    {
        private readonly BlankElement _blankElement;
        protected override SingleElement SingleElement => _blankElement;

        
        public BlankElementDraw( BlankElement blankElement )
        {
            _blankElement = blankElement;
        }
        

        protected override void DrawElementContents() {}
    }
}
