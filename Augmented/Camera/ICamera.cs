using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Augmented
{
    public interface ICamera : ITransform
    {
        void Reset(float z, CameraResetOptions cameraResetOptions);
        Matrix View { get; }
        Matrix Projection { get; }
        void Update();
        void Initialise();
    }
}