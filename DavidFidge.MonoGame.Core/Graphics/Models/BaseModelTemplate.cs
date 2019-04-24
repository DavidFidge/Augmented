using System;
using System.Collections.Generic;
using System.Linq;

using DavidFidge.MonoGame.Core.ContentPipeline;
using DavidFidge.MonoGame.Core.Graphics.Extensions;
using DavidFidge.MonoGame.Core.Graphics.Quads;
using DavidFidge.MonoGame.Core.Interfaces.Components;
using DavidFidge.MonoGame.Core.Interfaces.Graphics;
using DavidFidge.MonoGame.Core.Interfaces.Services;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DavidFidge.MonoGame.Core.Graphics.Models
{
    public abstract class BaseModelTemplate : IDrawable, ISelectable
    {
        protected readonly IGameProvider _gameProvider;
        protected Model _model { get; set; }

        // Holds working transforms used internally
        protected Matrix[] ModelTransforms;

        // Holds original transforms of all bones (meshes) that were loaded in from source
        protected Matrix[] OriginalTransforms;
        private Effect _boundingBoxEffect;
        private VertexBuffer _boundingBoxVertexBuffer;
        private IndexBuffer _boungingBoxIndexBuffer;
        private BoundingBox _boundingBox;
        private BoundingSphere _boundingSphere;
        private TexturedQuadTemplate _selectionQuad;

        public IWorldTransform WorldTransform { get; }
        public IConfigurationSettings ConfigurationSettings { get; set; }

        private Color _selectionColour;
        public Color SelectionColour
        {
            get => _selectionColour;
            set
            {
                _selectionColour = value;
                _selectionQuad.Effect.Parameters["Colour"].SetValue(_selectionColour.ToVector4());
            }
        }

        protected BaseModelTemplate(IGameProvider gameProvider)
        {
            _gameProvider = gameProvider;
            WorldTransform = new SimpleWorldTransform();
        }

        protected void LoadContentInternal(Model model)
        {
            _model = model;

            ModelTransforms = new Matrix[_model.Bones.Count];

            // Save original transforms of all bones (meshes) so that original state/positions of model can be returned.
            OriginalTransforms = new Matrix[_model.Bones.Count];

            _model.CopyBoneTransformsTo(OriginalTransforms);

            if (_model.Tag is TagObject tag)
            {
                _boundingBox = tag.BoundingBox;
                _boundingSphere = tag.BoundingSphere;
            }
            else
            {
                _boundingBox = new BoundingBox();
                _boundingSphere = new BoundingSphere();
            }

            SetupBoundingBoxEffect();
            SetupSelectionQuad();
        }

        private void SetupSelectionQuad()
        {
            _selectionQuad = new TexturedQuadTemplate(_gameProvider);

            _selectionQuad.LoadContent(
                (_boundingBox.Max.X - _boundingBox.Min.X),
                (_boundingBox.Max.Y - _boundingBox.Min.Y),
                _gameProvider.Game.CoreContent.SelectionTexture,
                _gameProvider.Game.CoreContent.SelectionEffect
                );

            SelectionColour = Color.Yellow;

            _selectionQuad.WorldTransform.ChangeTranslation(
                new Vector3(
                    _boundingBox.Min.X + ((_boundingBox.Max.X - _boundingBox.Min.X) / 2),
                    _boundingBox.Min.Y + ((_boundingBox.Max.Y - _boundingBox.Min.Y) / 2),
                    _boundingBox.Min.Z)
                );

            _selectionQuad.WorldTransform.ChangeScale(new Vector3(2f));
        }

        private void SetupBoundingBoxEffect()
        {
            if (ConfigurationSettings != null && ConfigurationSettings.GraphicsSettings.ShowBoundingBoxes)
            {
                var boundingBoxVertices = _boundingBox
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
                    boundingBoxIndices.Length,
                    BufferUsage.WriteOnly
                );

                _boungingBoxIndexBuffer.SetData(boundingBoxIndices);

                _boundingBoxEffect = _gameProvider.Game.EffectCollection.BuildMaterialEffect(Color.Yellow);
            }
        }

        protected void LoadContentInternal(string model)
        {
            var modelFromContent = _gameProvider.Game.Content.Load<Model>(model);
            LoadContentInternal(modelFromContent);
        }

        public void Draw(Matrix view, Matrix projection)
        {
            var graphicsDevice = _gameProvider.Game.GraphicsDevice;

            DrawModel(view, projection);
            DrawBoundingBox(view, projection, graphicsDevice);
            _selectionQuad.Draw(view, projection);
        }

        private void DrawModel(Matrix view, Matrix projection)
        {
            // All the model bone transforms are relative to its parent bone.  Create absolute transforms that shuffle up all values and are relative to the world (i.e. absolute values).  These can then by multiplied by world matrix.
            _model.CopyAbsoluteBoneTransformsTo(ModelTransforms);

            foreach (ModelMesh mesh in _model.Meshes)
            {
                foreach (Effect effect in mesh.Effects)
                {
                    effect.SetWorldViewProjection(
                        ModelTransforms[mesh.ParentBone.Index] * WorldTransform.World,
                        view,
                        projection
                    );

                    foreach (var pass in effect.CurrentTechnique.Passes)
                    {
                        pass.Apply();

                        mesh.Draw();
                    }
                }
            }
        }

        private void DrawBoundingBox(Matrix view, Matrix projection, GraphicsDevice graphicsDevice)
        {
            if (_boundingBoxVertexBuffer != null)
            {
                graphicsDevice.Indices = _boungingBoxIndexBuffer;
                graphicsDevice.SetVertexBuffer(_boundingBoxVertexBuffer);

                _boundingBoxEffect.SetWorldViewProjection(
                    WorldTransform.World,
                    view,
                    projection
                );

                foreach (var pass in _boundingBoxEffect.CurrentTechnique.Passes)
                {
                    pass.Apply();

                    graphicsDevice.DrawIndexedPrimitives(
                        PrimitiveType.LineList,
                        0,
                        0,
                        12
                    );
                }
            }
        }

        public bool IsSelected { get; set; }
    }

    public interface ISelectable
    {
        bool IsSelected { get; set; }
    }
}
