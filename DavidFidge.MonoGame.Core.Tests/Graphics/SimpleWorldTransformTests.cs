﻿using System;

using DavidFidge.MonoGame.Core.Graphics;
using DavidFidge.TestInfrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;

namespace DavidFidge.MonoGame.Core.Tests.Graphics
{
    [TestClass]
    public class SimpleWorldTransformTests : BaseTest
    {
        private SimpleWorldTransform _simpleWorldTransform;

        [TestInitialize]
        public override void Setup()
        {
            base.Setup();

            _simpleWorldTransform = new SimpleWorldTransform();
        }

        [TestMethod]
        public void World_Should_Return_Identity_Matrix_From_Constructor()
        {
            // Act
            var result = _simpleWorldTransform.World;

            // Assert
            AssertMatrixAreEquivalent(Matrix.Identity, result);
        }

        [TestMethod]
        public void ChangeTranslation_Should_Return_Translated_Matrix()
        {
            // Arrange
            var translation = new Vector3(2f, 3f, 4f);

            // Act
            _simpleWorldTransform.ChangeTranslation(translation);

            // Assert
            AssertMatrixAreEquivalent(Matrix.CreateTranslation(translation), _simpleWorldTransform.World);
        }

        [TestMethod]
        public void ChangeTranslationRelative_Should_Return_Matrix_Translated_From_Last_Translation()
        {
            // Arrange
            var translation = new Vector3(2f, 3f, 4f);
            _simpleWorldTransform.ChangeTranslation(translation);

            var relativeTranslation = new Vector3(1f, 2f, 3f);

            // Act
            _simpleWorldTransform.ChangeTranslationRelative(relativeTranslation);

            // Assert
            AssertMatrixAreEquivalent(
                Matrix.CreateTranslation(translation + relativeTranslation),
                _simpleWorldTransform.World
                );
        }

        [TestMethod]
        public void ChangeTranslationRelative_Should_Return_Matrix_Translated_From_Last_Translation_Before_Rotation_And_Scale()
        {
            // Arrange
            var translation = new Vector3(2f, 3f, 4f);
            var scale = new Vector3(5f, 6f, 7f);
            var rotation = new Vector3(0.1f, 0.2f, 0.3f);
            _simpleWorldTransform.ChangeTranslation(translation);
            _simpleWorldTransform.ChangeRotation(rotation.X, rotation.Y, rotation.Z);
            _simpleWorldTransform.ChangeScale(scale);

            var relativeTranslation = new Vector3(1f, 2f, 3f);

            // Act
            _simpleWorldTransform.ChangeTranslationRelative(relativeTranslation);

            // Assert
            AssertMatrixAreEquivalent(
                Matrix.CreateScale(scale)
                * Matrix.CreateRotationX(rotation.X)
                * Matrix.CreateRotationY(rotation.Y)
                * Matrix.CreateRotationZ(rotation.Z)
                * Matrix.CreateTranslation(translation + relativeTranslation),
                _simpleWorldTransform.World
            );
        }

        [TestMethod]
        public void ChangeScale_Should_Return_Scaled_Matrix()
        {
            // Arrange
            var scale = new Vector3(2f, 3f, 4f);

            // Act
            _simpleWorldTransform.ChangeScale(scale);

            // Assert
            AssertMatrixAreEquivalent(Matrix.CreateScale(scale), _simpleWorldTransform.World);
        }

        [TestMethod]
        public void ChangeScale_Should_Return_Matrix_Scaled_Relative_To_Current_Scale()
        {
            // Arrange
            var scale = new Vector3(2f, 3f, 4f);
            _simpleWorldTransform.ChangeScale(scale);

            var relativeScale = new Vector3(1f, 2f, 3f);

            // Act
            _simpleWorldTransform.ChangeScaleRelative(relativeScale);

            // Assert
            AssertMatrixAreEquivalent(
                Matrix.CreateScale(scale + relativeScale),
                _simpleWorldTransform.World
            );
        }

        [TestMethod]
        public void ChangeScaleRelative_Should_Return_Matrix_Scaled_From_Last_Scale_Before_Rotation_And_Translation()
        {
            // Arrange
            var translation = new Vector3(2f, 3f, 4f);
            var scale = new Vector3(5f, 6f, 7f);
            var rotation = new Vector3(0.1f, 0.2f, 0.3f);
            _simpleWorldTransform.ChangeTranslation(translation);
            _simpleWorldTransform.ChangeRotation(rotation.X, rotation.Y, rotation.Z);
            _simpleWorldTransform.ChangeScale(scale);

            var relativeScale = new Vector3(1f, 2f, 3f);

            // Act
            _simpleWorldTransform.ChangeScaleRelative(relativeScale);

            // Assert
            AssertMatrixAreEquivalent(
                Matrix.CreateScale(scale + relativeScale)
                * Matrix.CreateRotationX(rotation.X)
                * Matrix.CreateRotationY(rotation.Y)
                * Matrix.CreateRotationZ(rotation.Z)
                * Matrix.CreateTranslation(translation),
                _simpleWorldTransform.World
            );
        }

        [TestMethod]
        public void ChangeRotation_Should_Return_Rotated_Matrix()
        {
            // Arrange
            var rotation = new Vector3(0.1f, 0.2f, 0.3f);

            // Act
            _simpleWorldTransform.ChangeRotation(rotation.X, rotation.Y, rotation.Z);

            // Assert
            AssertMatrixAreEquivalent(
                Matrix.CreateRotationX(rotation.X)
                * Matrix.CreateRotationY(rotation.Y)
                * Matrix.CreateRotationZ(rotation.Z),
                _simpleWorldTransform.World);
        }

