using DavidFidge.MonoGame.Core.Graphics;
using DavidFidge.MonoGame.Core.Graphics.Extensions;
using DavidFidge.MonoGame.Core.Graphics.Terrain;
using DavidFidge.MonoGame.Core.Interfaces.Components;
using DavidFidge.MonoGame.Core.Interfaces.Graphics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Augmented.Graphics.TerrainSpace
{
    public class Terrain : IWorldTransformable
    {
        private readonly IHeightMapGenerator _heightMapGenerator;
        private readonly IGameProvider _gameProvider;
        private readonly IContentStrings _contentStrings;
        private IndexBuffer _terrainIndexBuffer;
        private VertexBuffer _terrainVertexBuffer;
        private Effect _effect;
        private SamplerState _samplerState;

        public Terrain(
            IHeightMapGenerator heightMapGenerator,
            IGameProvider gameProvider,
            IContentStrings contentStrings)
        {
            _heightMapGenerator = heightMapGenerator;
            _gameProvider = gameProvider;
            _contentStrings = contentStrings;
            WorldTransform = new SimpleWorldTransform();

            _samplerState = new SamplerState
            {
                AddressU = TextureAddressMode.Wrap,
                AddressV = TextureAddressMode.Wrap,
            };
        }

        public IWorldTransform WorldTransform { get; }
        
        public VertexPositionNormalTexture[] CreateTerrainVertices(HeightMap heightMap, Vector3 scale)
        {
            var width = heightMap.Width;
            var height = heightMap.Length;

            var terrainVertices = new VertexPositionNormalTexture[width * height];

            var i = 0;

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var position = new Vector3(x * scale.X, y * scale.Y, heightMap[x, y] * scale.Z);
                    var normal = new Vector3(0, 0, 1f);
                    var texture = new Vector2(x / (width / 10f), y / (height / 10f));

                    terrainVertices[i++] = new VertexPositionNormalTexture(position, normal, texture);
                }
            }

            return terrainVertices;
        }

        public int[] CreateTerrainIndexes(HeightMap heightMap)
        {
            var width = heightMap.Width;
            var height = heightMap.Length;
            var terrainIndexes = new int[width * 2 * (height - 1)];

            var i = 0;
            var y = 0;

            while (y < height - 1)
            {
                // create triangle strip indexes going forwards
                for (var x = 0; x < width; x++)
                {
                    terrainIndexes[i++] = x + y * width;
                    terrainIndexes[i++] = x + (y + 1) * width;
                }

                // move up to next row and create triangle strip indexes going backwards
                y++;

                if (y < height - 1)
                {
                    for (var x = width - 1; x >= 0; x--)
                    {
                        terrainIndexes[i++] = x + (y + 1) * width;
                        terrainIndexes[i++] = x + y * width;
                    }
                }

                y++;
            }

            return terrainIndexes;
        }

        public void LoadContent()
        {
            var heightMap = _heightMapGenerator.CreateHeightMap(33, 33)
                .DiamondSquare(32, -20000, 20000, new SubtractingHeightsReducer())
                .HeightMap();

            var terrainVertices = CreateTerrainVertices(heightMap, new Vector3(10f, 10f, 0.01f));
            var terrainIndexes = CreateTerrainIndexes(heightMap);

            terrainVertices.GenerateNormalsForTriangleStrip(terrainIndexes);

            _terrainVertexBuffer = new VertexBuffer(
                _gameProvider.Game.GraphicsDevice,
                VertexPositionNormalTexture.VertexDeclaration,
                terrainVertices.Length,
                BufferUsage.WriteOnly
             );

            _terrainVertexBuffer.SetData(terrainVertices);

            _terrainIndexBuffer = new IndexBuffer(
                _gameProvider.Game.GraphicsDevice,
                IndexElementSize.ThirtyTwoBits,
                terrainIndexes.Length,
                BufferUsage.WriteOnly
            );

            _terrainIndexBuffer.SetData(terrainIndexes);

            _effect = _gameProvider.Game.EffectCollection.BuildTextureEffect(_contentStrings.GrassTexture);
        }

        public void Draw(Matrix view, Matrix projection)
        {
            var graphicsDevice = _gameProvider.Game.GraphicsDevice;

            graphicsDevice.Indices = _terrainIndexBuffer;
            graphicsDevice.SetVertexBuffer(_terrainVertexBuffer);
            var oldSamplerState = graphicsDevice.SamplerStates[0];
            graphicsDevice.SamplerStates[0] = _samplerState;

            if (_effect != null)
            {
                _effect.SetWorldViewProjection(
                    WorldTransform.World,
                    view,
                    projection
                );

                foreach (var pass in _effect.CurrentTechnique.Passes)
                {
                    pass.Apply();

                    graphicsDevice.DrawIndexedPrimitives(
                        PrimitiveType.TriangleStrip,
                        0,
                        0,
                        _terrainIndexBuffer.IndexCount - 2
                    );
                }
            }

            graphicsDevice.SamplerStates[0] = oldSamplerState;
        }
    }
}