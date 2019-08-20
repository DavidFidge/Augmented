namespace DavidFidge.MonoGame.Core.Graphics.Terrain
{
    public interface IDiamondSquareHeightsReducer
    {
        void Initialise(DiamondSquare diamondSquare);
        int ReduceMaxHeight(DiamondSquare diamondSquare);
        int ReduceMinHeight(DiamondSquare diamondSquare);
    }
}