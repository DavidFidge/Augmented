using DavidFidge.MonoGame.Core.Graphics;
using DavidFidge.MonoGame.Core.Interfaces.Graphics;

namespace Augmented.Interfaces
{
    public interface IAugmentedGameWorld
    {
        void RecreateHeightMap();
        void Update();
        ISceneGraph SceneGraph { get; }
    }
}