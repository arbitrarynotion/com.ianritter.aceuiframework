namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.GroupElements
{
    /// <summary>
    ///     Used to store details about a line of elements to speed up performance by reducing per-frame calculations.
    ///     Used to store unchanging information about a line of elements.
    /// </summary>
    public class LineInfo
    {
        // Assigned during initialization.
        public readonly int NumberOfElements;
        public float ConstantWidthTotal;

        // Assigned during width and height calculations.
        public float Height;
        public float WidthPriorityTotal;


        public LineInfo( int numberOfElements )
        {
            NumberOfElements = numberOfElements;
        }

        
        public void SetWidthInfo( float constantWidthSum, float widthPrioritySum )
        {
            ConstantWidthTotal = constantWidthSum;
            WidthPriorityTotal = widthPrioritySum;
        }
    }
}