namespace Packages.com.ianritter.aceuiframework.Runtime.Scripts.RuntimeElementBuilding
{
    public static class RuntimeListOperations
    {

// #region BuildNewList
//         
//         public static Element[] GetElementListFromTuples( (
//                 string varName, 
//                 string title, 
//                 string tooltip
//                 )[] varNameTitleAndToolTip )
//         {
//             Element[] elements = new Element[varNameTitleAndToolTip.Length];
//             for (int i = 0; i < elements.Length; i++)
//             {
//                 elements[i] = new BasicProperty
//                 ( 
//                     varNameTitleAndToolTip[i].varName, 
//                     new GUIContent( 
//                         varNameTitleAndToolTip[i].title, 
//                         varNameTitleAndToolTip[i].tooltip 
//                     ),
//                     new SingleCustomSettings(),
//                     null
//                 );
//             }
//
//             return elements;
//         }
//         
//         
//         public static Element[] GetElementListFromTuples( (
//                 string varName, 
//                 string title, 
//                 string tooltip
//                 )[] varNameTitleAndToolTip,
//             Action callback )
//         {
//             Element[] elements = new Element[varNameTitleAndToolTip.Length];
//             for (int i = 0; i < elements.Length; i++)
//             {
//                 elements[i] = new BasicProperty
//                 ( 
//                     varNameTitleAndToolTip[i].varName, 
//                     new GUIContent(
//                         varNameTitleAndToolTip[i].title,
//                         varNameTitleAndToolTip[i].tooltip
//                     ),
//                     new SingleCustomSettings(), 
//                     callback
//                 );
//             }
//
//             return elements;
//         }
//         
//         public static Element[] GetElementListFromTuples( (
//                 string varName, 
//                 GUIContent guiContent
//                 )[] varNameTitleAndToolTip ) 
//         {
//             Element[] elements = new Element[varNameTitleAndToolTip.Length];
//             for (int i = 0; i < elements.Length; i++)
//             {
//                 elements[i] = new BasicProperty
//                 ( 
//                     varNameTitleAndToolTip[i].varName, 
//                     varNameTitleAndToolTip[i].guiContent,
//                     new SingleCustomSettings(),
//                     null
//                 );
//             }
//
//             return elements;
//         }
//
//         public static Element[] GetElementListFromTuples( 
//             (
//                 string varName, 
//                 string title, 
//                 string tooltip, 
//                 SingleCustomSettings settings 
//                 )[] varNameTitleAndToolTip )
//         {
//             Element[] elements = new Element[varNameTitleAndToolTip.Length];
//             for (int i = 0; i < elements.Length; i++)
//             {
//                 elements[i] = new BasicProperty
//                 ( 
//                     varNameTitleAndToolTip[i].varName, 
//                     new GUIContent( 
//                         varNameTitleAndToolTip[i].title, 
//                         varNameTitleAndToolTip[i].tooltip 
//                     ), 
//                     varNameTitleAndToolTip[i].settings,
//                     null
//                 );
//             }
//
//             return elements;
//         }
//         
//         public static Element[] GetElementListFromTuples( 
//             (
//                 string varName, 
//                 GUIContent guiContent, 
//                 SingleCustomSettings settings 
//                 )[] varNameTitleAndToolTip )
//         {
//             Element[] elements = new Element[varNameTitleAndToolTip.Length];
//             for (int i = 0; i < elements.Length; i++)
//             {
//                 elements[i] = new BasicProperty
//                 ( 
//                     varNameTitleAndToolTip[i].varName, 
//                     varNameTitleAndToolTip[i].guiContent,
//                     varNameTitleAndToolTip[i].settings,
//                     null
//                 );
//             }
//
//             return elements;
//         }
//
//         public static Element[] GetElementListFromTuples( 
//             (
//                 string varName, 
//                 string title, 
//                 string tooltip, 
//                 ElementCondition[] filters 
//                 )[] varNameTitleAndToolTip )
//         {
//             Element[] elements = new Element[varNameTitleAndToolTip.Length];
//             for (int i = 0; i < elements.Length; i++)
//             {
//                 elements[i] = new BasicProperty
//                 ( 
//                     varNameTitleAndToolTip[i].varName, 
//                     new GUIContent(
//                         varNameTitleAndToolTip[i].title,
//                         varNameTitleAndToolTip[i].tooltip
//                     ), 
//                     new SingleCustomSettings(),
//                     null,
//                     false, 
//                     varNameTitleAndToolTip[i].filters 
//                 );
//             }
//
//             return elements;
//         }
//
//         public static Element[] GetElementListFromTuples( 
//             (
//                 string varName, 
//                 string title, 
//                 string tooltip, 
//                 SingleCustomSettings settings, 
//                 ElementCondition[] filters 
//                 )[] varNameTitleAndToolTip )
//         {
//             Element[] elements = new Element[varNameTitleAndToolTip.Length];
//             for (int i = 0; i < elements.Length; i++)
//             {
//                 elements[i] = new BasicProperty
//                 ( 
//                     varNameTitleAndToolTip[i].varName, 
//                     new GUIContent(
//                         varNameTitleAndToolTip[i].title,
//                         varNameTitleAndToolTip[i].tooltip
//                     ), 
//                     varNameTitleAndToolTip[i].settings, 
//                     null,
//                     false, 
//                     varNameTitleAndToolTip[i].filters 
//                 );
//             }
//
//             return elements;
//         }
//         
// #endregion
//
// #region AppendToExistingList
//         
//         public static void AppendElementListWithElements( 
//             Element[] elements, 
//             Element[] destinationArray, 
//             int startingIndex )
//         {
//             for (int i = 0; i < elements.Length; i++)
//             {
//                 destinationArray[startingIndex + i] = elements[i];
//             }
//         }
//         
//         public static void AppendElementListWithTuples( 
//             (
//                 string varName, 
//                 string title, 
//                 string tooltip
//                 ) [] varNameTitleAndToolTip, 
//             Element[] destinationArray, 
//             int startingIndex )
//         {
//             for (int i = startingIndex; i < varNameTitleAndToolTip.Length; i++)
//             {
//                 destinationArray[i] = new BasicProperty
//                 ( 
//                     varNameTitleAndToolTip[i].varName, 
//                     new GUIContent(
//                         varNameTitleAndToolTip[i].title,
//                         varNameTitleAndToolTip[i].tooltip
//                     ),
//                     new SingleCustomSettings(),
//                     null
//                 );
//             }
//         }
//
//         public static void AppendElementListWithTuples( (
//                 string varName, 
//                 string title, 
//                 string tooltip, 
//                 SingleCustomSettings settings 
//                 )[] varNameTitleAndToolTip, 
//             Element[] destinationArray, 
//             int startingIndex )
//         {
//             for (int i = startingIndex; i < varNameTitleAndToolTip.Length; i++)
//             {
//                 destinationArray[i] = new BasicProperty
//                 ( 
//                     varNameTitleAndToolTip[i].varName, 
//                     new GUIContent(
//                         varNameTitleAndToolTip[i].title,
//                         varNameTitleAndToolTip[i].tooltip
//                     ), 
//                     varNameTitleAndToolTip[i].settings,
//                     null
//                 );
//             }
//         }
//
//         public static void AppendElementListWithTuples( 
//             (
//                 string varName, 
//                 string title, 
//                 string tooltip, 
//                 ElementCondition[] filters 
//                 ) [] varNameTitleAndToolTip, 
//             Element[] destinationArray, 
//             int startingIndex )
//         {
//             for (int i = startingIndex; i < varNameTitleAndToolTip.Length; i++)
//             {
//                 destinationArray[i] = new BasicProperty
//                 ( 
//                     varNameTitleAndToolTip[i].varName, 
//                     new GUIContent(
//                         varNameTitleAndToolTip[i].title,
//                         varNameTitleAndToolTip[i].tooltip
//                     ), 
//                     new SingleCustomSettings(),
//                     null,
//                     false, 
//                     varNameTitleAndToolTip[i].filters 
//                 );
//             }
//         }
//
//         public static void AppendElementListWithTuples( 
//             (
//                 string varName, 
//                 string title, 
//                 string tooltip, 
//                 SingleCustomSettings settings, 
//                 ElementCondition[] filters 
//                 )[] varNameTitleAndToolTip, 
//             Element[] destinationArray, 
//             int startingIndex )
//         {
//             for (int i = startingIndex; i < varNameTitleAndToolTip.Length; i++)
//             {
//                 destinationArray[i] = new BasicProperty
//                 ( 
//                     varNameTitleAndToolTip[i].varName, 
//                     new GUIContent(
//                         varNameTitleAndToolTip[i].title,
//                         varNameTitleAndToolTip[i].tooltip
//                     ), 
//                     varNameTitleAndToolTip[i].settings, 
//                     null,
//                     false, 
//                     varNameTitleAndToolTip[i].filters 
//                 );
//             }
//         }
//
// #endregion
//
//
// #region CombineElementsAndTuplesLists
//         
//         public static Element[] GetCombinedElementAndTuplesList(
//             (
//                 string varName, 
//                 string title, 
//                 string tooltip
//                 )
//                 [] elementTuples, 
//             params Element[] elements )
//         {
//             Element[] returnElements = new Element[elementTuples.Length + elements.Length];
//             AppendElementListWithTuples( elementTuples, returnElements, 0 );
//             AppendElementListWithElements( elements, returnElements, elementTuples.Length );
//             return returnElements;
//         }
//         
//         public static Element[] GetCombinedElementAndTuplesList(
//             (
//                 string varName, 
//                 string title, 
//                 string tooltip, 
//                 SingleCustomSettings settings
//                 )
//                 [] elementTuples, 
//             params Element[] elements )
//         {
//             Element[] returnElements = new Element[elementTuples.Length + elements.Length];
//             AppendElementListWithTuples( elementTuples, returnElements, 0 );
//             AppendElementListWithElements( elements, returnElements, elementTuples.Length );
//             return returnElements;
//         }
//         
//         public static Element[] GetCombinedElementAndTuplesList(
//             (
//                 string varName, 
//                 string title, 
//                 string tooltip, 
//                 ElementCondition[] filters
//                 )
//                 [] elementTuples, 
//             params Element[] elements )
//         {
//             Element[] returnElements = new Element[elementTuples.Length + elements.Length];
//             AppendElementListWithTuples( elementTuples, returnElements, 0 );
//             AppendElementListWithElements( elements, returnElements, elementTuples.Length );
//             return returnElements;
//         }
//
//         public static Element[] GetCombinedElementAndTuplesList(
//             (
//                 string varName, 
//                 string title, 
//                 string tooltip, 
//                 SingleCustomSettings settings,
//                 ElementCondition[] filters
//                 )
//                 [] elementTuples, 
//             params Element[] elements )
//         {
//             Element[] returnElements = new Element[elementTuples.Length + elements.Length];
//             AppendElementListWithTuples( elementTuples, returnElements, 0 );
//             AppendElementListWithElements( elements, returnElements, elementTuples.Length );
//             return returnElements;
//         }
//         
// #endregion

    }
}
