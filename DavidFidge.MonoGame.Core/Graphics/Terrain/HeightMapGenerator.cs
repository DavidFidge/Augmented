using System;
using System.Collections.Generic;

using DavidFidge.MonoGame.Core.Graphics.Extensions;
using DavidFidge.MonoGame.Core.Interfaces.Graphics;

using Microsoft.Xna.Framework;

namespace DavidFidge.MonoGame.Core.Graphics.Terrain
{
    public class HeightMapGenerator : IHeightMapGenerator
    {
        private int _length;
        private int _width;

        public int[,] HeightMap { get; private set; }
        
        public HeightMapGenerator CreateHeightMap(int width, int length)
        {
            _width = width;
            _length = length;

            HeightMap = new int[_width, _length];

            return this;
        }

        public HeightMapGenerator Randomise(int patchX, int patchY)
        {
            var random = new Random();

            for (int x = 0; x < _width - 1; x++)
            {
                for (int y = 0; y < _length - 1; y++)
                {
                    HeightMap[x, y] = random.Next(0, 2);
                }
            }

            return this;
        }

        public enum HillOptions
        {
            None,
            Replace,
            ReplaceIfHeigher,
            ReplaceIfLower,
            Addditive
        }

        public HeightMapGenerator Hill(
            Vector2 relativeCentre,
            Vector2 relativeSize,
            int height,
            HillOptions hillOptions = HillOptions.ReplaceIfHeigher
        )
        {
            var centre = GetActualPointFromRelativeVector(relativeCentre);

            return Hill(centre, relativeSize, height, hillOptions);
        }

        public HeightMapGenerator Hill(
            Point centre,
            Vector2 relativeSize,
            int height,
            HillOptions hillOptions = HillOptions.ReplaceIfHeigher
            )
        {
            if (relativeSize.X > 1.0f || relativeSize.X < 0f)
                throw new ArgumentException("x must be between 0-1 inclusive", nameof(relativeSize));

            if (relativeSize.Y > 1.0f || relativeSize.Y < 0f)
                throw new ArgumentException("y must be between 0-1 inclusive", nameof(relativeSize));

            SetHeightForHillPoint(hillOptions, centre.X, centre.Y, height);

            var hillEllipseRadius = new Point
            (
                (int) Math.Round(relativeSize.X * _width / 2f, MidpointRounding.AwayFromZero),
                (int) Math.Round(relativeSize.Y * _length / 2f, MidpointRounding.AwayFromZero)
            );

            // Get the rectangular region that contains all the points
            // that could be within the area of the ellipse
            var xMinBoundingSquare = Math.Max(centre.X - hillEllipseRadius.X, 0);
            var xMaxBoundingSquare = Math.Min(centre.X + hillEllipseRadius.X, _width - 1);

            var yMinBoundingSquare = Math.Max(centre.Y - hillEllipseRadius.Y, 0);
            var yMaxBoundingSquare = Math.Min(centre.Y + hillEllipseRadius.Y, _length - 1);

            for (var x = xMinBoundingSquare; x <= xMaxBoundingSquare; x++)
            {
                for (var y = yMinBoundingSquare; y <= yMaxBoundingSquare; y++)
                {
                    var candidatePoint = new Point(x, y);

                    if (candidatePoint == centre)
                        continue;

                    // See if the point is within the radius of ellipse
                    // Origin being point 0,0
                    var candidatePointRelativeToOrigin = candidatePoint - centre;

                    var angle = Math.Atan2(candidatePointRelativeToOrigin.Y, candidatePointRelativeToOrigin.X);

                    var ellipsePointAtAngleOfCandidatePoint = new Point(
                        (int) Math.Round(hillEllipseRadius.X * Math.Cos(angle), MidpointRounding.AwayFromZero),
                        (int) Math.Round(hillEllipseRadius.Y * Math.Sin(angle), MidpointRounding.AwayFromZero));

                    var ellipseLength = ellipsePointAtAngleOfCandidatePoint.ToVector2().Length();
                    var pointLength = candidatePointRelativeToOrigin.ToVector2().Length();

                    // if the point is further out than the point that lies on the ellipse then ignore this point
                    var ratio = pointLength / ellipseLength;

                    // if a point is on or beyond the ellipse circumference then it is beyond the range of the hill and thus ignored
                    if (ratio >= 1)
                        continue;

                    // Use Math.Abs on height so that valley depths are consistent with hill heights
                    var pointHeight = (int)Math.Ceiling((1 - ratio) * Math.Abs(height));

                    if (height < 0)
                        pointHeight = -pointHeight;

                    SetHeightForHillPoint(hillOptions, x, y, pointHeight);
                }
            }

            return this;
        }

