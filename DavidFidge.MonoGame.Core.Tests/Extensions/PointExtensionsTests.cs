using System.Collections.Generic;

using DavidFidge.MonoGame.Core.Graphics.Extensions;
using DavidFidge.TestInfrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;

namespace DavidFidge.MonoGame.Core.Tests.Services
{
    [TestClass]
    public class PointExtensionsTests : BaseTest
    {

        [TestInitialize]
        public override void Setup()
        {
            base.Setup();

        }

        [TestMethod]
        public void SurroundingPoints_Should_Return_All_Surrounding_Points()
        {
            // Arrange
            var point = new Point();

            // Act
            var result = point.SurroundingPoints();

            // Assert
            var expectedPoints = new List<Point>
            {
                new Point(-1, -1),
                new Point(-1, 0),
                new Point(-1, 1),
                new Point(0, -1),
                new Point(0, 1),
                new Point(1, -1),
                new Point(1, 0),
                new Point(1, 1),
            };

            CollectionAssert.AreEquivalent(expectedPoints, result);
        }
    }
}