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
            LoadContentInternal(@"Models\Augmented");

            var effects = _model.Meshes.SelectMany(m => m.Effects).ToList();

            foreach (Effect effect in effects)
            {
                if (effect is BasicEffect basicEffect)
                    basicEffect.CopyLightingFrom(_gameProvider.Game.EffectCollection.MasterEffectTemplate);
            }
        }
    }
}
