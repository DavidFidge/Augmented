using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;

namespace DavidFidge.MonoGame.Core.Graphics.Extensions
{
    public static class MatrixExtensions
    {
        public static Vector3 Scale(this Matrix matrix)
        {
            var vector3 = new Vector3(matrix.M11, matrix.M22, matrix.M33);

            return vector3;
        }
    }
}
