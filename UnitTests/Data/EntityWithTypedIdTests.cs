using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolKit.Data;
using Xunit;

namespace UnitTests.Data
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1600:ElementsMustBeDocumented",
        Justification = "Test Suites do not need XML Documentation.")]
    public class EntityWithTypedIdTests
    {
        [Fact]
        public void CompareTo_Should_ReturnEqual_When_OtherEntityIdIsSameAsThisEntity()
        {
            // Arrange
            var entity1 = new Patient();
            var entity2 = new Patient();

            // Act
            entity1.Id = "patient/-1";
            entity2.Id = "patient/-1";

            // Assert
            Assert.True(entity1.CompareTo(entity2) == 0);
        }

        [Fact]
        public void CompareTo_Should_ReturnEqual_When_OtherEntityTransientAndThisEntityIsTransient()
        {
            // Arrange
            var entity1 = new Patient();
            var entity2 = new Patient();

            // Act

            // Assert
            Assert.True(entity1.CompareTo(entity2) == 0);
        }

        [Fact]
        public void CompareTo_Should_ReturnGreaterThan_When_OtherEntityIsNull()
        {
            // Arrange
            var entity1 = new Patient();
            Patient entity2 = null;

            // Act

            // Assert
            Assert.True(entity1.CompareTo(entity2) == 1);
        }

        [Fact]
        public void CompareTo_Should_ReturnGreaterThan_When_OtherEntityTransientAndThisEntityIsNot()
        {
            // Arrange
            var entity1 = new Patient();
            var entity2 = new Patient();

            // Act
            entity1.Id = "patient/-1";

            // Assert
            Assert.True(entity1.CompareTo(entity2) == 1);
        }

        [Fact]
        public void CompareTo_Should_ReturnLessThan_When_ThisEntityIdIsLessThanOtherEntityId()
        {
            // Arrange
            var entity1 = new Patient();
            var entity2 = new Patient();

            // Act
            entity1.Id = "patient/-1";
            entity2.Id = "patient/-2";

            // Assert
            Assert.True(entity1.CompareTo(entity2) == -1);
        }

        [Fact]
        public void CompareTo_Should_ReturnLessThan_When_ThisEntityTransientAndOtherEntityIsNot()
        {
            // Arrange
            var entity1 = new Patient();
            var entity2 = new Patient();

            // Act
            entity2.Id = "patient/-2";

            // Assert
            Assert.True(entity1.CompareTo(entity2) == -1);
        }

        [Fact]
        public void Entity_Should_BeTransientByDefault()
        {
            // Arrange
            var entity = new Patient();

            // Act

            // Assert
            Assert.True(entity.IsTransient());
        }

        [Fact]
        public void Entity_Should_NotBeTransientAfterIdHasValue()
        {
            // Arrange
            var entity = new Patient();

            // Act
            entity.Id = "patient/-1";

            // Assert
            Assert.False(entity.IsTransient());
        }

        [Fact]
        public void EntityIdType_Should_BeNull_When_TypedAsStringAndBeforeIdHasValue()
        {
            // Arrange
            var entity = new Patient();
            
            // Act

            // Assert
            Assert.Null(entity.Id);
        }

        [Fact]
        public void EntityIdType_Should_BeString_When_TypedAsStringAndAfterIdHasValue()
        {
            // Arrange
            var entity = new Patient();

            // Act
            entity.Id = "patient/-1";

            // Assert
            Assert.IsType(typeof(String), entity.Id);
        }

        [Fact]
        public void Equals_Should_BeFalse_When_EntityIdIsDifferent()
        {
            // Arrange
            var entity1 = new Patient();
            var entity2 = new Patient();

            // Act
            entity1.Id = "patient/-1";
            entity2.Id = "patient/-2";

            // Assert
            Assert.NotEqual(entity1, entity2);
        }

        [Fact]
        public void Equals_Should_BeTrue_When_EntityIdIsSame()
        {
            // Arrange
            var entity1 = new Patient();
            var entity2 = new Patient();

            // Act
            entity1.Id = "patient/-1";
            entity2.Id = "patient/-1";

            // Assert
            Assert.Equal(entity1, entity2);
        }

        [Fact]
        public void GetHashCode_Should_GenerateSameHashCode_When_EntitiesAreTransient()
        {
            // Arrange
            var entity1 = new Patient();
            var entity2 = new Patient();

            // Act

            // Assert
            Assert.Equal(entity1.GetHashCode(), entity2.GetHashCode());
        }

        [Fact]
        public void GetHashCode_Should_GenerateDifferentHashCodes_When_EntitiesIdIsDifferent()
        {
            // Arrange
            var entity1 = new Patient();
            var entity2 = new Patient();

            // Act
            entity1.Id = "patient/-1";
            entity2.Id = "patient/-2";

            // Assert
            Assert.NotEqual(entity1.GetHashCode(), entity2.GetHashCode());
        }

        [Fact]
        public void GetHashCode_Should_GenerateSameHashCode_When_EntitiesIdIsTheSame()
        {
            // Arrange
            var entity1 = new Patient();
            var entity2 = new Patient();

            // Act
            entity1.Id = "patient/-1";
            entity2.Id = "patient/-1";

            // Assert
            Assert.Equal(entity1.GetHashCode(), entity2.GetHashCode());
        }

        [Fact]
        public void GetHashCode_Should_GenerateSameHashCode_When_EntityIsCopied()
        {
            // Arrange
            var entity1 = new Patient();
            var entity2 = entity1;

            // Act
            var hash1 = entity1.GetHashCode();
            var hash2 = entity2.GetHashCode();

            // Assert
            Assert.Equal(hash1, hash2);
        }


        [Fact]
        public void GetHashCode_Should_ReturnHashCodeBasedOnId_When_NotTransient()
        {
            // Arrange
            var entity = new Patient();

            // Act
            entity.Id = "patient/-1";
            var hash1 = entity.GetHashCode();
            var hash2 = entity.Id.GetHashCode();

            // Assert
            Assert.Equal(hash1, hash2);
        }

        internal class Patient : EntityWithTypedId<string>
        {
            public string Name { get; set; }
        }
    }
}
