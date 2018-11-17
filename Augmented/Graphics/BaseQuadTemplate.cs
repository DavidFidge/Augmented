using System;
using System.Collections.Generic;
using System.Linq;

using DavidFidge.MonoGame.Core.Interfaces;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Augmented.Graphics
{
    public abstract class BaseQuadTemplate
    {
        private int[] _quadIndices;
        private VertexPositionTexture[] _quadVertices;
        private Vector2 _dimensions;

        protected IGameProvider _gameProvider;
        protected BasicEffect _basicEffect;

        public VertexBuffer VertexBuffer { get; private set; }
        public IndexBuffer IndexBuffer { get; private set; }

        public void Draw(Matrix world, Matrix view, Matrix projection)
        {
            var graphicsDevice = _gameProvider.Game.GraphicsDevice;

            graphicsDevice.Indices = IndexBuffer;
            graphicsDevice.SetVertexBuffer(VertexBuffer);

            if (_basicEffect != null)
            {
                _basicEffect.World = world;
                _basicEffect.View = view;
                _basicEffect.Projection = projection;

                PrepareBasicEffectForDraw();

                foreach(var pass in _basicEffect.CurrentTechnique.Passes)
                {
                    pass.Apply();

                    graphicsDevice.DrawIndexedPrimitives(
                        PrimitiveType.TriangleList,
                        0,
                        0,
                        2
                    );
                }
            }
        }

        protected BaseQuadTemplate(IGameProvider gameProvider)
        {
            _gameProvider = gameProvider;
        }

        protected void Initialise(float width, float height)
        {
            InitialiseInternal(width, height, Vector3.Zero);
        }

        protected void Initialise(float width, float height, Vector3 displacement)
        {
            InitialiseInternal(width, height, displacement);
        }

        protected virtual void PrepareBasicEffectForDraw()
        {
        }

        private void InitialiseInternal(float width, float height, Vector3 displacement)
        {
            _dimensions.X = width;
            _dimensions.Y = height;
            LoadGeometry(displacement);
        }

        private void LoadGeometry(Vector3 displacement)
        {
            // Halve the width and height - this is used so that points are placed in a fashion that the object will be centred on world origin
            var halfWidth = _dimensions.X / 2.0f;
            var halfHeight = _dimensions.Y / 2.0f;

            var topLeft = new Vector3(-halfWidth, halfHeight, 0.0f);
            var topRight = new Vector3(halfWidth, halfHeight, 0.0f);
            var bottomLeft = new Vector3(-halfWidth, -halfHeight, 0.0f);
            var bottomRight = new Vector3(halfWidth, -halfHeight, 0.0f);

            //add in the displacement factor - this displaces the quad off-centre, so you can do some interesting rotations on a pivot
            topLeft = Vector3.Add(topLeft, displacement);
            topRight = Vector3.Add(topRight, displacement);
            bottomLeft = Vector3.Add(bottomLeft, displacement);
            bottomRight = Vector3.Add(bottomRight, displacement);

            // Initialize the texture coordinates.
            var textureTopLeft = new Vector2(0.0f, 0.0f);
            var textureTopRight = new Vector2(1.0f, 0.0f);
            var textureBottomLeft = new Vector2(0.0f, 1.0f);
            var textureBottomRight = new Vector2(1.0f, 1.0f);

            _quadVertices = new VertexPositionTexture[4];

            // Vertices for the front of the quad.
            _quadVertices[0] = new VertexPositionTexture(topLeft, textureTopLeft);
            _quadVertices[1] = new VertexPositionTexture(topRight, textureTopRight);
            _quadVertices[2] = new VertexPositionTexture(bottomLeft, textureBottomLeft);
            _quadVertices[3] = new VertexPositionTexture(bottomRight, textureBottomRight);

            VertexBuffer = new VertexBuffer(_gameProvider.Game.GraphicsDevice, typeof(VertexPositionTexture),
                _quadVertices.Length,
                BufferUsage.None
                );

            VertexBuffer.SetData(_quadVertices);

            _quadIndices = new [] { 0, 1, 2, 2, 1, 3 };

            IndexBuffer = new IndexBuffer(
                _gameProvider.Game.GraphicsDevice,
                IndexElementSize.ThirtyTwoBits,
                sizeof(int) * _quadIndices.Length,
                BufferUsage.None
                );

            IndexBuffer.SetData(_quadIndices);
        }
    }
}
