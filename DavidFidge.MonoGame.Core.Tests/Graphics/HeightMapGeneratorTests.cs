using System;

using DavidFidge.MonoGame.Core.Graphics;
using DavidFidge.TestInfrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;

namespace DavidFidge.MonoGame.Core.Tests.Graphics
{
    [TestClass]
    public class HeightMapGeneratorTests : BaseTest
    {
        private HeightMapGenerator _heightMapGenerator;

        [TestInitialize]
        public override void Setup()
        {
            base.Setup();

            _heightMapGenerator = new HeightMapGenerator();
        }

        [TestMethod]
        public void CreateHeightMap_Should_Create_HeightMap_With_Supplied_Dimensions()
        {
            // Act
            var result = _heightMapGenerator.CreateHeightMap(10, 11);

            // Assert
            Assert.AreEqual(10, result.HeightMap.GetLength(0));
            Assert.AreEqual(11, result.HeightMap.GetLength(1));
        }

        [TestMethod]
        public void Should_Create_Basic_Hill()
        {
            // Act
            var result = _heightMapGenerator.CreateHeightMap(9, 9)
                .Hill(
                    new Vector2(0.5f, 0.5f),
                    new Vector2(0.7f, 0.7f),
                    3)
                .HeightMap;

            // Assert
            var expectedMap = new int[9, 9]
            {
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 1, 1, 1, 0, 0, 0 },
                { 0, 0, 1, 2, 2, 2, 1, 0, 0 },
                { 0, 0, 1, 2, 3, 2, 1, 0, 0 },
                { 0, 0, 1, 2, 2, 2, 1, 0, 0 },
                { 0, 0, 0, 1, 1, 1, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 }
            };

            CollectionAssert.AreEquivalent(expectedMap, result);
        }

        [TestMethod]
        public void Should_Create_Hill_TopLeft()
        {
            // Act
            var result = _heightMapGenerator.CreateHeightMap(5, 5)
                .Hill(
                    new Vector2(0f, 0f),
                    new Vector2(1f, 1f),
                    3)
                .HeightMap;

            // Assert
            var expectedMap = new int[5, 5]
            {
                { 3, 2, 1, 0, 0 },
                { 2, 2, 1, 0, 0 },
                { 1, 1, 0, 0, 0 },
                { 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0 }
            };

            CollectionAssert.AreEquivalent(expectedMap, result);
        }

        [TestMethod]
        public void Should_Create_Hill_TopRight()
        {
            // Act
            var result = _heightMapGenerator.CreateHeightMap(5, 5)
                .Hill(
                    new Vector2(1f, 0f),
                    new Vector2(1f, 1f),
                    3)
                .HeightMap;

            // Assert
            var expectedMap = new int[5, 5]
            {
                { 0, 0, 1, 2, 3 },
                { 0, 0, 1, 2, 2 },
                { 0, 0, 0, 1, 1 },
                { 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0 }
            };

            CollectionAssert.AreEquivalent(expectedMap, result);
        }

        [TestMethod]
        public void Should_Create_Hill_BottmLeft()
        {
            // Act
            var result = _heightMapGenerator.CreateHeightMap(5, 5)
                .Hill(
                    new Vector2(0f, 1f),
                    new Vector2(1f, 1f),
                    3)
                .HeightMap;

            // Assert
            var expectedMap = new int[5, 5]
            {
                { 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0 },
                { 1, 1, 0, 0, 0 },
                { 2, 2, 1, 0, 0 },
                { 3, 2, 1, 0, 0 }
            };

            CollectionAssert.AreEquivalent(expectedMap, result);
        }

        [TestMethod]
        public void Should_Create_Hill_BottomRight()
        {
            // Act
            var result = _heightMapGenerator.CreateHeightMap(5, 5)
                .Hill(
                    new Vector2(1f, 1f),
                    new Vector2(1f, 1f),
                    3)
                .HeightMap;

            // Assert
            var expectedMap = new int[5, 5]
            {
                { 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0 },
                { 0, 0, 0, 1, 1 },
                { 0, 0, 1, 2, 2 },
                { 0, 0, 1, 2, 3 }
            };

            CollectionAssert.AreEquivalent(expectedMap, result);
        }

        [TestMethod]
        public void Should_Add_To_Existing_Hill_With_Additive_Option()
        {
            // Arrange
            var startingHeightMap = _heightMapGenerator.CreateHeightMap(5, 5)
                .Hill(
                    new Vector2(0f, 0f),
                    new Vector2(1f, 1f),
                    3,
                    HeightMapGenerator.HillOptions.Addditive);

            // Act
            var result = startingHeightMap
                .Hill(
                    new Vector2(0.25f, 0.25f),
                    new Vector2(1f, 1f),
                    3,
                    HeightMapGenerator.HillOptions.Addditive)
                .HeightMap;

            // Assert
            var expectedMap = new int[5, 5]
            {
                { 5, 4, 3, 1, 0 },
                { 4, 5, 3, 1, 0 },
                { 3, 3, 2, 1, 0 },
                { 1, 1, 1, 0, 0 },
                { 0, 0, 0, 0, 0 }
            };

            CollectionAssert.AreEquivalent(expectedMap, result);
        }

