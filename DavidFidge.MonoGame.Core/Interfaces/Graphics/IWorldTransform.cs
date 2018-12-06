using Microsoft.Xna.Framework;

namespace DavidFidge.MonoGame.Core.Interfaces.Graphics
{
    public interface IWorldTransform : ITransform
    {
        Matrix World
        {
            get;
        }
    }
}