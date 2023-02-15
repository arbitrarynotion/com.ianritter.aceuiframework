using UnityEditor;
using UnityEngine;

namespace Packages.com.ianritter.aceuiframework.Runtime.Scripts
{
    public class DebugPrinting
    {
        public static void Print( string message )
        {
            if (Event.current.type == EventType.Repaint)
                Debug.Log( message );
        }

        public static void PrintSerializedObjectContents( SerializedObject so )
        {
            // List all properties in this so.
            SerializedProperty iterator = so.GetIterator();
            Debug.Log( "CIH|PSOC: ---------------------- SO Contents ----------------------" );
            while (iterator.Next( true ))
            {
                Debug.Log( $"CIE|OE: {iterator.name}" );
            }
            Debug.Log( "CIH|PSOC: /---------------------- SO Contents ----------------------" );

        }
    }
}
