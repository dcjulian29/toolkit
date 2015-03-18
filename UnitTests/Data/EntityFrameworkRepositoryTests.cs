using System;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Infrastructure;
using System.Data.SQLite;
using System.Data.SQLite.EF6;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using ToolKit.Data;
using ToolKit.Data.EntityFramework;
using Xunit;

namespace UnitTests.Data
{
    [SuppressMessage("StyleCop.CSharp.DocumentationRules",
        "SA1600:ElementsMustBeDocumented",
        Justification = "Test Suites do not need XML Documentation.")]
    public class EntityFrameworkRepositoryTests
    {
        private static bool _databaseCreated = false;

        public EntityFrameworkRepositoryTests()
        {
            var connectionString = new SQLiteConnectionStringBuilder()
            {
                DataSource = "temp.db",
                ForeignKeys = true
            }.ConnectionString;

            // Entity Framework SQLite Provider doesn't create database file or tables so we'll create
            // it manually here for the unit test...
            if (!_databaseCreated)
            {
                File.Delete("temp.db");
                SQLiteConnection.CreateFile("temp.db");

                using (var c = new SQLiteConnection(connectionString))
                {
                    using (var cmd = new SQLiteCommand(c))
                    {
                        c.Open();
                        cmd.CommandText = @"CREATE TABLE `Patients` (
                            `Id`	integer PRIMARY KEY AUTOINCREMENT,
                            `Name`	TEXT NOT NULL,
                            `Sex`	integer,
                            `DateAdded`	DATETIME,
                            `AdmitDate`	DATETIME
                            );";
                        cmd.ExecuteNonQuery();
                        c.Close();
                    }
                }

                _databaseCreated = true;
            }

            // Delete any records that might be leftover from a previous unit test.
            using (var c = new SQLiteConnection(connectionString))
            {
                using (var cmd = new SQLiteCommand(c))
                {
                    c.Open();
                    cmd.CommandText = "DELETE FROM Patients;";
                    cmd.ExecuteNonQuery();
                    c.Close();
                }
            }
        }

        public enum Gender
        {
            /// <summary>
            /// The male gender.
            /// </summary>
            Male,

            /// <summary>
            /// The female gender.
            /// </summary>
            Female
        }

        [Fact]
        public void Delete_Should_RemoveEntitiesFromDatabase()
        {
            // Arrange
            InitializeDatabase();
            var patientCount = 0;

            // Act
            using (var repository = new PatientRepository())
            {
                var patients = repository.FindBy(p => p.DateAdded > p.AdmitDate);
                repository.Delete(patients);
            }

            using (var repository = new PatientRepository())
            {
                patientCount = repository.FindAll().Count();
            }

            // Assert
            Assert.Equal(patientCount, 1);
        }

        [Fact]
        public void Contains_Should_ReturnTrue_When_EntityIsInRepository()
        {
            // Arrange
            InitializeDatabase();
            Patient detachedPatient;
            var patientPresent = false;

            // Act
            using (var repository = new PatientRepository())
            {
                detachedPatient = repository.FindFirst(p => p.DateAdded > p.AdmitDate);
            }

            using (var repository = new PatientRepository())
            {
                patientPresent = repository.Contains(detachedPatient);
            }

            // Assert
            Assert.True(patientPresent);
        }

        [Fact]
        public void FindAll_Should_ReturnAllEntities()
        {
            // Arrange
            InitializeDatabase();
            var repository = new PatientRepository();

            // Act
            var patients = repository.FindAll();
            var patientCount = patients.Count();
            repository.Dispose();

            // Assert
            Assert.Equal(3, patientCount);
        }

        [Fact]
        public void FindBy_Should_ReturnCorrectEntities_When_QueriedByLinq()
        {
            // Arrange
            InitializeDatabase();
            var repository = new PatientRepository();

            // Act
            var patients = repository.FindBy(p => p.DateAdded > p.AdmitDate);
            var patientCount = patients.Count();
            repository.Dispose();

            // Assert
            Assert.Equal(patientCount, 2);
        }

        [Fact]
        public void FindById_Should_ReturnEntityAfterBeingSaved()
        {
            // Arrange
            InitializeDatabase();
            var entity = new Patient();
            Patient entityFromDatabase;

            // Act
            using (var repository = new PatientRepository())
            {
                entity.Name = "Test";
                entity.Sex = Gender.Female;
                entity.DateAdded = new DateTime(2012, 2, 1);
                entity.AdmitDate = new DateTime(2012, 2, 2);
                repository.Save(entity);
            }

            using (var repository = new PatientRepository())
            {
                entityFromDatabase = repository.FindById(entity.Id);
            }

            // Assert
            Assert.Equal(entity.Id, entityFromDatabase.Id);
        }

