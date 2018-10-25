using System;
using System.Collections.Generic;
using System.Linq;

namespace DavidFidge.MonoGame.Core.Interfaces
{
    public interface IRootPanel<T>
    {
        bool Visible { get; set; }
        void Initialize();
        void AddChild(T child);
        void AddNestedRoot(IRootPanel<T> child);
        void AddRootPanelToGraph(T root);
    }
}