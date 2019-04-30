using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DavidFidge.MonoGame.Core.Tests.Services
{
    public static class FloatExtensions
    {
        public static void IsEquivalentTo(this float source, float equivalent)
        {
            Assert.IsTrue(Math.Abs(source - equivalent) < 0.000001f);
        }
    }
}