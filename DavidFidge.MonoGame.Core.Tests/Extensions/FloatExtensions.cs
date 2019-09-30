using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DavidFidge.MonoGame.Core.Tests.Services
{
    public static class FloatExtensions
    {
        public static void IsEquivalentTo(this float source, float equivalent)
        {
            if (float.IsNaN(source) || float.IsNaN(equivalent))
            {
                Assert.IsTrue(float.IsNaN(source) && float.IsNaN(equivalent));
                return;
            }

            if (float.IsNegativeInfinity(source) || float.IsNegativeInfinity(equivalent))
            {
                Assert.IsTrue(float.IsNegativeInfinity(source) && float.IsNegativeInfinity(equivalent));
                return;
            }

            if (float.IsPositiveInfinity(source) || float.IsPositiveInfinity(equivalent))
            {
                Assert.IsTrue(float.IsPositiveInfinity(source) && float.IsPositiveInfinity(equivalent));
                return;
            }

            Assert.IsTrue(Math.Abs(source - equivalent) < 0.000001f);
        }
    }
}