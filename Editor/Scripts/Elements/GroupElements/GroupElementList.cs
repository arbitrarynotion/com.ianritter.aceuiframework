using System;
using System.Collections.Generic;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.ACECore;
using Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.SingleElements.Decorator.Label;
using Packages.com.ianritter.aceuiframework.Runtime.Scripts.SettingsCustom.Groups;
using UnityEditor;
using UnityEngine;

namespace Packages.com.ianritter.aceuiframework.Editor.Scripts.Elements.GroupElements
{
    public class GroupElementList
    {
        private readonly GroupElement _groupElement;
        private Element[][] _elementLines;
        private Element[] _elements;
        private LineInfo[] _lineInfoList;
        
        
        public GroupElementList( GroupElement groupElement )
        {
            _groupElement = groupElement;
            _elements = new Element[]
            {
                new LabelElement( new GUIContent( "This section has not been correctly initialized or is empty." ) )
            };
        }


        /// <summary>
        ///     Takes a list of elements assembled by the target script, adds spacer elements, and moves the finished
        ///     array to this class's local array.
        /// </summary>
        /// <param name="newElements"></param>
        public void AddElements( params Element[] newElements )
        {
            foreach ( Element element in newElements )
            {
                element.ParentElement = _groupElement;
            }

            _elements = newElements;
        }

        /// <summary>
        ///     Initialize all elements placed in the local Element list, formed in AddElements.
        /// </summary>
        public void InitializeElementsList( SerializedObject targetSerializedObject,
            AceTheme newAceTheme,
            GroupCustomSettings groupCustomSettings )
        {
            foreach ( Element element in _elements )
            {
                element.Initialize( targetSerializedObject, newAceTheme, _groupElement.DrawnInInspector );
            }

            SetLineAndColumnInfo( groupCustomSettings.NumberOfColumns );
            BuildElementLinesList();
            SetElementLevels( newAceTheme );
        }

        private void SetElementLevels( AceTheme newAceTheme )
        {
            int maxElementLevel = ( newAceTheme.GetTotalSettingsLevels() - 1 );
            _groupElement.ElementLevel = Mathf.Min( _groupElement.GetElementLevel(), maxElementLevel );
            foreach ( Element element in _elements )
            {
                element.ElementLevel = Mathf.Min( element.GetElementLevel(), maxElementLevel );
            }
        }

        /// <summary>
        ///     Determine and record what elements will go on what lines when drawn to the editor window. The line and column
        ///     number are saved to each element to speed up references later along with some details like if they are first or
        ///     last on the line.
        /// </summary>
        private void SetLineAndColumnInfo( float numberOfColumns )
        {
            List<LineInfo> lineInfoList = new List<LineInfo>();
            int line = 0;
            int column = 0;
            for (int i = 0; i < _elements.Length; i++)
            {
                Element element = _elements[i];
                ElementLayout elementLayout = _elements[i].Layout;

                if ( elementLayout == null )
                    throw new NullReferenceException( $"GEL|SLACI: {_groupElement.GetName()}: Error! \"{element.GetName()}\"'s layout is null!" );
                

                // if ( element.GetType() == typeof( LabelElement ) && element.GUIContent.text.Equals( "Super Coolant Fluid" ) )
                // {
                //     Debug.Log( $"Found a label element. {element.GUIContent.text}" );
                //     Debug.Log( $"    ForceSingleLine is set to: {element.CustomSettings.ForceSingleLine.ToString()}" );
                // }

                if ( element.CustomSettings.ForceSingleLine )
                {
                    // Element would otherwise be next on the current line.
                    if ( column != 0 )
                    {
                        // Finish previous line. Last element on previous line gets its last element bool set to true.
                        _elements[i - 1].Layout.IsLastOnLine = true;
                        lineInfoList.Add( new LineInfo( column ) );
                        line++;

                        // Place this element on its own line and move to next line.
                        column = 0;
                        elementLayout.SetListPositionInfo( line, column, true, true );
                        lineInfoList.Add( new LineInfo( column + 1 ) );
                        line++;
                        continue;
                    }

                    // Element was the first on this line so just place it on its own line and move to next line.
                    elementLayout.SetListPositionInfo( line, column, true, true );
                    lineInfoList.Add( new LineInfo( column + 1 ) );
                    line++;
                    continue;
                }

                // Place element and continue on same line.
                elementLayout.SetListPositionInfo( line, column );
                if ( column == 0 )
                    elementLayout.IsFirstOnLine = true;
                column++;

                // If all columns on this line are filled or this is the last element, finish current line.
                if ( column > numberOfColumns - 1 || i == _elements.Length - 1 )
                {
                    elementLayout.IsLastOnLine = true;
                    lineInfoList.Add( new LineInfo( column ) );
                    column = 0;
                    line++;
                }
            }

            _lineInfoList = lineInfoList.ToArray();
        }

