using Microsoft.Xna.Framework;

namespace DavidFidge.MonoGame.Core.ContentPipeline
{
    public class TagObject
    {
        public BoundingBox BoundingBox { get; set; }

        public TagObject(BoundingBox boundingBox)
        {
            BoundingBox = boundingBox;
        }
    }
}