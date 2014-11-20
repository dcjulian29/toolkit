using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        #region -- Test SetUp and TearDown --
        #endregion
        #region -- Test Cases --
        [Fact]
        public void DrivedClassDispose_Should_BeCalledDuringDispose()
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
        #endregion
        #region -- Supporting Test Classes --
        internal class ScarceResource : DisposableObject
        {
            public bool DisposeCalled { get; set; }

            public void Open()
            {
                DisposeCalled = false;
            }

            protected override void DisposeResources(bool disposing)
            {
                DisposeCalled = true;
            }
        }
        #endregion
    }
}
