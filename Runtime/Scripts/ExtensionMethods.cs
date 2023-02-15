
using UnityEngine;

namespace Packages.com.ianritter.aceuiframework.Runtime.Scripts
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Rounds the vector3 to the nearest whole number using Mathf.Round on each axis.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Vector3 Round( this Vector3 v )
        {
            v.x = Mathf.Round( v.x );
            v.y = Mathf.Round( v.y );
            v.z = Mathf.Round( v.z );
            return v;
        }
    
        public static Vector3 Round( this Vector3 v, float size )
        {
            return Round( v / size ) * size;
        }

        public static float Round( this float v, float size )
        {
            return Mathf.Round( v / size ) * size;
        }

        /// <summary>
        /// The value will be equal to or greater than this value.
        /// </summary>
        /// <param name="v"></param>
        /// <param name="min"></param>
        /// <returns></returns>
        public static float AtLeast( this float v, float min )
        {
            return Mathf.Max( v, min );
        }

        /// <summary>
        /// The value will be equal to or greater than this value.
        /// </summary>
        /// <param name="v"></param>
        /// <param name="min"></param>
        /// <returns></returns>
        public static int AtLeast( this int i, int min )
        {
            return Mathf.Max( i, min );
        }

        /// <summary>
        /// The value will be equal to or less than this value.
        /// </summary>
        /// <param name="v"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static float AtMost( this float v, float max )
        {
            return Mathf.Min( v, max );
        }
    
        /// <summary>
        /// The value will be equal to or less than this value.
        /// </summary>
        /// <param name="v"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int AtMost( this int v, int max )
        {
            return Mathf.Min( v, max );
        }

        /// <summary>
        /// Round the float value to include this many digits past the decimal point.
        /// </summary>
        /// <param name="v"></param>
        /// <param name="digitsBeyondDecimal"></param>
        /// <returns></returns>
        public static float RoundToDecimalPlace( this float v, int digitsBeyondDecimal )
        {
            float roundingPrecision = Mathf.Pow( 10f, digitsBeyondDecimal );
            return Mathf.Round(v * roundingPrecision) * ( 1 / roundingPrecision );
        }


        // Round to the nearest specified increment.
        public static Vector3 CustomRound( this Vector3 v, float increment )
        {
            v.x = RoundFloatToIncrement( v.x, increment );
            v.y = RoundFloatToIncrement( v.y, increment );
            v.z = RoundFloatToIncrement( v.z, increment );
            return v;
        }

        public static float RoundFloatToIncrement( float value, float increment )
        {
            float remainder = value % increment;
            float incrementMidpoint = ( increment / 2 );
            float distanceAlongGrid = Mathf.Floor( value / increment ) * increment;
            return ( remainder < incrementMidpoint ) ? distanceAlongGrid : distanceAlongGrid + increment;
        }
    }
}
