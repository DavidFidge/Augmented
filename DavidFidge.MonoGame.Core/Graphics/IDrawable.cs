using DavidFidge.MonoGame.Core.Interfaces.Graphics;

using Microsoft.Xna.Framework;

namespace DavidFidge.MonoGame.Core.Graphics
{
    public interface IDrawable : IWorldTransformable
    {
        void Draw(Matrix view, Matrix projection);
    }
}