        [TestMethod]
        public void ChangeRotation_Should_Return_Matrix_Rotated_Relative_To_Current_Rotation_Via_Mulitplying_With_Existing()
        {
            // Arrange
            var rotation = new Vector3(0.1f, 0.2f, 0.3f);
            _simpleWorldTransform.ChangeRotation(rotation.X, rotation.Y, rotation.Z);

            var relativeRotation = new Vector3(0.4f, 0.5f, 0.6f);

            // Act
            _simpleWorldTransform.ChangeRotationRelative(
                relativeRotation.X,
                relativeRotation.Y,
                relativeRotation.Z);

            // Assert
            var firstRotation = Matrix.CreateRotationX(rotation.X)
                * Matrix.CreateRotationY(rotation.Y)
                * Matrix.CreateRotationZ(rotation.Z);

            var secondRotation = Matrix.CreateRotationX(relativeRotation.X)
                * Matrix.CreateRotationY(relativeRotation.Y)
                * Matrix.CreateRotationZ(relativeRotation.Z);

            AssertMatrixAreEquivalent(
                secondRotation * firstRotation,
                _simpleWorldTransform.World);
        }

        [TestMethod]
        public void ChangeRotationRelative_Should_Return_Matrix_Rotated_From_Last_Rotation_Before_Scale_And_Translation()
        {
            // Arrange
            var translation = new Vector3(2f, 3f, 4f);
            var scale = new Vector3(5f, 6f, 7f);
            var rotation = new Vector3(0.1f, 0.2f, 0.3f);
            _simpleWorldTransform.ChangeTranslation(translation);
            _simpleWorldTransform.ChangeRotation(rotation.X, rotation.Y, rotation.Z);
            _simpleWorldTransform.ChangeScale(scale);

            var relativeRotation = new Vector3(0.4f, 0.5f, 0.6f);

            // Act
            _simpleWorldTransform.ChangeRotationRelative(
                relativeRotation.X,
                relativeRotation.Y,
                relativeRotation.Z);

            // Assert
            var firstRotation = Matrix.CreateRotationX(rotation.X)
                * Matrix.CreateRotationY(rotation.Y)
                * Matrix.CreateRotationZ(rotation.Z);

            var secondRotation = Matrix.CreateRotationX(relativeRotation.X)
                * Matrix.CreateRotationY(relativeRotation.Y)
                * Matrix.CreateRotationZ(relativeRotation.Z);

            AssertMatrixAreEquivalent(
                Matrix.CreateScale(scale)
                * secondRotation
                * firstRotation
                * Matrix.CreateTranslation(translation),
                _simpleWorldTransform.World
            );
        }

        [TestMethod]
        public void ChangeRotationRelative_RotatingXYZ_Then_MinusZYX_Should_Go_Back_To_Identity()
        {
            // Act
            _simpleWorldTransform.ChangeRotationRelative((float)Math.PI / 2f, 0f, 0f);
            _simpleWorldTransform.ChangeRotationRelative(0f, (float)Math.PI / 2f, 0f);
            _simpleWorldTransform.ChangeRotationRelative(0f, 0f, (float)Math.PI / 2f);
            _simpleWorldTransform.ChangeRotationRelative(0f, 0f, -(float)Math.PI / 2f);
            _simpleWorldTransform.ChangeRotationRelative(0f, -(float)Math.PI / 2f, 0f);
            _simpleWorldTransform.ChangeRotationRelative(-(float)Math.PI / 2f, 0f, 0f);

            // Assert
            AssertMatrixAreEquivalent(
                Matrix.Identity,
                _simpleWorldTransform.World);
        }

        public void AssertMatrixAreEquivalent(Matrix expected, Matrix actual)
        {
            var acceptableFloatDelta = 0.0001f;

            Assert.IsTrue(Math.Abs(expected.M11 - actual.M11) < acceptableFloatDelta);
            Assert.IsTrue(Math.Abs(expected.M12 - actual.M12) < acceptableFloatDelta);
            Assert.IsTrue(Math.Abs(expected.M13 - actual.M13) < acceptableFloatDelta);
            Assert.IsTrue(Math.Abs(expected.M14 - actual.M14) < acceptableFloatDelta);

            Assert.IsTrue(Math.Abs(expected.M21 - actual.M21) < acceptableFloatDelta);
            Assert.IsTrue(Math.Abs(expected.M22 - actual.M22) < acceptableFloatDelta);
            Assert.IsTrue(Math.Abs(expected.M23 - actual.M23) < acceptableFloatDelta);
            Assert.IsTrue(Math.Abs(expected.M24 - actual.M24) < acceptableFloatDelta);

            Assert.IsTrue(Math.Abs(expected.M31 - actual.M31) < acceptableFloatDelta);
            Assert.IsTrue(Math.Abs(expected.M32 - actual.M32) < acceptableFloatDelta);
            Assert.IsTrue(Math.Abs(expected.M33 - actual.M33) < acceptableFloatDelta);
            Assert.IsTrue(Math.Abs(expected.M34 - actual.M34) < acceptableFloatDelta);

            Assert.IsTrue(Math.Abs(expected.M41 - actual.M41) < acceptableFloatDelta);
            Assert.IsTrue(Math.Abs(expected.M42 - actual.M42) < acceptableFloatDelta);
            Assert.IsTrue(Math.Abs(expected.M43 - actual.M43) < acceptableFloatDelta);
            Assert.IsTrue(Math.Abs(expected.M44 - actual.M44) < acceptableFloatDelta);
        }
    }
}