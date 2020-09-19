using ToolKit;
using Xunit;

namespace UnitTests
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
         "StyleCop.CSharp.DocumentationRules",
         "SA1600:ElementsMustBeDocumented",
         Justification = "Test Suites do not need XML Documentation.")]
    public class DisposableObjectTests
    {
        [Fact]
        public void DisposeResources_Should_BeCalledDuringDispose()
        {
            // Arrange
            var o = new ScarceResource();

            // Act
            using (o)
            {
                o.Open();
            }

            // Assert
            Assert.True(o.DisposeCalled);
        }

        [Fact]
        public void DisposeResources_Should_NotDispose_When_DisposeIsFalse()
        {
            // Arrange
            var o = new ScarceResource();

            // Act
            o.Open();
            o.DisposeUnitTest(false);

            // Assert
            Assert.False(o.DisposeCalled);
        }

        internal class ScarceResource : DisposableObject
        {
            public bool DisposeCalled { get; set; }

            public void Open()
            {
                DisposeCalled = false;
            }

            public void DisposeUnitTest(bool disposing)
            {
                base.Dispose(disposing);
            }

            protected override void DisposeResources(bool disposing)
            {
                if (!disposing)
                {
                    return;
                }

                DisposeCalled = true;
            }
        }
    }
}
