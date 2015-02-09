using System.Diagnostics.CodeAnalysis;
using ToolKit.Data;
using Xunit;

namespace UnitTests.Data
{
    [SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1600:ElementsMustBeDocumented",
        Justification = "Test Suites do not need XML Documentation.")]
    public class RepositoryTests
    {
        [Fact]
        public void RepositoryBase_Should_Instantiate()
        {
            // Arrange
            var repository = new TestRepository();
            var entity = new TestEntity();

            // Act
            entity.Id = 1;
            entity.Name = "Test";
            repository.Save(entity);

            var entityFromDatabase = repository.FindById(1);

            // Assert
            Assert.Equal(entity.Name, entityFromDatabase.Name);
        }

        private class TestEntity : Entity
        {
            public string Name { get; set; }
        }

        private class TestRepository : Repository<TestEntity, int>
        {
            public TestRepository()
            {
                this.Context = new MemoryUnitOfWork();
            }
        }
    }
}
