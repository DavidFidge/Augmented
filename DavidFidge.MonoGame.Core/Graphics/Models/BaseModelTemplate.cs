using System;
using System.Collections.Generic;
using System.Linq;

using DavidFidge.MonoGame.Core.Interfaces.Components;
using DavidFidge.MonoGame.Core.Interfaces.Graphics;

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

        public IWorldTransform WorldTransform { get; }

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
        }

        protected void LoadContent(string model)
        {
            var modelFromContent = _gameProvider.Game.Content.Load<Model>(model);
            LoadContent(modelFromContent);
        }

        public void Draw(Matrix view, Matrix projection)
        {
            var graphicsDevice = _gameProvider.Game.GraphicsDevice;

            // All the model bone transforms are relative to its parent bone.  Create absolute transforms that shuffle up all values and are relative to the world (i.e. absolute values).  These can then by multiplied by world matrix.
            _model.CopyAbsoluteBoneTransformsTo(ModelTransforms);

            graphicsDevice.BlendState = BlendState.Opaque;
            graphicsDevice.DepthStencilState = DepthStencilState.Default;
            graphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;

            foreach (ModelMesh mesh in _model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = ModelTransforms[mesh.ParentBone.Index] * WorldTransform.World;
                    effect.View = view;
                    effect.Projection = projection;
                }

                mesh.Draw();
            }
        }
    }
}
