using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;

namespace Augmented
{
    public interface IWorldTransformable
    {
        IWorldTransform WorldTransform { get; }
    }

    public interface IWorldTransform : ITransform
    {
        Matrix World
        {
            get;
        }
    }

    public interface ITransform
    {
        void ChangeTranslationRelative(Vector3 translationDelta);
        void ChangeTranslation(Vector3 translation);
        void ChangeScaleRelative(Vector3 scaleDelta);
        void ChangeScale(Vector3 scale);
        void ChangeRotationRelative(float x, float y, float z);
        void ChangeRotation(float x, float y, float z);

        Vector3 Translation { get; }
        Matrix TranslationMatrix { get; }

        Vector3 Scale { get; }
        Matrix ScaleMatrix { get; }

        Matrix Rotation { get; }
    }
}
