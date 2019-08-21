using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using DavidFidge.MonoGame.Core.Graphics.Terrain;
using DavidFidge.TestInfrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;

namespace DavidFidge.MonoGame.Core.Tests.Graphics
{
    [TestClass]
    public class HeightMapTests : BaseTest
    {
        [TestMethod]
        public void HeightMap_Constructor_Should_Create_HeightMap_With_Supplied_Dimensions()
        {
            // Act
            var heightMap = new HeightMap(10, 11);

            // Assert
            Assert.AreEqual(10, heightMap.Width);
            Assert.AreEqual(11, heightMap.Length);
        }

        [TestMethod]
        public void Area_Should_Give_Area()
        {
            // Arrange
            var heightMap = new HeightMap(5, 4);

            // Assert
            Assert.AreEqual(20, heightMap.Area);
        }

        [TestMethod]
        public void ZeroAreaPercent_Should_Give_Area_Covered_By_ZeroValues()
        {
            // Arrange
            var heightMap = new HeightMap(5, 4);

            heightMap.FromArray(
                new int[5 * 4]
                {
                    1, 1, 1, 1, 1,
                    0, 0, 0, 3, 3,
                    -1, -1, -1, -1, -1,
                    2, 2, 2, 0, 0
                });

            // Act
            var result = heightMap.ZeroAreaPercent();

            // Assert
            Assert.AreEqual(0.25d, result);
        }

        [TestMethod]
        public void Patch_Should_Patch_In_Full()
        {
            // Arrange
            var heightMap = new HeightMap(5, 4);

            var heightMapPatch = new HeightMap(3, 2)
                .FromArray(new int[3 * 2]
                    {
                        1, 2, 3,
                        4, 5, 6
                    });

            var expectedMap = new int[5 * 4]
            {
                0, 0, 0, 0, 0,
                0, 0, 1, 2, 3,
                0, 0, 4, 5, 6,
                0, 0, 0, 0, 0,
            };

            // Act
            heightMap.Patch(heightMapPatch, new Point(2, 1));

            // Assert
            CollectionAssert.AreEquivalent(expectedMap, heightMap.ToArray());
        }

        [TestMethod]
        public void Patch_Should_Patch_Partial_Right()
        {
            // Arrange
            var heightMap = new HeightMap(5, 4);

            var heightMapPatch = new HeightMap(3, 2)
                .FromArray(new int[3 * 2]
                {
                    1, 2, 3,
                    4, 5, 6
                });

            var expectedMap = new int[5 * 4]
            {
                0, 0, 0, 0, 0,
                0, 0, 0, 1, 2,
                0, 0, 0, 4, 5,
                0, 0, 0, 0, 0,
            };

            // Act
            heightMap.Patch(heightMapPatch, new Point(3, 1));

            // Assert
            CollectionAssert.AreEquivalent(expectedMap, heightMap.ToArray());
        }

        [TestMethod]
        public void Patch_Should_Patch_Partial_Left()
        {
            // Arrange
            var heightMap = new HeightMap(5, 4);

            var heightMapPatch = new HeightMap(3, 2)
                .FromArray(new int[3 * 2]
                {
                    1, 2, 3,
                    4, 5, 6
                });

            var expectedMap = new int[5 * 4]
            {
                0, 0, 0, 0, 0,
                2, 3, 0, 0, 0,
                5, 6, 0, 0, 0,
                0, 0, 0, 0, 0,
            };

            // Act
            heightMap.Patch(heightMapPatch, new Point(-1, 1));

            // Assert
            CollectionAssert.AreEquivalent(expectedMap, heightMap.ToArray());
        }

        [TestMethod]
        public void Patch_Should_Patch_Partial_Top()
        {
            // Arrange
            var heightMap = new HeightMap(5, 4);

            var heightMapPatch = new HeightMap(3, 2)
                .FromArray(new int[3 * 2]
                {
                    1, 2, 3,
                    4, 5, 6
                });

            var expectedMap = new int[5 * 4]
            {
                0, 0, 4, 5, 6,
                0, 0, 0, 0, 0,
                0, 0, 0, 0, 0,
                0, 0, 0, 0, 0,
            };

            // Act
            heightMap.Patch(heightMapPatch, new Point(2, -1));

            // Assert
            CollectionAssert.AreEquivalent(expectedMap, heightMap.ToArray());
        }

