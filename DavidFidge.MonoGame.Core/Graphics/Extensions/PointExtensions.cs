using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

namespace DavidFidge.MonoGame.Core.Graphics.Extensions
{
    public static class PointExtensions
    {
        public static List<Point> SurroundingPoints(this Point centrePoint, int? xMin = null, int? xMax = null, int? yMin = null, int? yMax = null)
        {
            var pointList = new List<Point>();

            for (var x = Math.Max(xMin ?? centrePoint.X - 1, centrePoint.X - 1);
                x <= Math.Min(xMax ?? centrePoint.X + 1, centrePoint.X + 1);
                x++)
            {
                for (var y = Math.Max(yMin ?? centrePoint.Y - 1, centrePoint.Y - 1);
                    y <= Math.Min(yMax ?? centrePoint.Y + 1, centrePoint.Y + 1);
                    y++)
                {
                    var point = new Point(x, y);

                    if (point != centrePoint)
                        pointList.Add(point);
                }
            }

            return pointList;
        }

        public static Vector2 ToVector(this Point point)
        {
            return new Vector2(point.X, point.Y);
        }
    }
}
