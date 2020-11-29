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
        public void InitializeDatabase_Should_NotErrorIfCalledSecondTime()
        {
            // Arrange
            var db = new MockUnitTestDatabase(InitializeDatabase, ref _sessionName);

            // Act & Assert
            db.InitializeDatabase(InitializeDatabase);

            // Assert
            Assert.NotNull(db);
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

        [Fact]
        public void RemoveInstance_Should_NotRemoveSessionThatDoesNotExist()
        {
            var obj = new Object();

            lock (obj)
            {
                // Arrange
                var db = new MockUnitTestDatabase(InitializeDatabase, ref _sessionName);
                var sessions = db.DefinedSessions;

                // Act
                UnitTestDatabase.RemoveInstance("NONEXIST");

                // Assert
                Assert.Equal(sessions, db.DefinedSessions);
            }
        }

        [Fact]
        public void RemoveInstance_Should_RemoveOnlySessionSpecified()
        {
            // Arrange
            var db1 = new MockUnitTestDatabase(InitializeDatabase, ref _sessionName);
            var session = _sessionName;
            var db2 = new MockUnitTestDatabase(InitializeDatabase, ref _sessionName);

            // Act
            UnitTestDatabase.RemoveInstance(session);

            // Assert
            Assert.Equal(1, db1.DefinedSessions);
            Assert.Equal(1, db2.DefinedSessions);
        }

        [Fact]
        public void ResetInstanceCache_Should_RemoveAllSession()
        {
            // Arrange
            var db = new MockUnitTestDatabase(InitializeDatabase, ref _sessionName);

            // Act
            UnitTestDatabase.ResetInstanceCache();

            // Assert
            Assert.Equal(0, db.DefinedSessions);
        }

        [Fact]
        public void ResetInstanceCache_Should_RemoveAllSessionEvenWhenNonExists()
        {
            // Arrange
            var db = new MockUnitTestDatabase(InitializeDatabase, ref _sessionName);

            // Act
            UnitTestDatabase.ResetInstanceCache();
            UnitTestDatabase.ResetInstanceCache();

            // Assert
            Assert.Equal(0, db.DefinedSessions);
        }

        private void InitializeDatabase()
            => NHibernateDatabaseBase.Instance.SessionFactory(_sessionName).OpenSession();

        public class MockUnitTestDatabase : UnitTestDatabase
        {
            public MockUnitTestDatabase(Action initialization, ref string sessionName)
                : base(initialization, ref sessionName) { }

            public long DefinedSessions => Cache.GetCount();
        }
    }
}
