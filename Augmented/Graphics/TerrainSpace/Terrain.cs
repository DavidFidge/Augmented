using DavidFidge.MonoGame.Core.Graphics;
using DavidFidge.MonoGame.Core.Graphics.Extensions;
using DavidFidge.MonoGame.Core.Interfaces.Components;
using DavidFidge.MonoGame.Core.Interfaces.Graphics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Augmented.Graphics.TerrainSpace
{
    public class Terrain : IWorldTransformable
    {
        private readonly IHeightMapStore _heightMapStore;
        private readonly IGameProvider _gameProvider;
        private IndexBuffer _terrainIndexBuffer;
        private VertexBuffer _terrainVertexBuffer;
        private BasicEffect _basicEffect;
        private SamplerState _samplerState;

        public Terrain(IHeightMapStore heightMapStore, IGameProvider gameProvider)
        {
            _heightMapStore = heightMapStore;
            _gameProvider = gameProvider;
            WorldTransform = new SimpleWorldTransform();

            _samplerState = new SamplerState
            {
                AddressU = TextureAddressMode.Wrap,
                AddressV = TextureAddressMode.Wrap,
            };
        }

        public IWorldTransform WorldTransform { get; }
        
        public VertexPositionNormalTexture[] CreateTerrainVertices(int[,] heightMap)
        {
            var width = heightMap.GetLength(0);
            var height = heightMap.GetLength(1);
            var terrainVertices = new VertexPositionNormalTexture[width * height];

            var i = 0;

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var position = new Vector3(x, y, heightMap[x, y]);
                    var normal = new Vector3(0, 0, 1f);
                    var texture = new Vector2(x / (width / 10f), y / (height / 10f));

                    terrainVertices[i++] = new VertexPositionNormalTexture(position, normal, texture);
                }
            }

            return terrainVertices;
        }

        public int[] CreateTerrainIndexes(int[,] heightMap)
        {
            var width = heightMap.GetLength(0);
            var height = heightMap.GetLength(1);
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
            var heightMap = _heightMapStore.GetHeightMap();
            var terrainVertices = CreateTerrainVertices(heightMap);
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
                sizeof(int) * terrainIndexes.Length,
                BufferUsage.WriteOnly
            );

            _terrainIndexBuffer.SetData(terrainIndexes);

            LoadBasicEffect();
        }

        public void Draw(Matrix view, Matrix projection)
        {
            var graphicsDevice = _gameProvider.Game.GraphicsDevice;

            graphicsDevice.Indices = _terrainIndexBuffer;
            graphicsDevice.SetVertexBuffer(_terrainVertexBuffer);
            var oldSamplerState = graphicsDevice.SamplerStates[0];
            graphicsDevice.SamplerStates[0] = _samplerState;

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
                        _terrainIndexBuffer.IndexCount - 2
                    );
                }
            }

            graphicsDevice.SamplerStates[0] = oldSamplerState;
        }

        private void LoadBasicEffect()
        {
            _basicEffect = new BasicEffect(_gameProvider.Game.GraphicsDevice);

            _basicEffect.Texture = _gameProvider.Game.Content.Load<Texture2D>(Constants.GrassTexture);
            _basicEffect.TextureEnabled = true;
            _basicEffect.EnableDefaultLighting();
            _basicEffect.LightingEnabled = true;
            _basicEffect.DirectionalLight0.Direction = new Vector3(1, 1, 0);
            _basicEffect.DirectionalLight0.Enabled = true;
            _basicEffect.AmbientLightColor = new Vector3(0.3f, 0.3f, 0.3f);
            _basicEffect.DirectionalLight1.Enabled = false;
            _basicEffect.DirectionalLight2.Enabled = false;
            _basicEffect.SpecularColor = Vector3.Zero;
        }
    }
}