        private void SetHeightForHillPoint(HillOptions hillOptions, int x, int y, int pointHeight)
        {
            if (hillOptions == HillOptions.None || hillOptions == HillOptions.Replace)
            {
                HeightMap[x, y] = pointHeight;
            }
            else if (hillOptions == HillOptions.ReplaceIfHeigher && HeightMap[x, y] < pointHeight)
            {
                HeightMap[x, y] = pointHeight;
            }
            else if (hillOptions == HillOptions.ReplaceIfLower && HeightMap[x, y] > pointHeight)
            {
                HeightMap[x, y] = pointHeight;
            }
            else if (hillOptions == HillOptions.Addditive)
            {
                HeightMap[x, y] += pointHeight;
            }
        }

        private Point GetActualPointFromRelativeVector(Vector2 relativeVector)
        {
            if (relativeVector.X > 1.0f || relativeVector.X < 0f)
                throw new ArgumentException("x must be between 0-1 inclusive", nameof(relativeVector));

            if (relativeVector.Y > 1.0f || relativeVector.Y < 0f)
                throw new ArgumentException("y must be between 0-1 inclusive", nameof(relativeVector));


            var actualCentreX = (int)(relativeVector.X * (_width - 1));
            var actualCentreY = (int)(relativeVector.Y * (_length - 1));

            return new Point(actualCentreX, actualCentreY);
        }

        public HeightMapGenerator Ridge(
            Vector2 relativeStart,
            Vector2 relativeEnd,
            Vector2 relativeSize,
            int height
        )
        {
            var hillOptions = HillOptions.ReplaceIfHeigher;

            if (height < 0)
                hillOptions = HillOptions.ReplaceIfLower;

            var hillPoints = new List<Point>();

            var start = GetActualPointFromRelativeVector(relativeStart);
            var end = GetActualPointFromRelativeVector(relativeEnd);

            var startVector = start.ToVector2();
            var endVector = end.ToVector2();

            var selectedPoint = start;

            while (selectedPoint != end)
            {
                hillPoints.Add(selectedPoint);

                selectedPoint = GetNextHillPoint(selectedPoint, startVector, endVector);
            }

            if (!hillPoints.Contains(end))
                hillPoints.Add(end);

            foreach (var hillPoint in hillPoints)
            {
                Hill(hillPoint, relativeSize, height, hillOptions);
            }

            return this;
        }

        private Point GetNextHillPoint(
            Point currentPoint,
            Vector2 startVector,
            Vector2 endVector
        )
        {
            var points = currentPoint.SurroundingPoints();

            var currentVector = currentPoint.ToVector2();

            var lengthOfCurrentPointToEnd = (currentVector - endVector).Length();

            var shortestLengthStartToEnd = float.MaxValue;

            Point? shortestLengthStartToEndPoint = null;

            foreach (var point in points)
            {
                var pointVector = point.ToVector2();

                if (pointVector == endVector)
                    return point;

                var pointLengthToEnd = (pointVector - endVector).Length();

                if (pointLengthToEnd > lengthOfCurrentPointToEnd)
                    continue;

                var pointLengthToStart = (pointVector - startVector).Length();

                var pointLengthStartToEnd = pointLengthToStart + pointLengthToEnd;

                if (pointLengthStartToEnd < shortestLengthStartToEnd)
                {
                    shortestLengthStartToEnd = pointLengthStartToEnd;
                    shortestLengthStartToEndPoint = point;
                }
            }

            if (shortestLengthStartToEndPoint == null)
                throw new Exception("Could not find next hill point");

            return shortestLengthStartToEndPoint.Value;
        }
    }
}
