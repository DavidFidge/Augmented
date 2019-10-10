namespace Augmented.Interfaces
{
    public interface IAugmentedGameWorldFactory
    {
        IAugmentedGameWorld Create();
        void Release(IAugmentedGameWorld augmentedGameWorld);
    }
}