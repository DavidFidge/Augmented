using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;

namespace DavidFidge.MonoGame.Core.Graphics.Extensions
{
    public static class Vector3Extensions
    {
        public static Matrix CreateScale(this Vector3 vector3)
        {
            var matrix = Matrix.Identity;
            matrix.M11 = vector3.X;
            matrix.M22 = vector3.Y;
            matrix.M33 = vector3.Z;
            matrix.M44 = 1f;

            return matrix;
        }
    }
}
