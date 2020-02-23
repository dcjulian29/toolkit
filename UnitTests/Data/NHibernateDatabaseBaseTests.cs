using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using FluentNHibernate.Mapping;
using NHibernate.Criterion;
using ToolKit.Data;
using ToolKit.Data.NHibernate;
using ToolKit.Data.NHibernate.UnitTests;
using Xunit;

namespace UnitTests.Data
{
    [SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1600:ElementsMustBeDocumented",
        Justification = "Test Suites do not need XML Documentation.")]
    public class NHibernateDatabaseBaseTests
    {
        [Fact]
        public void NHibernateDatabaseBase_Should_ReturnDebbieForFirstRecord()
        {
            // Arrange
            _ = new UnitTestDatabase(Assembly.GetExecutingAssembly(), InitializeDatabase);

            var session = NHibernateDatabaseBase.Instance.SessionFactory("NHibernateDatabaseBase").OpenSession();
            var criteria = session.CreateCriteria<Employee>();
            _ = criteria.Add(Expression.Eq("Name", "Debbie"));

            // Act
            var employees = criteria.List<Employee>();
            var results = employees[0];

            // Assert

            Assert.Equal("Debbie", results.Name);
        }

        [Fact]
        public void NHibernateDatabaseBase_Should_ReturnFrankForSecondRecord()
        {
            // Arrange
            _ = new UnitTestDatabase(Assembly.GetExecutingAssembly(), InitializeDatabase);

            var session = NHibernateDatabaseBase.Instance.SessionFactory("NHibernateDatabaseBase").OpenSession();
            var criteria = session.CreateCriteria<Employee>();
            _ = criteria.Add(Expression.Eq("Name", "Frank"));

            // Act
            var employees = criteria.List<Employee>();
            var results = employees[0];

            // Assert

            Assert.Equal("Frank", results.Name);
        }

        private void InitializeDatabase()
        {
            var session = NHibernateDatabaseBase.Instance.SessionFactory("NHibernateDatabaseBase").OpenSession();

            _ = session.CreateSQLQuery("DELETE FROM Employee").ExecuteUpdate();

            using (var transaction = session.BeginTransaction())
            {
                var debbie = new Employee
                {
                    Name = "Debbie",
                    DateAdded = new DateTime(2012, 1, 1),
                    HireDate = new DateTime(2012, 1, 2)
                };

                var frank = new Employee
                {
                    Name = "Frank",
                    DateAdded = new DateTime(2012, 1, 3),
                    HireDate = new DateTime(2012, 1, 2)
                };

                session.SaveOrUpdate(debbie);
                session.SaveOrUpdate(frank);

                transaction.Commit();
            }

            session.Flush();
            _ = session.Close();
        }

        public class Employee : Entity
        {
            public virtual DateTime DateAdded { get; set; }
            public virtual DateTime HireDate { get; set; }
            public virtual string Name { get; set; }
        }

        public class EmployeeMap : ClassMap<Employee>
        {
            public EmployeeMap()
            {
                Id(x => x.Id);

                Map(x => x.Name)
                  .Length(16)
                  .Not.Nullable();
                Map(x => x.DateAdded);
                Map(x => x.HireDate);
            }
        }
    }
}
