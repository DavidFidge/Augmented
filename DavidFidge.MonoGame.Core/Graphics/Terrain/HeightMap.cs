using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

using Microsoft.Xna.Framework;

namespace DavidFidge.MonoGame.Core.Graphics.Terrain
{
    public class HeightMap : IEnumerable<int>, ICloneable
    {
        public int Length { get; }
        public int Width { get; }

        private readonly int[,] _heightMap;

        public int this[int y, int x]
        {
            get => _heightMap[y, x];
            set => _heightMap[y, x] = value;
        }

        public int Area => Length * Width;

        public HeightMap FromArray(int[] array)
        {
            if (array.Length != Width * Length)
                throw new ArgumentException($"Array must have length equal to area of {Area}", nameof(array));

            var i = 0;

            for (var y = 0; y < _heightMap.GetLength(0); y++)
            {
                for (var x = 0; x < _heightMap.GetLength(1); x++)
                {
                    _heightMap[y, x] = array[i++];
                }
            }

            return this;
        }

        public HeightMap(int width, int length)
        {
            Width = width;
            Length = length;

            _heightMap = new int[Length, Width];
        }

        public void Export(string folder = null, string name = null)
        {
            if (folder == null)
                folder = Assembly.GetExecutingAssembly().GetName().Name.Split('.')[0];

            var dirPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                folder);
            
            var dirInfo = new DirectoryInfo(dirPath);

            if (!dirInfo.Exists)
                dirInfo.Create();

            if (name == null)
                name = $"HeightMap{DateTime.Now:yyyyMMddHHmmssfff}.csv";

            var filePath = Path.Combine(
                dirPath,
                name);

            using (var file = new StreamWriter(filePath))
            {
                var stringBuilder = new StringBuilder(_heightMap.Length * 20);

                for (var y = 0; y < _heightMap.GetLength(0); y++)
                {
                    for (var x = 0; x < _heightMap.GetLength(1); x++)
                    {
                        stringBuilder.Append(_heightMap[y, x].ToString());

                        if (x != _heightMap.GetLength(1) - 1)
                            stringBuilder.Append(",");
                    }

                    file.WriteLine(stringBuilder.ToString());

                    stringBuilder.Clear();
                }
            }
        }

        public void Patch(HeightMap heightMap, Point topLeft, PatchMethod patchMethod = PatchMethod.Replace)
        {
            var xMinBoundingSquare = Math.Max(topLeft.X, 0);
            var xMaxBoundingSquare = Math.Min(topLeft.X + heightMap.Width - 1, Width - 1);

            var yMinBoundingSquare = Math.Max(topLeft.Y, 0);
            var yMaxBoundingSquare = Math.Min(topLeft.Y + heightMap.Length - 1, Length - 1);

            if (xMinBoundingSquare > xMaxBoundingSquare || yMinBoundingSquare > yMaxBoundingSquare)
                return;

            for (var y = yMinBoundingSquare; y <= yMaxBoundingSquare; y++)
            {
                for (var x = xMinBoundingSquare; x <= xMaxBoundingSquare; x++)
                {
                    var pointHeight = heightMap[y - topLeft.Y, x - topLeft.X];

                    if (patchMethod == PatchMethod.None || patchMethod == PatchMethod.Replace)
                    {
                        _heightMap[y, x] = pointHeight;
                    }
                    else if (patchMethod == PatchMethod.ReplaceIfHigher && _heightMap[y, x] < pointHeight)
                    {
                        _heightMap[y, x] = pointHeight;
                    }
                    else if (patchMethod == PatchMethod.ReplaceIfLower && _heightMap[y, x] > pointHeight)
                    {
                        _heightMap[y, x] = pointHeight;
                    }
                    else if (patchMethod == PatchMethod.Additive)
                    {
                        _heightMap[y, x] += pointHeight;
                    }
                }
            }
        }

        public float ZeroAreaPercent()
        {
            return (float)_heightMap.Cast<int>().Count(i => i == 0) / Area;
        }

        public IEnumerator<int> GetEnumerator()
        {
            return _heightMap.Cast<int>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public object Clone()
        {
            return new HeightMap(Width, Length)
                .FromArray(_heightMap.Cast<int>().ToArray());
        }

        public enum PatchMethod
        {
            None,
            Replace,
            ReplaceIfHigher,
            ReplaceIfLower,
            Additive
        }
    }
}
