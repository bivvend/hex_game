using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

namespace Scripts.Tiles
{
    //Useful methods for handling hexes
    public static class TIleUtilities
    {
        /// <summary>
        /// Converts hex basis vector indicies into XY coords.  Note: No scaling
        /// For flat top configuration
        /// </summary>
        /// <param name="q"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        public static PointF convertHexIndiciesToCartesianFlatTop(int q, int r)
        {
            var x = (3.0f / 2.0f * q);
            var y =Mathf.Sqrt(3.0f) / 2.0f * q + Mathf.Sqrt(3.0f) * r;
            return new PointF(x, y);


        }


        //    function flat_hex_to_pixel(hex):



    }
}
