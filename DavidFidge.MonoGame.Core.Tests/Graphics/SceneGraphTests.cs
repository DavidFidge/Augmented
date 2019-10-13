using DavidFidge.MonoGame.Core.Components;
using DavidFidge.MonoGame.Core.Graphics;
using DavidFidge.TestInfrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;

using IDrawable = DavidFidge.MonoGame.Core.Graphics.IDrawable;

namespace DavidFidge.MonoGame.Core.Tests.Graphics
{
    [TestClass]
    public class SceneGraphTests : BaseTest
    {
        private SceneGraph _sceneGraph;

        [TestInitialize]
        public override void Setup()
        {
            base.Setup();

            _sceneGraph = new SceneGraph();
        }

        [TestMethod]
        public void Should_Call_LoadContent_Using_BreadthFirst()
        {
            // Arrange
            var testEntityRoot = new TestEntity();
            var testEntityChild1 = new TestEntity();
            var testEntityChild2 = new TestEntity();

            testEntityRoot.NodeAfter = testEntityChild1;
            testEntityChild1.NodeBefore = testEntityRoot;
            testEntityChild1.NodeAfter = testEntityChild2;
            testEntityChild2.NodeBefore = testEntityChild1;

            testEntityRoot.SceneNode.Add(testEntityChild1.SceneNode);
            testEntityRoot.SceneNode.Add(testEntityChild2.SceneNode);

            _sceneGraph.Root = testEntityRoot.SceneNode;

            // Act
            _sceneGraph.LoadContent();

            // Assert
            Assert.IsTrue(testEntityRoot.HasLoadContentBeenCalled);
            Assert.IsTrue(testEntityChild1.HasLoadContentBeenCalled);
            Assert.IsTrue(testEntityChild2.HasLoadContentBeenCalled);
        }

        [TestMethod]
        public void Should_Call_Draw_With_Correct_Parameters()
        {
            // Arrange
            var testEntity = new TestEntity();

            _sceneGraph.Root = testEntity.SceneNode;

            // Act
            _sceneGraph.Draw(Matrix.Identity, Matrix.Identity * 2);

            // Assert
            Assert.IsTrue(testEntity.HasDrawBeenCalled);
            Assert.AreEqual(Matrix.Identity,testEntity.View);
            Assert.AreEqual(Matrix.Identity * 2,testEntity.Projection);
        }

        [TestMethod]
        public void Should_Call_Draw_Using_BreadthFirst()
        {
            // Arrange
            var testEntityRoot = new TestEntity();
            var testEntityChild1 = new TestEntity();
            var testEntityChild2 = new TestEntity();

            testEntityRoot.NodeAfter = testEntityChild1;
            testEntityChild1.NodeBefore = testEntityRoot;
            testEntityChild1.NodeAfter = testEntityChild2;
            testEntityChild2.NodeBefore = testEntityChild1;

            testEntityRoot.SceneNode.Add(testEntityChild1.SceneNode);
            testEntityRoot.SceneNode.Add(testEntityChild2.SceneNode);

            _sceneGraph.Root = testEntityRoot.SceneNode;

            // Act
            _sceneGraph.Draw(Matrix.Identity, Matrix.Identity);

            // Assert
            Assert.IsTrue(testEntityRoot.HasDrawBeenCalled);
            Assert.IsTrue(testEntityChild1.HasDrawBeenCalled);
            Assert.IsTrue(testEntityChild2.HasDrawBeenCalled);
        }

        [TestMethod]
        public void NonIntersecting_Entity_Should_Be_Deselected()
        {
            // Arrange
            var nonIntersectingEntity = new TestSelectEntity();

            _sceneGraph.Root = nonIntersectingEntity.SceneNode;

            // Act
            var result = _sceneGraph.Select(new Ray(Vector3.Zero, Vector3.Zero));

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Intersecting_Entities_Should_Return_Entity_With_Shortest_Intersecting_Distance()
        {
            // Arrange
            var entity1 = new TestSelectEntity { IntersectsReturnValue = 0.5f };
            var entity2 = new TestSelectEntity { IntersectsReturnValue = 0.4f };

            _sceneGraph.Root = entity1.SceneNode;
            entity1.SceneNode.Add(entity2.SceneNode);

            // Act
            var result = _sceneGraph.Select(new Ray(Vector3.Zero, Vector3.Zero));

            // Assert
            Assert.AreEqual(entity2, result);
        }

        public class TestSelectEntity : Entity, ISelectable
        {
            public bool IsSelected { get; set; }
            public float? IntersectsReturnValue { get; set; }

            public float? Intersects(Ray ray)
            {
                return IntersectsReturnValue;
            }
        }

        public class TestEntity : Entity, ILoadContent, IDrawable
        {
            public Matrix View { get; set; }
            public Matrix Projection { get; set; }
            public TestEntity NodeBefore { get; set; }
            public TestEntity NodeAfter { get; set; }

            public bool HasLoadContentBeenCalled { get; private set; }
            public bool HasDrawBeenCalled { get; private set; }

            public void LoadContent()
            {
                HasLoadContentBeenCalled = true;

                if (NodeBefore != null)
                    Assert.IsTrue(NodeBefore.HasLoadContentBeenCalled);

                if (NodeAfter != null)
                    Assert.IsFalse(NodeAfter.HasLoadContentBeenCalled);
            }

            public void Draw(Matrix view, Matrix projection)
            {
                Projection = projection;
                View = view;
                HasDrawBeenCalled = true;

                if (NodeBefore != null)
                    Assert.IsTrue(NodeBefore.HasDrawBeenCalled);

                if (NodeAfter != null)
                    Assert.IsFalse(NodeAfter.HasDrawBeenCalled);
            }
        }
   }
}