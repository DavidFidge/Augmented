using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace DavidFidge.MonoGame.Core.ContentPipeline
{
    public class TagObjectTypeReader : ContentTypeReader<TagObject>
    {
        protected override TagObject Read(ContentReader contentReader, TagObject tagObject)
        {
            var boundingBox = contentReader.ReadObject<BoundingBox>();

            if (tagObject == null)
                tagObject = new TagObject(boundingBox);
            else
                tagObject.BoundingBox = boundingBox;

            return tagObject;
        }
    }
}