namespace DavidFidge.MonoGame.Core.Graphics.Terrain
{
    public class DividingHeightsReducer : IDiamondSquareHeightsReducer
    {
        public int Divisor { get; set; }

        public void Initialise(DiamondSquare diamondSquare)
        {
            if (Divisor == 0)
                Divisor = 2;
        }

        public int ReduceMaxHeight(DiamondSquare diamondSquare)
        {
            return diamondSquare.MaxHeight / Divisor;
        }

        public int ReduceMinHeight(DiamondSquare diamondSquare)
        {
            return diamondSquare.MinHeight / Divisor;
        }
    }

    public class SubtractingHeightsReducer : IDiamondSquareHeightsReducer
    {
        private int _minHeightDeduction;
        private int _maxHeightDeduction;

        public void Initialise(DiamondSquare diamondSquare)
        {
            _maxHeightDeduction = diamondSquare.MaxHeight / diamondSquare.NumberOfSteps * 2;
            _minHeightDeduction = diamondSquare.MinHeight / diamondSquare.NumberOfSteps * 2;
        }

        public int ReduceMaxHeight(DiamondSquare diamondSquare)
        {
            return diamondSquare.MaxHeight - _maxHeightDeduction;
        }

        public int ReduceMinHeight(DiamondSquare diamondSquare)
        {
            return diamondSquare.MinHeight - _minHeightDeduction;
        }
    }
}