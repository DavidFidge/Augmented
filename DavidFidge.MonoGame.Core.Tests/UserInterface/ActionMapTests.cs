﻿using System;
using System.Collections.Generic;

using DavidFidge.MonoGame.Core.Interfaces.UserInterface;
using DavidFidge.MonoGame.Core.UserInterface;
using DavidFidge.TestInfrastructure;

using InputHandlers.Keyboard;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework.Input;

using NSubstitute;

namespace DavidFidge.MonoGame.Core.Tests.UserInterface
{
    [TestClass]
    public class ActionMapTests : BaseTest
    {
        private IActionMapStore _actionMapStore;
        private ActionMap _actionMap;

        [TestInitialize]
        public override void Setup()
        {
            base.Setup();

            _actionMapStore = Substitute.For<IActionMapStore>();
            _actionMap = new ActionMap(_actionMapStore);
        }

        [TestMethod]
        public void ActionIs_Should_Return_False_If_Action_Store_Does_Not_Contain_Action()
        {
            // Arrange
            var keyCombinations = new Dictionary<string, KeyCombination>()
            {
                {
                    "TestMapOther", new KeyCombination(Keys.A)
                }
            };

            _actionMapStore.GetKeyMap().Returns(keyCombinations);

            // Act
            var result = _actionMap.ActionIs<TestAction>(new KeyCombination(Keys.A));

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ActionIs_Should_Throw_Exception_If_Action_Is_Not_An_Attribute_On_Generic_Class()
        {
            // Arrange
            var keyCombinations = new Dictionary<string, KeyCombination>();

            _actionMapStore.GetKeyMap().Returns(keyCombinations);

            // Act
            var result = Assert.ThrowsException<Exception>(() => _actionMap.ActionIs<TestNoActionMapAttribute>(new KeyCombination(Keys.A)));

            // Assert
            Assert.AreEqual("No ActionMapAttribute found on class TestNoActionMapAttribute", result.Message);
        }

        [TestMethod]
        public void ActionIs_Should_Return_True_If_Key_And_Modifier_Match_Store()
        {
            // Arrange
            var keyCombinations = new Dictionary<string, KeyCombination>()
            {
                {
                    "TestMap1", new KeyCombination(Keys.A, KeyboardModifier.Alt)
                }
            };

            _actionMapStore.GetKeyMap().Returns(keyCombinations);

            // Act
            var result = _actionMap.ActionIs<TestAction>(new KeyCombination(Keys.A, KeyboardModifier.Alt));

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ActionIs_Should_Return_False_If_Key_Does_Not_Match_Store()
        {
            // Arrange
            var keyCombinations = new Dictionary<string, KeyCombination>()
            {
                {
                    "TestMap1", new KeyCombination(Keys.A)
                }
            };

            _actionMapStore.GetKeyMap().Returns(keyCombinations);

            // Act
            var result = _actionMap.ActionIs<TestAction>(new KeyCombination(Keys.B));

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        [DataRow(KeyboardModifier.Ctrl)]
        [DataRow(KeyboardModifier.Alt | KeyboardModifier.Ctrl)]
        [DataRow(KeyboardModifier.None)]
        public void ActionIs_Should_Return_False_If_Modifier_Does_Not_Match_Store(KeyboardModifier keyboardModifier)
        {
            // Arrange
            var keyCombinations = new Dictionary<string, KeyCombination>()
            {
                {
                    "TestMap1", new KeyCombination(Keys.A, KeyboardModifier.Alt)
                }
            };

            _actionMapStore.GetKeyMap().Returns(keyCombinations);

            // Act
            var result = _actionMap.ActionIs<TestAction>(new KeyCombination(Keys.A, keyboardModifier));

            // Assert
            Assert.IsFalse(result);
        }

        [ActionMap(Name = "TestMap1")]
        public class TestAction
        {
        }

        public class TestNoActionMapAttribute
        {
        }
    }
}
