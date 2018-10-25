using System;
using System.Collections.Generic;
using System.Linq;

using DavidFidge.MonoGame.Core.UserInterface;
using GeonBit.UI.Entities;

namespace DavidFidge.MonoGame.Core.Interfaces
{
    public interface IView<T>
    {
        void Show();
        void Hide();
        void Initialize();

        IRootPanel<T> RootPanel { get; }
    }
}