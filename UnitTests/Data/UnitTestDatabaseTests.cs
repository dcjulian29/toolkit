using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ToolKit.Data.NHibernate;
using ToolKit.Data.NHibernate.UnitTests;
using Xunit;

namespace UnitTests.Data
{
    /// <summary>
    ///     Primary Purpose for these test are to catch what NHibernateDatabaseBase doesn't.
    /// </summary>
    [SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1600:ElementsMustBeDocumented",
        Justification = "Test Suites do not need XML Documentation.")]
    public class UnitTestDatabaseTests
    {
        [Fact]
        public void InitializeDatabase_Should_DeleteDBFileIfExistsDuringIntialRun()
        {
            // Arrange
            _ = new UnitTestDatabase(Assembly.GetExecutingAssembly(), InitializeDatabase);

            // Act
            UnitTestDatabase.ResetInstanceCache();
            UnitTestDatabase.ResetDatabaseCreated();
            _ = new UnitTestDatabase(Assembly.GetExecutingAssembly(), InitializeDatabase);

            var session = NHibernateDatabaseBase.Instance.SessionFactory("UnitTestDatabase").OpenSession();

            // Assert
            Assert.NotNull(session);
        }

        [Fact]
        public void Instance_Should_RecreateSession_When_SessionIsRemovedOrExpiredFromCache()
        {
            // Arrange
            _ = new UnitTestDatabase(Assembly.GetExecutingAssembly(), InitializeDatabase);

            // Act
            UnitTestDatabase.ResetInstanceCache();
            var session = NHibernateDatabaseBase.Instance.SessionFactory("UnitTestDatabase").OpenSession();

            // Assert
            Assert.NotNull(session);
        }

        private void InitializeDatabase()
        {
            _ = NHibernateDatabaseBase.Instance.SessionFactory("UnitTestDatabase").OpenSession();
        }
    }
}
