using System;

using Castle.Core.Internal;

using DavidFidge.MonoGame.Core.Interfaces.UserInterface;

using InputHandlers.Keyboard;

using Microsoft.Xna.Framework.Input;

namespace DavidFidge.MonoGame.Core.UserInterface
{
    public class ActionMap : IActionMap
    {
        private readonly IActionMapStore _actionMapStore;

        public ActionMap(IActionMapStore actionMapStore)
        {
            _actionMapStore = actionMapStore;
        }

        public bool ActionIs<T>(Keys key, KeyboardModifier keyboardModifier)
        {
            return ActionIs<T>(new KeyCombination(key, keyboardModifier));
        }

        public bool ActionIs<T>(KeyCombination keyCombination)
        {
            var actionMap = typeof(T).GetAttribute<ActionMapAttribute>();

            if (actionMap == null)
                throw new Exception($"No {typeof(ActionMapAttribute).Name} found on class {typeof(T).Name}");

            var actionToKey = _actionMapStore.GetKeyMap();

            if (!actionToKey.ContainsKey(actionMap.Name))
                return false;

            return keyCombination.Equals(actionToKey[actionMap.Name]);
        }
    }
}
