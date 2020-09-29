using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ToolKit.OData;
using Xunit;

namespace UnitTests.OData
{
    [SuppressMessage(
            "StyleCop.CSharp.DocumentationRules",
            "SA1600:ElementsMustBeDocumented",
            Justification = "Test Suites do not need XML Documentation.")]
    public class ODataEntryTests
    {
        [Fact]
        public void AsDictionary_Should_ReturnTheDictionaryOfProperties()
        {
            // Arrange
            var entry = new ODataEntry(InitializeProperties());

            // Act
            var actual = entry.AsDictionary();

            // Assert
            Assert.IsType<Dictionary<string, object>>(actual);
        }

        [Fact]
        public void Properties_Should_HaveEmptyDictionaryWithDefaultConstructor()
        {
            // Arrange & Act
            var entry = new ShowMeEntry();

            // Assert
            Assert.Empty(entry.InternalProperties);
        }

        [Fact]
        public void Properties_Should_HavePropertiesWithProvidedDictionary()
        {
            // Arrange & Act
            var entry = new ShowMeEntry(InitializeProperties());

            // Assert
            Assert.Equal(30, entry.InternalProperties.Count);
        }

        [Fact]
        public void This_Should_ReturnTheCorrectValue()
        {
            // Arrange
            const string expected = "Moved to state Unassigned";
            var entry = new ShowMeEntry(InitializeProperties());

            //Act
            var actual = entry["Reason"];

            // Assert
            Assert.Equal(expected, actual);
        }

        private Dictionary<string, object> InitializeProperties()
        {
            return new Dictionary<string, object>()
            {
                { "WorkItemId", 11203 },
                { "InProgressDate", null },
                { "CompletedDate", null },
                { "InProgressDateSK", null },
                { "CompletedDateSK", null },
                { "AnalyticsUpdatedDate", "2020-09-23T18:21:10.7366667Z" },
                { "ProjectSK", "87202b68-c52c-451b-a15a-1ccdefe83fbe" },
                { "WorkItemRevisionSK", 172745 },
                { "AreaSK", "0eddc047-5365-4593-94d2-4d0a3515b922" },
                { "IterationSK", "c5b6834f-8df7-40d1-b45b-479cf4e3a1d0" },
                { "AssignedToUserSK", null },
                { "ChangedByUserSK", "25895bce-73d0-6534-b78c-9cd42c3b8cdc" },
                { "CreatedByUserSK", "25895bce-73d0-6534-b78c-9cd42c3b8cdc" },
                { "ActivatedByUserSK", null },
                { "ClosedByUserSK", null },
                { "ResolvedByUserSK", null },
                { "ActivatedDateSK", null },
                { "ChangedDateSK", 20200923 },
                { "ClosedDateSK", null },
                { "CreatedDateSK", 20200923 },
                { "ResolvedDateSK", null },
                { "StateChangeDateSK", 20200923 },
                { "Revision", 3 },
                { "Watermark", 134402 },
                { "Title", "(Android) Allergy not collapsed in Review summary screen" },
                { "WorkItemType", "Bug" },
                { "ChangedDate", "2020-09-23T14:21:06.647-04:00" },
                { "CreatedDate", "2020-09-23T14:20:42.453-04:00" },
                { "State", "Unassigned" },
                { "Reason", "Moved to state Unassigned" }
            };
        }

        private class ShowMeEntry : ODataEntry
        {
            public ShowMeEntry()
            {
            }

            public ShowMeEntry(IDictionary<string, object> entry) : base(entry)
            {
            }

            public Dictionary<string, object> InternalProperties => base.Properties;
        }
    }
}
