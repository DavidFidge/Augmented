using System;
using System.Collections.Generic;
using System.Linq;

using DavidFidge.MonoGame.Core.Extensions;
using DavidFidge.MonoGame.Core.Interfaces.Services;

using Microsoft.Xna.Framework;

namespace DavidFidge.MonoGame.Core.Physics
{
    public class SteeringBehaviors
    {
        public Vector3 Calculate()
        {
            return new Vector3();
        }
    }

    public class MovableComponent : IMovable
    {
        public float Mass { get; set; }
        public float MaxSpeed { get; set; }
        public float MaxForce { get; set; }
        public float MaxTurnRate { get; set; }
        public Vector3 Velocity { get; set; }
        public Vector3 Heading { get; set; }
        public Vector3 Side { get; set; }
        public SteeringBehaviors SteeringBehaviors { get; set; }

        public void Update(IGameTimeService gameTimeService)
        {
            var steeringForce = SteeringBehaviors.Calculate();

            var acceleration = steeringForce / Mass;
            Velocity = acceleration * gameTimeService.GameTime.ElapsedGameTime.Milliseconds;
            Velocity.Truncate(MaxSpeed);
        }
        

    }

    public interface IMovable
    {
        float Mass { get; set; }
        float MaxSpeed { get; set; }
        float MaxForce { get; set; }
        float MaxTurnRate { get; set; }
        Vector3 Velocity { get; set; }
        Vector3 Heading { get; set; }
        Vector3 Side { get; set; }
        SteeringBehaviors SteeringBehaviors { get; set; }
    }
}
