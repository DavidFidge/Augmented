﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

using DavidFidge.MonoGame.Core.Interfaces;
using DavidFidge.MonoGame.Core.UserInterface;
using DavidFidge.TestInfrastructure;

using GeonBit.UI.Entities;

using InputHandlers.Keyboard;
using InputHandlers.Mouse;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using NSubstitute;

namespace DavidFidge.MonoGame.Core.Tests.UserInterface
{
    [TestClass]
    public class BaseViewTests : BaseTest
    {
        [TestInitialize]
        public override void Setup()
        {
            base.Setup();
        }

        [TestMethod]
        public void LabelFor_Should_Return_DisplayAttribute_Name()
        {
            // Arrange
            var testView = new TestView(new TestViewModel());

            // Act
            var result = testView.GetLabelForPropertyWithDisplay();

            // Assert
            Assert.AreEqual("Test Header", result);
        }

        [TestMethod]
        public void LabelFor_Should_Return_Property_Name_Split_By_Casing_If_Property_Does_Not_Have_DisplayAttribute()
        {
            // Arrange
            var testView = new TestView(new TestViewModel());

            // Act
            var result = testView.GetLabelForPropertyWithoutDisplay();

            // Assert
            Assert.AreEqual("Property Without Display", result);
        }

        [TestMethod]
        public void Initialize_Should_Create_An_Invisible_Container_Panel_With_Zero_Padding()
        {
            // Arrange
            var testViewModel = new TestViewModel();
            var rootPanel = Substitute.For<IRootPanel<Entity>>();

            var testView = new TestView(testViewModel)
            {
                RootPanel = rootPanel
            };

            // Act
            testView.Initialize();

            // Assert
            Assert.IsTrue(testView.IsInitializeInternalCalled);
            rootPanel.Received().Initialize();
            Assert.IsNotNull(testViewModel.Data);
        }

        [TestMethod]
        public void Show_Should_Show_Panel_And_Change_Mouse_And_Keyboard_Handlers()
        {
            // Arrange
            var keyboardHandler = Substitute.For<IKeyboardHandler>();
            var mouseHandler = Substitute.For<IMouseHandler>();
            var gameInputService = Substitute.For<IGameInputService>();
            var rootPanel = Substitute.For<IRootPanel<Entity>>();

            var testView = new TestView(new TestViewModel())
            {
                KeyboardHandler = keyboardHandler,
                MouseHandler = mouseHandler,
                GameInputService = gameInputService,
                RootPanel = rootPanel
            };

            testView.Initialize();
            rootPanel.ClearReceivedCalls();

            // Act
            testView.Show();

            // Assert
            var setVisibleCall = rootPanel.ReceivedCalls().Single();
            var rootPanelType = typeof(IRootPanel<Entity>);
            var methodInfo = rootPanelType.GetMethod("set_Visible");

            Assert.AreEqual(methodInfo, setVisibleCall.GetMethodInfo());
            Assert.AreEqual(true, (bool)setVisibleCall.GetArguments().Single());

            gameInputService
                .Received()
                .ChangeInput(Arg.Is(keyboardHandler), Arg.Is(mouseHandler));
        }

        [TestMethod]
        public void Hide_Should_Hide_Panel_And_Revert_Input()
        {
            // Arrange
            var keyboardHandler = Substitute.For<IKeyboardHandler>();
            var mouseHandler = Substitute.For<IMouseHandler>();
            var gameInputService = Substitute.For<IGameInputService>();
            var rootPanel = Substitute.For<IRootPanel<Entity>>();

            var testView = new TestView(new TestViewModel())
            {
                KeyboardHandler = keyboardHandler,
                MouseHandler = mouseHandler,
                GameInputService = gameInputService,
                RootPanel = rootPanel
            };

            testView.Initialize();
            testView.Show();
            gameInputService.ClearReceivedCalls();
            rootPanel.ClearReceivedCalls();

            // Act
            testView.Hide();

            // Assert
            var setVisibleCall = rootPanel.ReceivedCalls().Single();
            var rootPanelType = typeof(IRootPanel<Entity>);
            var methodInfo = rootPanelType.GetMethod("set_Visible");

            Assert.AreEqual(methodInfo, setVisibleCall.GetMethodInfo());
            Assert.AreEqual(false, (bool)setVisibleCall.GetArguments().Single());

            gameInputService
                .Received()
                .RevertInput();
        }

        public class TestView : BaseView<TestViewModel, TestData>
        {
            public bool IsInitializeInternalCalled { get; private set; }

            public TestView(TestViewModel viewModel) : base(viewModel)
            {
            }

            protected override void InitializeInternal()
            {
                IsInitializeInternalCalled = true;
            }

            public string GetLabelForPropertyWithDisplay()
            {
                return LabelFor(() => Data.PropertyWithDisplay);
            }

            public string GetLabelForPropertyWithoutDisplay()
            {
                return LabelFor(() => Data.PropertyWithoutDisplay);
            }
        }

        public class TestData
        {
            [Display(Name = "Test Header")]
            public string PropertyWithDisplay { get; set; }

            public string PropertyWithoutDisplay { get; set; }
        }

        public class TestViewModel : BaseViewModel<TestData>
        {
        }
    }
}
