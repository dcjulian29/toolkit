using System;
using System.Net;
using ToolKit.Cryptography;
using Xunit;

namespace UnitTests.Cryptography
{
    public class PasswordGeneratorTests
    {
        [Fact]
        public void Generate_Should_ReturnExpectedPassword()
        {
            // Arrange
            var expected = @"tnObYKPhj9R!w6V1TZdlrQ*in";
            var generator = new PasswordGenerator(123);

            // Act
            var password = generator.Generate(25);

            // Assert
            Assert.Equal(expected, password);
        }

        [Fact]
        public void Generate_Should_ReturnExpectedPassword_When_ConstrainedByExtendedCharacters()
        {
            // Arrange
            var expected = @"{;:;\\\?";
            var options = PasswordComplexities.UseExtendedCharacters;
            var generator = new PasswordGenerator(123, options);

            // Act
            var password = generator.Generate(8);

            // Assert
            Assert.Equal(expected, password);
        }

        [Fact]
        public void Generate_Should_ReturnExpectedPassword_When_ConstrainedByLowerCaseLetters()
        {
            // Arrange
            var expected = @"ndybgcrv";
            var options = PasswordComplexities.UseLowerCharacters;
            var generator = new PasswordGenerator(123, options);

            // Act
            var password = generator.Generate(8);

            // Assert
            Assert.Equal(expected, password);
        }

        [Fact]
        public void Generate_Should_ReturnExpectedPassword_When_ConstrainedByNumbers()
        {
            // Arrange
            var expected = @"33296417";
            var options = PasswordComplexities.UseNumbers;
            var generator = new PasswordGenerator(123, options);

            // Act
            var password = generator.Generate(8);

            // Assert
            Assert.Equal(expected, password);
        }

        [Fact]
        public void Generate_Should_ReturnExpectedPassword_When_ConstrainedBySymbols()
        {
            // Arrange
            var expected = @"+!=#^*!!";
            var options = PasswordComplexities.UseSymbols;
            var generator = new PasswordGenerator(123, options);

            // Act
            var password = generator.Generate(8);

            // Assert
            Assert.Equal(expected, password);
        }

        [Fact]
        public void Generate_Should_ReturnExpectedPassword_When_ConstrainedByUpperCaseLetters()
        {
            // Arrange
            var expected = @"NDYBGCRV";
            var options = PasswordComplexities.UseUpperCharacters;
            var generator = new PasswordGenerator(123, options);

            // Act
            var password = generator.Generate(8);

            // Assert
            Assert.Equal(expected, password);
        }

        [Fact]
        public void Generate_Should_ReturnExpectedPassword_When_ExclusionsAreProvided()
        {
            // Arrange
            var expected = @"tnObYKhj9R!w6V1TZdlrQ*in+";
            var generator = new PasswordGenerator(123);
            generator.Exclusions = "P";

            // Act
            var password = generator.Generate(25);

            // Assert
            Assert.Equal(expected, password);
        }

        [Fact]
        public void Generate_Should_ReturnExpectedPassword_When_LargeSeedUsed()
        {
            // Arrange
            var expected = @"XIlCiUthquYVnDMGuYuKWnYWJvByCzQneCRViMbEcIygKvAlOL";
            var options = PasswordComplexities.UseUpperCharacters | PasswordComplexities.UseLowerCharacters;

            var generator = new PasswordGenerator(20161122, options);

            // Act
            var password = generator.Generate(50);

            // Assert
            Assert.Equal(expected, password);
        }

        [Fact]
        public void Generate_Should_ReturnPassword()
        {
            // Arrange
            var generator = new PasswordGenerator(123);

            // Act
            var password = generator.Generate(25);

            // Assert
            Assert.NotEqual(String.Empty, password.ToString());
        }