        [Fact]
        public void FindFirst_Should_ReturnFirstEntity_When_MultipleEntitiesAreReturnByQuery()
        {
            // Arrange
            InitializeDatabase();
            var repository = new PatientRepository();

            // Act
            var patient = repository.FindFirst(p => p.DateAdded > p.AdmitDate);
            repository.Dispose();

            // Assert
            Assert.Equal(patient.Name, "Bar");
        }

        [Fact]
        public void Save_Should_AddNewEntityToDb()
        {
            // Arrange
            InitializeDatabase();
            var records = 0;

            // Act
            using (var repository = new PatientRepository())
            {
                var entity = new Patient
                    {
                        Name = "Lotho Fairbairn",
                        Sex = Gender.Male,
                        AdmitDate = DateTime.Now.AddDays(-2),
                        DateAdded = DateTime.Now
                    };

                repository.Save(entity);
            }

            using (var repository = new PatientRepository())
            {
                var entityFromDatabase = repository.FindAll();
                records = entityFromDatabase.Count();
            }

            // Assert
            Assert.Equal(4, records);
        }

        [Fact]
        public void Save_Should_ReturnUpdatedEntityFromDb_When_EntityIsChangedAndSaved()
        {
            // Arrange
            InitializeDatabase();
            Patient entityFromDatabase;
            long recordId = 0;

            // Act
            using (var repository = new PatientRepository())
            {
                var entity = repository.FindBy(q => q.Name == "Foo").First();

                recordId = entity.Id;
                entity.Name = "John Smith";

                repository.Save(entity);
            }

            using (var repository = new PatientRepository())
            {
                entityFromDatabase = repository.FindById(recordId);
            }

            // Assert
            Assert.Equal("John Smith", entityFromDatabase.Name);
        }

        // Create a blank database for unit test.
        private static void InitializeDatabase()
        {
            // Add some entities to database
            using (var db = new PatientContext())
            {
                var foo = new Patient
                    {
                        Name = "Foo",
                        Sex = Gender.Male,
                        DateAdded = new DateTime(2012, 1, 1),
                        AdmitDate = new DateTime(2012, 1, 2)
                    };

                var bar = new Patient
                    {
                        Name = "Bar",
                        Sex = Gender.Female,
                        DateAdded = new DateTime(2012, 1, 3),
                        AdmitDate = new DateTime(2012, 1, 2)
                    };

                var doh = new Patient
                    {
                        Name = "Doh",
                        Sex = Gender.Female,
                        DateAdded = new DateTime(2012, 2, 3),
                        AdmitDate = new DateTime(2012, 2, 2)
                    };

                db.Patients.Add(foo);
                db.Patients.Add(bar);
                db.Patients.Add(doh);

                db.SaveChanges();
            }
        }

        public class PatientRepository : Repository<Patient, Int64>
        {
            public PatientRepository()
            {
                Context = new PatientContext();
            }
        }

        public class PatientContext : EntityFrameworkUnitOfWork
        {
            public DbSet<Patient> Patients { get; set; }
        }

        public class Patient : EntityWithTypedId<Int64>
        {
            public virtual string Name { get; set; }

            public virtual Gender Sex { get; set; }

            public virtual DateTime DateAdded { get; set; }

            public virtual DateTime AdmitDate { get; set; }
        }

        public class SqLiteConnectionFactory : IDbConnectionFactory
        {
            public DbConnection CreateConnection(string nameOrConnectionString)
            {
                var connectionString = new SQLiteConnectionStringBuilder()
                {
                    DataSource = "temp.db",
                    ForeignKeys = true
                }.ConnectionString;

                return new SQLiteConnection(connectionString);
            }
        }

        public class SqLiteConfiguration : DbConfiguration
        {
            public SqLiteConfiguration()
            {
                SetDefaultConnectionFactory(new SqLiteConnectionFactory());
                SetProviderFactory("System.Data.SQLite", SQLiteFactory.Instance);
                SetProviderFactory("System.Data.SQLite.EF6", SQLiteProviderFactory.Instance);
                var t = Type.GetType("System.Data.SQLite.EF6.SQLiteProviderServices, System.Data.SQLite.EF6");
                var fi = t.GetField("Instance", BindingFlags.NonPublic | BindingFlags.Static);
                SetProviderServices("System.Data.SQLite", (DbProviderServices)fi.GetValue(null));
            }
        }
    }
}
