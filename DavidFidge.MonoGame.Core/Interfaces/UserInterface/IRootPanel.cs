using GeonBit.UI.Entities;

namespace DavidFidge.MonoGame.Core.Interfaces
{
    public interface IRootPanel<T>
    {
        bool Visible { get; set; }
        void Initialize();
        void AddChild(T child);
        void AddChild(IRootPanel<T> child);
        void AddRootPanelToGraph(T root);
        void AddAsChildOf(Panel panel);
    }
}