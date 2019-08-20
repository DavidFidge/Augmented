﻿using System;
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
        private IRandom _random2;
        private IRandom _randomSubtractingHeights;

        private DiamondSquare _diamondSquare;

        [TestInitialize]
        public override void Setup()
        {
            base.Setup();

            _random = new TestRandom();
            _random2 = new TestRandom2();
            _randomSubtractingHeights = new TestRandomSubtractingHeights();
        }

        [TestMethod]
        public void Should_Create_Smallest_Size_HeightMap_Without_Reducing_Height_On_First_Diamond_Step()
        {
            // Arrange
            _diamondSquare = new DiamondSquare(_random);

            // Act
            var result = _diamondSquare.Execute(2, -100, 100).HeightMap;

            // Assert
            var expectedMap = new int[3 * 3]
            {
                0,  113,  0, 
                113, 100, 113, 
                0,  113,  0
            };

            CollectionAssert.AreEquivalent(expectedMap, result.ToArray());
        }

        [TestMethod]
        public void Should_Create_HeightMap_Which_Runs_Two_Diamond_Square_Steps()
        {
            // Arrange
            _diamondSquare = new DiamondSquare(_random);

            // Act
            var result = _diamondSquare.Execute(4, -100, 100).HeightMap;

            // Assert
            var expectedMap = new int[5 * 5]
            {
                0,   84,  113,  84,  0,
                84,  111, 118,  111, 84,
                113, 118, 100,  118, 113,
                84,  111, 118,  111, 84,
                0,   84,  113,   84,  0
            };

            CollectionAssert.AreEquivalent(expectedMap, result.ToArray());
        }

        [TestMethod]
        public void Should_Create_HeightMap_Which_Runs_Two_Diamond_Square_Steps2()
        {
            // Arrange
            _diamondSquare = new DiamondSquare(_random2);

            // Act
            var result = _diamondSquare.Execute(8, -100, 100).HeightMap;

            // Assert
            var expectedMap = new int[9 * 9]
            {
                0,42,78,91,113,91,78,42,0,
                42,46,67,80,95,80,67,46,42,
                78,67,61,82,103,82,61,67,78,
                91,80,82,83,93,83,82,80,91,
                113,95,103,93,100,93,103,95,113,
                91,80,82,83,93,83,82,80,91,
                78,67,61,82,103,82,61,67,78,
                42,46,67,80,95,80,67,46,42,
                0,42,78,91,113,91,78,42,0
            };

            CollectionAssert.AreEquivalent(expectedMap, result.ToArray());
        }


        [TestMethod]
        public void Should()
        {
            // Arrange
            var subtractingHeightsReducer = new SubtractingHeightsReducer();
            _diamondSquare = new DiamondSquare(_random)
            {
                HeightsReducer = subtractingHeightsReducer
            };

            // Act
            var result = _diamondSquare.Execute(4, -100, 100).HeightMap;

            // Assert
            var expectedMap = new int[5 * 5]
            {
                0,   84,  113,  84,  0,
                84,  111, 118,  111, 84,
                113, 118, 100,  118, 113,
                84,  111, 118,  111, 84,
                0,   84,  113,   84,  0
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
                if (min == -100 && max == 100)
                {
                    return 80;
                }

                if (min == -50 && max == 50)
                {
                    return 30;
                }

                if (min == -25 && max == 25)
                {
                    return 10;
                }

                throw new Exception("Unexpected call to IRandom");
            }
        }

        private class TestRandom2 : IRandom
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
                if (min == -100 && max == 100)
                {
                    return 80;
                }

                if (min == -50 && max == 50)
                {
                    return -20;
                }

                if (min == -25 && max == 25)
                {
                    return 20;
                }
                if (min == -12 && max == 12)
                {
                    return -8;
                }

                if (min == -6 && max == 6)
                {
                    return 1;
                }

                throw new Exception("Unexpected call to IRandom");
            }
        }

        private class TestRandomSubtractingHeights : IRandom
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
                if (min == -100 && max == 100)
                {
                    return 80;
                }

                if (min == 75 && max == 75)
                {
                    return 30;
                }

                if (min == -50 && max == 50)
                {
                    return 10;
                }

                if (min == -50 && max == 50)
                {
                    return 10;
                }

                throw new Exception("Unexpected call to IRandom");
            }
        }
    }
}