namespace DavidFidge.MonoGame.Core.Graphics
{
    public interface IActionable : IBaseSelectable
    {
        bool IsTargeted { get; set; }
    }
}