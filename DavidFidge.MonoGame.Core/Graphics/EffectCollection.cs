﻿using System.Collections.Generic;

using DavidFidge.MonoGame.Core.Graphics.Extensions;
using DavidFidge.MonoGame.Core.Interfaces.Components;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DavidFidge.MonoGame.Core.Graphics
{
    public class EffectCollection : Dictionary<string, BasicEffect>
    {
        private IGameProvider _gameProvider;

        public BasicEffect MasterEffectTemplate { get; set; }
        public BasicEffect TextureEffectTemplate { get; set; }

        public EffectCollection(IGameProvider gameProvider)
        {
            _gameProvider = gameProvider;
        }

        public void Initialize()
        {
            TextureEffectTemplate = new BasicEffect(_gameProvider.Game.GraphicsDevice)
            {
                TextureEnabled = true
            };

            MasterEffectTemplate = new BasicEffect(_gameProvider.Game.GraphicsDevice);

            MasterEffectTemplate.EnableDefaultLighting();
            MasterEffectTemplate.DirectionalLight0.Direction = new Vector3(1, 1, 0);
            MasterEffectTemplate.DirectionalLight0.Enabled = true;
            MasterEffectTemplate.DirectionalLight0.DiffuseColor = new Vector3(1, 1, 1);
            MasterEffectTemplate.AmbientLightColor = new Vector3(0.3f, 0.3f, 0.3f);
            MasterEffectTemplate.DirectionalLight1.Enabled = false;
            MasterEffectTemplate.DirectionalLight2.Enabled = false;
            MasterEffectTemplate.SpecularColor = Vector3.Zero;
        }

        public BasicEffect BuildTextureEffect(string texture)
        {
            var basicEffect = (BasicEffect)MasterEffectTemplate.Clone();

            basicEffect.CopyTextureFrom(TextureEffectTemplate);

            basicEffect.Texture = _gameProvider.Game.Content.Load<Texture2D>(texture);

            return basicEffect;
        }

        public BasicEffect BuildMaterialEffect(Color colour)
        {
            var basicEffect = (BasicEffect)MasterEffectTemplate.Clone();

            basicEffect.DiffuseColor = colour.ToVector3();
            basicEffect.Alpha = colour.A / 255.0f;

            return basicEffect;
        }
    }
}