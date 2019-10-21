using System;

using DavidFidge.MonoGame.Core.Components;
using DavidFidge.MonoGame.Core.Graphics;
using DavidFidge.MonoGame.Core.Graphics.Extensions;
using DavidFidge.MonoGame.Core.Graphics.Terrain;
using DavidFidge.MonoGame.Core.Interfaces.Components;
using DavidFidge.MonoGame.Core.Interfaces.Graphics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using IDrawable = DavidFidge.MonoGame.Core.Graphics.IDrawable;

namespace Augmented.Graphics.TerrainSpace
{
    public class Terrain : Entity, IDrawable, ILoadContent
    {
        private readonly IHeightMapGenerator _heightMapGenerator;
        private readonly IGameProvider _gameProvider;
        private readonly IContentStrings _contentStrings;
        private IndexBuffer _terrainIndexBuffer;
        private VertexBuffer _terrainVertexBuffer;
        private Effect _effect;
        private SamplerState _samplerState;
        private HeightMap _heightMap;
        private Vector3 _scale;

        public Terrain(
            IHeightMapGenerator heightMapGenerator,
            IGameProvider gameProvider,
            IContentStrings contentStrings)
        {
            _heightMapGenerator = heightMapGenerator;
            _gameProvider = gameProvider;
            _contentStrings = contentStrings;
            WorldTransform = new WorldTransform();

            _samplerState = new SamplerState
            {
                AddressU = TextureAddressMode.Wrap,
                AddressV = TextureAddressMode.Wrap,
            };
        }

        public VertexPositionNormalTexture[] CreateTerrainVertices()
        {
            var width = _heightMap.Width;
            var height = _heightMap.Length;

            var terrainVertices = new VertexPositionNormalTexture[width * height];

            var i = 0;

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var position = new Vector3(x, y, _heightMap[x, y]);
                    var normal = new Vector3(0, 0, 1f);
                    var texture = new Vector2(x / (width / 10f), y / (height / 10f));

                    terrainVertices[i++] = new VertexPositionNormalTexture(position, normal, texture);
                }
            }

            return terrainVertices;
        }

        public int[] CreateTerrainIndexes()
        {
            var width = _heightMap.Width;
            var height = _heightMap.Length;
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

        public void CreateHeightMap(TerrainParameters terrainParameters)
        {
            _scale = new Vector3(20f, 20f, 0.005f) * GetScale(terrainParameters);

            var hillHeight = GetHillHeight(terrainParameters);

            var heightMapSize = 32;

            _heightMap = _heightMapGenerator
                .CreateHeightMap(heightMapSize + 1, heightMapSize + 1)
                .DiamondSquare(heightMapSize, -hillHeight, hillHeight, new SubtractingHeightsReducer())
                .HeightMap();

            WorldTransform.ChangeScale(_scale);
        }

        private int GetHillHeight(TerrainParameters terrainParameters)
        {
            var hillHeight = 20000;

            switch (terrainParameters.HillHeight)
            {
                case TerrainSpace.HillHeight.Low:
                    hillHeight /= 2;
                    break;
                case TerrainSpace.HillHeight.High:
                    hillHeight *= 2;
                    break;
            }

            return hillHeight;
        }

        private Vector3 GetScale(TerrainParameters terrainParameters)
        {
            var scale = Vector3.One;

            switch (terrainParameters.Size)
            {
                case WorldSize.Small:
                    scale.X *= 0.5f;
                    scale.Y *= 0.5f;
                    break;
                case WorldSize.Big:
                    scale.X *= 2f;
                    scale.Y *= 2f;
                    break;
            }

            return scale;
        }

        public void LoadContent()
        {
            if (_heightMap == null)
                throw new Exception("Create height map first");

            var terrainVertices = CreateTerrainVertices();
            var terrainIndexes = CreateTerrainIndexes();

            terrainVertices.GenerateNormalsForTriangleStrip(terrainIndexes);

            if (_terrainVertexBuffer == null || _terrainVertexBuffer.VertexCount != terrainVertices.Length)
            {
                _terrainVertexBuffer = new VertexBuffer(
                    _gameProvider.Game.GraphicsDevice,
                    VertexPositionNormalTexture.VertexDeclaration,
                    terrainVertices.Length,
                    BufferUsage.WriteOnly
                );
            }

            _terrainVertexBuffer.SetData(terrainVertices);

            if (_terrainIndexBuffer == null || _terrainIndexBuffer.IndexCount != terrainIndexes.Length)
            {
                _terrainIndexBuffer = new IndexBuffer(
                    _gameProvider.Game.GraphicsDevice,
                    IndexElementSize.ThirtyTwoBits,
                    terrainIndexes.Length,
                    BufferUsage.WriteOnly
                );
            }

            _terrainIndexBuffer.SetData(terrainIndexes);

            if (_effect == null)
            {
                _effect = _gameProvider.Game.EffectCollection.BuildTextureEffect(_contentStrings.GrassTexture);
            }
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

        private float? GetExactHeightAt(float xCoord, float yCoord)
        {
            xCoord /= _scale.X;
            yCoord /= _scale.Y;

            return _heightMap.GetExactHeightAt(xCoord, yCoord);
        }

        private Ray? ClipRay(Ray ray)
        {
            var min = _heightMap.Min * _scale.Z;
            var max = _heightMap.Max * _scale.Z;

            return ray.ClipToZ(min, max);
        }

        private Vector3 BinarySearch(Ray ray)
        {
            var accuracy = 0.01f;
            var heightAtStartingPoint = GetExactHeightAt(ray.Position.X, -ray.Position.Y);

            var currentError = ray.Position.Z - heightAtStartingPoint;
            var counter = 0;

            while (currentError > accuracy)
            {
                ray.Direction /= 2.0f;
                var nextPoint = ray.Position + ray.Direction;
                var heightAtNextPoint = GetExactHeightAt(nextPoint.X, -nextPoint.Y);
                if (nextPoint.Z > heightAtNextPoint)
                {
                    ray.Position = nextPoint;
                    currentError = ray.Position.Z - heightAtNextPoint;
                }

                if (counter++ == 1000)
                    break;
            }

            return ray.Position;
        }
    }
}