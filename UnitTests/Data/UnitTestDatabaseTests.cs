using System;
using System.Diagnostics.CodeAnalysis;
using ToolKit.Data.NHibernate;
using ToolKit.Data.NHibernate.UnitTests;
using Xunit;

namespace UnitTests.Data
{
    /// <summary>
    /// Primary Purpose for these test are to catch what NHibernateDatabaseBase doesn't.
    /// </summary>
    [SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1600:ElementsMustBeDocumented",
        Justification = "Test Suites do not need XML Documentation.")]
    public class UnitTestDatabaseTests
    {
        private string _sessionName;

        [Fact]
        public void InitializeDatabase_Should_DeleteDBFileIfExistsDuringIntialRun()
        {
            // Arrange
            var db = new UnitTestDatabase(InitializeDatabase, ref _sessionName);

            // Act
            UnitTestDatabase.ResetInstanceCache();

            db.ResetDatabaseCreated();
            _ = new UnitTestDatabase(InitializeDatabase, ref _sessionName);

            var session = NHibernateDatabaseBase.Instance.SessionFactory(_sessionName).OpenSession();

            // Assert
            Assert.NotNull(session);
        }

        [Fact]
        public void Instance_Should_RecreateSession_When_SessionIsRemovedOrExpiredFromCache()
        {
            // Arrange
            _ = new UnitTestDatabase(InitializeDatabase, ref _sessionName);

            // Act
            UnitTestDatabase.ResetInstanceCache();
            var session = NHibernateDatabaseBase.Instance.SessionFactory("UnitTestDatabase").OpenSession();

            // Assert
            Assert.NotNull(session);
        }

        private void InitializeDatabase()
            => NHibernateDatabaseBase.Instance.SessionFactory(_sessionName).OpenSession();
    }
}
