using System;
using System.Collections.Generic;
using System.Linq;

using DavidFidge.MonoGame.Core.ContentPipeline;
using DavidFidge.MonoGame.Core.Interfaces.Components;
using DavidFidge.MonoGame.Core.Interfaces.Graphics;
using DavidFidge.MonoGame.Core.Interfaces.Services;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DavidFidge.MonoGame.Core.Graphics.Models
{
    public abstract class BaseModelTemplate : IDrawable
    {
        protected readonly IGameProvider _gameProvider;
        protected Model _model { get; set; }

        // Holds working transforms used internally
        protected Matrix[] ModelTransforms;

        // Holds original transforms of all bones (meshes) that were loaded in from source
        protected Matrix[] OriginalTransforms;
        private BasicEffect _boundingBoxEffect;
        private VertexBuffer _boundingBoxVertexBuffer;
        private IndexBuffer _boungingBoxIndexBuffer;

        public IWorldTransform WorldTransform { get; }

        public IConfigurationSettings ConfigurationSettings { get; set; }

        protected BaseModelTemplate(IGameProvider gameProvider)
        {
            _gameProvider = gameProvider;
            WorldTransform = new SimpleWorldTransform();
        }

        protected void LoadContent(Model model)
        {
            _model = model;

            ModelTransforms = new Matrix[_model.Bones.Count];

            // Save original transforms of all bones (meshes) so that original state/positions of model can be returned.
            OriginalTransforms = new Matrix[_model.Bones.Count];

            _model.CopyBoneTransformsTo(OriginalTransforms);

            SetupBoundingBoxEffect();
        }

        private void SetupBoundingBoxEffect()
        {
            if (ConfigurationSettings != null && ConfigurationSettings.GraphicsSettings.ShowBoundingBoxes)
            {
                if (_model.Tag is TagObject tag)
                {
                    var boundingBoxVertices = tag.BoundingBox
                        .GetCorners()
                        .Select(c => new VertexPositionColor(c, Color.White))
                        .ToArray();

                    var boundingBoxIndices = new int[24];

                    var currentIndex = 0;

                    boundingBoxIndices[currentIndex++] = 0;
                    boundingBoxIndices[currentIndex++] = 1;

                    boundingBoxIndices[currentIndex++] = 0;
                    boundingBoxIndices[currentIndex++] = 3;

                    boundingBoxIndices[currentIndex++] = 0;
                    boundingBoxIndices[currentIndex++] = 4;

                    boundingBoxIndices[currentIndex++] = 1;
                    boundingBoxIndices[currentIndex++] = 2;

                    boundingBoxIndices[currentIndex++] = 1;
                    boundingBoxIndices[currentIndex++] = 5;

                    boundingBoxIndices[currentIndex++] = 2;
                    boundingBoxIndices[currentIndex++] = 3;

                    boundingBoxIndices[currentIndex++] = 2;
                    boundingBoxIndices[currentIndex++] = 6;

                    boundingBoxIndices[currentIndex++] = 3;
                    boundingBoxIndices[currentIndex++] = 7;

                    boundingBoxIndices[currentIndex++] = 4;
                    boundingBoxIndices[currentIndex++] = 5;

                    boundingBoxIndices[currentIndex++] = 4;
                    boundingBoxIndices[currentIndex++] = 7;

                    boundingBoxIndices[currentIndex++] = 5;
                    boundingBoxIndices[currentIndex++] = 6;

                    boundingBoxIndices[currentIndex++] = 6;
                    boundingBoxIndices[currentIndex++] = 7;

                    _boundingBoxVertexBuffer = new VertexBuffer(
                        _gameProvider.Game.GraphicsDevice,
                        VertexPositionColor.VertexDeclaration,
                        boundingBoxVertices.Length,
                        BufferUsage.WriteOnly
                    );

                    _boundingBoxVertexBuffer.SetData(boundingBoxVertices);

                    _boungingBoxIndexBuffer = new IndexBuffer(
                        _gameProvider.Game.GraphicsDevice,
                        IndexElementSize.ThirtyTwoBits,
                        sizeof(int) * boundingBoxIndices.Length,
                        BufferUsage.WriteOnly
                    );

                    _boungingBoxIndexBuffer.SetData(boundingBoxIndices);

                    _boundingBoxEffect = _gameProvider.Game.EffectCollection.BuildMaterialEffect(Color.Yellow);
                }
            }
        }

        protected void LoadContent(string model)
        {
            var modelFromContent = _gameProvider.Game.Content.Load<Model>(model);
            LoadContent(modelFromContent);
        }

        public void Draw(Matrix view, Matrix projection)
        {
            var graphicsDevice = _gameProvider.Game.GraphicsDevice;

            graphicsDevice.BlendState = BlendState.Opaque;
            graphicsDevice.DepthStencilState = DepthStencilState.Default;
            graphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;

            DrawModel(view, projection);
            DrawBoundingBox(view, projection, graphicsDevice);
        }

        private void DrawModel(Matrix view, Matrix projection)
        {
            // All the model bone transforms are relative to its parent bone.  Create absolute transforms that shuffle up all values and are relative to the world (i.e. absolute values).  These can then by multiplied by world matrix.
            _model.CopyAbsoluteBoneTransformsTo(ModelTransforms);

            foreach (ModelMesh mesh in _model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    foreach (var pass in effect.CurrentTechnique.Passes)
                    {
                        pass.Apply();

                        effect.World = ModelTransforms[mesh.ParentBone.Index] * WorldTransform.World;
                        effect.View = view;
                        effect.Projection = projection;
                    }
                }

                mesh.Draw();
            }
        }

        private void DrawBoundingBox(Matrix view, Matrix projection, GraphicsDevice graphicsDevice)
        {
            if (_boundingBoxVertexBuffer != null)
            {
                graphicsDevice.Indices = _boungingBoxIndexBuffer;
                graphicsDevice.SetVertexBuffer(_boundingBoxVertexBuffer);

                foreach (var pass in _boundingBoxEffect.CurrentTechnique.Passes)
                {
                    pass.Apply();

                    _boundingBoxEffect.World = WorldTransform.World;
                    _boundingBoxEffect.View = view;
                    _boundingBoxEffect.Projection = projection;

                    graphicsDevice.DrawIndexedPrimitives(
                        PrimitiveType.LineList,
                        0,
                        0,
                        12
                    );
                }
            }
        }
    }
}