        /// <summary>
        ///     Builds a the list that contains a list for each line in the group. Also populates the line info list for
        ///     faster references to key info like how many elements have a constant width.
        /// </summary>
        private void BuildElementLinesList()
        {
            _elementLines = new Element[_lineInfoList.Length][];
            
            int currentElementIndex = 0;
            for (int line = 0; line < _lineInfoList.Length; line++)
            {
                for (int i = 0; i < _elements.Length; i++)
                {
                    // Find where the current line starts in the elements list.
                    if ( _elements[currentElementIndex].Layout.Line == line )
                        break;

                    currentElementIndex++;
                }

                float constantWidthTotal = 0;

                // Now we're on the correct line, grab the elements on this line and collect info for line info entry.
                _elementLines[line] = new Element[_lineInfoList[line].NumberOfElements];
                for (int column = 0; column < _elementLines[line].Length; column++)
                {
                    Element element = _elements[currentElementIndex++];
                    _elementLines[line][column] = element;

                    if ( !( element.CustomSettings.ConstantWidth > 0 ) ) continue;
                    
                    constantWidthTotal += element.CustomSettings.ConstantWidth;
                    element.Layout.Width = element.CustomSettings.ConstantWidth;
                }

                // Set number of constant width elements on this line.
                _lineInfoList[line].SetWidthInfo( constantWidthTotal, GetNewWidthPriorityTotalForLine( line ) );

                // Cache the number of elements on each elements line with the elements themselves as it is frequently referenced.
                for (int column = 0; column < _elementLines[line].Length; column++)
                {
                    _elementLines[line][column].Layout.NumberOfNeighbors = _lineInfoList[line].NumberOfElements - 1;
                }
            }
        }

        private float GetNewWidthPriorityTotalForLine( int line )
        {
            float widthPriorityTotal = 0;
            for (int column = 0; column < _elementLines[line].Length; column++)
            {
                Element element = GetElement( line, column );

                bool firstElementHasDefaultLabelWidth = element.Layout.IsFirstOnLine && element.CustomSettings.UseIndentedDefaultLabelWidth;

                if ( element.CustomSettings.ConstantWidth <= 0 && !firstElementHasDefaultLabelWidth )
                    widthPriorityTotal += element.Layout.ColumnWidthPriority;
            }

            return widthPriorityTotal;
        }


        /// <summary>
        ///     Get number of elements in this group.
        /// </summary>
        public int GetNumberOfElements() => _elements.Length;

        /// <summary>
        ///     Get number of lines required to draw all group elements.
        /// </summary>
        public int GetNumberOfLines() => _lineInfoList.Length;

        /// <summary>
        ///     Get the number elements that will be drawn on this line.
        /// </summary>
        public int GetNumberOfElementOnLine( int line ) => _lineInfoList[line].NumberOfElements;

        /// <summary>
        ///     Get the height of the tallest element on this line.
        /// </summary>
        public float GetHeightOfLine( int line ) => _lineInfoList[line].Height;

        /// <summary>
        ///     Get the element for the given line and column in this groups elements list.
        /// </summary>
        public Element GetElement( int line, int column ) => _elementLines[line][column];

        /// <summary>
        ///     Set the height required for this line to fit its elements. This is set to the height of the tallest element.
        /// </summary>
        public void SetHeightForLine( int line, float height )
        {
            _lineInfoList[line].Height = height;
        }

        /// <summary>
        ///     Get the amount of width taken up by elements using constant width.
        /// </summary>
        public float GetConstantWidthTotalForLine( int line ) => _lineInfoList[line].ConstantWidthTotal;

        /// <summary>
        ///     Get the sum of all of this line's width priorities.
        /// </summary>
        public float GetWidthPriorityTotalForLine( int line ) => _lineInfoList[line].WidthPriorityTotal;
    }
}