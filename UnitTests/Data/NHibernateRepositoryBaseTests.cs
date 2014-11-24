using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Mapping;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using ToolKit.Data;
using ToolKit.Data.NHibernate;
using Xunit;

namespace UnitTests.Data
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1600:ElementsMustBeDocumented",
        Justification = "Test Suites do not need XML Documentation.")]
    public class NHibernateRepositoryBaseTests
    {
        private static ISessionFactory _sessionFactory;

        public NHibernateRepositoryBaseTests()
        {
            if (_sessionFactory == null)
            {
                _sessionFactory = Fluently.Configure()
                    .Database(SQLiteConfiguration.Standard.UsingFile("temp.db"))
                    .Mappings(m => m.FluentMappings.AddFromAssemblyOf<NHibernateRepositoryBaseTests>())
                    .ExposeConfiguration(cfg => new SchemaExport(cfg).Create(false, true))
                    .BuildSessionFactory();
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
            using (var repository = new PatientRepository(_sessionFactory.OpenSession()))
            {
                var patients = repository.FindBy(p => p.DateAdded > p.AdmitDate);
                repository.Delete(patients);
            }

            using (var repository = new PatientRepository(_sessionFactory.OpenSession()))
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
            using (var repository = new PatientRepository(_sessionFactory.OpenSession()))
            {
                detachedPatient = repository.FindFirst(p => p.DateAdded > p.AdmitDate);
            }

            using (var repository = new PatientRepository(_sessionFactory.OpenSession()))
            {
                patientPresent = repository.Contains(detachedPatient);
            }

            // Assert
            Assert.True(patientPresent);
        }


//Contains(Patient)

        [Fact]
        public void FindAll_Should_ReturnAllEntities()
        {
            // Arrange
            InitializeDatabase();
            var repository = new PatientRepository(_sessionFactory.OpenSession());

            // Act
            var patients = repository.FindAll();
            var patientCount = patients.Count();
            repository.Dispose();

            // Assert
            Assert.Equal(patientCount, 3);
        }

        [Fact]
        public void FindBy_Should_ReturnCorrectEntities_When_QueriedByCriteria()
        {
            // Arrange
            InitializeDatabase();
            var repository = new PatientRepository(_sessionFactory.OpenSession());
            var criteria = new Dictionary<string, object>();

            criteria.Add("Sex", Gender.Female);

            // Act
            var patients = repository.FindBy(criteria);
            var patientCount = patients.Count();
            repository.Dispose();

            // Assert
            Assert.Equal(patientCount, 2);
        }

        [Fact]
        public void FindBy_Should_ReturnCorrectEntities_When_QueriedByLinq()
        {
            // Arrange
            InitializeDatabase();
            var repository = new PatientRepository(_sessionFactory.OpenSession());

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
            using (var repository = new PatientRepository(_sessionFactory.OpenSession()))
            {
                entity.Name = "Test";
                entity.Sex = Gender.Female;
                entity.DateAdded = new DateTime(2012, 2, 1);
                entity.AdmitDate = new DateTime(2012, 2, 2);
                repository.Save(entity);
            }

            using (var repository = new PatientRepository(_sessionFactory.OpenSession()))
            {
                entityFromDatabase = repository.FindById(entity.Id);
            }

            // Assert
            Assert.Equal(entity, entityFromDatabase);
        }

        [Fact]
        public void FindFirst_Should_ReturnFirstEntity_When_MultipleEntitiesAreReturnByCriteria()
        {
            // Arrange
            InitializeDatabase();
            var repository = new PatientRepository(_sessionFactory.OpenSession());
            var criteria = new Dictionary<string, object>();

            criteria.Add("Sex", Gender.Female);

            // Act
            var patient = repository.FindFirst(criteria);
            repository.Dispose();

            // Assert
            Assert.Equal(patient.Name, "Bar");
        }

        [Fact]
        public void FindFirst_Should_ReturnFirstEntity_When_MultipleEntitiesAreReturnByQuery()
        {
            // Arrange
            InitializeDatabase();
            var repository = new PatientRepository(_sessionFactory.OpenSession());

            // Act
            var patient = repository.FindFirst(p => p.DateAdded > p.AdmitDate);
            repository.Dispose();

            // Assert
            Assert.Equal(patient.Name, "Bar");
        }

        [Fact]
        public void Save_Should_ReturnUpdatedEntityFromDb_When_EntityIsChangedAndSaved()
        {
            // Arrange
            InitializeDatabase();
            Patient entityFromDatabase;
            var recordId = 0;

            // Act
            using (var repository = new PatientRepository(_sessionFactory.OpenSession()))
            {
                var entity = repository.FindBy(q => q.Name == "Foo").First();

                recordId = entity.Id;
                entity.Name = "John Smith";

                repository.Save(entity);
            }

            using (var repository = new PatientRepository(_sessionFactory.OpenSession()))
            {
                entityFromDatabase = repository.FindById(recordId);
            }

            // Assert
            Assert.Equal("John Smith", entityFromDatabase.Name);
        }

        // Create a blank database for unit test.
        private static void InitializeDatabase()
        {
            using (var session = _sessionFactory.OpenSession())
            {
                _sessionFactory.OpenSession().CreateSQLQuery("DELETE FROM Patient").ExecuteUpdate();


                // Add some entities to database
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
            }
        }

        public class PatientRepository : NHibernateRepositoryBase<Patient, int>
        {
            public PatientRepository(ISession session)
            {
                Context = new NHibernateUnitOfWork(session);
            }
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

        public class Patient : Entity
        {
            public virtual string Name { get; set; }

            public virtual Gender Sex { get; set; }

            public virtual DateTime DateAdded { get; set; }

            public virtual DateTime AdmitDate { get; set; }
        }
    }
}
