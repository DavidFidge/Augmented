using System.IO;

using DavidFidge.MonoGame.Core.Graphics.Extensions;
using DavidFidge.MonoGame.Core.Interfaces.Components;
using DavidFidge.MonoGame.Core.Interfaces.Graphics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DavidFidge.MonoGame.Core.Graphics.Cylinder
{
    public class Cylinder : IDrawable
    {
        private readonly IGameProvider _gameProvider;
        private VertexBuffer _vertexBuffer;
        private IndexBuffer _indexBuffer;
        private BasicEffect _basicEffect;
        private int _primitiveCount;

        public IWorldTransform WorldTransform { get; }

        public Cylinder(IGameProvider gameProvider)
        {
            _gameProvider = gameProvider;
            WorldTransform = new SimpleWorldTransform();
        }

        public void LoadContent(float radius, float height)
        {
            var cylinderGenerator = new CylinderGenerator(
                10,
                2,
                radius,
                height,
                PrimitiveType.TriangleStrip
            );

            cylinderGenerator.CreateGeometry();
            _primitiveCount = cylinderGenerator.PrimitiveCount;

            var vertices = cylinderGenerator.Vertices;
            var indexes = cylinderGenerator.Indexes;

            vertices.GenerateNormalsForTriangleStrip(indexes);

            _vertexBuffer = new VertexBuffer(
                _gameProvider.Game.GraphicsDevice,
                VertexPositionNormalTexture.VertexDeclaration,
                vertices.Length,
                BufferUsage.WriteOnly
            );

            _vertexBuffer.SetData(vertices);

            _indexBuffer = new IndexBuffer(
                _gameProvider.Game.GraphicsDevice,
                IndexElementSize.ThirtyTwoBits,
                sizeof(int) * indexes.Length,
                BufferUsage.WriteOnly
            );

            _indexBuffer.SetData(indexes);

            _basicEffect = _gameProvider.Game.EffectCollection.BuildMaterialEffect(Color.Blue);
        }

        public void Draw(Matrix view, Matrix projection)
        {
            var graphicsDevice = _gameProvider.Game.GraphicsDevice;

            graphicsDevice.Indices = _indexBuffer;
            graphicsDevice.SetVertexBuffer(_vertexBuffer);

            if (_basicEffect != null)
            {
                _basicEffect.World = WorldTransform.World;
                _basicEffect.View = view;
                _basicEffect.Projection = projection;

                foreach (var pass in _basicEffect.CurrentTechnique.Passes)
                {
                    pass.Apply();

                    graphicsDevice.DrawIndexedPrimitives(
                        PrimitiveType.TriangleStrip,
                        0,
                        0,
                        _primitiveCount
                    );
                }
            }
        }
    }
}
