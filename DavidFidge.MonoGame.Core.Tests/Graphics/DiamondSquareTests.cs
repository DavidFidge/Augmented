using System;
using System.Linq;

using DavidFidge.MonoGame.Core.Graphics.Terrain;
using DavidFidge.MonoGame.Core.Interfaces.Components;
using DavidFidge.TestInfrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DavidFidge.MonoGame.Core.Tests.Graphics
{
    [TestClass]
    public class DiamondSquareTests : BaseTest
    {
        private IRandom _random;
        private DiamondSquare _diamondSquare;

        [TestInitialize]
        public override void Setup()
        {
            base.Setup();

            _random = new TestRandom();
            _diamondSquare = new DiamondSquare(_random);
        }

        [TestMethod]
        public void Should_Create_Smallest_Size_HeightMap_Without_Reducing_Height_On_First_Diamond_Step()
        {
            // Act
            var result = _diamondSquare.Execute(2, 0, 100).HeightMap;

            // Assert
            var expectedMap = new int[3 * 3]
            {
                0,  80,  0, 
                80, 100, 80, 
                0,  80,  0
            };

            CollectionAssert.AreEquivalent(expectedMap, result.ToArray());
        }

        [TestMethod]
        public void Should_Create_HeightMap_Which_Runs_Two_Diamond_Square_Steps()
        {
            // Act
            var result = _diamondSquare.Execute(4, 0, 100).HeightMap;

            // Assert
            var expectedMap = new int[5 * 5]
            {
                0,  10, 80,  10, 0,
                10, 30, 10,  30, 10,
                80, 10, 100, 10, 80,
                10, 30, 10,  30, 10,
                0,  10, 80,  10, 0
            };

            CollectionAssert.AreEquivalent(expectedMap, result.ToArray());
        }

        private class TestRandom : IRandom
        {
            public double NextDouble()
            {
                throw new NotImplementedException();
            }

            public int Next()
            {
                throw new NotImplementedException();
            }

            public int Next(int min, int max)
            {
                if (min == 0 && max == 100)
                {
                    return 80;
                }

                if (min == 0 && max == 50)
                {
                    return 30;
                }

                if (min == 0 && max == 25)
                {
                    return 10;
                }

                throw new Exception("Unexpected call to IRandom");
            }
        }
    }
}