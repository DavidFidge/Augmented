using Microsoft.Xna.Framework;

namespace Augmented
{
    public interface IAugmentedGameWorld
    {
        void Draw(Matrix projection, Matrix view);
        void Initialise();
    }
}