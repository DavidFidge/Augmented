using System;
using System.Collections.Generic;
using System.Security.Cryptography;

using DavidFidge.MonoGame.Core.Interfaces.Components;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DavidFidge.MonoGame.Core.Graphics.Extensions
{
    public static class PointExtensions
    {
        public static List<Point> SurroundingPoints(this Point centrePoint, int? xMin, int? xMax, int? yMin, int? yMax)
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
    }
}