        [TestMethod]
        public void Patch_Should_Patch_Partial_Bottom()
        {
            // Arrange
            var heightMap = new HeightMap(5, 4);

            var heightMapPatch = new HeightMap(3, 2)
                .FromArray(new int[3 * 2]
                {
                    1, 2, 3,
                    4, 5, 6
                });

            var expectedMap = new int[5 * 4]
            {
                0, 0, 0, 0, 0,
                0, 0, 0, 0, 0,
                0, 0, 0, 0, 0,
                0, 0, 1, 2, 3,
            };

            // Act
            heightMap.Patch(heightMapPatch, new Point(2, 3));

            // Assert
            CollectionAssert.AreEquivalent(expectedMap, heightMap.ToArray());
        }

        [TestMethod]
        public void Patch_Should_Patch_When_Patch_Oversize()
        {
            // Arrange
            var heightMap = new HeightMap(5, 4);

            var heightMapPatch = new HeightMap(7, 6)
                .FromArray(new int[7 * 6]
                {
                    1, 2, 3, 4, 5, 6, 7,
                    8, 9, 10, 11, 12, 13, 14,
                    15, 16, 17, 18, 19, 20, 21,
                    22, 23, 24, 25, 26, 27, 28,
                    29, 30, 31, 32, 33, 34, 35,
                    36, 37, 38, 39, 40, 41, 42,
                });

            var expectedMap = new int[5 * 4]
            {
                9, 10, 11, 12, 13,
                16, 17, 18, 19, 20,
                23, 24, 25, 26, 27,
                30, 31, 32, 33, 34
            };

            // Act
            heightMap.Patch(heightMapPatch, new Point(-1, -1));

            // Assert
            CollectionAssert.AreEquivalent(expectedMap, heightMap.ToArray());
        }

        [TestMethod]
        public void Patch_Should_Not_Patch_When_Totally_Outside()
        {
            // Arrange
            var heightMap = new HeightMap(5, 4);

            var heightMapPatch = new HeightMap(3, 2)
                .FromArray(new int[3 * 2]
                {
                    1, 2, 3,
                    4, 5, 6
                });

            var expectedMap = new int[5 * 4]
            {
                0, 0, 0, 0, 0,
                0, 0, 0, 0, 0,
                0, 0, 0, 0, 0,
                0, 0, 0, 0, 0,
            };

            // Act
            heightMap.Patch(heightMapPatch, new Point(5, 0));

            // Assert
            CollectionAssert.AreEquivalent(expectedMap, heightMap.ToArray());
        }

        [TestMethod]
        public void Import_Should_Import_HeightMap_From_File()
        {
            // Arrange
            var heightMap = new HeightMap(3, 2)
                .FromArray(new int[3 * 2]
                {
                    1, 2, 3,
                    4, 5, 6
                });

            var tempFileName = $"{Guid.NewGuid().ToString().Replace("-", "")}.csv";

            heightMap.Export("Test", tempFileName);

            var filePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "Test",
                tempFileName);

            // Act
            var result = HeightMap.Import(filePath);

            // Assert
            var expectedMap = new int[3 * 2]
            {
                1, 2, 3,
                4, 5, 6
            };

            CollectionAssert.AreEquivalent(expectedMap, result.ToArray());
        }

        [TestMethod]
        public void Export_Should_Export_HeightMap_To_File()
        {
            // Arrange
            var heightMap = new HeightMap(3, 2)
                .FromArray(new int[3 * 2]
                {
                    1, 2, 3,
                    4, 5, 6
                });

            var tempFileName = $"{Guid.NewGuid().ToString().Replace("-", "")}.csv";

            // Act
            heightMap.Export("Test", tempFileName);

            // Assert
            var filePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "Test",
                tempFileName);

            var stringList = new List<string>();

            using (var streamReader = new StreamReader(filePath))
            {
                while (!streamReader.EndOfStream)
                {
                    stringList.Add(streamReader.ReadLine());
                }
            }

            Assert.AreEqual(2, stringList.Count);
            Assert.AreEqual("1,2,3", stringList[0]);
            Assert.AreEqual("4,5,6", stringList[1]);
        }
    }
}