using System.Linq;

using DavidFidge.MonoGame.Core.Graphics.Extensions;
using DavidFidge.MonoGame.Core.Services;
using DavidFidge.TestInfrastructure;

using InputHandlers.Keyboard;
using InputHandlers.Mouse;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using NSubstitute;

namespace DavidFidge.MonoGame.Core.Tests.Services
{
    [TestClass]
    public class VertexExtensionsTests : BaseTest
    {

        [TestInitialize]
        public override void Setup()
        {
            base.Setup();

        }

        [TestMethod]
        public void GenerateNormalsForTriangleStrip_Should_Generate_Normals_Correctly()
        {
            //        3
            //       /|\
            //      / | \
            //     /  |  \
            //    /  _|_  \
            // 1 /__/ 2 \__\4
            //
            // Arrange
            var vertexPositionNormalTextures = new VertexPositionNormalTexture[4];

            vertexPositionNormalTextures[0] = new VertexPositionNormalTexture(
                new Vector3(0, 0, 0),
                Vector3.Zero,
                Vector2.Zero
                );

            vertexPositionNormalTextures[1] = new VertexPositionNormalTexture(
                new Vector3(1, 1, 0),
                Vector3.Zero,
                Vector2.Zero
            );

            vertexPositionNormalTextures[2] = new VertexPositionNormalTexture(
                new Vector3(1, 1, 1),
                Vector3.Zero,
                Vector2.Zero
            );

            vertexPositionNormalTextures[3] = new VertexPositionNormalTexture(
                new Vector3(2, 0, 0),
                Vector3.Zero,
                Vector2.Zero
            );

            var indices = new[] { 0, 1, 2, 3 };

            // Act
            vertexPositionNormalTextures = vertexPositionNormalTextures.GenerateNormalsForTriangleStrip(indices);

            // Assert
            // 3 to 2 and 3 to 1
            var expectedNormalTriangle1 = Vector3.Cross(
                new Vector3(0, 0, 1),
                new Vector3(1,1, 1)
            );

            // 4 to 2 and 4 to 3 (different order from
            // above so that normal is in the correct direction)
            var expectedNormalTriangle2 = Vector3.Cross(
                new Vector3(1, -1, 0),
                new Vector3(1, -1, -1)
            );

            vertexPositionNormalTextures[0].Normal.IsEquivalentTo(Vector3.Normalize(expectedNormalTriangle1));

            vertexPositionNormalTextures[1].Normal.IsEquivalentTo(Vector3.Normalize(expectedNormalTriangle1 + expectedNormalTriangle2));

            vertexPositionNormalTextures[2].Normal.IsEquivalentTo(Vector3.Normalize(expectedNormalTriangle1 + expectedNormalTriangle2));

            vertexPositionNormalTextures[3].Normal.IsEquivalentTo(Vector3.Normalize(expectedNormalTriangle2));
        }
    }
}