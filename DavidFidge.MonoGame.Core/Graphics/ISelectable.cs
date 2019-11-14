using Microsoft.Xna.Framework;

namespace DavidFidge.MonoGame.Core.Graphics
{
    public interface ISelectable
    {
        bool IsSelected { get; set; }
        float? Intersects(Ray ray, Matrix worldTransform);
    }
}