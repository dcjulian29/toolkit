using ToolKit;
using Xunit;

namespace UnitTests
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1600:ElementsMustBeDocumented",
        Justification = "Test Suites do not need XML Documentation.")]
    public class SuspendableObjectTests
    {
        #region -- Test SetUp and TearDown --
        #endregion
        #region -- Test Cases --
        [Fact]
        public void Suspended_Should_ReturnFalse_When_ResumeIsCalled()
        {
            // Arrange
            var o = new Suspending();
            o.Suspend();

            // Act
            o.Resume();

            // Assert
            Assert.False(o.Suspended);
        }

        [Fact]
        public void Suspended_Should_ReturnTrue_When_SuspendIsCalled()
        {
            // Arrange
            var o = new Suspending();

            // Act
            o.Suspend();

            // Assert
            Assert.True(o.Suspended);
        }
        #endregion
        #region -- Supporting Test Classes --
        internal class Suspending : SuspendableObject
        {
            private bool _suspended = false;

            public override bool IsSuspended()
            {
                return _suspended;
            }

            public override void Resume()
            {
                _suspended = false;
            }

            public override void Suspend()
            {
                _suspended = true;
            }
        }
        #endregion
    }
}
