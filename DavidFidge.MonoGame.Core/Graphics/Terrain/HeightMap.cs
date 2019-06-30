using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DavidFidge.MonoGame.Core.Graphics.Terrain
{
    public class HeightMap : IEnumerable<int>
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
        
        public HeightMap(int width, int length)
        {
            Width = width;
            Length = length;

            _heightMap = new int[Length, Width];
        }

        public void Export()
        {
            var filePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "Augmented",
                $"HeightMap{DateTime.Now:yyyyMMddHHmmssfff}.csv");

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

        public float ZeroAreaPercent()
        {
            return (float)_heightMap.Cast<int>().Count(i => i != 0) / Area;
        }

        public IEnumerator<int> GetEnumerator()
        {
            return _heightMap.Cast<int>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
