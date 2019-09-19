using System;

using InputHandlers.Keyboard;

using Microsoft.Xna.Framework.Input;

namespace DavidFidge.MonoGame.Core.UserInterface
{
    public class ActionMapAttribute : Attribute
    {
        public string Name { get; set; }
        public Keys DefaultKey { get; set; }
        public KeyboardModifier DefaultKeyboardModifier { get; set; }

        public ActionMapAttribute()
        {
            DefaultKeyboardModifier = KeyboardModifier.None;
        }
    }
}
