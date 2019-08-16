using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

using DavidFidge.MonoGame.Core.Graphics.Extensions;
using DavidFidge.MonoGame.Core.Interfaces.Components;

using Microsoft.Xna.Framework;

namespace DavidFidge.MonoGame.Core.Graphics.Terrain
{
    public class DiamondSquare
    {
        private readonly IRandom _random;
        private int _minHeight;
        private int _maxHeight;
        private int _numberOfSteps;
        private int _currentStep;
        private Point _midPoint;

        public HeightMap HeightMap { get; private set; }

        public List<Square> Squares { get; set; }

        public DiamondSquare(IRandom random)
        {
            _random = random;
        }

        public DiamondSquare Execute(int heightMapSize, int minHeight, int maxHeight)
        {
            _maxHeight = maxHeight;
            _minHeight = minHeight;

            Squares = new List<Square>();

            if (Math.Log(heightMapSize, 2) % (int)Math.Log(heightMapSize, 2) > 0.0001d)
            {
                throw new Exception("Diamond square currently only supports square heightmaps with size equal to square root 2.  If your height map is a different size then generate a diamond square map bigger and patch your height map with a portion of it");
            }

            if (heightMapSize < 2)
                throw new Exception("Height map size must be at least 4");

            _numberOfSteps = (int) Math.Log(heightMapSize, 2);
            _currentStep = _numberOfSteps;

            HeightMap = new HeightMap(heightMapSize + 1, heightMapSize + 1);

            _midPoint = new Point(HeightMap.Width / 2, HeightMap.Width / 2);

            HeightMap[_midPoint.X, _midPoint.Y] = _maxHeight;

            Squares.Add(
                new Square(
                    new Point(0, 0),
                    new Point(0, heightMapSize),
                    new Point(heightMapSize, 0),
                    new Point(heightMapSize, heightMapSize)
                ));

            while (_currentStep > 0)
            {
                ExecuteNextDiamondSquareStep();
                _currentStep--;
            }

            return this;
        }

        private void ExecuteNextDiamondSquareStep()
        {
            var newSquares = new List<Square>();
            var squareStepPoints = new List<Point>();

            foreach (var squarePart in Squares)
            {
                var midPoint = squarePart.Midpoint;

                SetHeight(midPoint);

                BuildNewSquares(midPoint, squarePart, newSquares, squareStepPoints);
            }

            if (_currentStep != _numberOfSteps)
                ReduceHeightLimits();

            foreach (var squareStepPoint in squareStepPoints)
            {
                SetHeight(squareStepPoint);
            }

            ReduceHeightLimits();

            Squares = newSquares;
        }

        private void ReduceHeightLimits()
        {
            _maxHeight /= 2;
            _minHeight /= 2;
        }

        private void SetHeight(Point squareStepPoint)
        {
            if (squareStepPoint != _midPoint)
            {
                HeightMap[squareStepPoint.X, squareStepPoint.Y] = _random.Next(_minHeight, _maxHeight);
            }
        }

        private void BuildNewSquares(Point midPoint, Square currentSquare, List<Square> newSquares, List<Point> squareStepPoints)
        {
            // Top Left quadrant
            var diamondPoint1 = new Point(currentSquare.LeftX, midPoint.Y);
            var diamondPoint2 = new Point(midPoint.X, currentSquare.TopY);

            var newSquare = new Square(
                midPoint,
                new Point(currentSquare.LeftX, currentSquare.TopY),
                diamondPoint1,
                diamondPoint2
            );

            newSquares.Add(newSquare);
            squareStepPoints.Add(diamondPoint1);
            squareStepPoints.Add(diamondPoint2);

            // Top Right quadrant
            diamondPoint1 = new Point(currentSquare.RightX, midPoint.Y);
            diamondPoint2 = new Point(midPoint.X, currentSquare.TopY);

            newSquare = new Square(
                midPoint,
                new Point(currentSquare.RightX, currentSquare.TopY),
                diamondPoint1,
                diamondPoint2
            );

            newSquares.Add(newSquare);
            squareStepPoints.Add(diamondPoint1);
            squareStepPoints.Add(diamondPoint2);

            // Bottom left quadrant
            diamondPoint1 = new Point(currentSquare.LeftX, midPoint.Y);
            diamondPoint2 = new Point(midPoint.X, currentSquare.BottomY);

            newSquare = new Square(
                midPoint,
                new Point(currentSquare.LeftX, currentSquare.BottomY),
                diamondPoint1,
                diamondPoint2
            );

            newSquares.Add(newSquare);
            squareStepPoints.Add(diamondPoint1);
            squareStepPoints.Add(diamondPoint2);

            // Bottom right quadrant
            diamondPoint1 = new Point(currentSquare.RightX, midPoint.Y);
            diamondPoint2 = new Point(midPoint.X, currentSquare.BottomY);

            newSquare = new Square(
                midPoint,
                new Point(currentSquare.RightX, currentSquare.BottomY),
                diamondPoint1,
                diamondPoint2
            );

            newSquares.Add(newSquare);
            squareStepPoints.Add(diamondPoint1);
            squareStepPoints.Add(diamondPoint2);
        }

        public class Square
        {
            public Square(Point point1, Point point2, Point point3, Point point4)
            {
                Points = new List<Point> { point1, point2, point3, point4 };
            }

            public List<Point> Points { get; set; }

            public int TopY => Points.Max(p => p.Y);
            public int BottomY => Points.Min(p => p.Y);
            public int LeftX => Points.Min(p => p.X);
            public int RightX => Points.Max(p => p.X);

            public Point Midpoint => Points.GetMidpoint();
        }
    }
}