using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

using Microsoft.Xna.Framework;

namespace Augmented.Graphics.TerrainSpace
{
    public class HeightMapGenerator : IHeightMapStore
    {
        private int _length = 100;
        private int _width = 100;

        public int[,] HeightMap { get; set; }

        public HeightMapGenerator CreateHeightMap(int width, int length)
        {
            _width = width;
            _length = length;

            HeightMap = new int[_width, _length];

            return this;
        }

        private HeightMapGenerator Randomise(int patchX, int patchY)
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

        private HeightMapGenerator Hill(Vector2 relativeCentre, Vector2 relativeSize, int height)
        {
            if (relativeCentre.X > 1.0f || relativeCentre.X < 0f)
                throw new ArgumentException("x must be between 0-1 inclusive", nameof(relativeCentre));

            if (relativeCentre.Y > 1.0f || relativeCentre.Y < 0f)
                throw new ArgumentException("y must be between 0-1 inclusive", nameof(relativeCentre));

            if (relativeSize.X > 1.0f || relativeSize.X < 0f)
                throw new ArgumentException("x must be between 0-1 inclusive", nameof(relativeSize));

            if (relativeSize.Y > 1.0f || relativeSize.Y < 0f)
                throw new ArgumentException("y must be between 0-1 inclusive", nameof(relativeSize));

            var actualCentreX = (int)(relativeCentre.X * _width - 1);
            var actualCentreY = (int)(relativeCentre.Y * _length - 1);

            var centrePoint = new Point(actualCentreX, actualCentreY);

            HeightMap[actualCentreX, actualCentreY] = height;

            var xMin = Math.Max(actualCentreX - (int)(relativeSize.X * _width), 0);
            var xMax = Math.Min(actualCentreX + (int)(relativeSize.X * _width), _width - 1);

            var yMin = Math.Max(actualCentreY - (int)(relativeSize.Y * _length), 0);
            var yMax = Math.Min(actualCentreY + (int)(relativeSize.Y * _length), _length - 1);

            var radiusX = (int) (relativeSize.X * _width / 2);
            var radiusY = (int) (relativeSize.Y * _length / 2);

            for (var x = xMin; x <= xMax; x++)
            {
                for (var y = yMin; y <= yMax; y++)
                {
                    var point = new Point(x, y);

                    if (point == centrePoint)
                        continue;

                    var xOrigin = x - centrePoint.X;
                    var yOrigin = y - centrePoint.Y;

                    var angle = Math.Atan2(yOrigin, xOrigin);

                    var xEllipse = (int) (radiusX * Math.Cos(angle));
                    var yEllipse = (int) (radiusY * Math.Sin(angle));

                    var ellipseLength = new Vector2(xEllipse, yEllipse).Length();
                    var pointLength = new Vector2(xOrigin, yOrigin).Length();

                    var ratio = pointLength / ellipseLength;

                    if (ratio > 1)
                        continue;

                    HeightMap[x, y] += (int)((1 - ratio) * height);
                }
            }

            return this;
        }

        public int[,] GetHeightMap()
        {
            return CreateHeightMap(101, 101)
                .Hill(new Vector2(0.5f, 0.5f), new Vector2(0.4f, 0.2f), 20)
                .HeightMap;
        }
    }
}