        [Fact]
        public void Generate_Should_ThrowException_When_NotEnoughUniqueCharactersAvailable()
        {
            // Arrange
            var options = PasswordComplexities.UseNumbers | PasswordComplexities.NoRepeatingCharacters;
            var generator = new PasswordGenerator(123, options);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() =>
            {
                var password = generator.Generate(25);
            });
        }

        [Fact]
        public void GenerateSecureString_Should_ResultExpectedPassword()
        {
            // Arrange
            var expected = "tnObYKPhj9R!w6V1TZdlrQ*in";
            var generator = new PasswordGenerator(123);

            // Act
            var password = generator.GenerateSecureString(25);
            var s = new NetworkCredential("", password).Password;

            // Assert
            Assert.Equal(expected, s);
        }

        [Fact]
        public void IncludeAll_Should_ReturnExpectedPassword()
        {
            // Arrange
            var expected = @"C:5tFe\w?j}cMyY.&0IHvONDEl8s,r+4*%TX6Kx>LBQqdk3;uW";
            var generator = new PasswordGenerator(123);
            generator.DisableAll();

            // Act
            generator.IncludeAll();
            var password = generator.Generate(50);

            // Assert
            Assert.Equal(expected, password);
        }

        [Fact]
        public void IncludeExtended_Should_ReturnExpectedPassword()
        {
            // Arrange
            var expected = @"{;:;\\\?";
            var generator = new PasswordGenerator(123);
            generator.DisableAll();

            // Act
            generator.IncludeExtended = true;
            var password = generator.Generate(8);

            // Assert
            Assert.Equal(expected, password);
        }

        [Fact]
        public void IncludeLowerCase_Should_ReturnExpectedPassword()
        {
            // Arrange
            var expected = @"ndybgcrv";
            var generator = new PasswordGenerator(123);
            generator.DisableAll();

            // Act
            generator.IncludeLowerCase = true;
            var password = generator.Generate(8);

            // Assert
            Assert.Equal(expected, password);
        }

        [Fact]
        public void IncludeNumbers_Should_ReturnExpectedPassword()
        {
            // Arrange
            var expected = @"33296417";
            var generator = new PasswordGenerator(123);
            generator.DisableAll();

            // Act
            generator.IncludeNumbers = true;
            var password = generator.Generate(8);

            // Assert
            Assert.Equal(expected, password);
        }

        [Fact]
        public void IncludeSymbols_Should_ReturnExpectedPassword()
        {
            // Arrange
            var expected = @"+!=#^*!!";
            var generator = new PasswordGenerator(123);
            generator.DisableAll();

            // Act
            generator.IncludeSymbols = true;
            var password = generator.Generate(8);

            // Assert
            Assert.Equal(expected, password);
        }

        [Fact]
        public void IncludeUpperCase_Should_ReturnExpectedPassword()
        {
            // Arrange
            var expected = @"NDYBGCRV";
            var generator = new PasswordGenerator(123);
            generator.DisableAll();

            // Act
            generator.IncludeUpperCase = true;
            var password = generator.Generate(8);

            // Assert
            Assert.Equal(expected, password);
        }

        [Fact]
        public void ProhibitConsecutiveCharacters_Should_ReturnExpectedPassword()
        {
            // Arrange
            var expected = @"{;:;\?<?";
            var generator = new PasswordGenerator(123);
            generator.DisableAll();

            // Act
            generator.IncludeExtended = true;
            generator.ProhibitConsecutiveCharacters = true;
            var password = generator.Generate(8);

            // Assert
            Assert.Equal(expected, password);
        }

        [Fact]
        public void ProhibitRepeatingCharacters_Should_ReturnExpectedPassword()
        {
            // Arrange
            var expected = @"{;:\?<}.";
            var generator = new PasswordGenerator(123);
            generator.DisableAll();

            // Act
            generator.IncludeExtended = true;
            generator.ProhibitRepeatingCharacters = true;
            var password = generator.Generate(8);

            // Assert
            Assert.Equal(expected, password);
        }
    }
}