        [TestMethod]
        public void Should_Replace_Existing_Height_Only_If_Result_Is_Higher_With_ReplaceIfHeigher_Option()
        {
            // Arrange
            var startingHeightMap = _heightMapGenerator.CreateHeightMap(5, 5)
                .Hill(
                    new Vector2(0f, 0f),
                    new Vector2(1f, 1f),
                    3,
                    HeightMapGenerator.HillOptions.ReplaceIfHeigher);

            // Act
            var result = startingHeightMap
                .Hill(
                    new Vector2(0.25f, 0.25f),
                    new Vector2(1f, 1f),
                    3,
                    HeightMapGenerator.HillOptions.ReplaceIfHeigher)
                .HeightMap;

            // Assert
            var expectedMap = new int[5, 5]
            {
                { 3, 2, 2, 1, 0 },
                { 2, 3, 2, 1, 0 },
                { 2, 2, 2, 1, 0 },
                { 1, 1, 1, 0, 0 },
                { 0, 0, 0, 0, 0 }
            };

            CollectionAssert.AreEquivalent(expectedMap, result);
        }

        [TestMethod]
        public void Should_Create_Valley_When_Negative_Height_Provided_And_Using_Replace_Option()
        {
            // Act
            var result = _heightMapGenerator.CreateHeightMap(5, 5)
                .Hill(
                    new Vector2(0f, 0f),
                    new Vector2(1f, 1f),
                    -3,
                    HeightMapGenerator.HillOptions.Replace)
                .HeightMap;

            // Assert
            var expectedMap = new int[5, 5]
            {
                { -3, -2, -1, 0, 0 },
                { -2, -2, -1, 0, 0 },
                { -1, -1, 0, 0, 0 },
                { 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0 }
            };

            CollectionAssert.AreEquivalent(expectedMap, result);
        }

        [TestMethod]
        public void Should_Replace_Existing_Height_Only_If_Result_Is_Lower_With_ReplaceIfLower_Option()
        {
            // Arrange
            var startingHeightMap = _heightMapGenerator.CreateHeightMap(5, 5)
                .Hill(
                    new Vector2(0f, 0f),
                    new Vector2(1f, 1f),
                    -3,
                    HeightMapGenerator.HillOptions.ReplaceIfLower);

            // Act
            var result = startingHeightMap
                .Hill(
                    new Vector2(0.25f, 0.25f),
                    new Vector2(1f, 1f),
                    -3,
                    HeightMapGenerator.HillOptions.ReplaceIfLower)
                .HeightMap;

            // Assert
            var expectedMap = new int[5, 5]
            {
                { -3, -2, -2, -1, 0 },
                { -2, -3, -2, -1, 0 },
                { -2, -2, -2, -1, 0 },
                { -1, -1, -1, 0, 0 },
                { 0, 0, 0, 0, 0 }
            };

            CollectionAssert.AreEquivalent(expectedMap, result);
        }

        [TestMethod]
        public void Should_Create_Ridge()
        {
            // Act
            var result = _heightMapGenerator.CreateHeightMap(5, 5)
                .Ridge(
                    new Vector2(0, 0), 
                    new Vector2(1f, 1f),
                    new Vector2(1f, 1f),
                    3)
                .HeightMap;

            // Assert
            var expectedMap = new int[5, 5]
            {
                { 3, 2, 2, 1, 0 },
                { 2, 3, 2, 2, 1 },
                { 2, 2, 3, 2, 2 },
                { 1, 2, 2, 3, 2 },
                { 0, 1, 2, 2, 3 }
            };

            CollectionAssert.AreEquivalent(expectedMap, result);
        }

        [TestMethod]
        public void Should_Create_Ridge_Using_ReplaceIfHeigher_Option()
        {
            // Arrange
            var heightMap = _heightMapGenerator.CreateHeightMap(5, 5)
                .Ridge(
                    new Vector2(0, 0),
                    new Vector2(1f, 1f),
                    new Vector2(1f, 1f),
                    3);

            // Act
            var result = heightMap
                .Ridge(
                    new Vector2(1f, 0),
                    new Vector2(1f, 1f),
                    new Vector2(1f, 1f),
                    3)
                .HeightMap;

            // Assert
            var expectedMap = new int[5, 5]
            {
                { 3, 2, 2, 1, 0 },
                { 2, 3, 2, 2, 1 },
                { 2, 2, 3, 2, 2 },
                { 2, 2, 2, 3, 2 },
                { 3, 3, 3, 3, 3 }
            };

            CollectionAssert.AreEquivalent(expectedMap, result);
        }

        [TestMethod]
        public void Should_Create_Valley_Using_Negative_Height()
        {
            // Arrange
            var heightMap = _heightMapGenerator.CreateHeightMap(5, 5)
                .Ridge(
                    new Vector2(0, 0),
                    new Vector2(1f, 1f),
                    new Vector2(1f, 1f),
                    -3);

            // Act
            var result = heightMap
                .Ridge(
                    new Vector2(1f, 0),
                    new Vector2(1f, 1f),
                    new Vector2(1f, 1f),
                    -3)
                .HeightMap;

            // Assert
            var expectedMap = new int[5, 5]
            {
                { -3, -2, -2, -1, 0 },
                { -2, -3, -2, -2, -1 },
                { -2, -2, -3, -2, -2 },
                { -2, -2, -2, -3, -2 },
                { -3, -3, -3, -3, -3 }
            };

            CollectionAssert.AreEquivalent(expectedMap, result);
        }
    }
}