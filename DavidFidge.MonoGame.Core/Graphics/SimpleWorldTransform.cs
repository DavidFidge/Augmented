using DavidFidge.MonoGame.Core.Interfaces.Graphics;

using Microsoft.Xna.Framework;

namespace DavidFidge.MonoGame.Core.Graphics
{
    public class SimpleWorldTransform : IWorldTransform
    {
        public SimpleWorldTransform()
        {
            Scale = Vector3.One;
            Translation = Vector3.Zero;
            Rotation = Matrix.Identity;
            UpdateWorldTransform();
        }

        private void UpdateWorldTransform()
        {
            World = ScaleMatrix * Rotation * TranslationMatrix;
        }

        public void ChangeTranslationRelative(Vector3 translationDelta)
        {
            Translation = Vector3.Add(Translation, translationDelta);
            UpdateWorldTransform();
        }

        public void ChangeTranslation(Vector3 translation)
        {
            Translation = translation;
            UpdateWorldTransform();
        }

        public void ChangeScaleRelative(Vector3 scaleDelta)
        {
            Scale = Vector3.Add(Scale, scaleDelta);
            UpdateWorldTransform();
        }

        public void ChangeScale(Vector3 scale)
        {
            Scale = scale;
            UpdateWorldTransform();
        }

        public void ChangeRotationRelative(float x, float y, float z)
        {
            var rotation = Matrix.Identity;

            if (x != 0)
                rotation *= Matrix.CreateRotationX(x);
            if (y != 0)
                rotation *= Matrix.CreateRotationY(y);
            if (z != 0)
                rotation *= Matrix.CreateRotationZ(z);

            Rotation = rotation * Rotation;
            UpdateWorldTransform();
        }

        public void ChangeRotation(float x, float y, float z)
        {
            var rotation = Matrix.Identity;

            if (x != 0)
                rotation *= Matrix.CreateRotationX(x);
            if (y != 0)
                rotation *= Matrix.CreateRotationY(y);
            if (z != 0)
                rotation *= Matrix.CreateRotationZ(z);

            Rotation = rotation;
            UpdateWorldTransform();
        }

        public Vector3 Translation { get; private set; }
        public Matrix TranslationMatrix => Matrix.CreateTranslation(Translation);
        public Vector3 Scale { get; private set; }
        public Matrix ScaleMatrix => Matrix.CreateScale(Scale);
        public Matrix Rotation { get; private set; }
        public Matrix World { get;  private set; }
    }
}