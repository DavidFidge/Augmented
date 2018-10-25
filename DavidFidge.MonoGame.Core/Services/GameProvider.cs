using System;
using System.Collections.Generic;
using System.Linq;

using DavidFidge.MonoGame.Core.Interfaces;

namespace DavidFidge.MonoGame.Core.Services
{
    public class GameProvider : IGameProvider
    {
        public IGame Game { get; set; }
    }
}