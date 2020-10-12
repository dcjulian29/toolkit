using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Mapping;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using ToolKit.Data;
using ToolKit.Data.NHibernate;
using Xunit;

namespace UnitTests.Data
{
    [SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1600:ElementsMustBeDocumented",
        Justification = "Test Suites do not need XML Documentation.")]
    public class NHibernateRepositoryTests
    {
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
            var repository = new PatientRepository(true);

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
            Assert.Equal(entity, entityFromDatabase);
        }

        [Fact]
        public void FindFirst_Should_ReturnFirstEntity_When_MultipleEntitiesAreReturnByQuery()
        {
            // Arrange
            var repository = new PatientRepository(true);

            // Act
            var patient = repository.FindFirst(p => p.DateAdded > p.AdmitDate);
            repository.Dispose();

            // Assert
            Assert.Equal("Bar", patient.Name);
        }

        [Fact]
        public void Save_Should_ReturnUpdatedEntityFromDb_When_EntityIsChangedAndSaved()
        {
            // Arrange
            Patient entityFromDatabase;
            var recordId = 0;

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

        public class Patient : Entity
        {
            public virtual DateTime AdmitDate { get; set; }

            public virtual DateTime DateAdded { get; set; }

            public virtual string Name { get; set; }

            public virtual Gender Sex { get; set; }
        }

        public class PatientMap : ClassMap<Patient>
        {
            public PatientMap()
            {
                Id(x => x.Id);

                Map(x => x.Name)
                  .Length(16)
                  .Not.Nullable();
                Map(x => x.Sex);
                Map(x => x.DateAdded);
                Map(x => x.AdmitDate);
            }
        }

        public class PatientRepository : Repository<Patient, int>
        {
            private static bool _databaseCreated = false;
            private readonly ISession _session;

            public PatientRepository(bool initializeDatabase = false)
            {
                ISessionFactory sessionFactory;

                if (_databaseCreated)
                {
                    sessionFactory = Fluently.Configure()
                        .Database(SQLiteConfiguration.Standard.UsingFile("NHibernateRepositoryTests.db"))
                        .Mappings(m => m.FluentMappings.AddFromAssemblyOf<NHibernateRepositoryTests>())
                        .ExposeConfiguration(cfg => new SchemaExport(cfg).Create(false, false))
                        .BuildSessionFactory();

                    if (initializeDatabase)
                    {
                        using (_session = sessionFactory.OpenSession())
                        {
                            InitializeDatabase(_session);
                        }
                    }

                    if ((_session == null) || (!_session.IsOpen))
                    {
                        _session = sessionFactory.OpenSession();
                    }
                }
                else
                {
                    if (File.Exists("NHibernateRepositoryTests.db"))
                    {
                        File.Delete("NHibernateRepositoryTests.db");
                    }

                    sessionFactory = Fluently.Configure()
                        .Database(SQLiteConfiguration.Standard.UsingFile("NHibernateRepositoryTests.db"))
                        .Mappings(m => m.FluentMappings.AddFromAssemblyOf<NHibernateRepositoryTests>())
                        .ExposeConfiguration(cfg => new SchemaExport(cfg).Create(false, true))
                        .BuildSessionFactory();

                    _databaseCreated = true;

                    using (_session = sessionFactory.OpenSession())
                    {
                        InitializeDatabase(_session);
                    }

                    _session = sessionFactory.OpenSession();
                }

                Context = new NHibernateUnitOfWork(_session);
            }

            ~PatientRepository()
            {
                // SQLite doesn't always unlock DB file when test ends, Let's force it.
                _session.Flush();
                _session.Disconnect();
                _session.Close();

#pragma warning disable S1215 // "GC.Collect" should not be called
                GC.Collect();
#pragma warning restore S1215 // "GC.Collect" should not be called
            }

            private void InitializeDatabase(ISession session)
            {
                session.CreateSQLQuery("DELETE FROM Patient").ExecuteUpdate();

                using (var transaction = session.BeginTransaction())
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

                    session.SaveOrUpdate(foo);
                    session.SaveOrUpdate(bar);
                    session.SaveOrUpdate(doh);

                    transaction.Commit();
                }

                session.Flush();
                session.Close();
            }
        }
    }
}
