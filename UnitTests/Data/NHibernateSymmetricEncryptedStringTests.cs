using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Mapping;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using ToolKit.Cryptography;
using ToolKit.Data;
using ToolKit.Data.NHibernate;
using ToolKit.Data.NHibernate.UserTypes;
using Xunit;

namespace UnitTests.Data
{
    [SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1600:ElementsMustBeDocumented",
        Justification = "Test Suites do not need XML Documentation.")]
    public class NHibernateSymmetricEncryptedStringTests
    {
        private static EncryptionData _initializationVector = new EncryptionData("vector");
        private static EncryptionData _key = new EncryptionData("privatekey");
        private static ISessionFactory _sessionFactory;

        public NHibernateSymmetricEncryptedStringTests()
        {
            if (_sessionFactory == null)
            {
                File.Delete("NHibernateSymmetricEncryptedStringTests.db");
                _sessionFactory = Fluently.Configure()
                    .Database(SQLiteConfiguration.Standard.UsingFile("NHibernateSymmetricEncryptedStringTests.db"))
                    .Mappings(m => m.FluentMappings.AddFromAssemblyOf<NHibernateSymmetricEncryptedStringTests>())
                    .ExposeConfiguration(cfg => new SchemaExport(cfg).Create(false, true))
                    .BuildSessionFactory();
            }
        }

        ~NHibernateSymmetricEncryptedStringTests()
        {
            // SQLite doesn't always unlock DB file when test ends, Let's force it.
            GC.Collect();
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("FXntXw0KPMbaIgK3", null)]
        [InlineData(null, "ss5odkTcjq7yIXa1")]
        [InlineData("fwQzLd9JAI3Lb38N", "1Pob778UNTneIN9F")]
        [InlineData("JksRMbAi2toooP15", "HhtLCHYEM28c2uwg")]
        public void UserType_Should_GenerateDifferentEncryptedValues(string vector, string key)
        {
            // Arrange
            var order = new Order()
            {
                Name = "Smith The " + DateTime.Now.Millisecond,
                CreditCardNumber = "5602212409248699",
                ExpirationDate = DateTime.Today.AddDays(-1)
            };

            // Act
            var originalVector = SymmetricEncryptedString.InitializationVector;
            var originalKey = SymmetricEncryptedString.EncryptionKey;

            if (key != null)
            {
                SymmetricEncryptedString.InitializationVector = new EncryptionData(key);
            }

            if (vector != null)
            {
                SymmetricEncryptedString.EncryptionKey = new EncryptionData(vector);
            }

            using (var repository = new OrderRepository(_sessionFactory.OpenSession()))
            {
                repository.Save(order);
            }

            SymmetricEncryptedString.InitializationVector = originalVector;
            SymmetricEncryptedString.EncryptionKey = originalKey;

            var criteria = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd 00:00:00");
            var sql = $"SELECT CreditCardNumber FROM [Order] WHERE ExpirationDate = '{criteria}'";
            var query = _sessionFactory.OpenSession().CreateSQLQuery(sql).List();

            if (query.Count != 5)
            {
                return;
            }

            // Assert
            var previous = new object();
            foreach (var actual in query)
            {
                Assert.NotEqual(previous, actual);

                previous = actual;
            }
        }

        [Fact]
        public void UserType_Should_ReturnUnencryptedString_When_ExplicitInitializationVectorUsed()
        {
            // Arrange
            var expected = "5602212409248699";

            var order = new Order()
            {
                Name = "John V Smith",
                CreditCardNumber = expected,
                ExpirationDate = DateTime.Now
            };

            Order entityFromDatabase;

            // Act
            var original = SymmetricEncryptedString.InitializationVector;
            SymmetricEncryptedString.InitializationVector = _initializationVector;

            using (var repository = new OrderRepository(_sessionFactory.OpenSession()))
            {
                repository.Save(order);
            }

            using (var repository = new OrderRepository(_sessionFactory.OpenSession()))
            {
                entityFromDatabase = repository.FindBy(p => p.Name == "John V Smith").First();
            }

            SymmetricEncryptedString.InitializationVector = original;

            // Assert
            Assert.Equal(expected, entityFromDatabase.CreditCardNumber);
        }

        [Fact]
        public void UserType_Should_ReturnUnencryptedString_When_ExplicitKeyUsed()
        {
            // Arrange
            var expected = "5602212409248699";

            var order = new Order()
            {
                Name = "John K Smith",
                CreditCardNumber = expected,
                ExpirationDate = DateTime.Now
            };

            Order entityFromDatabase;

            // Act
            var original = SymmetricEncryptedString.EncryptionKey;
            SymmetricEncryptedString.EncryptionKey = _key;

            using (var repository = new OrderRepository(_sessionFactory.OpenSession()))
            {
                repository.Save(order);
            }

            using (var repository = new OrderRepository(_sessionFactory.OpenSession()))
            {
                entityFromDatabase = repository.FindBy(p => p.Name == "John K Smith").First();
            }

            SymmetricEncryptedString.EncryptionKey = original;

            // Assert
            Assert.Equal(expected, entityFromDatabase.CreditCardNumber);
        }

        [Fact]
        public void UserType_Should_ReturnUnencryptedString_When_ExplicitParametersUsed()
        {
            // Arrange
            var expected = "5602212409248699";

            var order = new Order()
            {
                Name = "John B Smith",
                CreditCardNumber = expected,
                ExpirationDate = DateTime.Now
            };

            Order entityFromDatabase;

            // Act
            var originalVector = SymmetricEncryptedString.InitializationVector;
            var originalKey = SymmetricEncryptedString.EncryptionKey;
            SymmetricEncryptedString.InitializationVector = _initializationVector;
            SymmetricEncryptedString.EncryptionKey = _key;

            using (var repository = new OrderRepository(_sessionFactory.OpenSession()))
            {
                repository.Save(order);
            }

            using (var repository = new OrderRepository(_sessionFactory.OpenSession()))
            {
                entityFromDatabase = repository.FindBy(p => p.Name == "John B Smith").First();
            }

            SymmetricEncryptedString.InitializationVector = originalVector;
            SymmetricEncryptedString.EncryptionKey = originalKey;

            // Assert
            Assert.Equal(expected, entityFromDatabase.CreditCardNumber);
        }

        [Fact]
        public void UserType_Should_ReturnUnencryptedString_When_ReturnedFromDatabase()
        {
            // Arrange
            var expected = "5602212409248699";

            var order = new Order()
            {
                Name = "John Smith",
                CreditCardNumber = expected,
                ExpirationDate = DateTime.Now
            };

            Order entityFromDatabase;

            // Act
            using (var repository = new OrderRepository(_sessionFactory.OpenSession()))
            {
                repository.Save(order);
            }

            using (var repository = new OrderRepository(_sessionFactory.OpenSession()))
            {
                entityFromDatabase = repository.FindBy(p => p.Name == "John Smith").First();
            }

            // Assert
            Assert.Equal(expected, entityFromDatabase.CreditCardNumber);
        }

        public class Order : Entity
        {
            public virtual string CreditCardNumber { get; set; }

            public virtual DateTime ExpirationDate { get; set; }

            public virtual string Name { get; set; }
        }

        public class OrderMap : ClassMap<Order>
        {
            public OrderMap()
            {
                Id(x => x.Id);

                Map(x => x.Name)
                  .Length(16)
                  .Not.Nullable();
                Map(x => x.CreditCardNumber)
                    .CustomType<SymmetricEncryptedString>();
                Map(x => x.ExpirationDate);
            }
        }

        public class OrderRepository : Repository<Order, int>
        {
            public OrderRepository(ISession session)
            {
                Context = new NHibernateUnitOfWork(session);
            }
        }
    }
}
