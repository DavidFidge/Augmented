using System;
using System.Collections.Generic;
using System.Linq;

namespace DavidFidge.MonoGame.Core.Interfaces
{
    public interface IGameProvider
    {
        IGame Game { get; set; }
    }
}