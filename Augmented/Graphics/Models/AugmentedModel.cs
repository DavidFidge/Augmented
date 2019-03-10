using System;
using System.Collections.Generic;
using System.Linq;

using DavidFidge.MonoGame.Core.Graphics.Extensions;
using DavidFidge.MonoGame.Core.Graphics.Models;
using DavidFidge.MonoGame.Core.Interfaces.Components;

using Microsoft.Xna.Framework.Graphics;

namespace Augmented.Graphics.Models
{
    public class AugmentedModel : BaseModelTemplate
    {
        public AugmentedModel(IGameProvider gameProvider) : base(gameProvider)
        {
        }

        public void LoadContent()
        {
            LoadContent(@"Models\Augmented");

            var effects = _model.Meshes.SelectMany(m => m.Effects).ToList();

            foreach (BasicEffect effect in effects)
            {
                effect.CopyLightingFrom(_gameProvider.Game.EffectCollection.MasterEffectTemplate);
            }
        }
    }
}
