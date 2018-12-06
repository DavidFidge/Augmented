using Microsoft.Xna.Framework;

namespace Augmented.Interfaces
{
    public interface IAugmentedGameWorld
    {
        void Draw(Matrix projection, Matrix view);
        void Initialise();
    }
}