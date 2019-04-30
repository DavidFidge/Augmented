using Microsoft.Xna.Framework;

namespace DavidFidge.MonoGame.Core.Tests.Services
{
    public static class Vector3Extensions
    {
        public static void IsEquivalentTo(this Vector3 source, Vector3 equivalent)
        {
            source.X.IsEquivalentTo(equivalent.X);
            source.Y.IsEquivalentTo(equivalent.Y);
            source.Z.IsEquivalentTo(equivalent.Z);
        }
    }
}