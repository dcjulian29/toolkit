using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ToolKit.Data;
using Xunit;

namespace UnitTests.Data
{
    public class MyOtherValueObject : ValueObject
    {
        public MyOtherValueObject(int value1, string value2)
        {
            MyValue1 = value1;
            MyValue2 = value2;
        }

        public int MyValue1 { get; set; }

        public string MyValue2 { get; set; }

        protected override IEnumerable<object> GetPropertyValues()
        {
            yield return MyValue1;
            yield return MyValue2;
        }
    }

    [SuppressMessage(
            "StyleCop.CSharp.DocumentationRules",
        "SA1600:ElementsMustBeDocumented",
        Justification = "Test Suites do not need XML Documentation.")]
    public class ValueObjectTests
    {
        [Fact]
        public void EqualOperator_Should_ReturnFalse_When_LeftIsNull()
        {
            // Arrange
            var value1 = new MyValueObject(10, "Unit Tests");

            // Act
            var actual = MyValueObject.TestEqualOperator(null, value1);

            // Assert
            Assert.False(actual);
        }

        [Fact]
        public void EqualOperator_Should_ReturnFalse_When_RightIsNull()
        {
            // Arrange
            var value1 = new MyValueObject(10, "Unit Tests");

            // Act
            var actual = MyValueObject.TestEqualOperator(value1, null);

            // Assert
            Assert.False(actual);
        }

        [Fact]
        public void EqualOperator_Should_ReturnTrue_When_BothAreNull()
        {
            // Arrange & Act
            var actual = MyValueObject.TestEqualOperator(null, null);

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void EqualOperator_Should_ReturnTrueWithSameValues()
        {
            // Arrange
            var value1 = new MyValueObject(10, "Unit Tests");
            var value2 = new MyValueObject(10, "Unit Tests");

            // Act
            var actual = MyValueObject.TestEqualOperator(value1, value2);

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void Equals_Should_ReturnFalse_When_TwoDifferentTypesButSameProperties()
        {
            // Arrange
            var value1 = new MyValueObject(10, "Unit Tests");
            var value2 = new MyOtherValueObject(10, "Unit Tests");

            // Act
            var actual = value1.Equals(value2);

            // Assert
            Assert.False(actual);
        }

        [Fact]
        public void Equals_Should_ReturnFalseWithDifferentValues()
        {
            // Arrange
            var value1 = new MyValueObject(10, "Unit Tests");
            var value2 = new MyValueObject(10, "Unit Tests For Life1");

            // Act
            var actual = value1.Equals(value2);

            // Assert
            Assert.False(actual);
        }

        [Fact]
        public void Equals_Should_ReturnFalseWithNull()
        {
            // Arrange
            var value1 = new MyValueObject(10, "Unit Tests");

            // Act
            var actual = value1.Equals(null);

            // Assert
            Assert.False(actual);
        }

        [Fact]
        public void Equals_Should_ReturnTrue_When_BothHaveSameValuesButPassedasObject()
        {
            // Arrange
            var value1 = new MyValueObject(10, "Unit Tests");
            var value2 = new MyValueObject(10, "Unit Tests");

            // Act
            var actual = value1.Equals((object)value2);

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void Equals_Should_ReturnTrue_When_OtherObjectIsSame()
        {
            // Arrange
            var value1 = new MyValueObject(10, "Unit Tests");

            // Act
            var actual = value1.Equals(value1);

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void Equals_Should_ReturnTrueWithSameValues()
        {
            // Arrange
            var value1 = new MyValueObject(10, "Unit Tests");
            var value2 = new MyValueObject(10, "Unit Tests");

            // Act
            var actual = value1.Equals(value2);

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void GetCopy_Should_StillBeEqual()
        {
            // Arrange & Act
            var value1 = new MyValueObject(10, "Unit Tests");
            var value2 = value1.GetCopy();

            // Assert
            Assert.Equal(value1, value2);
        }

        [Fact]
        public void GetHashCode_Should_ReturnSameValues()
        {
            // Arrange
            var value1 = new MyValueObject(10, "Unit Tests");
            var value2 = new MyValueObject(10, "Unit Tests");

            // Act & Assert
            Assert.Equal(value1.GetHashCode(), value2.GetHashCode());
        }

        [Fact]
        public void GetHashCode_Should_ReturnValue_WhenItContainsNullProperties()
        {
            // Arrange
            const int expected = 512;
            var value1 = new MyValueObject(512, null);

            // Act
            var actual = value1.GetHashCode();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void NotEqualExplicitOperator_Should_ReturnFalseWithSameValues()
        {
            // Arrange
            var value1 = new MyValueObject(10, "Unit Tests");
            var value2 = new MyValueObject(10, "Unit Tests");

            // Act
            var actual = value1 != value2;

            // Assert
            Assert.False(actual);
        }

        [Fact]
        public void NotEqualOperator_Should_ReturnFalseWithSameValues()
        {
            // Arrange
            var value1 = new MyValueObject(10, "Unit Tests");
            var value2 = new MyValueObject(10, "Unit Tests");

            // Act
            var actual = MyValueObject.TestNotEqualOperator(value1, value2);

            // Assert
            Assert.False(actual);
        }

        public class MyValueObject : ValueObject
        {
            public MyValueObject(int value1, string value2)
            {
                MyValue1 = value1;
                MyValue2 = value2;
            }

            public int MyValue1 { get; set; }

            public string MyValue2 { get; set; }

            public static bool TestEqualOperator(ValueObject left, ValueObject right)
                => ValueObject.EqualOperator(left, right);

            public static bool TestNotEqualOperator(ValueObject left, ValueObject right)
                => ValueObject.NotEqualOperator(left, right);

            protected override IEnumerable<object> GetPropertyValues()
            {
                yield return MyValue1;
                yield return MyValue2;
            }
        }
    }
}
