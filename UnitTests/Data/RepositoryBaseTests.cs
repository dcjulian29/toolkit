using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolKit.Data;
using Xunit;

namespace UnitTests.Data
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Test Suites do not need XML Documentation.")]
    public class RepositoryBaseTests
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

        private class TestRepository : RepositoryBase<TestEntity, int>
        {
            public TestRepository()
            {
                this.Context = new MemoryUnitOfWork();
            }
        }
    }
}
