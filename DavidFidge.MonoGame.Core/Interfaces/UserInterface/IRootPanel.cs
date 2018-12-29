using GeonBit.UI.Entities;

namespace DavidFidge.MonoGame.Core.Interfaces
{
    public interface IRootPanel<T>
    {
        bool Visible { get; set; }
        bool IsMouseInRootPanelEmptySpace { get; }
        void Initialize(string panelIdentifier);
        void AddChild(T child);
        void AddChild(IRootPanel<T> child);
        void AddRootPanelToGraph(T root);
        void AddAsChildOf(Panel panel);
    }
}