using System;
using System.Data.SQLite;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using Toolkit.Data.EFCore;
using ToolKit.Data;
using Xunit;

namespace UnitTests.Data
{
    [SuppressMessage("StyleCop.CSharp.DocumentationRules",
        "SA1600:ElementsMustBeDocumented",
        Justification = "Test Suites do not need XML Documentation.")]
    public class EfCoreRepositoryTests
    {
        private static bool _databaseCreated = false;

        public EfCoreRepositoryTests()
        {
            var databaseFile = $"EFCoreRepositoryTests.{Thread.CurrentThread.ManagedThreadId}.db";
            var connectionString = new SQLiteConnectionStringBuilder()
            {
                DataSource = databaseFile,
                ForeignKeys = true
            }.ConnectionString;

            if (!_databaseCreated)
            {
                if (File.Exists(databaseFile))
                {
                    File.Delete(databaseFile);
                }

                SQLiteConnection.CreateFile(databaseFile);

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
            Assert.Equal(1, patientCount);
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
            Assert.Equal(2, patientCount);
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
            Assert.Equal("Bar", patient.Name);
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

        private void InitializeDatabase()
        {
            var connectionString = new SQLiteConnectionStringBuilder()
            {
                DataSource = $"EFCoreRepositoryTests.{Thread.CurrentThread.ManagedThreadId}.db"
            }.ConnectionString;

            var options = new DbContextOptionsBuilder<PatientContext>()?.UseSqlite(connectionString).Options;

            using (var db = new PatientContext(options))
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

        public class Patient : EntityWithTypedId<long>
        {
            public virtual DateTime AdmitDate { get; set; }

            public virtual DateTime DateAdded { get; set; }

            public virtual string Name { get; set; }

            public virtual Gender Sex { get; set; }
        }

        public class PatientContext : EfCoreUnitOfWork
        {
            public PatientContext(DbContextOptions options)
                : base(options)
            {
            }

            public DbSet<Patient> Patients { get; set; }
        }

        public class PatientRepository : Repository<Patient, long>
        {
            public PatientRepository()
            {
                var connectionString = new SQLiteConnectionStringBuilder()
                {
                    DataSource = $"EFCoreRepositoryTests.{Thread.CurrentThread.ManagedThreadId}.db",
                }.ConnectionString;

                var options = new DbContextOptionsBuilder<PatientContext>()?.UseSqlite(connectionString).Options;
                var context = new PatientContext(options);

                Context = context;
            }
        }
    }
}
