using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

namespace DavidFidge.MonoGame.Core.ContentPipeline
{
    [ContentTypeWriter]
    public class TagObjectTypeWriter : ContentTypeWriter<TagObject>
    {
        protected override void Write(ContentWriter contentWriter, TagObject tagObject)
        {
            contentWriter.WriteObject(tagObject.BoundingBox);
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(TagObjectTypeReader).AssemblyQualifiedName;
        }
    }
}