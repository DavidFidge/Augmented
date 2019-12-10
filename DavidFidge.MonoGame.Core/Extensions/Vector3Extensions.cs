using Microsoft.Xna.Framework;

namespace DavidFidge.MonoGame.Core.Extensions
{
    public static class Vector3Extensions
    {
        public static Vector3 Truncate(this Vector3 vector3, float limit)
        {
            if (vector3.LengthSquared() > limit * limit)
                return Vector3.Normalize(vector3) * limit;

            return vector3;
        }
    }
}
